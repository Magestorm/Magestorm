using System;
using System.Linq;
using System.Threading;
using Helper;
using Helper.Timing;

namespace MageServer
{  
    public class ArenaManager : ListCollection<Arena>
    {
        public static ArenaManager Arenas = new ArenaManager();

        public new void Add(Arena arena)
        {
            base.Add(arena);

            Network.SendTo(GamePacket.Outgoing.World.ArenaCreated(arena), Network.SendToType.Tavern);

            for (Int32 j = 0; j < PlayerManager.Players.Count; j++)
            {
                Player player = PlayerManager.Players[j];
                if (player == null) continue;

                if (!player.IsInArena && player.TableId != 0)
                {
                    Network.Send(player, GamePacket.Outgoing.World.ArenaState(arena, player));
                }
            }
        }

        public Arena FindById(UInt32 arenaId)
        {
            return this.FirstOrDefault(a => arenaId == a.ArenaId);
        }
        public Arena FindByTableId(Int16 tableId)
        {
            return this.FirstOrDefault(a => tableId == a.TableId);
        }

        public Byte GetAvailableArenaId()
        {
            for (Byte i = 1; i <= 16; i++)
            {
                if (FindById(i) == null) return i;
            }
            return 0;
        }

        public readonly Thread WorkerThread;

        public Interval StatusTick = new Interval(2000, false);

        public ArenaManager()
        {
            WorkerThread = new Thread(ProcessArenas);
            WorkerThread.Start();
        }    

        private void ProcessArenas()
        {
            Boolean resetStatusUpdate = false;

            while (WorkerThread != null)
            {
                lock (Arenas.SyncRoot)
                {
                    for (Int32 i = Arenas.Count - 1; i >= 0; i--)
                    {
                        Arena arena = Arenas[i];
                        if (arena == null) continue;

                        lock (arena.SyncRoot)
                        {
                            if (arena.CurrentState == Arena.State.Ended) continue;

                            if (arena.CurrentState == Arena.State.CleanUp)
                            {
                                Arenas.Remove(arena);
                                continue;
                            }

                            Team winningTeam = arena.WinningTeam;

                            if (arena.CountdownTick == null && winningTeam != Team.Neutral)
                            {
                                switch (winningTeam)
                                {
                                    case Team.Chaos:
                                    {
                                        arena.CurrentState = Arena.State.ChaosVictory;
                                        break;
                                    }
                                    case Team.Balance:
                                    {
                                        arena.CurrentState = Arena.State.BalanceVictory;
                                        break;
                                    }
                                    case Team.Order:
                                    {
                                        arena.CurrentState = Arena.State.OrderVictory;
                                        break;
                                    }
                                }

                                arena.CountdownTick = new Interval(29000, false);
                            }
                            else if (winningTeam == Team.Neutral)
                            {
                                arena.CurrentState = Arena.State.Normal;
                                arena.CountdownTick = null;
                            }

                            if (arena.Ruleset.Rules.HasFlag(ArenaRuleset.ArenaRule.GuildRules))
                            {
                                if (arena.GuildRulesBroadcast.HasElapsed)
                                {
                                    if (arena.ArenaPlayers.GetTeamPlayerCount(Team.Chaos) > 0 || !arena.ArenaTeams.Chaos.Shrine.IsDead)
                                    {
                                        Network.SendTo(arena, GamePacket.Outgoing.System.DirectTextMessage(null, String.Format("[Guild Match] Chaos: {0:0.00}", arena.ArenaTeams.Chaos.Shrine.GuildPoints)), Network.SendToType.Arena);
                                    }

                                    if (arena.ArenaPlayers.GetTeamPlayerCount(Team.Order) > 0 || !arena.ArenaTeams.Order.Shrine.IsDead)
                                    {
                                        Network.SendTo(arena, GamePacket.Outgoing.System.DirectTextMessage(null, String.Format("[Guild Match] Order: {0:0.00}", arena.ArenaTeams.Order.Shrine.GuildPoints)), Network.SendToType.Arena);
                                    }

                                    if (arena.ArenaPlayers.GetTeamPlayerCount(Team.Balance) > 0 || !arena.ArenaTeams.Balance.Shrine.IsDead)
                                    {
                                        Network.SendTo(arena, GamePacket.Outgoing.System.DirectTextMessage(null, String.Format("[Guild Match] Balance: {0:0.00}", arena.ArenaTeams.Balance.Shrine.GuildPoints)), Network.SendToType.Arena);
                                    }

                                    Team guildWinTeam = Team.Neutral;

                                    if (winningTeam == Team.Neutral)
                                    {
                                        if (arena.ArenaTeams.Order.Shrine.GuildPoints > arena.ArenaTeams.Chaos.Shrine.GuildPoints && arena.ArenaTeams.Order.Shrine.GuildPoints > arena.ArenaTeams.Balance.Shrine.GuildPoints)
                                        {
                                            guildWinTeam = Team.Order;
                                        }

                                        if (arena.ArenaTeams.Balance.Shrine.GuildPoints > arena.ArenaTeams.Chaos.Shrine.GuildPoints && arena.ArenaTeams.Balance.Shrine.GuildPoints > arena.ArenaTeams.Order.Shrine.GuildPoints)
                                        {
                                            guildWinTeam = Team.Balance;
                                        }

                                        if (arena.ArenaTeams.Chaos.Shrine.GuildPoints > arena.ArenaTeams.Order.Shrine.GuildPoints && arena.ArenaTeams.Chaos.Shrine.GuildPoints > arena.ArenaTeams.Balance.Shrine.GuildPoints)
                                        {
                                            guildWinTeam = Team.Chaos;

                                        }
                                    }
                                    else
                                    {
                                        guildWinTeam = winningTeam;
                                    }

                                    Network.SendTo(arena, GamePacket.Outgoing.System.DirectTextMessage(null, String.Format("[Guild Match] Winning Team: {0}", (guildWinTeam == Team.Neutral) ? "None" : guildWinTeam.ToString())), Network.SendToType.Arena);
                                }

                                if (arena.Duration.RemainingSeconds < 600 && arena.GuildRulesBroadcast.Duration == 600000)
                                {
                                    arena.GuildRulesBroadcast = new Interval(120000, true);
                                }

                                if (arena.CountdownTick != null && arena.CountdownTick.ElapsedSeconds >= 9)
                                {
                                    Single pointsGiven = 1f;

                                    if (arena.CountdownTick.ElapsedSeconds >= 10)
                                    {
                                        pointsGiven += 0.33f * (arena.CountdownTick.ElapsedSeconds - 10);
                                    }

                                    switch (winningTeam)
                                    {
                                        case Team.Chaos:
                                        {
                                            arena.ArenaTeams.Chaos.Shrine.GuildPoints += pointsGiven;
                                            break;
                                        }
                                        case Team.Balance:
                                        {
                                            arena.ArenaTeams.Balance.Shrine.GuildPoints += pointsGiven;
                                            break;
                                        }
                                        case Team.Order:
                                        {
                                            arena.ArenaTeams.Order.Shrine.GuildPoints += pointsGiven;
                                            break;
                                        }
                                        case Team.Neutral:
                                        {
                                            break;
                                        }
                                    }
                                }

                            }

                            if (arena.CountdownTick != null && arena.CountdownTick.HasElapsed)
                            {
                                arena.EndState = arena.CurrentState;
                                arena.CurrentState = Arena.State.Ended;
                                continue;
                            }

                            if ((arena.TimeLimit - arena.Duration.ElapsedSeconds) <= 60 && arena.CurrentState == Arena.State.Normal)
                            {
                                arena.CurrentState = Arena.State.OneMinute;
                            }

                            if (arena.Duration.HasElapsed)
                            {
                                arena.CurrentState = Arena.State.Ended;
                                continue;
                            }

                            if (StatusTick.HasElapsed)
                            {
								if (arena.ArenaPlayers.Count > 0) arena.IdleDuration.Reset();
								if (arena.IdleDuration.HasElapsed) arena.CurrentState = Arena.State.Ended;

                                resetStatusUpdate = true;

                                for (Int32 j = 0; j < arena.ArenaPlayers.Count; j++)
                                {
                                    ArenaPlayer arenaPlayer = arena.ArenaPlayers[j];
                                    if (arenaPlayer == null) continue;

                                    Network.Send(arenaPlayer.WorldPlayer, GamePacket.Outgoing.World.ArenaState(arena, arenaPlayer.WorldPlayer));
                                    Network.SendTo(arena, GamePacket.Outgoing.Arena.PlayerState(arenaPlayer), Network.SendToType.Arena);
                                }
                            }
                        }
                    }
                }

                if (resetStatusUpdate)
                {
                    resetStatusUpdate = false;
                    StatusTick.Reset();
                }

                Thread.Sleep(100);
            }
        }
    }
}