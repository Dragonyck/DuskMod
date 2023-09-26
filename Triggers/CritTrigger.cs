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
    public class CritTrigger : Trigger
    {
        public float chance
        {
            get
            {
                return PlayerController.Instance.stats[Prefabs.criticalChance]._multiplierBonus;
            }
        }
        public float damage
        {
            get
            {
                return PlayerController.Instance.stats[Prefabs.criticalDamage]._multiplierBonus;
            }
        }
        public override void OnEquip(PlayerController player)
        {
            On.flanne.Health.TakeDamage += Health_TakeDamage;
        }
        public void Health_TakeDamage(On.flanne.Health.orig_TakeDamage orig, Health self, DamageType damageType, int damage, float finalMultiplier)
        {
            bool isTarget = self.GetComponentInChildren<Target>();
            bool isCrit = damageType == DamageType.Bullet && UnityEngine.Random.Range(0f, 1f) <= chance || isTarget;
            if (isCrit)
            {
                finalMultiplier += this.damage;
                UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Prefabs.critFX, self.transform.position, Quaternion.identity, self.transform), 0.15f);
                //self.SpawnDamagePopup(damage * finalMultiplier, Prefabs.colorDict["crit"].Item2, 0);
                damageType = Prefabs.crit;
            }
            orig(self, damageType, damage, finalMultiplier);
        }
        public override void OnUnEquip(PlayerController player)
        {
            On.flanne.Health.TakeDamage -= Health_TakeDamage;
        }
    }
}
