using System;
using Helper;

namespace MageServer
{
    public enum CTFOrbState
    {
        InHomeShrine,
        OnEnemyPlayer,
        OnAnotherPlayer,
        OnGround,
    }

    public class CTFOrb
    {
        private readonly Object _syncRoot = new Object();
        
        public Object SyncRoot
        {
            get { return _syncRoot; }
        }

        private readonly Team _team;
        private readonly Int16 _objectId ;
        private CTFOrbState _orbState;
        private ArenaPlayer _orbPlayer;
        private Sign _orbSign;
        
        public CTFOrbState OrbState
        {
            get
            {
                return _orbState;
            }
        }

        public ArenaPlayer OrbPlayer
        {
            get
            {
                return _orbPlayer;
            }
        }

        public Sign OrbSign
        {
            get
            {
                return _orbSign;
            }
        }

        public Int16 ObjectId
        {
            get
            {
                return _objectId;
            }
        }

        public CTFOrb(Team team, Int16 objectId)
        {
            _orbState = CTFOrbState.InHomeShrine;
            _orbPlayer = null;
            _orbSign = null;
            _team = team;
            _objectId = objectId;
        }

        public CTFOrbState ChangeState(ArenaPlayer arenaPlayer)
        {
            lock (SyncRoot)
            {
                switch (OrbState)
                {
                    case CTFOrbState.InHomeShrine:
                    {
                        if (arenaPlayer.ActiveTeam != _team && arenaPlayer.ActiveTeam != Team.Neutral)
                        {
                            _orbSign = null;
                            _orbPlayer = arenaPlayer;
                            _orbState = CTFOrbState.OnEnemyPlayer;
                        }

                        break;
                    }
                    case CTFOrbState.OnEnemyPlayer:
                    {
                        if (arenaPlayer.ActiveTeam != _team)
                        {
                            ResetOrb();
                        }

                        break;
                    }
                    case CTFOrbState.OnGround:
                    {
                        if (arenaPlayer.ActiveTeam != _team && arenaPlayer.ActiveTeam != Team.Neutral)
                        {
                            _orbSign = null;
                            _orbPlayer = arenaPlayer;
                            _orbState = CTFOrbState.OnEnemyPlayer;
                        }
                        else
                        {
                            ResetOrb();
                        }
                        break;
                    }
                }
            }
            return OrbState;
        }

        public CTFOrbState ChangeState(Sign sign)
        {
            lock (SyncRoot)
            {
                switch (OrbState)
                {
                    case CTFOrbState.OnEnemyPlayer:
                    {
                        _orbPlayer = null;
                        _orbSign = sign;
                        _orbState = CTFOrbState.OnGround;
                        break;
                    }
                }
            }
            return OrbState;
        }

        public void ResetOrb()
        {
            lock (SyncRoot)
            {
                _orbPlayer = null;
                _orbSign = null;
                _orbState = CTFOrbState.InHomeShrine;
            }
        }
    }
}
