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
    class ElderDragonFireFireFireballsState : AITargetState
    {
        public ElderDragonBaseState baseState;
        public SoundEffectSO fireSound = Prefabs.elderDragonAttack;
        public int maxProjectiles = 3;
        public int projectileCount = 0;
        public float projectileSpeed = 8;
        public bool firing = false;
        public SoundEffectSO actionSound = Prefabs.elderDragonAttack;
        public override void Enter()
        {
            base.Enter();
            baseState = base.GetComponent<ElderDragonBaseState>();
            if (actionSound)
            {
                actionSound.Play();
            }
            components.animator.SetTrigger("Special");
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedDeltaTime >= 0.25f)
            {
                fixedDeltaTime = 0;
                if (projectileCount < maxProjectiles)
                {
                    projectileCount++;
                    if (projectileCount == maxProjectiles)
                    {
                        components.animator.SetTrigger("SpecialEnd");
                    }
                    FireFireFireball();
                }
                else
                {
                    components.machine.ChangeState<ElderDragonBaseState>();
                }
            }
        }
        public override void Exit()
        {
            base.Exit();
            projectileCount = 0;
        }
        public void FireFireFireball()
        {
            if (!baseState.target || !baseState.firePos)
            {
                return;
            }
            var direction = baseState.target.transform.position - base.transform.position;
            float angle = 90f / (float)maxProjectiles;
            Vector3 forward = Quaternion.AngleAxis(angle * (float)projectileCount, Vector3.forward) * direction;

            Vector2 firePos = baseState.firePos ? baseState.firePos.transform.position : base.transform.position;

            Projectile proj = Instantiate(Prefabs.elderDragonProjectileFireball, firePos, Quaternion.identity, ObjectPooler.SharedInstance.transform).GetComponent<Projectile>();
            proj.vector = forward.normalized * projectileSpeed;
            proj.angle = Mathf.Atan2(forward.y, forward.x) * 57.29578f;
            proj.size = 1 * baseState.dragonballMult;
            proj.damage = components.player.stats[StatType.SummonDamage].Modify(baseState.dragonballMult);
            proj.knockback = components.player.stats[StatType.Knockback].Modify(baseState.dragonballMult);
            proj.bounce = 0;
            proj.piercing = Mathf.CeilToInt(components.player.stats[StatType.Piercing].Modify(0));
            proj.owner = base.gameObject;

        }
    }
}
