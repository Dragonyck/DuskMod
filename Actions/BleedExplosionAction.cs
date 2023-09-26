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
using UnityEngine.Events;
using flanne.PerkSystem.Triggers;

namespace DuskMod
{
    class BleedExplosionAction : flanne.PerkSystem.Action
    {
        public float damage
        {
            get
            {
                return PlayerController.Instance.gun.damage;
            }
        }
        public override void Activate(GameObject target)
        {
            var h = target.GetComponent<Health>();
            if (!h || h && BleedManager.instance && !BleedManager.instance.IsOnPeriod(h))
            {
                return;
            }

            foreach (Collider2D c in Physics2D.OverlapCircleAll(target.transform.position, 2, 1 << TagLayerUtil.Enemy))
            {
                var health = c.GetComponent<Health>();
                if (health)
                {
                    health.TakeDamage(Prefabs.bleed, Mathf.FloorToInt(damage));
                }
            }
            UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Prefabs.bleedExplosionFX, target.transform.position, Quaternion.identity, ObjectPooler.SharedInstance.transform), 0.2f);
        }
    }
}
