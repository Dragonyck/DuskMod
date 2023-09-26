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
    class BleedManager : MonoBehaviour
    {
        public static BleedManager instance;
        public bool canStack = false;
        public Dictionary<Health, BleedBehaviour> bleedingTargets = new Dictionary<Health, BleedBehaviour>();
        public static string InflictEvent = "Bleed.InflictEvent";
        public static string KillEvent = "Bleed.KillEvent";
        public static string DamageEvent = "Bleed.DamageEvent";
        public void Awake()
        {
            instance = this;
        }
        public void Dispiriodize(Health target)
        {
            if (!target)
            {
                return;
            }
            if (bleedingTargets.ContainsKey(target))
            {
                bleedingTargets.Remove(target);
            }
        }
        public void PeriodizeTarget(Health target, float multiplier = 1, int startingStacks = 1)
        {
            if (!target)
            {
                return;
            }
            if (MainPlugin.debug)
            {
                Debug.LogWarning("Bleed Inflicted");
            }
            base.gameObject.PostNotification(BleedManager.InflictEvent, target);
            BleedBehaviour bleeding = null;
            if (bleedingTargets.TryGetValue(target, out bleeding))
            {
                bleeding.basePercentage *= multiplier;
                if (!canStack)
                {
                    return;
                }
                bleeding.stacks++;
            }
            bleedingTargets.Add(target, AddBleed(target.gameObject, multiplier, startingStacks));
        }
        public BleedBehaviour AddBleed(GameObject target, float mult, int stack)
        {
            var bleed = target.AddComponent<BleedBehaviour>();
            bleed.basePercentage *= mult;
            bleed.stacks = stack;
            return bleed;
        }
        public bool IsOnPeriod(Health target)
        {
            if (!target)
            {
                return false;
            }
            return bleedingTargets.ContainsKey(target);
        }
        public bool IsOnPeriod(Health target, out BleedBehaviour b)
        {
            if (!target)
            {
                b = null;
                return false;
            }
            return bleedingTargets.TryGetValue(target, out b);
        }
    }
}
