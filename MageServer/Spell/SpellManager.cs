using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Helper;

namespace MageServer
{
    public struct SpellCheatInfo
    {
        public readonly String Error;
        public readonly Spell Spell;
		public readonly Int32 ListLevel;
        public readonly Int32 SpellLevel;
        public readonly String ListName;
        public readonly Boolean HasSpell;

        public SpellCheatInfo(Boolean hasSpell)
        {
            Error = "";
            HasSpell = hasSpell;
            Spell = null;
            ListName = "";
	        ListLevel = 0;
            SpellLevel = 0;
        }

        public SpellCheatInfo(Spell spell, Boolean hasSpell, Int32 listLevel, Int32 spellLevel, String listName, String error)
        {
            HasSpell = hasSpell;
            Spell = spell;
            ListName = listName;
	        ListLevel = listLevel;
            SpellLevel = spellLevel;
            Error = error;
        }
    }


    public static class SpellManager
    {
        // ReSharper disable InconsistentNaming
        private const String ALIGNMENT_TRANSLUCENT = "alignment_translucent";
        private const String AREA_EFFECT_SPELL = "area_effect_spell";
        private const String AURA_CASTER_EFFECT = "aura_caster_effect";
        private const String AURA_HEALTH = "aura_health";
        private const String AURA_PULSE_TIMER = "aura_pulse_timer";
        private const String AURA_STACKABLE = "aura_stackable";
        private const String AURA_TARGET_EFFECT = "aura_target_effect";
        private const String BOLT_DEATH_EFFECT = "bolt_death_effect";
        private const String BOLT_DEATH_EFFECT_CHANCE = "bolt_death_effect_chance";
        private const String BOLT_DEATH_EFFECT_RANGE = "bolt_death_effect_range";
        private const String BOUNCE = "bounce";
        private const String CASTER_SPELL_EFFECT = "caster_spell_effect";
        private const String CAST_ANGLE = "cast_angle";
        private const String CAST_DISTANCE = "cast_distance";
        private const String CAST_SOUND = "cast_sound";
        private const String CAST_SOUND_2 = "cast_sound_2";
        private const String CAST_SOUND_3 = "cast_sound_3";
        private const String CAST_SOUND_4 = "cast_sound_4";
        private const String CENTER_DEATH_IMAGE = "center_death_image";
        private const String COLLISION_VELOCITY = "collision_velocity";
        private const String CREATION_EFFECT = "creation_effect";
        private const String DAMAGE_BASE = "damage_base";
        private const String DAMAGE_BY_DISTANCE_TRAVELED = "damage_by_distance_traveled";
        private const String DAMAGE_DICE = "damage_dice";
        private const String DAMAGE_NUM_DICE = "damage_num_dice";
        private const String DEATH_EFFECT = "death_effect";
        private const String DEATH_EFFECT_CHANCE = "death_effect_chance";
        private const String DEATH_EFFECT_RANGE = "death_effect_range";
        private const String DEATH_FRAME_START = "death_frame_start";
        private const String DEATH_IMAGENUM = "death_imagenum";
        private const String DEATH_IMAGENUM_CHANCE = "death_imagenum_chance";
        private const String DEATH_IMAGE_TIMER_MAX = "death_image_timer_max";
        private const String DEATH_SOUND = "death_sound";
        private const String DEATH_SOUND_RANGE = "death_sound_range";
        private const String DEATH_SPELL_EFFECT = "death_spell_effect";
        private const String DEATH_TRANSLUCENT = "death_translucent";
        private const String DEATH_TRANS_COLOR = "death_trans_color";
        private const String DISPELL_TYPE = "dispell_type";
        private const String DURATION = "duration";
        private const String DURATION_TIMER = "duration_timer";
        private const String DURATION_TYPE = "duration_type";
        private const String EFFECT = "effect";
        private const String EFFECT_IMAGENUM = "effect_imagenum";
        private const String EFFECT_IMAGE_TIMER_MAX = "effect_image_timer_max";
        private const String EFFECT_RADIUS = "effect_radius";
        private const String EFFECT_SOUND = "effect_sound";
        private const String EFFECT_SOUND_RANGE = "effect_sound_range";
        private const String EFFECT_TRANSLUCENT = "effect_translucent";
        private const String ELEMENT = "element";
        private const String ELEVATION = "elevation";
        private const String EMPTY_SOUND = "empty_sound";
        private const String ETHEREAL = "ethereal";
        private const String FATIGUE = "fatigue";
        private const String FIRE_TIMER = "fire_timer";
        private const String FRIENDLY = "friendly";
        private const String GRAVITY = "gravity";
        private const String GROUP = "group";
        private const String HIT_POINTS = "hit_points";
        private const String HORIZONTAL_SPREAD = "horizontal_spread";
        private const String HUG_FLOOR = "hug_floor";
        private const String IMAGENUM = "imagenum";
        private const String IMAGE_TIMER = "image_timer";
        private const String IMAGE_TIMER_MAX = "image_timer_max";
        private const String LEADING_EDGE_IMAGENUM = "leading_edge_imagenum";
        private const String LENGTH = "length";
        private const String LEVEL = "level";
        private const String LIGHT_GLOW = "light_glow";
        private const String LIGHT_PATTERN = "light_pattern";
        private const String MAX = "max";
        private const String MAX_DAMAGE = "max_damage";
        private const String MAX_TIMER = "max_timer";
        private const String MAX_FLICKER = "max_flicker";
        private const String MAX_POWER_DRAIN = "max_power_drain";
        private const String MAX_STEP = "max_step";
        private const String MAX_WALLHEIGHT = "max_wallheight";
        private const String MIN = "min";
        private const String MIN_DAMAGE = "min_damage";
        private const String MIN_FATIGUE = "min_fatigue";
        private const String MIN_POWER_DRAIN = "min_power_drain";
        private const String MISS_SOUND = "miss_sound";
        private const String NAME = "name";
        private const String NO_TEAM = "no_team";
        private const String NUM_CAST_SOUNDS = "num_cast_sounds";
        private const String NUM_OBJECTS = "num_Objects";
        private const String NUM_PROJECTILES = "num_projectiles";
        private const String OBJECT_LAYOUT = "Object_layout";
        private const String OBJECT_SPACING = "Object_spacing";
        private const String OVERLAY = "overlay";
        private const String OVERLAY_DURATION = "overlay_duration";
        private const String OVERLAY_DUR_TYPE = "overlay_dur_type";
        private const String OVERLAY_GLOW = "overlay_glow";
        private const String OVERLAY_HEIGHT = "overlay_height";
        private const String OVERLAY_IMAGE_TIMER = "overlay_image_timer";
        private const String OVERLAY_IMAGENUM = "overlay_imagenum";
        private const String OVERLAY_OPAQUE = "overlay_opaque";
        private const String OVERLAY_TRANS_COLOR = "overlay_trans_color";
        private const String POWER = "power";
        private const String PROJECTILE_SPACING = "projectile_spacing";
        private const String RANDOM_DEATH_POSITION = "random_death_position";
        private const String RANGE = "range";
        private const String SIDE_BY_SIDE = "side_by_side";
        private const String SKILL_USED = "skill_used";
        private const String SOUND = "sound";
        private const String SOUND_RANGE = "sound_range";
        private const String STICKY_LIGHT = "sticky_light";
        private const String SWITCH_SOUND = "switch_sound";
        private const String TALL = "tall";
        private const String TARGET_SPELL_EFFECT = "target_spell_effect";
        private const String TELEPORT_TYPE = "teleport_type";
        private const String TEXTURENUM = "texturenum";
        private const String THICK = "thick";
        private const String TRANSPARENT = "transparent";
        private const String TRANSLUCENT = "translucent";
        private const String TRANS_COLOR = "trans_color";
        private const String TYPE = "type";
        private const String VELOCITY = "velocity";
        private const String VERTICAL_SPREAD = "vertical_spread";
        private const String WALLHEIGHT = "wallheight";
        private const String WIDTH = "width";
        private const String Z_VELOCITY = "z_velocity";
        // ReSharper restore InconsistentNaming

        public static readonly SpellCollection Spells;
        public static readonly SpellTreeCollection SpellTrees;

        public static Spell CTFOrbSpell;

        static SpellManager()
        {
            Spells = new SpellCollection();
            SpellTrees = new SpellTreeCollection();
        }

        public static void LoadSpells()
        {
            String fileName = String.Format("{0}\\Spells.dat", Directory.GetCurrentDirectory());
            Int32 spellCount = NativeMethods.GetPrivateProfileInt32("spelldefs", "numspells", fileName);

            Program.ServerForm.MainLog.WriteMessage("Spell Lists loaded.", Color.Blue);

            Program.ServerForm.MainLog.WriteMessage(String.Format("Loading {0} Spells...", spellCount), Color.Blue);

            for (Int16 i = 1; i <= spellCount; i++)
            {
                Spell spell = LoadSpellFromDat(i, fileName);

                if (spell == null)
                {
                    Program.ServerForm.MainLog.WriteMessage(String.Format("Error loading Spell #{0}", i), Color.Red);
                    Spells.Add(new Spell());
                    continue;
                }

                Spells.Add(spell);

                Program.ServerForm.MainLog.WriteMessage(String.Format("Loaded Spell: {0}", spell.Name), Color.Green);

                Application.DoEvents();
            }

            Program.ServerForm.MainLog.WriteMessage(String.Format("{0} Spells loaded.", spellCount), Color.Blue);

            Program.ServerForm.MainLog.WriteMessage("Loading Spell Lists...", Color.Blue);

            for (Int32 i = 0; i <= 9; i++)
            {
                SpellTree spellTree = LoadSpellTree(Character.PlayerClass.Magician, i, fileName);

                if (spellTree != null)
                {
                    SpellTrees.Add(spellTree);
                }

                spellTree = LoadSpellTree(Character.PlayerClass.Arcanist, i, fileName);

                if (spellTree != null)
                {
                    SpellTrees.Add(spellTree);
                }

                spellTree = LoadSpellTree(Character.PlayerClass.Mentalist, i, fileName);

                if (spellTree != null)
                {
                    SpellTrees.Add(spellTree);
                }

                spellTree = LoadSpellTree(Character.PlayerClass.Cleric, i, fileName);

                if (spellTree != null)
                {
                    SpellTrees.Add(spellTree);
                }
            }

            Program.ServerForm.MainLog.WriteMessage("Linking Spells with Lists...", Color.Blue);

            for (Int32 i = 1; i < Spells.Count; i++)
            {
                for (Int32 j = 0; j < SpellTrees.Count; j++)
                {
                    for (Int16 k = 0; k <= 49; k++)
                    {
                        if (SpellTrees[j].TreeSpells[k] == null) continue;

                        if (Spells[i].Id == SpellTrees[j].TreeSpells[k].Id)
                        {
                            Spells[i].SpellTreeLevels.Add(new SpellTreeLevel(k, SpellTrees[j].Id));
                        }
                    }
                }

                Application.DoEvents();
            }

            Program.ServerForm.MainLog.WriteMessage("Finished linking Spells.", Color.Blue);

            // Set Hardcoded Spells
            CTFOrbSpell = Spells[331];
        }

	    private static Spell LoadSpellFromDat(Int16 spellId, String fileName)
        {
            Spell spell = new Spell();

            String keyName = String.Format("spell{0:00}", spellId);
            String spellType = NativeMethods.GetPrivateProfileString(keyName, TYPE, fileName);

            spell.SpellTreeLevels = new ListCollection<SpellTreeLevel>();
            spell.Id = spellId;
            spell.Name = NativeMethods.GetPrivateProfileString(keyName, NAME, fileName);
            spell.Fatigue = NativeMethods.GetPrivateProfileInt32(keyName, FATIGUE, fileName);
            spell.MinFatigue = NativeMethods.GetPrivateProfileInt32(keyName, MIN_FATIGUE, fileName);
            spell.Power = NativeMethods.GetPrivateProfileInt32(keyName, POWER, fileName);
            spell.NumCastSounds = NativeMethods.GetPrivateProfileInt32(keyName, NUM_CAST_SOUNDS, fileName);
            spell.CastSound = NativeMethods.GetPrivateProfileInt32(keyName, CAST_SOUND, fileName);
            spell.CastSound2 = NativeMethods.GetPrivateProfileInt32(keyName, CAST_SOUND_2, fileName);
            spell.CastSound3 = NativeMethods.GetPrivateProfileInt32(keyName, CAST_SOUND_3, fileName);
            spell.CastSound4 = NativeMethods.GetPrivateProfileInt32(keyName, CAST_SOUND_4, fileName);
            spell.EmptySound = NativeMethods.GetPrivateProfileInt32(keyName, EMPTY_SOUND, fileName);
            spell.SwitchSound = NativeMethods.GetPrivateProfileInt32(keyName, SWITCH_SOUND, fileName);
            spell.FireTimer = NativeMethods.GetPrivateProfileInt32(keyName, FIRE_TIMER, fileName);
            spell.Overlay = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY, fileName);

            switch (spellType)
            {
                case "projectile":
                {
                    spell.Type = SpellType.Projectile;
                    spell.ImageNum = NativeMethods.GetPrivateProfileInt32(keyName, IMAGENUM, fileName);
                    spell.DeathImageNum = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGENUM, fileName);
                    spell.Width = NativeMethods.GetPrivateProfileInt32(keyName, WIDTH, fileName);
                    spell.Tall = NativeMethods.GetPrivateProfileInt32(keyName, TALL, fileName);
                    spell.ImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, IMAGE_TIMER_MAX, fileName);
                    spell.DeathImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGE_TIMER_MAX, fileName);
                    spell.Gravity = NativeMethods.GetPrivateProfileBoolean(keyName, GRAVITY, fileName);
                    spell.LightPattern = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_PATTERN, fileName);
                    spell.MaxFlicker = NativeMethods.GetPrivateProfileInt32(keyName, MAX_FLICKER, fileName);
                    spell.LightGlow = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_GLOW, fileName);
                    spell.StickyLight = NativeMethods.GetPrivateProfileInt32(keyName, STICKY_LIGHT, fileName);
                    spell.DurationTimer = NativeMethods.GetPrivateProfileInt32(keyName, DURATION_TIMER, fileName);
                    spell.TransColor = NativeMethods.GetPrivateProfileInt32(keyName, TRANS_COLOR, fileName);
                    spell.DeathTransColor = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_TRANS_COLOR, fileName);
                    spell.EffectRadius = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_RADIUS, fileName);
                    spell.MissSound = NativeMethods.GetPrivateProfileInt32(keyName, MISS_SOUND, fileName);
                    spell.MinDamage = NativeMethods.GetPrivateProfileInt16(keyName, MIN_DAMAGE, fileName);
                    spell.MaxDamage = NativeMethods.GetPrivateProfileInt16(keyName, MAX_DAMAGE, fileName);
                    spell.DamageDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_DICE, fileName);
                    spell.DamageNumDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_NUM_DICE, fileName);
                    spell.DamageBase = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_BASE, fileName);
                    spell.MinPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MIN_POWER_DRAIN, fileName);
                    spell.MaxPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MAX_POWER_DRAIN, fileName);
                    spell.Velocity = NativeMethods.GetPrivateProfileInt32(keyName, VELOCITY, fileName);
                    spell.ZVelocity = NativeMethods.GetPrivateProfileInt32(keyName, Z_VELOCITY, fileName);
                    spell.CastAngle = NativeMethods.GetPrivateProfileInt32(keyName, CAST_ANGLE, fileName);
                    spell.NumProjectiles = NativeMethods.GetPrivateProfileInt32(keyName, NUM_PROJECTILES, fileName);
                    spell.ProjectileSpacing = NativeMethods.GetPrivateProfileInt32(keyName, PROJECTILE_SPACING, fileName);
                    spell.SideBySide = (SpellProjectileType)NativeMethods.GetPrivateProfileInt32(keyName, SIDE_BY_SIDE, fileName);
                    spell.CastDistance = NativeMethods.GetPrivateProfileInt32(keyName, CAST_DISTANCE, fileName);
                    spell.Elevation = NativeMethods.GetPrivateProfileInt32(keyName, ELEVATION, fileName);
                    spell.MaxStep = NativeMethods.GetPrivateProfileInt32(keyName, MAX_STEP, fileName);
                    spell.Translucent = NativeMethods.GetPrivateProfileInt32(keyName, TRANSLUCENT, fileName);
                    spell.AlignmentTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, ALIGNMENT_TRANSLUCENT, fileName);
                    spell.DeathTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_TRANSLUCENT, fileName);
                    spell.RandomDeathPosition = NativeMethods.GetPrivateProfileInt32(keyName, RANDOM_DEATH_POSITION, fileName);
                    spell.CenterDeathImage = NativeMethods.GetPrivateProfileInt32(keyName, CENTER_DEATH_IMAGE, fileName);
                    spell.DeathEffect = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT, fileName);
                    spell.DeathEffectRange = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT_RANGE, fileName);
                    spell.DeathEffectChance = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT_CHANCE, fileName);
                    spell.DeathSpellEffect = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SPELL_EFFECT, fileName);
                    spell.CreationEffect = NativeMethods.GetPrivateProfileInt32(keyName, CREATION_EFFECT, fileName);
                    spell.HorizontalSpread = NativeMethods.GetPrivateProfileInt32(keyName, HORIZONTAL_SPREAD, fileName);
                    spell.VerticalSpread = NativeMethods.GetPrivateProfileInt32(keyName, VERTICAL_SPREAD, fileName);
                    spell.Sound = NativeMethods.GetPrivateProfileInt32(keyName, SOUND, fileName);
                    spell.SoundRange = NativeMethods.GetPrivateProfileInt32(keyName, SOUND_RANGE, fileName);
                    spell.DeathSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SOUND_RANGE, fileName);
                    spell.EffectSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND_RANGE, fileName);
                    spell.DeathSound = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SOUND, fileName);
                    spell.EffectSound = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND, fileName);
                    spell.DeathFrameStart = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_FRAME_START, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    spell.NoTeam = NativeMethods.GetPrivateProfileBoolean(keyName, NO_TEAM, fileName);
                    spell.SkillUsed = NativeMethods.GetPrivateProfileInt32(keyName, SKILL_USED, fileName);
                    spell.Bounce = NativeMethods.GetPrivateProfileInt32(keyName, BOUNCE, fileName);
                    spell.DamageByDistanceTraveled = NativeMethods.GetPrivateProfileBoolean(keyName, DAMAGE_BY_DISTANCE_TRAVELED, fileName);
                    spell.AreaEffectSpell = NativeMethods.GetPrivateProfileInt32(keyName, AREA_EFFECT_SPELL, fileName);
                    spell.Friendly = (SpellFriendlyType)NativeMethods.GetPrivateProfileInt32(keyName, FRIENDLY, fileName);
                    spell.Ethereal = NativeMethods.GetPrivateProfileBoolean(keyName, ETHEREAL, fileName);
                    break;
                }
                case "wall":
                {
                    spell.Type = SpellType.Wall;
                    spell.TextureNum = NativeMethods.GetPrivateProfileInt32(keyName, TEXTURENUM, fileName);
                    spell.Length = NativeMethods.GetPrivateProfileInt32(keyName, LENGTH, fileName);
                    spell.WallHeight = NativeMethods.GetPrivateProfileInt32(keyName, WALLHEIGHT, fileName);
                    spell.MaxWallHeight = NativeMethods.GetPrivateProfileInt32(keyName, MAX_WALLHEIGHT, fileName);
                    spell.Transparent = NativeMethods.GetPrivateProfileInt32(keyName, TRANSPARENT, fileName);
                    spell.TransColor = NativeMethods.GetPrivateProfileInt32(keyName, TRANS_COLOR, fileName);
                    spell.DurationTimer = NativeMethods.GetPrivateProfileInt32(keyName, DURATION_TIMER, fileName);
                    spell.Thick = NativeMethods.GetPrivateProfileInt32(keyName, THICK, fileName);
                    spell.CastDistance = NativeMethods.GetPrivateProfileInt32(keyName, CAST_DISTANCE, fileName);
                    spell.HugFloor = NativeMethods.GetPrivateProfileInt32(keyName, HUG_FLOOR, fileName);
                    spell.MinDamage = NativeMethods.GetPrivateProfileInt16(keyName, MIN_DAMAGE, fileName);
                    spell.MaxDamage = NativeMethods.GetPrivateProfileInt16(keyName, MAX_DAMAGE, fileName);
                    spell.MinPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MIN_POWER_DRAIN, fileName);
                    spell.MaxPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MAX_POWER_DRAIN, fileName);
                    spell.CollisionVelocity = NativeMethods.GetPrivateProfileInt32(keyName, COLLISION_VELOCITY, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    spell.HitPoints = NativeMethods.GetPrivateProfileInt16(keyName, HIT_POINTS, fileName);
                    spell.Friendly = (SpellFriendlyType)NativeMethods.GetPrivateProfileInt32(keyName, FRIENDLY, fileName);
                    break;
                }
                case "healing":
                {
                    spell.Type = SpellType.Healing;
                    spell.Min = NativeMethods.GetPrivateProfileInt32(keyName, MIN, fileName);
                    spell.Max = NativeMethods.GetPrivateProfileInt32(keyName, MAX, fileName);
                    break;
                }
                case "effect":
                {
                    spell.Type = SpellType.Effect;
                    spell.Effect = (SpellEffectType)NativeMethods.GetPrivateProfileInt32(keyName, EFFECT, fileName);
                    spell.DurationType = NativeMethods.GetPrivateProfileInt32(keyName, DURATION_TYPE, fileName);
                    spell.Duration = NativeMethods.GetPrivateProfileInt32(keyName, DURATION, fileName);
                    spell.Level = NativeMethods.GetPrivateProfileInt16(keyName, LEVEL, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    spell.ImageNum = NativeMethods.GetPrivateProfileInt32(keyName, IMAGENUM, fileName);
                    spell.ImageTimer = NativeMethods.GetPrivateProfileInt32(keyName, IMAGE_TIMER, fileName);
                    spell.OverlayImageNum = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_IMAGENUM, fileName);
                    spell.OverlayImageTImer = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_IMAGE_TIMER, fileName);
                    spell.OverlayDuration = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_DURATION, fileName);
                    spell.OverlayDurType = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_DUR_TYPE, fileName);
                    spell.OverlayHeight = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_HEIGHT, fileName);
                    spell.OverlayOpaque = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_OPAQUE, fileName);
                    spell.OverlayGlow = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_GLOW, fileName);
                    spell.OverlayTransColor = NativeMethods.GetPrivateProfileInt32(keyName, OVERLAY_TRANS_COLOR, fileName);
                    break;
                }
                case "special":
                {
                    spell.Type = SpellType.Special;
                    spell.ImageNum = NativeMethods.GetPrivateProfileInt32(keyName, IMAGENUM, fileName);
                    spell.DeathImageNum = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGENUM, fileName);
                    spell.Width = NativeMethods.GetPrivateProfileInt32(keyName, WIDTH, fileName);
                    spell.Tall = NativeMethods.GetPrivateProfileInt32(keyName, TALL, fileName);
                    spell.ImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, IMAGE_TIMER_MAX, fileName);
                    spell.DeathImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGE_TIMER_MAX, fileName);
                    spell.Gravity = NativeMethods.GetPrivateProfileBoolean(keyName, GRAVITY, fileName);
                    spell.LightPattern = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_PATTERN, fileName);
                    spell.MaxFlicker = NativeMethods.GetPrivateProfileInt32(keyName, MAX_FLICKER, fileName);
                    spell.LightGlow = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_GLOW, fileName);
                    spell.StickyLight = NativeMethods.GetPrivateProfileInt32(keyName, STICKY_LIGHT, fileName);
                    spell.DurationTimer = NativeMethods.GetPrivateProfileInt32(keyName, DURATION_TIMER, fileName);
                    spell.TransColor = NativeMethods.GetPrivateProfileInt32(keyName, TRANS_COLOR, fileName);
                    spell.MinDamage = NativeMethods.GetPrivateProfileInt16(keyName, MIN_DAMAGE, fileName);
                    spell.MaxDamage = NativeMethods.GetPrivateProfileInt16(keyName, MAX_DAMAGE, fileName);
                    spell.DamageDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_DICE, fileName);
                    spell.DamageNumDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_NUM_DICE, fileName);
                    spell.DamageBase = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_BASE, fileName);
                    spell.MinPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MIN_POWER_DRAIN, fileName);
                    spell.MaxPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MAX_POWER_DRAIN, fileName);
                    spell.CollisionVelocity = NativeMethods.GetPrivateProfileInt32(keyName, COLLISION_VELOCITY, fileName);
                    spell.NumObjects = NativeMethods.GetPrivateProfileInt32(keyName, NUM_OBJECTS, fileName);
                    spell.ObjectSpacing = NativeMethods.GetPrivateProfileInt32(keyName, OBJECT_SPACING, fileName);
                    spell.ObjectLayout = NativeMethods.GetPrivateProfileInt32(keyName, OBJECT_LAYOUT, fileName);
                    spell.CastDistance = NativeMethods.GetPrivateProfileInt32(keyName, CAST_DISTANCE, fileName);
                    spell.Elevation = NativeMethods.GetPrivateProfileInt32(keyName, ELEVATION, fileName);
                    spell.Translucent = NativeMethods.GetPrivateProfileInt32(keyName, TRANSLUCENT, fileName);
                    spell.DeathTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_TRANSLUCENT, fileName);
                    spell.Sound = NativeMethods.GetPrivateProfileInt32(keyName, SOUND, fileName);
                    spell.SoundRange = NativeMethods.GetPrivateProfileInt32(keyName, SOUND_RANGE, fileName);
                    spell.DeathSound = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SOUND, fileName);
                    spell.DeathSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SOUND_RANGE, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    break;
                }
                case "bolt":
                {
                    spell.Type = SpellType.Bolt;
                    spell.ImageNum = NativeMethods.GetPrivateProfileInt32(keyName, IMAGENUM, fileName);
                    spell.DeathImageNum = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGENUM, fileName);
                    spell.DeathImageNumChance = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGENUM_CHANCE, fileName);
                    spell.EffectImageNum = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_IMAGENUM, fileName);
                    spell.LeadingEdgeImageNum = NativeMethods.GetPrivateProfileInt32(keyName, LEADING_EDGE_IMAGENUM, fileName);
                    spell.Width = NativeMethods.GetPrivateProfileInt32(keyName, WIDTH, fileName);
                    spell.Tall = NativeMethods.GetPrivateProfileInt32(keyName, TALL, fileName);
                    spell.ImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, IMAGE_TIMER_MAX, fileName);
                    spell.DeathImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGE_TIMER_MAX, fileName);
                    spell.EffectImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_IMAGE_TIMER_MAX, fileName);
                    spell.Gravity = NativeMethods.GetPrivateProfileBoolean(keyName, GRAVITY, fileName);
                    spell.LightPattern = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_PATTERN, fileName);
                    spell.MaxFlicker = NativeMethods.GetPrivateProfileInt32(keyName, MAX_FLICKER, fileName);
                    spell.LightGlow = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_GLOW, fileName);
                    spell.StickyLight = NativeMethods.GetPrivateProfileInt32(keyName, STICKY_LIGHT, fileName);
                    spell.DurationTimer = NativeMethods.GetPrivateProfileInt32(keyName, DURATION_TIMER, fileName);
                    spell.TransColor = NativeMethods.GetPrivateProfileInt32(keyName, TRANS_COLOR, fileName);
                    spell.DeathTransColor = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_TRANS_COLOR, fileName);
                    spell.EffectRadius = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_RADIUS, fileName);
                    spell.MinDamage = NativeMethods.GetPrivateProfileInt16(keyName, MIN_DAMAGE, fileName);
                    spell.MaxDamage = NativeMethods.GetPrivateProfileInt16(keyName, MAX_DAMAGE, fileName);
                    spell.DamageDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_DICE, fileName);
                    spell.DamageNumDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_NUM_DICE, fileName);
                    spell.DamageBase = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_BASE, fileName);
                    spell.MinPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MIN_POWER_DRAIN, fileName);
                    spell.MaxPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MAX_POWER_DRAIN, fileName);
                    spell.NumObjects = NativeMethods.GetPrivateProfileInt32(keyName, NUM_OBJECTS, fileName);
                    spell.ObjectSpacing = NativeMethods.GetPrivateProfileInt32(keyName, OBJECT_SPACING, fileName);
                    spell.ObjectLayout = NativeMethods.GetPrivateProfileInt32(keyName, OBJECT_LAYOUT, fileName);
                    spell.CastDistance = NativeMethods.GetPrivateProfileInt32(keyName, CAST_DISTANCE, fileName);
                    spell.Elevation = NativeMethods.GetPrivateProfileInt32(keyName, ELEVATION, fileName);
                    spell.Translucent = NativeMethods.GetPrivateProfileInt32(keyName, TRANSLUCENT, fileName);
                    spell.DeathTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_TRANSLUCENT, fileName);
                    spell.EffectTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_TRANSLUCENT, fileName);
                    spell.Range = NativeMethods.GetPrivateProfileInt16(keyName, RANGE, fileName);
                    spell.MaxTimer = NativeMethods.GetPrivateProfileInt32(keyName, MAX_TIMER, fileName);
                    spell.DeathEffect = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT, fileName);
                    spell.DeathEffectRange = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT_RANGE, fileName);
                    spell.DeathEffectChance = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT_CHANCE, fileName);
                    spell.DeathSpellEffect = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SPELL_EFFECT, fileName);
                    spell.CreationEffect = NativeMethods.GetPrivateProfileInt32(keyName, CREATION_EFFECT, fileName);
                    spell.BoltDeathEffect = NativeMethods.GetPrivateProfileInt32(keyName, BOLT_DEATH_EFFECT, fileName);
                    spell.BoltDeathEffectRange = NativeMethods.GetPrivateProfileInt32(keyName, BOLT_DEATH_EFFECT_RANGE, fileName);
                    spell.BoltDeathEffectChance = NativeMethods.GetPrivateProfileInt32(keyName, BOLT_DEATH_EFFECT_CHANCE, fileName);
                    spell.EffectSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND_RANGE, fileName);
                    spell.EffectSound = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    spell.Friendly = (SpellFriendlyType)NativeMethods.GetPrivateProfileInt32(keyName, FRIENDLY, fileName);
                    spell.SkillUsed = NativeMethods.GetPrivateProfileInt32(keyName, SKILL_USED, fileName);
                    spell.HugFloor = NativeMethods.GetPrivateProfileInt32(keyName, HUG_FLOOR, fileName);
                    break;
                }
                case "target":
                {
                    spell.Type = SpellType.Target;
                    spell.MinDamage = NativeMethods.GetPrivateProfileInt16(keyName, MIN_DAMAGE, fileName);
                    spell.MaxDamage = NativeMethods.GetPrivateProfileInt16(keyName, MAX_DAMAGE, fileName);
                    spell.DamageDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_DICE, fileName);
                    spell.DamageNumDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_NUM_DICE, fileName);
                    spell.DamageBase = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_BASE, fileName);
                    spell.MinPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MIN_POWER_DRAIN, fileName);
                    spell.MaxPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MAX_POWER_DRAIN, fileName);
                    spell.Range = NativeMethods.GetPrivateProfileInt16(keyName, RANGE, fileName);
                    spell.CasterSpellEffect = NativeMethods.GetPrivateProfileInt32(keyName, CASTER_SPELL_EFFECT, fileName);
                    spell.TargetSpellEffect = NativeMethods.GetPrivateProfileInt32(keyName, TARGET_SPELL_EFFECT, fileName);
                    spell.EffectSound = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND, fileName);
                    spell.EffectSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND_RANGE, fileName);
                    spell.MissSound = NativeMethods.GetPrivateProfileInt32(keyName, MISS_SOUND, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    spell.Friendly = (SpellFriendlyType)NativeMethods.GetPrivateProfileInt32(keyName, FRIENDLY, fileName);
                    spell.Group = NativeMethods.GetPrivateProfileInt32(keyName, GROUP, fileName);
                    break;
                }
                case "dispell":
                {
                    spell.Type = SpellType.Dispell;
                    spell.DispellType = NativeMethods.GetPrivateProfileInt32(keyName, DISPELL_TYPE, fileName);
                    spell.Range = NativeMethods.GetPrivateProfileInt16(keyName, RANGE, fileName);
                    spell.Level = NativeMethods.GetPrivateProfileInt16(keyName, LEVEL, fileName);
                    spell.EffectSound = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND, fileName);
                    spell.EffectSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND_RANGE, fileName);
                    break;
                }
                case "teleport":
                {
                    spell.Type = SpellType.Teleport;
                    spell.TeleportType = NativeMethods.GetPrivateProfileInt32(keyName, TELEPORT_TYPE, fileName);
                    spell.CasterSpellEffect = NativeMethods.GetPrivateProfileInt32(keyName, CASTER_SPELL_EFFECT, fileName);
                    break;
                }
                case "rune":
                {
                    spell.Type = SpellType.Rune;
                    spell.ImageNum = NativeMethods.GetPrivateProfileInt32(keyName, IMAGENUM, fileName);
                    spell.DeathImageNum = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGENUM, fileName);
                    spell.Width = NativeMethods.GetPrivateProfileInt32(keyName, WIDTH, fileName);
                    spell.Tall = NativeMethods.GetPrivateProfileInt32(keyName, TALL, fileName);
                    spell.ImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, IMAGE_TIMER_MAX, fileName);
                    spell.DeathImageTimerMax = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_IMAGE_TIMER_MAX, fileName);
                    spell.Gravity = NativeMethods.GetPrivateProfileBoolean(keyName, GRAVITY, fileName);
                    spell.LightPattern = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_PATTERN, fileName);
                    spell.MaxFlicker = NativeMethods.GetPrivateProfileInt32(keyName, MAX_FLICKER, fileName);
                    spell.LightGlow = NativeMethods.GetPrivateProfileInt32(keyName, LIGHT_GLOW, fileName);
                    spell.StickyLight = NativeMethods.GetPrivateProfileInt32(keyName, STICKY_LIGHT, fileName);
                    spell.DurationTimer = NativeMethods.GetPrivateProfileInt32(keyName, DURATION_TIMER, fileName);
                    spell.TransColor = NativeMethods.GetPrivateProfileInt32(keyName, TRANS_COLOR, fileName);
                    spell.EffectRadius = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_RADIUS, fileName);
                    spell.MissSound = NativeMethods.GetPrivateProfileInt32(keyName, MISS_SOUND, fileName);
                    spell.MinDamage = NativeMethods.GetPrivateProfileInt16(keyName, MIN_DAMAGE, fileName);
                    spell.MaxDamage = NativeMethods.GetPrivateProfileInt16(keyName, MAX_DAMAGE, fileName);
                    spell.DamageDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_DICE, fileName);
                    spell.DamageNumDice = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_NUM_DICE, fileName);
                    spell.DamageBase = NativeMethods.GetPrivateProfileInt16(keyName, DAMAGE_BASE, fileName);
                    spell.MinPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MIN_POWER_DRAIN, fileName);
                    spell.MaxPowerDrain = NativeMethods.GetPrivateProfileInt16(keyName, MAX_POWER_DRAIN, fileName);
                    spell.Velocity = NativeMethods.GetPrivateProfileInt32(keyName, VELOCITY, fileName);
                    spell.ZVelocity = NativeMethods.GetPrivateProfileInt32(keyName, Z_VELOCITY, fileName);
                    spell.CastAngle = NativeMethods.GetPrivateProfileInt32(keyName, CAST_ANGLE, fileName);
                    spell.NumProjectiles = NativeMethods.GetPrivateProfileInt32(keyName, NUM_PROJECTILES, fileName);
                    spell.ProjectileSpacing = NativeMethods.GetPrivateProfileInt32(keyName, PROJECTILE_SPACING, fileName);
                    spell.SideBySide = (SpellProjectileType)NativeMethods.GetPrivateProfileInt32(keyName, SIDE_BY_SIDE, fileName);
                    spell.CastDistance = NativeMethods.GetPrivateProfileInt32(keyName, CAST_DISTANCE, fileName);
                    spell.Elevation = NativeMethods.GetPrivateProfileInt32(keyName, ELEVATION, fileName);
                    spell.MaxStep = NativeMethods.GetPrivateProfileInt32(keyName, MAX_STEP, fileName);
                    spell.Translucent = NativeMethods.GetPrivateProfileInt32(keyName, TRANSLUCENT, fileName);
                    spell.AlignmentTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, ALIGNMENT_TRANSLUCENT, fileName);
                    spell.DeathTranslucent = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_TRANSLUCENT, fileName);
                    spell.RandomDeathPosition = NativeMethods.GetPrivateProfileInt32(keyName, RANDOM_DEATH_POSITION, fileName);
                    spell.CenterDeathImage = NativeMethods.GetPrivateProfileInt32(keyName, CENTER_DEATH_IMAGE, fileName);
                    spell.DeathEffect = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT, fileName);
                    spell.DeathEffectRange = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT_RANGE, fileName);
                    spell.DeathEffectChance = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_EFFECT_CHANCE, fileName);
                    spell.DeathSpellEffect = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SPELL_EFFECT, fileName);
                    spell.HorizontalSpread = NativeMethods.GetPrivateProfileInt32(keyName, HORIZONTAL_SPREAD, fileName);
                    spell.VerticalSpread = NativeMethods.GetPrivateProfileInt32(keyName, VERTICAL_SPREAD, fileName);
                    spell.Sound = NativeMethods.GetPrivateProfileInt32(keyName, SOUND, fileName);
                    spell.SoundRange = NativeMethods.GetPrivateProfileInt32(keyName, SOUND_RANGE, fileName);
                    spell.DeathSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SOUND_RANGE, fileName);
                    spell.EffectSoundRange = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND_RANGE, fileName);
                    spell.DeathSound = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_SOUND, fileName);
                    spell.EffectSound = NativeMethods.GetPrivateProfileInt32(keyName, EFFECT_SOUND, fileName);
                    spell.DeathFrameStart = NativeMethods.GetPrivateProfileInt32(keyName, DEATH_FRAME_START, fileName);
                    spell.Element = (SpellElementType)NativeMethods.GetPrivateProfileInt32(keyName, ELEMENT, fileName);
                    spell.NoTeam = NativeMethods.GetPrivateProfileBoolean(keyName, NO_TEAM, fileName);
                    spell.CollisionVelocity = NativeMethods.GetPrivateProfileInt32(keyName, COLLISION_VELOCITY, fileName);
                    spell.AuraCasterEffect = NativeMethods.GetPrivateProfileInt32(keyName, AURA_CASTER_EFFECT, fileName);
                    spell.AuraTargetEffect = NativeMethods.GetPrivateProfileInt32(keyName, AURA_TARGET_EFFECT, fileName);
                    spell.Friendly = (SpellFriendlyType)NativeMethods.GetPrivateProfileInt32(keyName, FRIENDLY, fileName);
                    spell.AuraPulseTimer = NativeMethods.GetPrivateProfileInt32(keyName, AURA_PULSE_TIMER, fileName);
                    spell.AuraHealth = NativeMethods.GetPrivateProfileInt16(keyName, AURA_HEALTH, fileName);
                    spell.AuraStackable = NativeMethods.GetPrivateProfileBoolean(keyName, AURA_STACKABLE, fileName);
                    break;
                }
                default:
                {
                    spell = null;
                    break;
                }
            }

            return spell;
        }

	    private static SpellTree LoadSpellTree(Character.PlayerClass playerClass, Int32 treeId, String fileName)
        {
            SpellTree newTree = new SpellTree();
            String keyName;

            switch (playerClass)
            {
                case Character.PlayerClass.Magician:
                {
                    keyName = "magician";
                    break;
                }
                case Character.PlayerClass.Arcanist:
                {
                    keyName = "arcanist";
                    break;
                }
                case Character.PlayerClass.Mentalist:
                {
                    keyName = "mentalist";
                    break;
                }
                case Character.PlayerClass.Cleric:
                {
                    keyName = "cleric";
                    break;
                }
                default:
                {
                    return null;
                }
            }

            Int32 listId = NativeMethods.GetPrivateProfileInt32(keyName, String.Format("list{0:00}", treeId), fileName);

            if (listId == 0) return null;

            String listKeyName = String.Format("spelllist{0:00}", listId);

            newTree.Id = listId;
            newTree.ListSlot = treeId;
            newTree.PlayerClass = playerClass;
	        newTree.Level = 0;
			newTree.Name = NativeMethods.GetPrivateProfileString(listKeyName, "name", fileName);

            for (Int32 i = 1; i <= 49; i++)
            {
                Int32 spellId = NativeMethods.GetPrivateProfileInt32(listKeyName, String.Format("level{0:00}", i), fileName);
                if (spellId == 0)
                {
                    newTree.TreeSpells.Add(null);
                    continue;
                }
                newTree.TreeSpells.Add(Spells[spellId]);
            }

            return newTree;
        }

	    [SuppressMessage("ReSharper", "UnusedVariable")]
	    public static Byte GetNumLists(Character.PlayerClass playerClass)
        {
            Byte count = 0;

	        foreach (SpellTree spellTree in SpellTrees.Where(spellTree => spellTree != null && spellTree.PlayerClass == playerClass))
            {
                count++;
            }

            return count;
        }

        public static SpellCheatInfo DoesPlayerHaveSpell(Player player, Spell spell)
        {
            if (player.IsAdmin || player.Admin == AdminLevel.Tester)
            {
                return new SpellCheatInfo(true);
            }

            if (player.ActiveCharacter == null) return new SpellCheatInfo(false);

            foreach (SpellTreeLevel spellTreeLevel in spell.SpellTreeLevels)
            {
                foreach (SpellTree spellTree in player.ActiveCharacter.SpellTrees)
                {
                    if (spellTree.Id == spellTreeLevel.List)
                    {
						if (player.ActiveCharacter.Level < spellTreeLevel.Level)
	                    {
							return new SpellCheatInfo(spell, false, spellTree.Level, spellTreeLevel.Level, spellTree.Name, "Spell Level Too High");
	                    }

						//Program.ServerForm.CheatLog.WriteMessage(String.Format("[Spell Check] ({0}){1} -> Spell: {2}, List Level: {3}, Spell Level: {4}, List: {5}", player.AccountId, player.ActiveCharacter.Name, spell.Name, spellTree.Level, spellTreeLevel.Level, spellTree.Name), Color.Chocolate);
						
						if (spellTree.Level < spellTreeLevel.Level)
	                    {
							return new SpellCheatInfo(spell, false, spellTree.Level, spellTreeLevel.Level, spellTree.Name, "Not Enough List Picks");
	                    }

						return new SpellCheatInfo(true);
                    }
                }
            }

            return new SpellCheatInfo(spell, false, 0, 0, "None", "Wrong Class");
        }

        public static Byte GetListId(Character.PlayerClass playerClass, Byte listNum)
        {
            SpellTree spellTree = SpellTrees.FindBySlotAndClass(listNum, playerClass);
            return spellTree == null ? (Byte)0 : (Byte)spellTree.Id;
        }
    }
}