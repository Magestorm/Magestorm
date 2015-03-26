using System;
using Helper;
using Helper.Math;
using Helper.Timing;
using SharpDX;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace MageServer
{
    public class Projectile
    {
        public Single Angle;
        public readonly Single OriginalAngle;
        public OrientedBoundingBox BoundingBox;
        public Single Direction;
        public Single GravityStepDelta;
        public Single GravityStepCount;
        public Vector3 OriginalOrigin;
        public Vector3 Size;
        public Vector3 Location;
        public ArenaPlayer Owner;
        public Spell Spell;
        public Team Team;
        public Single Velocity;
        public TickCounter DistanceTicks;
        public Interval Duration;

        public Projectile(Vector3 location, Spell spell, Single direction, Single angle, ArenaPlayer owner)
        {
            Spell = spell;
            Location = location;
            Direction = direction;
            Velocity = spell.Velocity;
            Angle = angle;
            Size = new Vector3(spell.Width * 0.5f, spell.Width, spell.Tall);

            Single zRadians = MathHelper.DegreesToRadians(Angle);
            Single cosZRadians = (Single)Math.Cos(zRadians);

            Location.X += -(((Single)Math.Sin(Direction) + (Size.X * 0.5f)) * cosZRadians);
            Location.Y += (Single)Math.Sin(Direction) * cosZRadians;

            if (zRadians < 0)
            {
                Location.Z += Math.Abs(zRadians * 4f) + 4f;
            }

            Owner = owner;
            Team = owner.ActiveTeam;
            GravityStepDelta = 0;
            GravityStepCount = 0;
            DistanceTicks = new TickCounter(30, 50);
            Duration = new Interval(Spell.DurationTimer, false);
            BoundingBox = new OrientedBoundingBox(Location, Size, direction);

            OriginalOrigin = BoundingBox.Origin;
            OriginalAngle = Angle;
        }
    }
}
