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

namespace DuskMod
{
    internal class Prefabs
    {
        internal static GameObject dontDestroyOnLoad;
        internal static GameObject flamethrowerProjectile;
        internal static GameObject baseReaperPrefab;
        internal static GameObject reaperAnimator;

        internal static CharacterData reaperData;
        internal static PowerupPoolProfile reaperPool;
        internal static GameObject reaperPassive;
        internal static Powerup reaperPU1;
        internal static PerkEffect[] reaperPU1Effects;
        internal static Powerup reaperPU2;
        internal static PerkEffect[] reaperPU2Effects;
        internal static Powerup reaperPU3;
        internal static PerkEffect[] reaperPU3Effects;
        internal static SoundEffectSO preventDeathSFX;

        internal static GunData[] guns;
        internal static CharacterData[] characters;

        internal static void Init()
        {
            dontDestroyOnLoad = new GameObject("ModPrefabs");
            UnityEngine.Object.DontDestroyOnLoad(dontDestroyOnLoad);
            dontDestroyOnLoad.SetActive(false);

            guns = (GunData[])Resources.FindObjectsOfTypeAll(typeof(GunData));
            characters = (CharacterData[])Resources.FindObjectsOfTypeAll(typeof(CharacterData));

            CreatePrefabs();
        }
        internal static void CreatePrefabs()
        {
            CreateReaper();
        }
        internal static void CreateReaper()
        {
            preventDeathSFX = CreateSound(Load<AudioClip>("death_touch"));

            reaperPassive = Create("ReaperPassive");
            reaperPassive.AddComponent<ReaperBehaviour>();

            reaperPU1Effects = CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(), new SoulStackingAction()));
            reaperPU1 = CreatePowerup(Language.reaperPU1NameToken, Language.reaperPU1DescriptionToken, Load<Sprite>("reaperPU1Icon"), reaperPU1Effects);

            reaperPU2Effects = CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(), new CarcassDropAction()));
            reaperPU2 = CreatePowerup(Language.reaperPU2NameToken, Language.reaperPU2DescriptionToken, Load<Sprite>("reaperPU2Icon"), reaperPU2Effects, CreateList<Powerup>(reaperPU1));

            reaperPU3Effects = CreateArray<PerkEffect>(CreatePerkEffect(NewOnKillTrigger(1), new DeathPreventionAction()));
            reaperPU3 = CreatePowerup(Language.reaperPU3NameToken, Language.reaperPU3DescriptionToken, Load<Sprite>("reaperPU3Icon"), reaperPU3Effects, CreateList<Powerup>(reaperPU2));

            reaperPool = ScriptableObject.CreateInstance<PowerupPoolProfile>();
            reaperPool.powerupPool = new List<Powerup>() { reaperPU1, reaperPU2, reaperPU3 };

            reaperData = ScriptableObject.CreateInstance<CharacterData>();
            reaperData.nameStringID = new LocalizedString() { key = Language.reaperNameToken };
            reaperData.descriptionStringID = new LocalizedString() { key = Language.reaperDescriptionToken };
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
        }
        internal static SoundEffectSO CreateSound(params AudioClip[] clips)
        {
            SoundEffectSO sound = ScriptableObject.CreateInstance<SoundEffectSO>();
            sound.volume = Vector2.one;
            sound.clips = clips;
            return sound;
        }
        internal static OnKillTrigger NewOnKillTrigger(int killsToTrigger = 0, bool anyDamageType = true, DamageType damageType = DamageType.None)
        {
            OnKillTrigger trigger = new OnKillTrigger()
            {
                killsToTrigger = killsToTrigger,
                anyDamageType = anyDamageType,
                damageType = damageType,
                actionTargetPlayer = false
            };
            return trigger;
        }
        internal static T[] CreateArray<T>(params T[] parameters)
        {
            return parameters;
        }
        internal static List<T> CreateList<T>(params T[] parameters)
        {
            return parameters.ToList();
        }
        internal static GameObject Create(string name)
        {
            GameObject instance = new GameObject(name);
            instance.transform.SetParent(dontDestroyOnLoad.transform);
            return instance;
        }
        internal static GameObject Instantiate(GameObject obj)
        {
            GameObject instance =  UnityEngine.Object.Instantiate(obj, dontDestroyOnLoad.transform);
            return instance;
        }
        internal static PerkEffect CreatePerkEffect(Trigger trigger, flanne.PerkSystem.Action action, bool limit = false, int limitCount = 0)
        {
            PerkEffect perkEffect = new PerkEffect()
            {
                limitActivations = limit,
                limit = limitCount,
                trigger = trigger,
                action = action
            };
            return perkEffect;
        }
        internal static Powerup CreatePowerup(string nameKey, string descKey, Sprite sprite, PerkEffect[] effects = null, List<Powerup> preReqs = null, StatChange[] statChanges = null)
        {
            Powerup powerup = ScriptableObject.CreateInstance<Powerup>();
            powerup.nameStrID = new LocalizedString() { key = nameKey };
            powerup.desStrID = new LocalizedString() { key = descKey };
            powerup.icon = sprite;
            powerup.effects = effects ?? new PerkEffect[0];
            powerup.prereqs = preReqs ?? new List<Powerup>();
            powerup.statChanges = statChanges ?? new StatChange[0];
            return powerup;
        }
    }
}
