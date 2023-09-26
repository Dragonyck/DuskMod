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
    public class SummonDMGMultOnPoisonedTrigger : Trigger
    {
        public float damageMultiplier = 0.25f;
        public override void OnEquip(PlayerController player)
        {
            On.flanne.Health.TakeDamage += Health_TakeDamage;
        }
        private void Health_TakeDamage(On.flanne.Health.orig_TakeDamage orig, Health self, DamageType damageType, int damage, float finalMultiplier)
        {
            if (damageType == DamageType.Summon && PoisonManager.instance && PoisonManager.instance.IsPoisoned(self))
            {
                finalMultiplier += damageMultiplier;
            }
            orig(self, damageType, damage, finalMultiplier);
        }
        public override void OnUnEquip(PlayerController player)
        {
            On.flanne.Health.TakeDamage -= Health_TakeDamage;
        }
    }
}
