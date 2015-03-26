using System;
using Helper;

namespace MageServer
{
    public enum SpellEffectType
    {
        None,
        Presence,
        Light,
        Bless,
        Resist,
        Bleed,
        Prayer,
        Leaping,
        Levitate,
        Empty1,
        Fly,
        Hinder,
        Empty2,
        Empty3,
        Resurrect,
        Healing,
        Speed,
        HealingReduction,
        Empty5,
        TargetResist,
        Expulse,
    }
    public enum SpellElementType
    {
        None,
        Fire,
        Cold,
        Light,
        Void,
        Holy,
        Earth,
        Nature,
        Air,
        Mana,
    }
    public enum SpellFriendlyType
    {
        NonFriendly,
        Friendly,
        FriendlyDead,
    }
    public enum SpellProjectileType
    {
        Tandem,
        HorizontalLine,
        VerticalLine,
        Spiral,
        DoubleRow,
    }
    public enum SpellType
    {
        None,
        Projectile,
        Wall,
        Healing,
        Effect,
        Special,
        Bolt,
        Target,
        Dispell,
        Teleport,
        Rune,
    }

    public class Spell
    {
        public ListCollection<SpellTreeLevel> SpellTreeLevels;
 
        public Int32 AlignmentTranslucent;
        public Int32 AreaEffectSpell;
        public Int32 AuraCasterEffect;
        public Int16 AuraHealth;
        public Int32 AuraPulseTimer;
        public Boolean AuraStackable;
        public Int32 AuraTargetEffect;
        public Int32 BoltDeathEffect;
        public Int32 BoltDeathEffectChance;
        public Int32 BoltDeathEffectRange;
        public Int32 Bounce;
        public Int32 CastAngle;
        public Int32 CastDistance;
        public Int32 CastSound;
        public Int32 CastSound2;
        public Int32 CastSound3;
        public Int32 CastSound4;
        public Int32 CasterSpellEffect;
        public Int32 CenterDeathImage;
        public Int32 CollisionVelocity;
        public Int32 CreationEffect;
        public Int16 DamageBase;
        public Boolean DamageByDistanceTraveled;
        public Int16 DamageDice;
        public Int16 DamageNumDice;
        public Int32 DeathEffect;
        public Int32 DeathEffectChance;
        public Int32 DeathEffectRange;
        public Int32 DeathFrameStart;
        public Int32 DeathImageNum;
        public Int32 DeathImageNumChance;
        public Int32 DeathImageTimerMax;
        public Int32 DeathSound;
        public Int32 DeathSoundRange;
        public Int32 DeathSpellEffect;
        public Int32 DeathTransColor;
        public Int32 DeathTranslucent;
        public Int32 DispellType;
        public Int32 Duration;
        public Int32 DurationTimer;
        public Int32 DurationType;
        public SpellEffectType Effect;
        public Int32 EffectImageNum;
        public Int32 EffectImageTimerMax;
        public Int32 EffectRadius;
        public Int32 EffectSound;
        public Int32 EffectSoundRange;
        public Int32 EffectTranslucent;
        public SpellElementType Element;
        public Int32 Elevation;
        public Int32 EmptySound;
        public Boolean Ethereal;
        public Int32 Fatigue;
        public Int32 FireTimer;
        public SpellFriendlyType Friendly;
        public Boolean Gravity;
        public Int32 Group;
        public Int16 HitPoints;
        public Int32 HorizontalSpread;
        public Int32 HugFloor;
        public Int16 Id;
        public Int32 ImageNum;
        public Int32 ImageTimer;
        public Int32 ImageTimerMax;
        public Int32 LeadingEdgeImageNum;
        public Int32 Length;
        public Int16 Level;
        public Int32 LightGlow;
        public Int32 LightPattern;
        public Int32 Max;
        public Int16 MaxDamage;
        public Int32 MaxFlicker;
        public Int16 MaxPowerDrain;
        public Int32 MaxStep;
        public Int32 MaxTimer;
        public Int32 MaxWallHeight;
        public Int32 Min;
        public Int16 MinDamage;
        public Int32 MinFatigue;
        public Int16 MinPowerDrain;
        public Int32 MissSound;
        public String Name;
        public Boolean NoTeam;
        public Int32 NumCastSounds;
        public Int32 NumObjects;
        public Int32 NumProjectiles;
        public Int32 ObjectLayout;
        public Int32 ObjectSpacing;
        public Int32 Overlay;
        public Int32 OverlayDurType;
        public Int32 OverlayDuration;
        public Int32 OverlayGlow;
        public Int32 OverlayHeight;
        public Int32 OverlayImageNum;
        public Int32 OverlayImageTImer;
        public Int32 OverlayOpaque;
        public Int32 OverlayTransColor;
        public Int32 Power;
        public Int32 ProjectileSpacing;
        public Int32 RandomDeathPosition;
        public Int16 Range;
        public SpellProjectileType SideBySide;
        public Int32 SkillUsed;
        public Int32 Sound;
        public Int32 SoundRange;
        public Int32 StickyLight;
        public Int32 SwitchSound;
        public Int32 Tall;
        public Int32 TargetSpellEffect;
        public Int32 TeleportType;
        public Int32 TextureNum;
        public Int32 Thick;
        public Int32 TransColor;
        public Int32 Translucent;
        public Int32 Transparent;
        public SpellType Type;
        public Int32 Velocity;
        public Int32 VerticalSpread;
        public Int32 WallHeight;
        public Int32 Width;
        public Int32 ZVelocity;

        public Boolean CanDamage
        {
            get { return DamageBase > 0 || (DamageNumDice > 0 && DamageDice > 0) || MaxDamage > 0 || MaxPowerDrain > 0; }
        }
    }

}
