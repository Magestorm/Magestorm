using System;
using Helper;
using Helper.Timing;
using SharpDX;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace MageServer
{
    public class Wall
    {
        public OrientedBoundingBox BoundingBox;
        public Int16 CurrentHp;
        public Single Direction;
        public Interval Duration;
        public Interval WeakenedDuration;
        public Vector3 Location;
        public Int16 ObjectId;
        public ArenaPlayer Owner;
        public Spell Spell;
        public Team Team;

        public Byte[] RawData { get; private set; }

        public Wall(Int16 objectId, ArenaPlayer owner, Spell spell, Vector3 location, Single direction, Byte[] rawData)
        {
            RawData = rawData;
            RawData[13] = 0;

            ObjectId = objectId;
            Owner = owner;
            Team = Owner.ActiveTeam;
            Spell = spell;
            CurrentHp = spell.HitPoints;
            Location = new Vector3(location.X, location.Y, location.Z);

            Location.X += (Single)(-(spell.CastDistance + (spell.Thick * 0.5f)) * Math.Sin(direction)) - (spell.Length * 0.5f);
            Location.Y += (Single)((spell.CastDistance + (spell.Thick * 0.5f)) * Math.Cos(direction)) - (spell.Thick * 0.5f);

            Direction = direction;

            BoundingBox = new OrientedBoundingBox(Location, new Vector3(spell.Length, spell.Thick, spell.MaxWallHeight), direction);

            Duration = new Interval(Spell.DurationTimer, false);
            WeakenedDuration = null;
        } 
    }
}
