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

namespace DuskMod
{
    public class BleedPierceTrigger : Trigger
    {
        public override void OnEquip(PlayerController player)
        {
            On.flanne.Projectile.OnCollisionEnter2D += Projectile_OnCollisionEnter2D;
        }

        private void Projectile_OnCollisionEnter2D(On.flanne.Projectile.orig_OnCollisionEnter2D orig, Projectile self, Collision2D other)
        {
            orig(self, other);
            Health health = other.gameObject.GetComponent<Health>();
            if (health == null || !BleedManager.instance || health && health.isDead || health && !BleedManager.instance.IsOnPeriod(health))
            {
                return;
            }
            if (!self.GetComponent<BleedPierceProjectile>())
            {
                if (self.piercing > 0)
                {
                    self.gameObject.AddComponent<BleedPierceProjectile>();
                }
            }
            else
            {
                BleedManager.instance.PeriodizeTarget(health, 1.5f);
            }
        }

        public override void OnUnEquip(PlayerController player)
        {
            On.flanne.Projectile.OnCollisionEnter2D -= Projectile_OnCollisionEnter2D;
        }
    }
}
