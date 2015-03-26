using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Helper;
using Helper.Timing;

namespace MageServer
{
    [Flags]
    public enum PlayerFlag
    {
        None = 0x00,
        MagestormPlus = 0x01,
        ExpLocked = 0x02,
        Hidden = 0x04,
        Muted = 0x08,
        MusicDisabled = 0x10,
        ChatDisabled = 0x20,
    }

    public enum AdminLevel
    {
        None = 0,
        Tester = 1,
        Moderator = 2,
        Staff = 3,
        Developer = 4,
    }

    public enum WorldLocation
    {
        None = 0,
        Study,
        Tavern,
        Table,
        Arena,
    }

    public class Player
    {
	    private const Int32 MaxPing = 500;
	    private const Int32 PingSampleCount = 6;
	    private const Int32 PingInterval = 4000;

	    private readonly Thread ProcessReceiveThread;
        public readonly TcpClient TcpClient;
        public Int32 PacketCounter;

        public readonly String IpAddress;
        public Int32 Ping;
        public Boolean PingInitialized;
        public Boolean Disconnect;
        public String DisconnectReason;

        public Int16 PlayerId;
        public Int32 AccountId;
        public String Username;
        public String Serial;
        public ArenaPlayer ActiveArenaPlayer;
        public Character ActiveCharacter;
        public AdminLevel Admin;
        public PlayerFlag Flags;
        public Byte LastArenaId;

        public ArenaRuleset.ArenaMode PreferredArenaMode;
        public ArenaRuleset.ArenaRule PreferredArenaRules;
        public Int32 PreferredEventExp;

        private UInt32 _lastHeartbeat;
        private Arena _arena;
        private Byte _tableId;
        private Team _team;

        private readonly Interval _pingInterval;
        private Int64 _pingTime;
        private Int32 _pingSamples;

		private readonly Byte[] _receiveBuffer;

	    private Boolean Connected
        {
            get
            {
                try
                {
                    return !((TcpClient.Client.Poll(1, SelectMode.SelectRead) && TcpClient.Client.Available == 0));
                }
                catch (SocketException)
                {
                    return false;
                }
            }
        }

	    private Boolean IsPingSpiking
        {
            get
            {
                if (!PingInitialized)
                {
                    _pingTime = 0;
                    _pingSamples = 0;
                    _pingInterval.Reset();

                    PingInitialized = true;
                }
                else
                {
                    if (_pingSamples < PingSampleCount)
                    {
                        _pingTime += _pingInterval.ElapsedMilliseconds;
                        _pingSamples++;
                    }
                    else
                    {
                        Ping = (Int32)((_pingTime - (PingInterval * PingSampleCount)) / PingSampleCount);

                        _pingSamples = 0;
                        _pingTime = 0;

                        if (Ping > MaxPing && !IsAdmin) return true;
                    }

                    _pingInterval.Reset();
                }

                return false;
            }
        }

        public UInt32 LastHeartbeat
        {
            get { return _lastHeartbeat; }
	        private set
            {
                _lastHeartbeat = value;
                Network.Send(this, GamePacket.Outgoing.Player.HeartbeatReply(this));
            }
        }

        public Arena ActiveArena
        {
            get { return _arena; }
            set
            {
                _arena = value;
                if (value != null)
                {
	                MySQL.OnlineCharacters.SetOnline(ActiveCharacter.CharacterId, TableId, _arena.ArenaId,_arena.ShortGameName);
                }
            }
        } 

        public Byte TableId
        {
            get { return _tableId; }
            set
            {
                _tableId = value;

	            if (value <= 0) return;

	            if (ActiveArena == null)
	            {
					MySQL.OnlineCharacters.SetOnline(ActiveCharacter.CharacterId, TableId, 0, "");
	            }
	            else
	            {
					MySQL.OnlineCharacters.SetOnline(ActiveCharacter.CharacterId, TableId, ActiveArena.ArenaId, ActiveArena.ShortGameName);
	            }

	            Network.SendTo(this, GamePacket.Outgoing.Player.SwitchedToTable(this), Network.SendToType.Tavern, false);
            }
        } 

        public Team ActiveTeam
        {
            get { return _team; }
            set
            {
                Team team = value;
                if ((Byte)team > 3) team = Team.Neutral;
                _team = team;
            }
        }  

        public Boolean IsLoggedIn
        {
            get { return AccountId > 0; }
        }

        public Boolean IsInArena
        {
            get { return ActiveArena != null && ActiveArenaPlayer != null; }
        }

        public Boolean IsInLobby
        {
            get { return TableId >= 50; }
        }

        public Boolean IsAdmin
        {
            get { return Admin >= AdminLevel.Moderator; }
        }

        public WorldLocation WorldLocation
        {
            get
            {
                if (ActiveCharacter != null)
                {
                    if (ActiveArena != null && ActiveArenaPlayer != null) return WorldLocation.Arena;
                    if (TableId == 255) return WorldLocation.Tavern;
                    if (TableId >= 50) return WorldLocation.Table;
                }

                return IsLoggedIn ? WorldLocation.Study : WorldLocation.None;
            }
        }
        public String WorldLocationString
        {
            get
            {
                if (TableId == 255) return "Tavern";

                return ActiveArena == null ? String.Format("Table {0}", TableId) : String.Format("Arena {0}", ActiveArena.ArenaId);
            }
        }

        public Color ChatColor
        {
            get
            {
                if (IsAdmin) return Color.DarkViolet;

                switch (ActiveTeam)
                {
                    case Team.Balance:
                    {
                        return Color.DarkGreen;
                    }
                    case Team.Order:
                    {
                        return Color.Blue;
                    }
                    case Team.Chaos:
                    {
                        return Color.DarkRed;
                    }
                    default:
                    {
                        return Color.Black;
                    }
                }
            }
        }

        public Player(TcpClient client)
        {
            TcpClient = client;

            TcpClient.NoDelay = true;

            ProcessReceiveThread = new Thread(ProcessReceive);
            _receiveBuffer = new Byte[TcpClient.ReceiveBufferSize];
            IpAddress = ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Address.ToString();

            Program.ServerForm.MainLog.WriteMessage(String.Format("Connection from {0}.", IpAddress), Color.Black);

            Disconnect = false;
	        DisconnectReason = Resources.Strings_Disconnect.LostConnection;
            
            Ping = 0;
            Serial = "Not_Found";
            LastArenaId = 255;

            Flags = PlayerFlag.None;
            PreferredArenaMode = ArenaRuleset.ArenaMode.Normal;
            PreferredArenaRules = ArenaRuleset.ArenaRule.None;
            PreferredEventExp = 0;

            _pingInterval = new Interval(0, false);
            _pingTime = 0;
            _pingSamples = 0;
            PingInitialized = false;

            lock (PlayerManager.Players.SyncRoot)
            {
                PlayerId = PlayerManager.Players.AvailableId;

                if (PlayerId == 0)
                {
                    Network.Send(this, GamePacket.Outgoing.Login.Error(Subscription.ErrorType.ServerFull));
                    Network.Disconnect(this);
                    return;
                }

                PlayerManager.Players.Add(this);
            }
			
            ProcessReceiveThread.Start();
        }

	    private void ProcessReceive()
        {
	        try
	        {
		        Int32 overflowBytes = 0;

		        while (Connected)
		        {
			        Int32 bytesReceived;

			        try
			        {
				        bytesReceived = TcpClient.Client.Receive(_receiveBuffer, overflowBytes, _receiveBuffer.Length - overflowBytes,
					        SocketFlags.None);
				        if (bytesReceived == 0) break;
			        }
			        catch (Exception)
			        {
				        break;
			        }

			        Int32 bytesProcessed = Network.GameRecv(this, _receiveBuffer, bytesReceived + overflowBytes);

			        if (bytesReceived + overflowBytes != bytesProcessed)
			        {
				        overflowBytes = (bytesReceived + overflowBytes) - bytesProcessed;
				        Array.Copy(_receiveBuffer, bytesProcessed, _receiveBuffer, 0, overflowBytes);
			        }
			        else
			        {
				        overflowBytes = 0;
			        }

			        if (Disconnect)
			        {
				        Network.Send(this, GamePacket.Outgoing.System.DirectTextMessage(this, String.Format("[System] You are being disconnected: {0}", DisconnectReason)));
				        break;
			        }
		        }

	        }
	        catch (Exception ex)
	        {
		        Program.ServerForm.MainLog.WriteMessage(String.Format("[ProcessReceive] , Player: {0}, Function: {1}, Message: {2}, Trace: {3}", Username, ex.TargetSite, ex.Message, ex.GetStackTrace()), Color.Red);
	        }
	        finally
	        {
				Network.Disconnect(this);
	        }
        }

        public void Heartbeat(UInt32 time)
        {
            LastHeartbeat = time;

            if (!IsInArena || IsAdmin) return;

            if (IsPingSpiking)
            {
                DisconnectReason = String.Format(Resources.Strings_Disconnect.PingSpiking, Ping);
                Disconnect = true;
                return;
            }

            Int64 elapsedActiveTime = ActiveArenaPlayer.ActiveTime.ElapsedSeconds;

            if (elapsedActiveTime >= (Flags.HasFlag(PlayerFlag.MagestormPlus) ? 480 : 300))
            {
                ActiveArena.ArenaKickPlayer(ActiveArenaPlayer);
            }
            else if (elapsedActiveTime >= (Flags.HasFlag(PlayerFlag.MagestormPlus) ? 460 : 280))
            {

            }
        }
    }
}
