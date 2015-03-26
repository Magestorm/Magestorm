using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Helper;
using Helper.Network;
using MageServer.Properties;

namespace MageServer
{
    public static class Network
    {
        [Flags]
        public enum SendToType
        {
            None = 0x00,
            All = 0x01,
            World = 0x02,
            Tavern = 0x04,
            Table = 0x08,
            Arena = 0x10,
            Team = 0x20,
        }

		private const Int32 PacketHeaderFooterLength = 14;
	    private static TcpListener _gameListener;

        public static void Listen()
        {
            _gameListener = new TcpListener(new IPAddress(0), Settings.Default.ListenPort);
            _gameListener.Server.LingerState = new LingerOption(true, 10);
            _gameListener.Server.NoDelay = true;
            _gameListener.Server.ReceiveTimeout = 10000;
            _gameListener.Server.SendTimeout = 10000;

            while (!_gameListener.Server.IsBound)
            {
                try
                {
                    _gameListener.Start();
                }
                catch (SocketException)
                {
					foreach (Process p in Process.GetProcesses().Where(p => p.ProcessName == "MageServer" && p.Handle != Process.GetCurrentProcess().Handle))
					{
						p.Kill();
						p.WaitForExit();
					}
                }   
            }

            _gameListener.BeginAcceptTcpClient(OnAcceptConnection, _gameListener);

            Program.ServerForm.MainLog.WriteMessage(String.Format("Server listening on port {0}.", Settings.Default.ListenPort), Color.Blue);
        }

	    private static void OnAcceptConnection(IAsyncResult asyn)
        {
            TcpListener listener = (TcpListener)asyn.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(asyn);

            listener.BeginAcceptTcpClient(OnAcceptConnection, listener);

            new Player(client);
        }

        public static void Disconnect(Player player)
        {
            player.TcpClient.Client.DisconnectAsync(new SocketAsyncEventArgs());

            Program.ServerForm.MainLog.WriteMessage(String.Format("{0} has disconnected. ({1})", player.IsLoggedIn ? player.Username : player.IpAddress, player.DisconnectReason), Color.BlueViolet);
            
            if (PlayerManager.Players.Contains(player))
            {
                if (player.IsLoggedIn)
                {
                    if (player.ActiveArena != null)
                    {
                        if (player.ActiveArenaPlayer != null)
                        {
                            player.ActiveArena.PlayerLeft(player.ActiveArenaPlayer);
                        }
                    }

                    if (player.ActiveCharacter != null)
                    {
	                    MySQL.OnlineCharacters.SetOffline(player.ActiveCharacter.CharacterId);
                    }

                    SendTo(player, GamePacket.Outgoing.World.PlayerLeave(player), SendToType.Tavern, false);
                }

				MySQL.OnlineAccounts.SetOffline(player.AccountId);
            }

            PlayerManager.Players.Remove(player);
        }

	    private static Int32 GetChecksum(Byte[] data, Int32 position, Int32 length)
        {
            Int32 x = 0x7E, y = x;

            for (Int32 i = 2; i < (length - 2); i++)
            {
                x = (data[position + i] + x) & 0xFF;
                y = (x + y) & 0xFF;
            }

            return (y - ((x + y) << 8) & 0xFFFF);
        }

        public static Int32 GameRecv(Player player, Byte[] data, Int32 size)
        {
            Int32 position = 0;

            while (position < size)
            {
                if (BitConverter.ToInt16(data, position) != 0x1B1B) break;

                Int32 length = NetHelper.FlipBytes(BitConverter.ToInt16(data, position+2)) + PacketHeaderFooterLength;
                Int32 packetEnd = length + position;

                if (length <= 0 || packetEnd > size) break;

                if (NetHelper.FlipBytes((BitConverter.ToUInt16(data, packetEnd-2))) == GetChecksum(data, position, length))
                {
                    Int32 packetNumber = NetHelper.FlipBytes((BitConverter.ToUInt16(data, position + 4)));

                    if (packetNumber != player.PacketCounter)
                    {
                        if (player.PacketCounter >= 65535 || (player.PacketCounter == 10000 && packetNumber == 0))
                        {
                            player.PacketCounter = 0;
                        }
                        else if (player.PacketCounter == 0)
                        {
                            player.PacketCounter = packetNumber;
                        }
                        else
                        {
                            player.DisconnectReason = String.Format("Packet Order Corruption (E: {0:X4}, G: {1:X4})", player.PacketCounter, packetNumber);
                            player.Disconnect = true;
                            break;
                        }
                    }

                    player.PacketCounter++;

                    MemoryStream inStream = new MemoryStream(data, position+10, length-12, false);

                    switch (data[position + 11])
                    {
                        case 0x01:
                        {
                            GamePacket.Incoming.Arena.PlayerMoveState(player, inStream);
                            break;
                        }
                        case 0xB8:
                        case 0x03:
                        {
                            GamePacket.Incoming.Player.EnterWorld(player, inStream);
                            break;
                        }
                        case 0x04:
                        {
                            GamePacket.Incoming.Player.ExitWorld(player);
                            break;
                        }
                        case 0x07:
                        {
                            GamePacket.Incoming.Player.Chat(player, inStream);
                            break;
                        }
                        case 0x0B:
                        {
                            GamePacket.Incoming.Player.Heartbeat(player, inStream);
                            break;
                        }
                        case 0x0F:
                        {
                            GamePacket.Incoming.Login.Authenticate(player, inStream);
                            break;
                        }
                        case 0x10:
                        {
                            GamePacket.Incoming.Login.Disconnect(player);
                            break;
                        }
                        case 0x11:
                        {
                            GamePacket.Incoming.World.RequestedArenaStatus(player);
                            break;
                        }
                        case 0x12:
                        {
                            GamePacket.Incoming.Arena.PlayerMoveStateShort(player, inStream);
                            break;
                        }
                        case 0x17:
                        {
                            GamePacket.Incoming.Player.HasEnteredWorld(player);
                            break;
                        }
                        case 0x22:
                        {
                            GamePacket.Incoming.World.CreateArena(player, inStream);
                            break;
                        }
                        case 0x23:
                        {
                            GamePacket.Incoming.World.DeleteArena(player, inStream);
                            break;
                        }
                        case 0x25:
                        {
                            GamePacket.Incoming.Arena.CastBolt(player, inStream);
                            break;
                        }
                        case 0x26:
                        {
                            GamePacket.Incoming.World.RequestedPlayer(player, inStream);
                            break;
                        }
                        case 0x28:
                        {
                            GamePacket.Incoming.Arena.BiasedPool(player, inStream);
                            break;
                        }
                        case 0x2A:
                        {
                            GamePacket.Incoming.Arena.BiasedShrine(player, inStream);
                            break;
                        }
                        case 0x2C:
                        {
                            GamePacket.Incoming.Arena.CastDispell(player, inStream);
                            break;
                        }
                        case 0x2D:
                        {
                            GamePacket.Incoming.Arena.CastTargeted(player, inStream);
                            break;
                        }
                        case 0x2F:
                        {
                            GamePacket.Incoming.Arena.ThinDamage(player, inStream);
                            break;
                        }
                        case 0x30:
                        {
                            GamePacket.Incoming.Arena.CalledGhost(player, inStream);
                            break;
                        }
                        case 0x31:
                        {
                            GamePacket.Incoming.Arena.ActivatedTrigger(player, inStream);
                            break;
                        }
                        case 0x32:
                        {
                            GamePacket.Incoming.World.RequestedArena(player, inStream);
                            break;
                        }
                        case 0x33:
                        {
                            GamePacket.Incoming.World.RequestedAllPlayers(player);
                            break;
                        }
                        case 0x35:
                        {
                            GamePacket.Incoming.World.RequestedAllArenas(player);
                            break;
                        }
                        case 0x45:
                        {
                            GamePacket.Incoming.World.CreateTable(player, inStream);
                            break;
                        }
                        case 0x46:
                        {
                            GamePacket.Incoming.World.DeleteTable(inStream);
                            break;
                        }
                        case 0x47:
                        {
                            GamePacket.Incoming.World.RequestedAllTables(player);
                            break;
                        }
                        case 0x52:
                        {
                            GamePacket.Incoming.Player.InviteToTable(inStream);
                            break;
                        }
                        case 0x53:
                        {
                            GamePacket.Incoming.Player.SwitchedToTableOrArena(player, inStream);
                            break;
                        }
                        case 0x54:
                        {
                            GamePacket.Incoming.Study.RequestCharacterInSlot(player, inStream);
                            break;
                        }
                        case 0x57:
                        {
                            GamePacket.Incoming.Character.Save(player, inStream);
                            break;
                        }
                        case 0x63:
                        {
                            GamePacket.Incoming.Study.IsNameTaken(player, inStream);
                            break;
                        }
                        case 0x68:
                        {
                            GamePacket.Incoming.Character.Delete(player, inStream);
                            break;
                        }
                        case 0x6A:
                        {
                            GamePacket.Incoming.Study.IsNameValid(player, inStream);
                            break;
                        }
                        case 0xA1:
                        {
                            GamePacket.Incoming.Study.HighScores(player, inStream);
                            break;
                        }
                        case 0xA4:
                        {
                            GamePacket.Incoming.Arena.Yank(player, inStream);
                            break;
                        }
                        case 0xAC:
                        {
                            GamePacket.Incoming.Arena.Jump(player, inStream);
                            break;
                        }
                        case 0xAD:
                        {
                            GamePacket.Incoming.Arena.God(player, inStream);
                            break;
                        }
                        case 0xB0:
                        {
                            GamePacket.Incoming.Arena.CastProjectile(player, inStream);
                            break;
                        }
                        case 0xB1:
                        {
                            GamePacket.Incoming.Arena.TappedAtShrine(player);
                            break;
                        }
                        case 0xB2:
                        {
                            GamePacket.Incoming.Arena.CastSign(player, inStream);
                            break;
                        }
                        case 0xB3:
                        {
                            GamePacket.Incoming.Arena.CastEffect(player, inStream);
                            break;
                        }
                        case 0xB4:
                        {
                            GamePacket.Incoming.Arena.CastWall(player, inStream);
                            break;
                        }
                        case 0xE0:
                        {
                            GamePacket.Incoming.MageHook.HackNotification(player, inStream);
                            break;
                        }
                        case 0xE1:
                        {
                            GamePacket.Incoming.MageHook.CheatProgramNotification(player, inStream);
                            break;
                        }
                        default:
                        {
                            //Program.ServerForm.AdminLog.WriteMessage(String.Format("\n Function: {0}\n{1}", data[position + 11], BitConverter.ToString(inStream.ToArray())), Color.Blue);
                            break;
                        }
                    }

                    inStream.Dispose();
                }
                else break;

                position += length;
            }

            return position;
        }

        public static void Send(Player player, MemoryStream inStream)
        {
            try
            {
                Packet packet = new Packet(inStream);

                player.TcpClient.Client.BeginSend(packet.PacketData, 0, packet.PacketData.Length, SocketFlags.None, SendCallback, new SendCallbackSyncResult(player));
            }
            catch (Exception)
            {
                player.DisconnectReason = "Send (Stream) Error";
                player.Disconnect = true;
            }
        }

        public static void Send(Player player, Packet packet)
        {
            try
            {
                player.TcpClient.Client.BeginSend(packet.PacketData, 0, packet.PacketData.Length, SocketFlags.None, SendCallback, new SendCallbackSyncResult(player));
            }
            catch (Exception)
            {
                player.DisconnectReason = "Send (Byte) Error";
                player.Disconnect = true;
            }
        }

        public static void SendTo(MemoryStream inStream, SendToType sendToType)
        {
            Packet packet = new Packet(inStream);

            for (Int16 i = 0; i < PlayerManager.Players.Count; i++)
            {
                Player p = PlayerManager.Players[i];

                if (p == null) continue;

                if (sendToType.HasFlag(SendToType.All))
                {
                    Send(p, packet);
                    continue;
                }

                if (sendToType.HasFlag(SendToType.World))
                {
                    if (p.TableId > 0)
                    {
                        Send(p, packet);
                        continue;
                    }
                }

                if (sendToType.HasFlag(SendToType.Tavern))
                {
                    if (p.TableId >= 50)
                    {
                        Send(p, packet);
                    }
                }
            }
        }

        public static void SendTo(Player player, MemoryStream inStream, SendToType sendToType, Boolean toSelf)
        {
            if (player == null) return;

            Packet packet = new Packet(inStream);

            for (Int16 i = 0; i < PlayerManager.Players.Count; i++)
            {
                Player p = PlayerManager.Players[i];

                if (p == null || (!toSelf && p == player)) continue;

                if (sendToType.HasFlag(SendToType.All))
                {
                    Send(p, packet);
                    continue;
                }

                if (sendToType.HasFlag(SendToType.World))
                {
                    if (p.TableId > 0)
                    {
                        Send(p, packet);
                        continue;
                    }
                }

                if (sendToType.HasFlag(SendToType.Tavern))
                {
                    if (p.TableId >= 50)
                    {
                        Send(p, packet);
                        continue;
                    }
                }

                if (sendToType.HasFlag(SendToType.Table))
                {
                    if (p.TableId == player.TableId)
                    {
                        Send(p, packet);
                        continue;
                    }
                }

                if (sendToType.HasFlag(SendToType.Arena))
                {
                    if (player.ActiveArena != null && p.ActiveArena == player.ActiveArena)
                    {
                        Send(p, packet);
                        continue;
                    }
                }

                if (sendToType.HasFlag(SendToType.Team))
                {
                    if (p.ActiveArena != null && p.ActiveArena == player.ActiveArena && p.ActiveTeam == player.ActiveTeam)
                    {
                        Send(p, packet);
                    }
                }
            }
        }

        public static void SendTo(Arena arena, MemoryStream inStream, SendToType sendToType)
        {
            if (arena == null) return;

            Packet packet = new Packet(inStream);

            for (Byte i = 0; i < arena.ArenaPlayers.Count; i++)
            {
                ArenaPlayer arenaPlayer = arena.ArenaPlayers[i];

                if (arenaPlayer == null) continue;

                if (sendToType.HasFlag(SendToType.All))
                {
                    Send(arenaPlayer.WorldPlayer, packet);
                    continue;
                }

                if (sendToType.HasFlag(SendToType.Arena))
                {
                    if (arenaPlayer.WorldPlayer.ActiveArena == arena)
                    {
                        Send(arenaPlayer.WorldPlayer, packet);
                    }
                }
            }
        }

        public static void SendToArena(ArenaPlayer arenaPlayer, MemoryStream inStream, Boolean sendToSource)
        {
            if (arenaPlayer == null) return;

            Arena arena = arenaPlayer.WorldPlayer.ActiveArena;
            if (arena == null) return;

            Packet packet = new Packet(inStream);

            for (Byte i = 0; i < arena.ArenaPlayers.Count; i++)
            {
                ArenaPlayer targetArenaPlayer = arena.ArenaPlayers[i];

                if (targetArenaPlayer == null || (targetArenaPlayer == arenaPlayer && !sendToSource)) continue;

                Send(targetArenaPlayer.WorldPlayer, packet);
            }
        }

	    private static void SendCallback(IAsyncResult ar)
        {
            SendCallbackSyncResult result = (SendCallbackSyncResult)ar.AsyncState;

            try
            {
                result.Player.TcpClient.Client.EndSend(ar);
            }
            catch (Exception)
            {
                result.Player.DisconnectReason = "SendCallback Error";
                result.Player.Disconnect = true;
            }
        }

	    private struct SendCallbackSyncResult
        {
            public Player Player;

            public SendCallbackSyncResult(Player player)
            {
                Player = player;
            }
        }
    }
}