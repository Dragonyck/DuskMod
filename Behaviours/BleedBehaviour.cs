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
    class BleedBehaviour : MonoBehaviour
    {
        public GameObject bleedFX = Prefabs.bleedFX;
        public Health target;
        public float basePercentage = 0.05f;
        public int stacks = 1;
        public float healthPercentage
        {
            get
            {
                return 0.05f * stacks;
            }
        }
        public int damage
        {
            get
            {
                return target ? Mathf.CeilToInt(target.maxHP * healthPercentage) : 0;
            }
        }
        public float stopwatch;
        public void Start()
        {
            target = base.GetComponent<Health>();
        }
        public void FixedUpdate()
        {
            if (target)
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= 1 / stacks)
                {
                    stopwatch = 0;
                    base.gameObject.PostNotification(BleedManager.DamageEvent, target);
                    if (target.HP <= damage)
                    {
                        base.gameObject.PostNotification(BleedManager.KillEvent, target);
                    }
                    target.TakeDamage(Prefabs.bleed, damage);
                    Destroy(Instantiate(bleedFX, target.transform.position, Quaternion.identity, target.transform), 0.25f);
                }
            }
        }
        public void OnDisable()
        {
            BleedManager.instance.Dispiriodize(target);
            Destroy(this);
        }
    }
}
