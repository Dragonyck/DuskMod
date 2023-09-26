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
using flanne.Player.Buffs; 

namespace DuskMod
{
    public class EnemyTimedBuffBehaviour : MonoBehaviour
    {
        public Transform scalerTransform;
        public float duration = 1.1f;
        public float healthMult = 0;
        public float speedMult = 0;
        public float stopwatch;
        public Health health;
        public int ogHealth;
        public int ogMaxHP;
        public AIComponent ai;
        public SpriteRenderer sprite;
        public Material buffMaterial = Prefabs.redBuffMat;
        public Material ogMaterial;
        public PropertyInfo property;

        public void Start()
        {
            base.transform.SetParent(scalerTransform);
            AddBuff();
        }
        public void AddBuff()
        {
            if (healthMult > 0)
            {
                health = base.GetComponent<Health>();
                if (health)
                {
                    ogHealth = health.HP;
                    ogMaxHP = health.maxHP;
                    health.maxHP += Mathf.CeilToInt(ogMaxHP * healthMult);
                    property = health.GetType().GetProperty("HP");
                    if (property != null)
                    {
                        property.SetValue(health, Mathf.CeilToInt(ogHealth + (ogHealth * healthMult)));
                    }
                }
            }
            sprite = base.GetComponent<SpriteRenderer>();
            if (sprite)
            {
                ogMaterial = sprite.material;
                sprite.material = buffMaterial;
            }
            if (speedMult != 0)
            {
                ai = base.GetComponent<AIComponent>();
                if (ai)
                {
                    ai.maxMoveSpeed += ai.maxMoveSpeed * speedMult;
                    ai.acceleration += ai.acceleration * speedMult;
                }
            }
        }
        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= duration)
            {
                RemoveBuff();
            }
        }
        public void UpdateBuff()
        {
            RemoveBuff(false);
            AddBuff();
        }
        public void RemoveBuff(bool destroy = true)
        {
            if (healthMult > 0 && health)
            {
                if (property != null)
                {
                    property.SetValue(health, ogHealth);
                }
                health.maxHP = ogMaxHP;
            }
            if (sprite)
            {
                sprite.material = ogMaterial;
            }
            if (speedMult != 0 && ai)
            {
                ai.maxMoveSpeed = ai.baseMaxMoveSpeed;
                ai.acceleration = ai.baseAcceleration;
            }
            if (destroy)
            {
                base.transform.SetParent(ObjectPooler.SharedInstance.transform);
                Destroy(this);
            }
        }
        public void OnDisable()
        {
            RemoveBuff();
        }
        public void OnDestroy()
        {
            RemoveBuff();
        }
    }
}
