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
using static UnityEngine.Object;

namespace DuskMod
{
    class ExplosionOnHitAction : flanne.PerkSystem.Action
    {
        public float radius = 2;
        public float dmgMult = 0.35f;
        public override void Activate(GameObject target)
        {
            Destroy(Instantiate(Prefabs.redExplosionEffect, target.transform.position, Quaternion.identity, ObjectPooler.SharedInstance.transform), 0.3f);
            foreach (Collider2D c in Physics2D.OverlapCircleAll(target.transform.position, radius, 1 << TagLayerUtil.Enemy))
            {
                c.gameObject.GetComponent<Health>().TakeDamage(DamageType.Burn, Mathf.FloorToInt(PlayerController.Instance.gun.damage * dmgMult));
            }
        }
    }
}
