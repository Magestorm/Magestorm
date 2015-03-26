using System;
using System.Threading;
using Helper;
using Helper.Timing;
using SharpDX;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace MageServer
{ 
    public class ArenaPlayer
    {
        private readonly Object _statusFlagsSync = new Object();

        [Flags]
        public enum StatusFlag
        {
            None = 0x00,
            Backwards = 0x08,
            Crouching = 0x10,
            Flying = 0x20,
            Hurt = 0x40,
            Torch = 0x80,
            Dead = Crouching | Flying | Hurt,
        }

        [Flags]
        public enum SpecialFlag
        {
            None,
            God,
        }

        public enum ExperienceType
        {
            Combat,
            Objective,
            Bonus
        }

        public static readonly Vector3 PlayerStandingSize = new Vector3(44, 44, 64);
        public static readonly Vector3 PlayerCrouchingSize = new Vector3(44, 44, 32);
        public static readonly Vector3 PlayerOrigin = new Vector3(20, 20, 0);

        public Arena OwnerArena;

        public Byte ArenaPlayerId;

        public Character ActiveCharacter;
        public Team ActiveTeam; 

        public OrientedBoundingBox BoundingBox;

        public DateTime JoinTime;
        public ArenaPlayer LastAttacker;

        public Interval NonFriendlyWallTime;
        public Interval FriendlyWallTime;
        public Interval InCombatTime;
        public Interval ActiveTime;

        public Vector3 Location;
        public Single Direction;

        public GridBlock CurrentGridBlock;
        public GridBlockFlagData CurrentGridBlockFlagData;

        public SpecialFlag SpecialFlags;
        public Byte MoveSpeed;
        public Effect[] Effects;
        public Int64 LastStateReceived;
        public Int16 StateReceivedCount;
        public Player WorldPlayer;
        public Boolean HasFliedSinceHackDetect;

        public Interval ValhallaProtection;

        private Vector3 _previousLocation;
        private Int16 _previousLocationTick;

        private Int16 _maxHp;
        private Int16 _currentHp;

        private Int16 _deathCount;
        private Int16 _killCount;
        private Int16 _raiseCount;
        
        private Int32 _combatExp;
        private Int32 _objectiveExp;
        private Int32 _bonusExp;

        private StatusFlag _statusFlags;

        public StatusFlag StatusFlags
        {
            get { return _statusFlags; }
            set
            {
                lock (_statusFlagsSync)
                {
                    if (value.HasFlag(StatusFlag.Flying) && HasFliedSinceHackDetect == false)
                    {
                        HasFliedSinceHackDetect = true;
                    }

                    _statusFlags = value;
                }
            }
        }

        public Vector3 PreviousLocation
        {
            set
            {
                if (_previousLocationTick >= 3)
                {
                    _previousLocationTick = 0;
                    _previousLocation = value;
                }
                else
                {
                    _previousLocationTick++;
                }
            }

            get { return _previousLocation; }
        }

        public Int16 KillCount
        {
            get { return _killCount; }
            set
            {
                if (value < 0) value = 0;
                if (value > 255) value = 255;

                _killCount = value;
            }
        }
        public Int16 DeathCount
        {
            get { return _deathCount; }
            set
            {
                if (value < 0) value = 0;
                if (value > 255) value = 255;

                _deathCount = value;
            }
        }
        public Int16 RaiseCount
        {
            get { return _raiseCount; }
            set
            {
                if (value < 0) value = 0;
                if (value > 255) value = 255;

                _raiseCount = value;
            }
        }

        public Int32 Points
        {
            get
            {
                return (KillCount - DeathCount) + (RaiseCount/2);
            }
        }

        public Boolean IsAlive
        {
            get { return CurrentHp > 0; }
        }

        public Boolean IsDamageable
        {
            get
            {
                return IsAlive && !WorldPlayer.Flags.HasFlag(PlayerFlag.Hidden) && !SpecialFlags.HasFlag(SpecialFlag.God);
            }
        }


        public Boolean IsInValhalla
        {
            get
            {
                return CurrentGridBlockFlagData.BlockFlag == GridBlockFlag.Valhalla || !ValhallaProtection.HasElapsed;
            }
        }

        public Boolean IsMoving
        {
            get
            {
                return MoveSpeed > 0 || Location != PreviousLocation;
            }
        }

        public Boolean IsInCombat
        {
            get
            {
                return !InCombatTime.HasElapsed;
            }
            set
            {
                if (value)
                {
                    InCombatTime.Reset();
                }
                else
                {
                    InCombatTime.End();
                }

                IsAwayFromKeyboard = false;
            }
        }

        public Boolean IsAwayFromKeyboard
        {
            get
            {
                return ActiveTime.HasElapsed;
            }
            set
            {
                if (value)
                {
                    ActiveTime.End();
                }
                else
                {
                    ActiveTime.Reset();
                }
            }
        }

        public Int16 MaxHp
        {
            get { return _maxHp; }
            set
            {
                if (value < 0) value = 0;
                if (value > 32767) value = 32767;

                _maxHp = value;
            }
        }
        public Int16 CurrentHp
        {
            get { return _currentHp; }
            set
            {
                if (value < 0) value = 0;
                if (value > MaxHp) value = MaxHp;
                if (value > 32767) value = 32767;

                _currentHp = value;

                if (StatusFlags.HasFlag(StatusFlag.Hurt))
                {
                    if ((Single)_currentHp / _maxHp > 0.65f)
                    {
                        StatusFlags &= ~StatusFlag.Hurt;
                    }
                }
                else
                {
                    if ((Single)_currentHp / _maxHp <= 0.65f)
                    {
                        StatusFlags |= StatusFlag.Hurt;
                    }
                }
            }
        }

        public Int32 CombatExp
        {
            get { return _combatExp; }
            set
            {
                if (WorldPlayer.Flags.HasFlag(PlayerFlag.ExpLocked)) return;

                if (value < 0) value = 0;
                if (value > 999999) value = 999999;

                if (WorldPlayer.ActiveArena != null && WorldPlayer.ActiveArena.ArenaPlayers.Count >= 2)
                {
                    _combatExp = value;

                    Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.UpdateExperience(this));
                }
            }
        }
        public Int32 ObjectiveExp
        {
            get { return _objectiveExp; }
            set
            {
                if (WorldPlayer.Flags.HasFlag(PlayerFlag.ExpLocked)) return;

                if (value < 0) value = 0;
                if (value > 999999) value = 999999;

                if (WorldPlayer.ActiveArena != null && WorldPlayer.ActiveArena.ArenaPlayers.Count >= 2)
                {
                    _objectiveExp = value;

                    Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.UpdateExperience(this));
                }
            }
        }
        public Int32 BonusExp
        {
            get { return _bonusExp; }
            set
            {
                if (WorldPlayer.Flags.HasFlag(PlayerFlag.ExpLocked)) return;

                if (value < 0) value = 0;
                if (value > 999999) value = 999999;

                if (WorldPlayer.ActiveArena != null)
                {
                    _bonusExp = value;

                    Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.UpdateExperience(this));
                }
            }
        }
        public Int32 TotalExp
        {
            get { return CombatExp + ObjectiveExp + BonusExp; }
        }

        public Int32 ExpPenalty
        {
            set
            {
                Single normalPenalty = (Single)Math.Ceiling(value * 0.5f);
                Single objectivePenalty = normalPenalty;

                if (normalPenalty > CombatExp)
                {
                    objectivePenalty += normalPenalty - CombatExp;
                    normalPenalty = CombatExp;
                }

                if (objectivePenalty > ObjectiveExp)
                {
                    normalPenalty += objectivePenalty - ObjectiveExp;
                    objectivePenalty = ObjectiveExp;
                }

                CombatExp -= (Int16)normalPenalty;
                ObjectiveExp -= (Int16)objectivePenalty;
            }
        }

        public Int32 SecondsPlayed
        {
            get
            {
                return (Int16)DateTime.Now.Subtract(JoinTime).TotalSeconds;
            }
        }

        public Shrine ActiveShrine
        {
            get { return OwnerArena.Grid.GetShrineByTeam(ActiveTeam); }
        }

        public ArenaPlayer(Player player, Arena arena)
        {
            lock (arena.SyncRoot)
            {
                WorldPlayer = player;
                OwnerArena = arena;

                lock (OwnerArena.ArenaPlayers.SyncRoot)
                {
                    OwnerArena.ArenaPlayers.ForEach(delegate(ArenaPlayer arenaPlayer)
                    {
                        if (arenaPlayer.WorldPlayer == player) OwnerArena.PlayerLeft(arenaPlayer);
                    });
                }

                if ((ArenaPlayerId = OwnerArena.ArenaPlayers.GetAvailablePlayerId()) == 0) return;

                WorldPlayer.PingInitialized = false;
                WorldPlayer.TableId = 0;
                WorldPlayer.ActiveArena = arena;
                WorldPlayer.LastArenaId = arena.ArenaId;

                ActiveTeam = OwnerArena.Ruleset.Rules.HasFlag(ArenaRuleset.ArenaRule.NoTeams) ? Team.Neutral : WorldPlayer.ActiveTeam;
                ActiveCharacter = player.ActiveCharacter;

                _previousLocation = new Vector3(0, 0, 0);
                _previousLocationTick = 0;

                Location = new Vector3(0, 0, 0);
                Direction = 0;

                CurrentGridBlock = null;
                CurrentGridBlockFlagData = new GridBlockFlagData();

                InCombatTime = new Interval(7000, false);
                NonFriendlyWallTime = new Interval(1000, false);
                FriendlyWallTime = new Interval(1000, false);
                ValhallaProtection = new Interval(2000, false);
                ActiveTime = new Interval(0, false);
                BoundingBox = new OrientedBoundingBox(Location, PlayerStandingSize, 0.0f);

                StatusFlags = StatusFlag.None;
                SpecialFlags = SpecialFlag.None;

                Effects = new Effect[21];

                MoveSpeed = 0;
                StateReceivedCount = 0;
                LastStateReceived = NativeMethods.PerformanceCount;

                LastAttacker = null;

                JoinTime = DateTime.Now;

                HasFliedSinceHackDetect = false;

                MaxHp = player.ActiveCharacter.MaxHealth;

                if (ActiveShrine == null)
                {
                    if (ActiveTeam == Team.Neutral)
                    {
                        CurrentHp = MaxHp;
                    }
                    else return;
                }
                else
                {
                    if (ActiveShrine.IsDisabled)
                    {
                        Network.Send(WorldPlayer, GamePacket.Outgoing.Player.SendPlayerId(this));

                        Thread.Sleep(500);

                        Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.SuccessfulArenaEntry());

                        Thread.Sleep(100);

                        OwnerArena.ArenaKickPlayer(this);

                        return;
                    }

                    CurrentHp = ActiveShrine.IsDead ? (Int16)0 : MaxHp;
                }

                Network.Send(WorldPlayer, GamePacket.Outgoing.Player.SendPlayerId(this));

                if (!WorldPlayer.Flags.HasFlag(PlayerFlag.Hidden))
                {
                    Network.SendTo(WorldPlayer, GamePacket.Outgoing.World.PlayerLeave(WorldPlayer), Network.SendToType.Tavern, false);
                    Network.SendTo(WorldPlayer, GamePacket.Outgoing.World.PlayerJoin(WorldPlayer), Network.SendToType.Tavern, false);

                    Network.SendToArena(this, GamePacket.Outgoing.Arena.PlayerJoin(this), false);
                }

                if (OwnerArena.ArenaPlayerHistory.FindByCharacterId(WorldPlayer.ActiveCharacter.CharacterId) == null)
                {
                    OwnerArena.ArenaPlayerHistory.Add(this);
                }

                WorldPlayer.ActiveArenaPlayer = this;
                OwnerArena.ArenaPlayers.Add(this);

                OwnerArena.AveragePlayerLevel = OwnerArena.ArenaPlayers.GetAveragePlayerLevel();
            }

            lock (OwnerArena.SyncRoot)
            {
                Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.UpdateShrinePoolState(arena));
            }

            Thread.Sleep(500);

            Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.SuccessfulArenaEntry());

            Thread.Sleep(100);

            lock (OwnerArena.SyncRoot)
            {
                for (Int32 i = 0; i < OwnerArena.Signs.Count; i++)
                {
                    Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.CastSign(this, OwnerArena.Signs[i].RawData));
                }
            }

            Thread.Sleep(100);

            lock (OwnerArena.SyncRoot)
            {
                for (Int32 i = 0; i < OwnerArena.Walls.Count; i++)
                {
                    Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.CastWall(arena.Walls[i].RawData));
                }
            }

            Thread.Sleep(100);

            lock (OwnerArena.SyncRoot)
            {
                for (Int32 i = 0; i < arena.Grid.Triggers.Count; i++)
                {
                    Network.Send(WorldPlayer, GamePacket.Outgoing.Arena.ActivatedTrigger(OwnerArena.Grid.Triggers[i]));
                }
            }

            if (OwnerArena.Ruleset.Mode == ArenaRuleset.ArenaMode.Custom)
            {
                Network.Send(WorldPlayer, GamePacket.Outgoing.System.DirectTextMessage(WorldPlayer, String.Format("This arena has the following rules: {0}.", arena.Ruleset.Rules)));
            }

            if (OwnerArena.Ruleset.Rules.HasFlag(ArenaRuleset.ArenaRule.ExpEvent))
            {
                Network.Send(WorldPlayer, GamePacket.Outgoing.System.DirectTextMessage(WorldPlayer, String.Format("If your team wins this match, you will earn {0:0,0} experience.", (WorldPlayer.Flags.HasFlag(PlayerFlag.MagestormPlus) ? OwnerArena.EventExp * 2f : OwnerArena.EventExp))));
            }

			Network.Send(WorldPlayer, GamePacket.Outgoing.System.DirectTextMessage(WorldPlayer, String.Format("This arena currently has an EXP bonus of {0}%.", ((arena.Grid.ExpBonus + (Properties.Settings.Default.ExpMultiplier - 1.0f) + (WorldPlayer.Flags.HasFlag(PlayerFlag.MagestormPlus) ? 0.2f : 0.0f)) * 100))));
        }
    }
}
