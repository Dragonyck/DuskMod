using System;
using System.Reflection;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using UnityEngine.Networking;
using BepInEx.Configuration;
using UnityEngine.UI;
using System.Security;
using System.Security.Permissions;
using System.Linq;
using flanne;
using flanne.AISpecials;
using flanne.Audio;
using flanne.CharacterPassives;
using flanne.Core;
using flanne.InputExtensions;
using flanne.PerkSystem;
using flanne.Pickups;
using flanne.Player;
using flanne.PowerupSystem;
using flanne.PowerupSystems;
using flanne.RuneSystem;
using flanne.TitleScreen;
using flanne.UI;
using flanne.UIExtensions;
using System.IO;
using flanne.PerkSystem.Triggers;
using flanne.PerkSystem.Actions;
using static DuskMod.Assets;
using TMPro;

namespace DuskMod
{
    public class Prefabs
    {
        public static GameObject dontDestroyOnLoad;
        public static Dictionary<object, Tuple<Color, string>> colorDict = new Dictionary<object, Tuple<Color, string>>();
        public static Material spriteMat;
        public static Material redBuffMat;
        public static TMP_FontAsset[] fonts;
        public static TMP_FontAsset express;
        public static TMP_FontAsset lantern;

        public static GameObject flamethrowerProjectile;

        public static GameObject bossHealthUI;

        public static Sprite powerupSprite;

        public static GameObject redExplosionEffect;
        public static GameObject whiteExplosionEffect;

        public static GameObject baseReaperPrefab;
        public static GameObject reaperAnimator;
        public static GameObject carcass;

        public static CharacterData reaperData;
        public static PowerupPoolProfile reaperPool;
        public static GameObject reaperPassive;
        public static Powerup reaperPU1;
        public static PerkEffect[] reaperPU1Effects;
        public static Powerup reaperPU2;
        public static PerkEffect[] reaperPU2Effects;
        public static Powerup reaperPU3;
        public static PerkEffect[] reaperPU3Effects;
        public static SoundEffectSO preventDeathSFX;

        public static GunData[] guns;
        public static GunData dualSMG;
        public static CharacterData[] characters;
        public static Dictionary<string, Powerup> powerups = new Dictionary<string, Powerup>();

        public static MapData catacombsData;
        public static GameObject tombstone;
        public static RuntimeAnimatorController handAnimator;
        public static RuntimeAnimatorController skellyAnimator;
        public static RuntimeAnimatorController wheelyAnimator;
        public static RuntimeAnimatorController skellyMageAnimator;
        public static GameObject catacombsMapPrefab;
        public static GameObject catacombsGrid;
        public static Sprite catacombsPreview;
        public static GameObject skellyEnemy;
        public static GameObject wheelyEnemy;
        public static GameObject skellyMageEnemy;
        public static AIProjectileShootSpecial skellyMageSpecial;
        public static GameObject skellyMageSpecialProjectile;
        public static SoundEffectSO skellyProjectileFire;

        public static GameObject wingedBossProjectile;

        public static GameObject statPanel;

        public static GameObject fireTransformationEffect;
        public static RuntimeAnimatorController blackDragonAnimator;
        public static GameObject blackDragonProjectile;
        public static GameObject elderDragonProjectileFireball;
        public static GameObject elderDragonProjectileFireballExplosion;
        public static GameObject elderDragonProjectileWave;
        public static GameObject elderDragon;
        public static SoundEffectSO blackDragonSpawn;
        public static SoundEffectSO blackDragonAttack;
        public static SoundEffectSO elderDragonSpawn;
        public static SoundEffectSO elderDragonAttack;
        public static SoundEffectSO elderDragonSpecial;
        public static GameObject dragonball;
        public static GameObject dragonballPickupFX;
        public static SoundEffectSO dragonballPickupSFX;
        public static PerkEffect dragonBallDrop;
        public static GameObject dragonUI;

        public static GameObject minimap;
        public static GameObject minimapChestIcon;
        public static GameObject minimapDevilPickupIcon;
        public static GameObject minimapPlayerArrowIcon;
        public static GameObject minimapXPIcon;
        public static GameObject minimapBossIcon;

        public static Material spriteMaterial;
        public static GameObject fmg9DisplayPrefab;
        public static GameObject fmg9Prefab;
        public static GameObject fmg9Bullet;
        public static GameObject fmg9RedDot;
        public static RuntimeAnimatorController fmg9Animator;
        public static Sprite fmg9MuzzleFlash;
        public static GunData fmg9;
        public static GunEvolution fmg9ExMag;
        public static GunEvolution fmg9RedSight;
        public static GunEvolution fmg9ExRound;
        public static SoundEffectSO fmg9clipout;
        public static SoundEffectSO fmg9clipin;
        public static SoundEffectSO fmg9chamba;
        public static SoundEffectSO fmg9shot;

        public static GameObject bleedFX;
        public static GameObject bleedExplosionFX;

        public static PowerupTreeUIData bleedTree;
        public static Powerup bleedPU1;
        public static Powerup bleedPU2;
        public static Powerup bleedPU3;
        public static Powerup bleedPU4;

        public static GameObject poisonFX;

        public static PowerupTreeUIData poisonTree;
        public static Powerup poisonPU1;
        public static Powerup poisonPU2;
        public static Powerup poisonPU3;
        public static Powerup poisonPU4;

        public static GameObject critFX;

        public static PowerupTreeUIData critTree;
        public static Powerup critPU1;
        public static Powerup critPU2;
        public static Powerup critPU3;
        public static Powerup critPU4;

        public static Powerup headhunterSyn;
        public static GameObject headhunterTag;

        public static List<Powerup> powerupsToPool = new List<Powerup>();

        public static List<StatMod> statMods = new List<StatMod>();

        public static StatType criticalChance;
        public static StatType criticalDamage;
        public static DamageType crit;

        public static DamageType bleed;
        public static DamageType poison;

        public static int damageTypeLength;
        public static int statTypeLength;

        public static GameObject regularDamagePopup;
        public static GameObject statusDamagePopup;

        public static string DamageTypeInflicted =  "DamageType.Inflict{0}Damage";

        public static void Init()
        {
            dontDestroyOnLoad = new GameObject("ModPrefabs");
            UnityEngine.Object.DontDestroyOnLoad(dontDestroyOnLoad);
            dontDestroyOnLoad.SetActive(false);

            damageTypeLength = Enum.GetValues(typeof(DamageType)).Length;
            statTypeLength = Enum.GetValues(typeof(StatType)).Length - 1;

            guns = (GunData[])Resources.FindObjectsOfTypeAll(typeof(GunData));
            characters = (CharacterData[])Resources.FindObjectsOfTypeAll(typeof(CharacterData));
            fonts = (TMP_FontAsset[])Resources.FindObjectsOfTypeAll(typeof(TMP_FontAsset));
            foreach (TMP_FontAsset f in fonts)
            {
                switch (f.name)
                {
                    case "Express":
                        express = f;
                        break;
                    case "Lantern":
                        lantern = f;
                        break;
                }
            }
            CreatePrefabs();
        }
        public static void CreatePrefabs()
        {
            regularDamagePopup = Instantiate(Assets.Load<GameObject>("RegularPopupDamage"));
            regularDamagePopup.GetComponent<TextMeshPro>().font = lantern;
            var regularWT = regularDamagePopup.AddComponent<WorldTextPopup>();
            regularWT.lifetime = 0.4f;
            regularWT.destroyOnStop = true;

            statusDamagePopup = Instantiate(Assets.Load<GameObject>("StatusPopupDamage"));
            statusDamagePopup.GetComponent<TextMeshPro>().font = lantern;
            var statusWT = statusDamagePopup.AddComponent<WorldTextPopup>();
            statusWT.lifetime = 0.4f;
            statusWT.destroyOnStop = true;

            Color darkBlue = new Color(0.15294f, 0.12549f, 0.18824f);
            colorDict.Add("darkBlue", Tuple.Create(darkBlue, "272030"));
            Color blue = new Color(0.16078f, 0.20392f, 0.28235f);
            colorDict.Add("blue", Tuple.Create(blue, "293448"));
            Color darkGreen = new Color(0.23922f, 0.33333f, 0.33333f);
            colorDict.Add("darkGreen", Tuple.Create(darkGreen, "3d5555"));
            Color green = new Color(0.34509f, 0.43922f, 0.37255f);
            colorDict.Add("green", Tuple.Create(green, "58705f"));
            Color white = new Color(0.96078f, 0.83922f, 0.75686f);
            colorDict.Add("white", Tuple.Create(white, "f5d6c1"));
            Color red = new Color(0.99216f, 0.31765f, 0.38039f);
            colorDict.Add("red", Tuple.Create(red, "fd5161"));
            Color darkRed = new Color(0.79216f, 0.11765f, 0.18039f);
            colorDict.Add("darkRed", Tuple.Create(darkRed, "ca1e2e"));

            Color burn = new Color(0.98225f, 0.32314f, 0.08228f);
            colorDict.Add("burn", Tuple.Create(burn, "fd9a51"));
            colorDict.Add(DamageType.Burn, Tuple.Create(burn, "fd9a51"));

            Color lightning = new Color(0.98225f, 0.93011f, 0.08228f);
            colorDict.Add("lightning", Tuple.Create(lightning, "fdf751"));
            colorDict.Add(DamageType.Thunder, Tuple.Create(lightning, "fdf751"));

            Color curse = new Color(0.05448f, 0.048172f, 0.04374f);
            colorDict.Add("curse", Tuple.Create(curse, "423e3b"));
            colorDict.Add(DamageType.Curse, Tuple.Create(curse, "423e3b"));

            Color smite = new Color(0.87962f, 0.91309f, 0.53327f);
            colorDict.Add("smite", Tuple.Create(smite, "f1f5c1"));
            colorDict.Add(DamageType.Smite, Tuple.Create(smite, "f1f5c1"));
            
            Color summon = new Color(0.08228f, 0.15293f, 0.98225f);
            colorDict.Add("summon", Tuple.Create(summon, "516dfd"));
            colorDict.Add(DamageType.Summon, Tuple.Create(summon, "516dfd"));

            Color glare = new Color(0.08228f, 0.76815f, 0.98225f);
            colorDict.Add("glare", Tuple.Create(glare, "51e3fd"));
            colorDict.Add(DamageType.Glare, Tuple.Create(glare, "51e3fd"));

            Color gale = new Color(0.08228f, 0.98225f, 0.53948f);
            colorDict.Add("gale", Tuple.Create(gale, "51fdc2"));
            colorDict.Add(DamageType.Gale, Tuple.Create(gale, "51fdc2"));

            CreateReaper();
            CreateCatacombs();
            CreateDragons();
            CreateFMG9();
            CreateUIElements();
            CreatePowerups();
        }
        public static void CreatePowerups()
        {
            bleedFX = Instantiate(Assets.Load<GameObject>("bleedFX"));
            critFX = Instantiate(Assets.Load<GameObject>("critFX"));
            poisonFX = Instantiate(Assets.Load<GameObject>("poisonFX"));
            bleedExplosionFX = Instantiate(Assets.Load<GameObject>("bleedExplosionFX"));

            criticalChance = NewStatType(Language.critChanceNameToken);
            criticalDamage = NewStatType(Language.critDamageNameToken);
            statMods[1]._multiplierBonus = 1;

            bleed = NewDamageType();
            Color blood = new Color(0.43415f, 0.03955f, 0.03955f);
            colorDict.Add("bleed", Tuple.Create(blood, "b03838"));
            colorDict.Add(bleed, Tuple.Create(blood, "b03838"));
            bleedPU1 = CreatePowerup(Language.bleedPU1NameToken, Language.bleedPU1DescriptionToken, Load<Sprite>("openwounds_pu"), CreateArray<PerkEffect>(CreatePerkEffect(NewHitTrigger(0.4f), new BleedAction())));
            bleedPU2 = CreatePowerup(Language.bleedPU2NameToken, Language.bleedPU2DescriptionToken, Load<Sprite>("rupture_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new BleedPierceTrigger(), null)), CreateList<Powerup>(bleedPU1));
            bleedPU3 = CreatePowerup(Language.bleedPU3NameToken, Language.bleedPU3DescriptionToken, Load<Sprite>("bleed_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new BleedStackTrigger(), null)), CreateList<Powerup>(bleedPU1));
            bleedPU4 = CreatePowerup(Language.bleedPU4NameToken, Language.bleedPU4DescriptionToken, Load<Sprite>("lifeleech_pu"), CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(500, bleed), new HealOrIncreaseMaxHPAction())), CreateList<Powerup>(bleedPU2, bleedPU3), null, true);
            bleedTree = CreatePUTree("BleedTree", bleedPU1, bleedPU2, bleedPU3, bleedPU4);

            poison = NewDamageType();
            Color pois = new Color(0.34902f, 0.99216f, 0.31765f);
            colorDict.Add("poison", Tuple.Create(pois, "59fd51"));
            colorDict.Add(poison, Tuple.Create(pois, "59fd51"));
            poisonPU1 = CreatePowerup(Language.poisonPU1NameToken, Language.poisonPU1DescriptionToken, Load<Sprite>("blight_pu"), CreateArray<PerkEffect>(CreatePerkEffect(NewSummonHitTrigger(0.4f), new PoisonAction())));
            poisonPU2 = CreatePowerup(Language.poisonPU2NameToken, Language.poisonPU2DescriptionToken, Load<Sprite>("corrosion_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new SummonDMGMultOnPoisonedTrigger(), null)), CreateList<Powerup>(poisonPU1));
            poisonPU3 = CreatePowerup(Language.poisonPU3NameToken, Language.poisonPU3DescriptionToken, Load<Sprite>("acid_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new PoisonDurDMGMultTrigger(), null)), CreateList<Powerup>(poisonPU1));
            poisonPU4 = CreatePowerup(Language.poisonPU4NameToken, Language.poisonPU4DescriptionToken, Load<Sprite>("smog_pu"), CreateArray<PerkEffect>(CreatePerkEffect(NewSummonHitTrigger(0.6f), new PoisonAction()), CreatePerkEffect(NewOnKillTrigger(0, poison), new HealOrIncreaseMaxHPAction())), CreateList<Powerup>(poisonPU2, poisonPU3), null, true);
            poisonTree = CreatePUTree("PoisonTree", poisonPU1, poisonPU2, poisonPU3, poisonPU4);

            crit = NewDamageType();
            Color critical = new Color(0.98225f, 0.01599f, 0.03689f);
            colorDict.Add("crit", Tuple.Create(critical, "fd2236"));
            colorDict.Add(crit, Tuple.Create(critical, "fd2236"));
            critPU1 = CreatePowerup(Language.critPU1NameToken, Language.critPU1DescriptionToken, Load<Sprite>("criticalstrike_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new CritTrigger(), null)), null, CreateArray<StatChange>(CreateStatChange(criticalChance, 0.2f)));
            critPU2 = CreatePowerup(Language.critPU2NameToken, Language.critPU2DescriptionToken, Load<Sprite>("buckshot_pu"), null, CreateList<Powerup>(critPU1), CreateArray<StatChange>(CreateStatChange(criticalDamage, 0.5f)));
            critPU3 = CreatePowerup(Language.critPU3NameToken, Language.critPU3DescriptionToken, Load<Sprite>("shrapnel_pu"), CreateArray<PerkEffect>(CreatePerkEffect(NewOnTakeDamageTrigger(Prefabs.crit), new BleedAction()), CreatePerkEffect(NewOnTakeDamageTrigger(Prefabs.crit), new BleedExplosionAction())), CreateList<Powerup>(critPU1));
            critPU4 = CreatePowerup(Language.critPU4NameToken, Language.critPU4DescriptionToken, Load<Sprite>("jhp_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new BleedCritStackTrigger(), null)), CreateList<Powerup>(critPU2, critPU3), CreateArray<StatChange>(CreateStatChange(criticalChance, 0.05f), CreateStatChange(criticalDamage, 0.25f)), true);
            critTree = CreatePUTree("CritTree", critPU1, critPU2, critPU3, critPU4);

            headhunterSyn = CreatePowerup(Language.headHunterNameToken, Language.headHunterDescriptionToken, Load<Sprite>("headhunter_pu"), CreateArray<PerkEffect>(CreatePerkEffect(new InstantTrigger(), new AddTargetComponentAction()), CreatePerkEffect(NewOnKillTrigger(), new ModCritOnTagKillAction())));
            headhunterTag = Instantiate(Load<GameObject>("tag"));
            headhunterTag.AddComponent<Target>();

            powerupsToPool.AddRange(CreateList<Powerup>(bleedPU1, bleedPU2, bleedPU3, bleedPU4, poisonPU1, poisonPU2, poisonPU3, poisonPU4, critPU1, critPU2, critPU3, critPU4, headhunterSyn));
        }
        public static void CreateUIElements()
        {
            statPanel = Assets.Load<GameObject>("stats");
            var texts = statPanel.GetComponentsInChildren<TextMeshProUGUI>();
            var statPanelBehaviour = statPanel.AddComponent<StatPanelBehaviour>();
            statPanelBehaviour.gunName = texts[0];
            statPanelBehaviour.gunStats = texts[1];
            statPanelBehaviour.charName = texts[2];
            statPanelBehaviour.charStats = texts[3];

            bossHealthUI = Instantiate(Assets.Load<GameObject>("BossUI"));
            powerupSprite = Assets.Load<Sprite>("powerupBorder");
            spriteMat = Assets.Load<Material>("spriteMaterial");
            redBuffMat = UnityEngine.Object.Instantiate(spriteMat);
            redBuffMat.color = colorDict["red"].Item1;

            minimap = Instantiate(Assets.Load<GameObject>("minimap"));
            minimapChestIcon = Instantiate(Assets.Load<GameObject>("ChestMPIcon"));
            minimapDevilPickupIcon = Instantiate(Assets.Load<GameObject>("DevilPickupMPIcon"));
            minimapPlayerArrowIcon = Instantiate(Assets.Load<GameObject>("PlayerArrowMPIcon"));
            minimapPlayerArrowIcon.AddComponent<PointAtCursor>();
            minimapXPIcon = Instantiate(Assets.Load<GameObject>("XPMPIcon"));
            minimapBossIcon = Instantiate(Assets.Load<GameObject>("BossMPIcon"));
        }
        public static void CreateFMG9()
        {
            spriteMaterial = Assets.Load<Material>("spriteMaterial 1");
            fmg9RedDot = Instantiate(Assets.Load<GameObject>("RedDot"));
            fmg9DisplayPrefab = Assets.Load<GameObject>("fmg9Display");
            fmg9Animator = Assets.Load<RuntimeAnimatorController>("AC_FMG9");
            fmg9MuzzleFlash = Assets.Load<Sprite>("muzzleflash");
            fmg9clipout = CreateSound(Assets.Load<AudioClip>("clipout"));
            fmg9clipin = CreateSound(Assets.Load<AudioClip>("clipin"));
            fmg9chamba = CreateSound(Assets.Load<AudioClip>("chamba"));
            fmg9shot = CreateSound(Assets.Load<AudioClip>("bong"));

            fmg9ExMag = CreateEvolution(Language.fmg9ExMagNameToken, Language.fmg9ExMagDescriptionToken, Load<Sprite>("fmg9ExMag"), CreateArray<PerkEffect>(CreatePerkEffect(new NoReloadTrigger(), new ReloadOverTimeAction(true)), CreatePerkEffect(new OnShootTrigger(), new ReloadOverTimeAction(false))), CreateStatChange((StatType)2, 0, 108), CreateStatChange((StatType)1, -1));
            fmg9RedSight = CreateEvolution(Language.fmg9RedDotNameToken, Language.fmg9RedDotDescriptionToken, Load<Sprite>("fmg9LaserSight"), CreateArray<PerkEffect>(CreatePerkEffect(new LaserSightTrigger(), null)), CreateStatChange((StatType)7, 2), CreateStatChange((StatType)12, 0, -1000), CreateStatChange((StatType)0, 1), CreateStatChange((StatType)13, 0, 2));
            fmg9ExRound = CreateEvolution(Language.fmg9ExRoundNameToken, Language.fmg9ExRoundDescriptionToken, Load<Sprite>("fmg9ExRound"), CreateArray<PerkEffect>(CreatePerkEffect(new OnHitTrigger(), new ExplosionOnHitAction())), CreateStatChange((StatType)3, 0.7f), CreateStatChange((StatType)14, 0, 999));

            fmg9 = ScriptableObject.CreateInstance<GunData>();
            fmg9.nameStringID = new LocalizedString(Language.fmg9NameToken);
            fmg9.descriptionStringID = new LocalizedString(Language.fmg9DescriptionToken);
            fmg9.bounce = 0;
            //fmg9.bullet
            fmg9.damage = 5;
            fmg9.gunEvolutions = CreateList<GunEvolution>(fmg9ExMag, fmg9RedSight, fmg9ExRound);
            fmg9.gunshotSFX = fmg9shot;
            fmg9.icon = Load<Sprite>("fmg9Shoot");
            fmg9.inaccuracy = 12;
            fmg9.knockback = 0.7f;
            fmg9.maxAmmo = 36;
            //fmg9.model
            fmg9.numOfProjectiles = 1;
            fmg9.piercing = 0;
            fmg9.projectileSpeed = 20;
            fmg9.reloadDuration = 2;
            //fmg9.reloadSFXOverride
            fmg9.shotCooldown = 0.082f;
            fmg9.spread = 16;
        }
        public static void CreateDragons()
        {
            blackDragonAttack = CreateSound(Load<AudioClip>("blackDragon_atk1"), Load<AudioClip>("blackDragon_atk2"));
            blackDragonSpawn = CreateSound(Load<AudioClip>("blackDragon_spawn"));

            fireTransformationEffect = Instantiate(Load<GameObject>("fireTransformationEffect"));

            dragonballPickupFX = Instantiate(Load<GameObject>("dragonballPickupFX"));
            dragonballPickupSFX = CreateSound(Load<AudioClip>("gotballs"));

            elderDragonSpawn = CreateSound(Load<AudioClip>("elderDragon_spawn1"), Load<AudioClip>("elderDragon_spawn2"));
            elderDragonAttack = CreateSound(Load<AudioClip>("elderDragon_atk1"), Load<AudioClip>("elderDragon_atk2"));
            elderDragonSpecial = CreateSound(Load<AudioClip>("elderDragon_special1"), Load<AudioClip>("elderDragon_special2"));

            dragonUI = Instantiate(Load<GameObject>("dragonUI"));

            dragonball = Instantiate(Load<GameObject>("dragonball"));
            dragonball.AddComponent<DragonBallPickupBehaviour>();
            dragonBallDrop = CreatePerkEffect(NewOnKillTrigger(), new DragonBallDropAction());

            blackDragonAnimator = Assets.Load<RuntimeAnimatorController>("AC_BlackDragon");
            CharacterData scarlett = Array.Find<CharacterData>(characters, x => x.nameString == "Scarlett");
            if (scarlett && scarlett.passivePrefab)
            {
                ProjectileOnShoot p = scarlett.passivePrefab.GetComponent<ProjectileOnShoot>();
                if (p && p.projectilePrefab)
                {
                    blackDragonProjectile = Instantiate(p.projectilePrefab);
                    blackDragonProjectile.name = "BlackDragonProjectile";
                    blackDragonProjectile.GetComponent<SpriteRenderer>().ResizeSprite(Vector2.one * 1.5f, Assets.Load<Sprite>("blackDragonFireWave"));

                    elderDragonProjectileWave = Instantiate(p.projectilePrefab);
                    elderDragonProjectileWave.name = "ElderDragonWaveProjectile";
                    elderDragonProjectileWave.GetComponent<SpriteRenderer>().ResizeSprite(Vector2.one * 2, Assets.Load<Sprite>("elderDragonFireWave2"));
                }
            }

            elderDragon = Instantiate(Assets.Load<GameObject>("ElderDragon"));
            //elderDragon.AddComponent<DragonAI>().size = 3;
            var move = elderDragon.AddComponent<MoveComponent2D>();
            move.drag = 1.4f;
            move.Rb = elderDragon.GetComponent<Rigidbody2D>();
            elderDragon.AddComponent<EntityStateMachine>().spawnState = elderDragon.AddComponent<ElderDragonSpawnState>();

        }
        public static void CreateCatacombs()
        {
            tombstone = Assets.Load<GameObject>("tombstone");
            handAnimator = Assets.Load<RuntimeAnimatorController>("AC_Tombstone");
            skellyAnimator = Assets.Load<RuntimeAnimatorController>("AC_Skelly");
            skellyMageAnimator = Assets.Load<RuntimeAnimatorController>("AC_SkellyMage");
            wheelyAnimator = Assets.Load<RuntimeAnimatorController>("AC_Wheely");
            catacombsPreview = Assets.Load<Sprite>("catacombsPreview");

            catacombsData = ScriptableObject.CreateInstance<MapData>();
            catacombsData.nameStringID = new LocalizedString(Language.catacombsNameToken);
            catacombsData.descriptionStringID = new LocalizedString(Language.catacombsDescriptionToken);
            catacombsData.timeLimit = 1200;
            catacombsData.numPowerupsRepeat = 1;
            catacombsData.endlessBossSpawn = new List<BossSpawn>();
            catacombsData.endlessSpawnSessions = new List<SpawnSession>();
            catacombsData.spawnSessions = new List<SpawnSession>();
            catacombsData.bossSpawns = new List<BossSpawn>();

            catacombsGrid = Assets.Load<GameObject>("Grid");

            skellyMageSpecial = ScriptableObject.CreateInstance<AIProjectileShootSpecial>();
        }
        public static void CreateReaper()
        {
            preventDeathSFX = CreateSound(Load<AudioClip>("death_touch"));

            reaperPassive = Create("ReaperPassive");
            reaperPassive.AddComponent<ReaperBehaviour>();

            reaperPU1Effects = CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(), new SoulStackingAction()));
            reaperPU1 = CreatePowerup(Language.reaperPU1NameToken, Language.reaperPU1DescriptionToken, Load<Sprite>("reaperPU1Icon"), reaperPU1Effects);

            reaperPU2Effects = CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(), new CarcassDropAction()));
            reaperPU2 = CreatePowerup(Language.reaperPU2NameToken, Language.reaperPU2DescriptionToken, Load<Sprite>("reaperPU2Icon"), reaperPU2Effects, CreateList<Powerup>(reaperPU1));

            reaperPU3Effects = CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(MainPlugin.debug ? 1 : 666), new DeathPreventionAction()));
            reaperPU3 = CreatePowerup(Language.reaperPU3NameToken, Language.reaperPU3DescriptionToken, Load<Sprite>("reaperPU3Icon"), reaperPU3Effects, CreateList<Powerup>(reaperPU2));

            reaperPool = ScriptableObject.CreateInstance<PowerupPoolProfile>();
            reaperPool.powerupPool = new List<Powerup>() { reaperPU1, reaperPU2, reaperPU3 };

            reaperData = ScriptableObject.CreateInstance<CharacterData>();
            reaperData.nameStringID = new LocalizedString(Language.reaperNameToken);
            reaperData.descriptionStringID = new LocalizedString(Language.reaperDescriptionToken);
            reaperData.icon = Load<Sprite>("T_Reaper_0");
            reaperData.portrait = Load<Sprite>("reaperPortrait");
            reaperData.startHP = 0;
            reaperData.animController = Load<RuntimeAnimatorController>("AC_Reaper");
            reaperData.uiAnimController = reaperData.animController;
            reaperData.passivePrefab = reaperPassive;
            reaperData.exclusivePowerups = reaperPool;

            reaperAnimator = Instantiate(Load<GameObject>("AC_Reaper"));

            if (MainPlugin.debug)
            {
                /*var reaper = Array.Find<GameObject>((GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)), x => x.name == "PF_Reaper");
                if (reaper)
                {
                    baseReaperPrefab = Instantiate(reaper);
                }*/
            }

            carcass = Instantiate(Assets.Load<GameObject>("Carcass"));
            carcass.AddComponent<CarcassBehaviour>();
            carcass.AddComponent<AnimEventHandler>();
        }
        public static OnTakeDamageTrigger NewOnTakeDamageTrigger(DamageType damageType)
        {
            OnTakeDamageTrigger n = new OnTakeDamageTrigger();
            n.damageType = damageType;
            return n;
        }
        public static StatType NewStatType(string nameToken)
        {
            statMods.Add(new StatMod());
            statTypeLength++;
            StatType stat = (StatType)statTypeLength - 1;
            StatLabels.Labels.Add(stat, nameToken);
            return stat;
        }
        public static DamageType NewDamageType()
        {
            damageTypeLength++;
            return (DamageType)damageTypeLength - 1;
        }
        public static NotificationTrigger NewNotificationTrigger(string notification)
        {
            NotificationTrigger n = new NotificationTrigger();
            n.notification = notification;
            return n;
        }
        public static SummonOnHitTrigger NewSummonHitTrigger(float triggerChance = 1, string summonID = "")
        {
            SummonOnHitTrigger s = new SummonOnHitTrigger();
            s.triggerChance = triggerChance;
            s.summonTypeID = summonID;
            return s;
        }
        public static OnHitTrigger NewHitTrigger(float triggerChance = 1, bool actionTargetPlayer = false)
        {
            OnHitTrigger s = new OnHitTrigger();
            s.triggerChance = triggerChance;
            s.actionTargetPlayer = actionTargetPlayer;
            return s;
        }
        public static PowerupTreeUIData CreatePUTree(string name, params Powerup[] powerups)
        {
            PowerupTreeUIData tree = ScriptableObject.CreateInstance<PowerupTreeUIData>();
            tree.name = name;
            tree.startingPowerup = powerups[0];
            tree.rightPowerup = powerups[1];
            tree.leftPowerup = powerups[2];
            tree.finalPowerup = powerups[3];
            foreach (Powerup p in powerups)
            {
                p.powerupTreeUIData = tree;
            }
            return tree;
        }
        public static StatChange CreateStatChange(StatType stat, float mult, int flat = 0)
        {
            StatChange s = new StatChange 
            {
                type = stat,
                value = mult,
                flatValue = flat,
                isFlatMod = flat != 0
            };
            return s;
        }
        public static GunEvolution CreateEvolution(string name, string desc, Sprite icon, PerkEffect[] effects = null, params StatChange[] statChanges)
        {
            GunEvolution evo = ScriptableObject.CreateInstance<GunEvolution>();
            evo.nameStrID = new LocalizedString(name);
            evo.desStrID = new LocalizedString(desc);
            evo.icon = icon;
            evo.effects = effects ?? new PerkEffect[0];
            evo.statChanges = statChanges ?? new StatChange[0];
            return evo;
        }
        public static SoundEffectSO CreateSound(params AudioClip[] clips)
        {
            SoundEffectSO sound = ScriptableObject.CreateInstance<SoundEffectSO>();
            sound.volume = Vector2.one;
            sound.clips = clips;
            return sound;
        }
        public static OnKillTrigger NewOnKillTrigger(int killsToTrigger = 0, DamageType damageType = DamageType.None)
        {
            OnKillTrigger trigger = new OnKillTrigger()
            {
                killsToTrigger = killsToTrigger,
                anyDamageType = damageType == DamageType.None,
                damageType = damageType,
                actionTargetPlayer = false
            };
            return trigger;
        }
        public static T[] CreateArray<T>(params T[] parameters)
        {
            return parameters;
        }
        public static List<T> CreateList<T>(params T[] parameters)
        {
            return parameters.ToList();
        }
        public static GameObject Create(string name)
        {
            GameObject instance = new GameObject(name);
            instance.transform.SetParent(dontDestroyOnLoad.transform);
            return instance;
        }
        public static GameObject Instantiate(GameObject obj)
        {
            GameObject instance =  UnityEngine.Object.Instantiate(obj, dontDestroyOnLoad.transform);
            return instance;
        }
        public static PerkEffect CreatePerkEffect(Trigger trigger, flanne.PerkSystem.Action action, bool limit = false, int limitCount = 0)
        {
            PerkEffect perkEffect = new PerkEffect()
            {
                limitActivations = limit,
                limit = limitCount,
                trigger = trigger,
                action = action ?? new EmptyAction()
            };
            return perkEffect;
        }
        public static Powerup CreatePowerup(string nameKey, string descKey, Sprite sprite, PerkEffect[] effects = null, List<Powerup> preReqs = null, StatChange[] statChanges = null, bool any = false)
        {
            Powerup powerup = ScriptableObject.CreateInstance<Powerup>();
            powerup.name = LocalizationSystem.GetLocalizedValue(nameKey);
            powerup.nameStrID = new LocalizedString(nameKey);
            powerup.desStrID = new LocalizedString(descKey);
            powerup.icon = sprite;
            powerup.effects = effects ?? new PerkEffect[0];
            powerup.prereqs = preReqs ?? new List<Powerup>();
            powerup.statChanges = statChanges ?? new StatChange[0];
            powerup.anyPrereqFulfill = any;
            return powerup;
        }
    }
}
