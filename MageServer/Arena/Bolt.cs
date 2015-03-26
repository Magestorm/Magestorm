using System;

namespace MageServer
{
    public class Bolt
    {
        public Single Distance;
        public ArenaPlayer Owner;
        public Spell Spell;
        public ArenaPlayer Target;
        public Single Velocity;

        public Bolt(ArenaPlayer owner, ArenaPlayer target, Spell spell, Single distance)
        {
            Owner = owner;
            Target = target;
            Spell = spell;
            Distance = distance;
            Velocity = spell.Range / ((Single)spell.MaxTimer / 1000);
        }
    }
}
