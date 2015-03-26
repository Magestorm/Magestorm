using Helper;
using Helper.Timing;

namespace MageServer
{
    public enum EffectType
    {
        Default,
        Death,
        Caster,
        Target,
        Area,
        AuraCaster,
        AuraTarget,
    }

    public class Effect
    {
        public readonly ArenaPlayer Owner;
        public readonly Interval Duration;
        public readonly Spell OwnerSpell;
        public readonly Spell EffectSpell;

        public Effect(Spell spell, ArenaPlayer caster, EffectType effectType)
        {
            Owner = caster;

            switch (spell.Type)
            {
                case SpellType.Projectile:
                {
                    switch (effectType)
                    {
                        case EffectType.Death:
                        {
                            OwnerSpell = spell;
                            EffectSpell = SpellManager.Spells[spell.DeathSpellEffect];
                            break;
                        }
                        case EffectType.Area:
                        {
                            Spell areaSpell = SpellManager.Spells[spell.AreaEffectSpell];
                            OwnerSpell = areaSpell;
                            EffectSpell = SpellManager.Spells[areaSpell.TargetSpellEffect];
                            break;
                        }
                    }
                    break;
                }
                case SpellType.Rune:
                {
                    switch (effectType)
                    {
                        case EffectType.Death:
                        {
                            OwnerSpell = spell;
                            EffectSpell = SpellManager.Spells[spell.DeathSpellEffect];
                            break;
                        }
                        case EffectType.AuraCaster:
                        {
                            Spell auraCasterSpell = SpellManager.Spells[spell.AuraCasterEffect];
                            OwnerSpell = auraCasterSpell;
                            EffectSpell = SpellManager.Spells[auraCasterSpell.TargetSpellEffect];
                            break;
                        }
                        case EffectType.AuraTarget:
                        {
                            Spell auraTargetSpell = SpellManager.Spells[spell.AuraCasterEffect];
                            OwnerSpell = auraTargetSpell;
                            EffectSpell = SpellManager.Spells[auraTargetSpell.TargetSpellEffect];
                            break;
                        }
                    }

                    break;
                }
                case SpellType.Target:
                {
                    OwnerSpell = spell;

                    switch (effectType)
                    {
                        case EffectType.Caster:
                        {
                            EffectSpell = SpellManager.Spells[spell.CasterSpellEffect];
                            break;
                        }
                        case EffectType.Target:
                        {
                            EffectSpell = SpellManager.Spells[spell.TargetSpellEffect];
                            break;
                        }
                    }
                    break;
                }
                default:
                {
                    OwnerSpell = spell;
                    EffectSpell = spell;
                    break;
                }
            }
           
            if (EffectSpell != null)
            {
                Duration = EffectSpell.Effect == SpellEffectType.Bleed ? new Interval(1000, EffectSpell.Duration / 1000) : new Interval(EffectSpell.Duration, false);
            }
            else
            {
                Duration = new Interval(1, false);
            }
        }
    }
}
