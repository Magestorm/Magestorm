using System;
using Helper;

namespace MageServer
{
    public class SpellDamage
    {
        private Int16 _healing;
        private Int16 _damage;
        private Int16 _power;

        public Spell Spell { get; private set; }
        public Int16 Healing
        {
            get { return _healing; }
            set
            {
                if (value > 255) value = 255;
                if (value < 0) value = 0;
                _healing = value;
            }

        }
        public Int16 Damage
        {
            get { return _damage; }
            set
            {
                if (value > 255) value = 255;
                if (value < 0) value = 0;
                _damage = value;
            }

        }
        public Int16 Power
        {
            get { return _power; }
            set
            {
                if (value > 255) value = 255;
                if (value < 0) value = 0;
                _power = value;
            }

        }

        public SpellDamage(Spell spell, Int16 damage, Int16 healing, Int16 power)
        {
            Spell = spell;
            Damage = damage;
            Healing = healing;
            Power = power;
        }

        public SpellDamage(Spell spell)
        {
            Spell = spell;

            Spell tSpell;

            switch (spell.Type)
            {
                case SpellType.Dispell:
                {
                    Damage = Spell.Level;
                    break;
                }
                case SpellType.Special:
                case SpellType.Wall:
                {
                    switch (Spell.Friendly)
                    {
                        case SpellFriendlyType.NonFriendly:
                        {
                            Damage = CryptoRandom.GetInt16(spell.MinDamage, spell.MaxDamage);
                            Power = CryptoRandom.GetInt16(spell.MinPowerDrain, spell.MaxPowerDrain);
                            break;
                        }

                        case SpellFriendlyType.Friendly:
                        {
                            Healing = CryptoRandom.GetInt16(spell.MinDamage, spell.MaxDamage);
                            break;
                        }
                    }
                    break;
                }
                case SpellType.Effect:
                case SpellType.Target:
                {
                    switch (spell.Type)
                    {
                        case SpellType.Target:
                        {
                            tSpell = SpellManager.Spells[spell.TargetSpellEffect] ?? spell;

                            Damage += spell.DamageBase;

                            if (spell.DamageNumDice > 0)
                            {
                                for (Int32 i = 0; i < spell.DamageNumDice; i++)
                                {
                                    Damage += CryptoRandom.GetInt16(1, spell.DamageDice);
                                }
                            }

                            Power = CryptoRandom.GetInt16(spell.MinPowerDrain, spell.MaxPowerDrain);

                            break;
                        }
                        default:
                        {
                            tSpell = spell;
                            break;
                        }
                    }

                    switch (tSpell.Effect)
                    {
                        case SpellEffectType.Healing:
                        {
                            Healing = tSpell.Level != 255 ? CryptoRandom.GetInt16(Convert.ToInt16(Math.Floor(tSpell.Level * 0.80)), Convert.ToInt16(Math.Floor(tSpell.Level * 1.00))) : tSpell.Level;

                            break;
                        }
                        case SpellEffectType.Bleed:
                        {
                            Damage = tSpell.Level;
                            break;
                        }
                    }

                    break;
                }
                case SpellType.Rune:
                {
                    tSpell = SpellManager.Spells[spell.DeathSpellEffect] ?? spell;


                    if (tSpell.Effect == SpellEffectType.Bleed)
                    {
                        Damage = tSpell.Level;
                    }

                    Damage += spell.DamageBase;

                    if (spell.DamageNumDice > 0)
                    {
                        for (Int32 i = 0; i < spell.DamageNumDice; i++)
                        {
                            Damage += CryptoRandom.GetInt16(1, spell.DamageDice);
                        }
                    }

                    Power = CryptoRandom.GetInt16(spell.MinPowerDrain, spell.MaxPowerDrain);

                    break;
                }
                case SpellType.Bolt:
                case SpellType.Projectile:
                {
                    Damage += spell.DamageBase;

                    if (spell.DamageNumDice > 0)
                    {
                        for (Int32 i = 0; i < spell.DamageNumDice; i++)
                        {
                            Damage += CryptoRandom.GetInt16(1, spell.DamageDice);
                        }
                    }

                    Power = CryptoRandom.GetInt16(spell.MinPowerDrain, spell.MaxPowerDrain);

                    break;
                }
                default:
                {
                    return;
                }
            }
        }
    }
}