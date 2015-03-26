using System;
using System.Linq;
using Helper;
using Helper.Timing;
using SharpDX;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace MageServer
{
    public class Sign
    {
        public OrientedBoundingBox BoundingBox;

        public BoundingSphere AuraBoundingSphere;
        public BoundingSphere AuraEffectSphere;

        public Interval AuraPulse;
        public Int16 AuraHealth;

        public Single Direction;
        public Interval Duration;
        public Vector3 Location;
        public Int16 ObjectId;
        public ArenaPlayer Owner;
        public Spell Spell;
        public Team Team;
        public Single OwnerDistance;

        public Byte[] RawData { get; private set; }
        public Boolean IsCTFOrb { get; private set; }

        public Boolean IsAura
        {
            get
            {
                if (Spell == null) return false;
                return Spell.AuraCasterEffect > 0 || Spell.AuraTargetEffect > 0;
            }
        }

        public Sign(Int16 objectId, ArenaPlayer owner, Spell spell, Vector3 location, Single direction, Byte[] rawData)
        {
            if (spell == SpellManager.CTFOrbSpell)
            {
                IsCTFOrb = true;
            }

            RawData = rawData;
            RawData[13] = 0;

            ObjectId = objectId;
            Owner = owner;
            Team = spell.NoTeam ? Team.None : owner.ActiveTeam;
            Spell = spell;
            Location = new Vector3(location.X, location.Y, location.Z);
            Direction = direction;
            Location.X += (Single)(-spell.CastDistance * Math.Sin(Direction)) - (spell.Width * 0.5f);
            Location.Y += (Single)(spell.CastDistance * Math.Cos(Direction)) - (spell.Width * 0.5f);
            Location.Z = (Location.Z + spell.Elevation) - (spell.Tall * 0.25f);
            BoundingBox = new OrientedBoundingBox(Location, new Vector3(spell.Width, spell.Width, spell.Tall), Direction);
            
            Duration = new Interval(Spell.DurationTimer, false);

            OwnerDistance = Vector3.Distance(BoundingBox.Origin, Owner.BoundingBox.Origin);

            if (IsAura)
            {
                AuraBoundingSphere = new BoundingSphere(BoundingBox.Origin, (Spell.EffectRadius * 2) + (owner.BoundingBox.ExtentSphere.Radius * 3));
                AuraEffectSphere = new BoundingSphere(BoundingBox.Origin, Spell.EffectRadius);
                
                AuraPulse = new Interval(Spell.AuraPulseTimer, true);
                AuraHealth = Spell.AuraHealth;
            }
        }

        public Boolean IsInWall(Grid grid)
        {
            if (grid == null) return true;

            GridBlock block = grid.GridBlocks.GetBlockByLocation(BoundingBox.Origin.X, BoundingBox.Origin.Y);

            if (block == null) return false;
            if (block.LowBox.PointInBox(BoundingBox.Origin) || block.MidBox.PointInBox(BoundingBox.Origin) || block.HighBox.PointInBox(BoundingBox.Origin))
            {
                return true;
            }

            if (block.LowBoxTile == null) return false;
            if (block.LowBoxTile.TileBlocks.Where(tileBlock => tileBlock.BottomHeight > 0).Any(tileBlock => tileBlock.BottomBoundingBox.PointInBox(BoundingBox.Origin)))
            {
                return true;
            }

            return false;
        }
    }
}
