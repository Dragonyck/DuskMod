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
    class DragonAI : EntityAI
    {
        public float actionStopwatch;
        public int actionCasts;
        public int maxActionCasts = 3;
        public float actionTimer;
        public float actionCooldown = 1.5f;
        public float projectileSpeed = 8;
        public int dragonBalls = 0;
        public float actionRange = 5;
        public SoundEffectSO actionSound = Prefabs.elderDragonAttack;
        public SoundEffectSO specialSound = Prefabs.elderDragonSpecial;
        public float dragonballMult
        {
            get
            {
                return dragonBalls > 0 ? 1 + (dragonBalls * 0.1f) : 1;
            }
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!startupFinished)
            {
                return;
            }
            actionStopwatch += Time.fixedDeltaTime;
            if (distance <= actionRange && actionStopwatch >= actionCooldown)
            {
                if (!inAction)
                {
                    inAction = true;
                    if (actionSound)
                    {
                        actionSound.Play();
                    }
                    animator.SetTrigger("Special");
                }
                actionTimer += Time.fixedDeltaTime;
                if (actionCasts < maxActionCasts)
                {
                    if (actionTimer >= 0.25f)
                    {
                        actionTimer = 0;
                        FireFireFireball();
                        actionCasts++;
                    }
                }
                else
                {
                    actionCasts = 0;
                    actionStopwatch = 0;
                    actionTimer = 0;
                    animator.SetTrigger("SpecialEnd");
                }
            }
            else if (inAction)
            {
                inAction = false;
            }
        }
        protected override void MoveObject()
        {
            if (inAction)
            {
                return;
            }
            base.MoveObject();
        }
        protected override void FlipDirection(Vector2 direction)
        {

            base.FlipDirection(direction);
        }
        public void FireFireFirewave()
        {
            if (!target)
            {
                return;
            }
            var direction = target.transform.position - base.transform.position;
            float num = 90f / (float)maxActionCasts;
            Vector3 forward = Quaternion.AngleAxis(num * (float)actionCasts, Vector3.forward) * direction;

            Vector2 firePos = this.firePos ? this.firePos.transform.position : base.transform.position;

            Projectile proj = Instantiate(Prefabs.elderDragonProjectileWave, firePos, Quaternion.identity, ObjectPooler.SharedInstance.transform).GetComponent<Projectile>();
            proj.vector = forward.normalized * projectileSpeed;
            proj.angle = Mathf.Atan2(forward.y, forward.x) * 57.29578f;
            proj.size = 1 * dragonballMult;
            proj.damage = player.stats[StatType.SummonDamage].Modify(dragonballMult);
            proj.knockback = player.stats[StatType.Knockback].Modify(dragonballMult);
            proj.bounce = 0;
            proj.piercing = 999;
            proj.owner = base.gameObject;

        }
        public void FireFireFireball()
        {
            if (!target)
            {
                return;
            }
            var direction = target.transform.position - base.transform.position;
            float angle = 90f / (float)maxActionCasts;
            Vector3 forward = Quaternion.AngleAxis(angle * (float)actionCasts, Vector3.forward) * direction;

            Vector2 firePos = this.firePos ? this.firePos.transform.position : base.transform.position;

            Projectile proj = Instantiate(Prefabs.elderDragonProjectileFireball, firePos, Quaternion.identity, ObjectPooler.SharedInstance.transform).GetComponent<Projectile>();
            proj.vector = forward.normalized * projectileSpeed;
            proj.angle = Mathf.Atan2(forward.y, forward.x) * 57.29578f;
            proj.size = 1 * dragonballMult;
            proj.damage = player.stats[StatType.SummonDamage].Modify(dragonballMult);
            proj.knockback = player.stats[StatType.Knockback].Modify(dragonballMult);
            proj.bounce = 0;
            proj.piercing = Mathf.CeilToInt(player.stats[StatType.Piercing].Modify(0));
            proj.owner = base.gameObject;

        }
    }
}
