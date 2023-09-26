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
    class PoisonBehaviour : MonoBehaviour
    {
        public GameObject bleedFX = Prefabs.bleedFX;
        public Health target;
        public float stopwatch;
        public float durationStopwatch;
        public int baseDamage = 10;
        public float baseDuration;
        public float baseDamageCoefficient;
        public int ticksPerSec = 8;
        public GameObject poisonEffect;
        public StatMod summonDamageMod
        {
            get
            {
                return PlayerController.Instance ? PlayerController.Instance.stats[StatType.SummonDamage] : null;
            }
        }
        public float damage
        {
            get
            {
                return summonDamageMod.Modify((float)this.baseDamage);
            }
        }
        public void Start()
        {
            target = base.GetComponent<Health>();
            poisonEffect = UnityEngine.Object.Instantiate(Prefabs.critFX, base.transform.position, Quaternion.identity, base.transform);
        }
        public void FixedUpdate()
        {
            if (target)
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= 1 / ticksPerSec)
                {
                    stopwatch = 0;
                    base.gameObject.PostNotification(PoisonManager.DamageEvent, target);
                    if (MainPlugin.debug)
                    {
                        Debug.LogWarning("Poison Damage");
                    }
                    base.gameObject.PostNotification(PoisonManager.DamageEvent, target);
                    if (target.HP <= damage)
                    {
                        if (MainPlugin.debug)
                        {
                            Debug.LogWarning("Poison Kill");
                        }
                        base.gameObject.PostNotification(PoisonManager.KillEvent, target);
                    }
                    target.TakeDamage(Prefabs.poison, Mathf.FloorToInt(damage / ticksPerSec));
                }
            }
            durationStopwatch += Time.fixedDeltaTime;
            if (durationStopwatch >= baseDuration)
            {
                PoisonManager.instance.Dispoisonize(target);
                Destroy(poisonEffect);
                Destroy(this);
            }
        }
        public void DealRemainingDamage()
        {
            float remainingDamage = Mathf.FloorToInt(damage / ticksPerSec * (baseDuration * ticksPerSec));
            base.gameObject.PostNotification(PoisonManager.DamageEvent, target);
            if (MainPlugin.debug)
            {
                Debug.LogWarning("Dealt Remaining Poison Damage");
            }
            base.gameObject.PostNotification(PoisonManager.DamageEvent, target);
            if (target.HP <= remainingDamage)
            {
                if (MainPlugin.debug)
                {
                    Debug.LogWarning("Poison Kill");
                }
                base.gameObject.PostNotification(PoisonManager.KillEvent, target);
            }
            target.TakeDamage(Prefabs.poison, Mathf.FloorToInt(remainingDamage));
            PoisonManager.instance.Dispoisonize(target);
            Destroy(poisonEffect);
            Destroy(this);
        }
        public void OnDisable()
        {
            PoisonManager.instance.Dispoisonize(target);
            Destroy(poisonEffect);
            Destroy(this);
        }
    }
}
