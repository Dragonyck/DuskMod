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
using System.Collections;
using flanne.PerkSystem.Triggers;

namespace DuskMod
{
    class BleedCritStackTrigger : Trigger
    {
        public int stacks = 0;
        public float multiplier = 0.05f;
        public StatMod crit
        {
            get
            {
                return PlayerController.Instance.stats[Prefabs.criticalChance];
            }
        }
        public override void OnEquip(PlayerController player)
        {
            this.AddObserver(new Action<object, object>(Reset), string.Format(Prefabs.DamageTypeInflicted, Prefabs.crit.ToString()));
            this.AddObserver(new Action<object, object>(Stack), string.Format(Prefabs.DamageTypeInflicted, Prefabs.bleed.ToString()));
        }
        public void Reset(object sender, object args)
        {
            crit.AddMultiplierBonus(-1 * (stacks * multiplier));
            stacks = 0;
            Debug.LogWarning("Crit Chance Reset");
        }
        public void Stack(object sender, object args)
        {
            Debug.LogWarning("Stack");
            Health h = (Health)sender;
            if (!h)
            {

                Debug.LogError("No Health");
            }
            if (h && BleedManager.instance && BleedManager.instance.IsOnPeriod(h))
            {
                stacks++;
                crit.AddMultiplierBonus(multiplier);
                Debug.LogWarning("Crit Chance Added");
            }
        }
        public override void OnUnEquip(PlayerController player)
        {
            this.RemoveObserver(new Action<object, object>(Reset), string.Format(Prefabs.DamageTypeInflicted, Prefabs.crit.ToString()));
            this.RemoveObserver(new Action<object, object>(Stack), string.Format(Prefabs.DamageTypeInflicted, Prefabs.bleed.ToString()));
        }
    }
}
