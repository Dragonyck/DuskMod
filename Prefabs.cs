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
using UnityEngine.AddressableAssets;
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
        internal static PerkEffect[] reaperPU1PerkEffects;
        internal static Powerup reaperPU2;
        internal static PerkEffect[] reaperPU2PerkEffects;
        internal static Powerup reaperPU3;
        internal static PerkEffect[] reaperPU3PerkEffects;

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
            reaperPassive = Create("ReaperPassive");
            reaperPassive.AddComponent<ReaperBehaviour>();

            reaperPU1PerkEffects = new PerkEffect[] { CreatePerkEffect(new OnKillTrigger(), new SoulStackingAction()) };
            reaperPU1 = CreatePowerup(Language.reaperPU1NameToken, Language.reaperPU1DescriptionToken, Assets.MainAssetBundle.LoadAsset<Sprite>("reaperPU1Icon"), reaperPU1PerkEffects);

            reaperPU2PerkEffects = new PerkEffect[] { CreatePerkEffect(new OnKillTrigger(), new CarcassDropAction()) };
            reaperPU2 = CreatePowerup(Language.reaperPU2NameToken, Language.reaperPU2DescriptionToken, Assets.MainAssetBundle.LoadAsset<Sprite>("reaperPU2Icon"), reaperPU2PerkEffects, CreateList<Powerup>(reaperPU1));

            reaperPU3PerkEffects = new PerkEffect[] { CreatePerkEffect(new OnKillTrigger(), new SoulStackingAction()) };
            reaperPU3 = CreatePowerup(Language.reaperPU3NameToken, Language.reaperPU3DescriptionToken, Assets.MainAssetBundle.LoadAsset<Sprite>("reaperPU3Icon"), reaperPU3PerkEffects, CreateList<Powerup>(reaperPU2));

            reaperPool = ScriptableObject.CreateInstance<PowerupPoolProfile>();
            reaperPool.powerupPool = new List<Powerup>() { reaperPU1, reaperPU2, reaperPU3 };

            reaperData = ScriptableObject.CreateInstance<CharacterData>();
            reaperData.nameStringID = new LocalizedString() { key = Language.reaperNameToken };
            reaperData.descriptionStringID = new LocalizedString() { key = Language.reaperDescriptionToken };
            reaperData.icon = Assets.MainAssetBundle.LoadAsset<Sprite>("T_Reaper_0");
            reaperData.portrait = Assets.MainAssetBundle.LoadAsset<Sprite>("reaperPortrait");
            reaperData.startHP = 0;
            reaperData.animController = Assets.MainAssetBundle.LoadAsset<RuntimeAnimatorController>("AC_Reaper");
            reaperData.uiAnimController = reaperData.animController;
            reaperData.passivePrefab = reaperPassive;
            reaperData.exclusivePowerups = reaperPool;

            /*var reaper = Array.Find<GameObject>((GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)), x => x.name == "PF_Reaper");
            if (reaper)
            {
                baseReaperPrefab = Instantiate(reaper);
            }*/

            reaperAnimator = Instantiate(Assets.MainAssetBundle.LoadAsset<GameObject>("AC_Reaper"));
        }
        internal static List<T> CreateList<T>(params T[] parameters)
        {
            List<T> list = new List<T>();
            list.AddRange(parameters);
            return list;
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
        internal static Powerup CreatePowerup(string nameKey, string descKey, Sprite sprite, PerkEffect[] effects, List<Powerup> preReqs = null, StatChange[] statChanges = null)
        {
            Powerup powerup = ScriptableObject.CreateInstance<Powerup>();
            powerup.nameStrID = new LocalizedString() { key = nameKey };
            powerup.desStrID = new LocalizedString() { key = descKey };
            powerup.icon = sprite;
            powerup.effects = effects;
            powerup.statChanges = statChanges;
            powerup.prereqs = preReqs ?? new List<Powerup>();
            powerup.statChanges = statChanges ?? new StatChange[0];
            return powerup;
        }
    }
}
