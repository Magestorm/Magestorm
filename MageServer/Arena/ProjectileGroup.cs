using System;
using Helper;
using Helper.Math;
using SharpDX;

namespace MageServer
{
    public class ProjectileGroup
    {
		public readonly ListCollection<Projectile> Projectiles;
        public ArenaPlayer Owner;
        public Team Team;

        public ProjectileGroup(ArenaPlayer owner, Spell spell, Vector3 location, Single direction, Single angle)
        {
            Single xOffset, yOffset;
            Vector3 tLocation;

            Projectiles = new ListCollection<Projectile>();

            Owner = owner;
            Team = Owner.ActiveTeam;

            location.X += (-spell.CastDistance * (Single)Math.Sin(direction)) + 1f;
            location.Y += (spell.CastDistance * (Single)Math.Cos(direction)) - (spell.Width * 0.5f);

            switch (spell.Gravity)
            {
                case true:
                {
                    location.Z += (spell.MaxStep * 0.5f) * (MathHelper.DegreesToRadians(angle) + 1);
                    break;
                }
                case false:
                {
                    location.Z += spell.Elevation + angle;
                    break;
                }
            }

            switch (spell.SideBySide)
            {
                case SpellProjectileType.Tandem:
                {
                    for (Int32 i = 0; i < spell.NumProjectiles; i++)
                    {
                        Projectiles.Add(new Projectile(location, spell, direction, angle, owner));

                        location.X += -spell.ProjectileSpacing * (Single)Math.Sin(direction);
                        location.Y += spell.ProjectileSpacing * (Single)Math.Cos(direction);
                    }
                    break;
                }

                case SpellProjectileType.HorizontalLine:
                {
                    if (spell.NumProjectiles != 2) return;

                    xOffset = -(spell.ProjectileSpacing * 0.5f) * (Single)Math.Sin(direction + MathHelper.RightAngleRadians);
                    yOffset = (spell.ProjectileSpacing * 0.5f) * (Single)Math.Cos(direction + MathHelper.RightAngleRadians);
                    tLocation = new Vector3(location.X + xOffset, location.Y + yOffset, location.Z);
                    Projectiles.Add(new Projectile(tLocation, spell, direction, angle, owner));

                    xOffset = -(spell.ProjectileSpacing * 0.5f) * (Single)Math.Sin(direction - MathHelper.RightAngleRadians);
                    yOffset = (spell.ProjectileSpacing * 0.5f) * (Single)Math.Cos(direction - MathHelper.RightAngleRadians);
                    tLocation = new Vector3(location.X + xOffset, location.Y + yOffset, location.Z);
                    Projectiles.Add(new Projectile(tLocation, spell, direction, angle, owner));

                    break;
                }
                case SpellProjectileType.DoubleRow:
                {
                    if (spell.NumProjectiles != 3) return;

                    Single zOffset = spell.ProjectileSpacing;

                    xOffset = -(spell.ProjectileSpacing * 0.5f) * (Single)Math.Sin(direction + MathHelper.RightAngleRadians);
                    yOffset = (spell.ProjectileSpacing * 0.5f) * (Single)Math.Cos(direction + MathHelper.RightAngleRadians);
                    tLocation = new Vector3(location.X + xOffset, location.Y + yOffset, location.Z - zOffset);
                    Projectiles.Add(new Projectile(tLocation, spell, direction, angle, owner));

                    xOffset = -(spell.ProjectileSpacing * 0.5f) * (Single)Math.Sin(direction - MathHelper.RightAngleRadians);
                    yOffset = (spell.ProjectileSpacing * 0.5f) * (Single)Math.Cos(direction - MathHelper.RightAngleRadians);
                    tLocation = new Vector3(location.X + xOffset, location.Y + yOffset, location.Z - zOffset);
                    Projectiles.Add(new Projectile(tLocation, spell, direction, angle, owner));

                    tLocation = new Vector3(location.X, location.Y, location.Z);
                    Projectiles.Add(new Projectile(tLocation, spell, direction, angle, owner));

                    break;
                }
            }
        }
    }
}
