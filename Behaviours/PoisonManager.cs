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

namespace DuskMod
{
    class PoisonManager : MonoBehaviour
    {
        public static PoisonManager instance;
        public bool canStack = false;
        public Dictionary<Health, PoisonBehaviour> poisonedTargets = new Dictionary<Health, PoisonBehaviour>();
        public static string InflictEvent = "Poison.InflictEvent";
        public static string KillEvent = "Poison.KillEvent";
        public static string DamageEvent = "Poison.DamageEvent";
        public float baseDuration = 6;
        public float baseDamageMult = 1;
        public void Awake()
        {
            instance = this;
        }
        public void Dispoisonize(Health target)
        {
            if (!target)
            {
                return;
            }
            if (poisonedTargets.ContainsKey(target))
            {
                poisonedTargets.Remove(target);
            }
        }
        public void PoisonizeTarget(Health target)
        {
            if (!target)
            {
                return;
            }
            if (MainPlugin.debug)
            {
                Debug.LogWarning("Poison Inflicted");
            }
            base.gameObject.PostNotification(PoisonManager.InflictEvent, target);
            PoisonBehaviour poise = null;
            if (poisonedTargets.TryGetValue(target, out poise))
            {
                return;
            }
            poisonedTargets.Add(target, AddPoison(target.gameObject, baseDamageMult, baseDuration));
        }
        public PoisonBehaviour AddPoison(GameObject target, float mult, float dur)
        {
            var bleed = target.AddComponent<PoisonBehaviour>();
            bleed.baseDamageCoefficient = mult;
            bleed.baseDuration = dur;
            return bleed;
        }
        public bool IsPoisoned(Health target)
        {
            if (!target)
            {
                return false;
            }
            return poisonedTargets.ContainsKey(target);
        }
        public bool IsPoisoned(Health target, out PoisonBehaviour b)
        {
            if (!target)
            {
                b = null;
                return false;
            }
            return poisonedTargets.TryGetValue(target, out b);
        }
    }
}
