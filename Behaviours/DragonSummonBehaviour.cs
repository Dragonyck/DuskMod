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
    public class DragonSummonBehaviour : MonoBehaviour
    {
        public Summon summon;
        public SoundEffectSO blackDragonSpawn = Prefabs.blackDragonSpawn;
        public SoundEffectSO blackDragonAttack = Prefabs.blackDragonAttack;
        public ParticleSystem hatchParticles;
        public ShootingSummon shootingSummon;
        public int burnDamage = 3;
        public float size = 0.75f;
        public float baseEvolutionSize = 0.75f;
        public float evolution1Size = 1.8f;
        public float evolution2Size = 3;
        public Sprite sprite;
        public bool canEvolve = false;
        public int currentEvolution = 0;
        public float evolutionSpeed;
        public float evolutionTime = 30;
        public float evolutionStopwatch;
        public Animator animator;
        public Orbital orbit;
        public void Start()
        {
            animator = base.GetComponent<Animator>();
            orbit = base.GetComponentInParent<Orbital>();
            this.AddObserver(new Action<object, object>(this.OnImpact), Summon.SummonOnHitNotification);
            //base.GetComponent<SpriteRenderer>().sprite = Assets.MainAssetBundle.LoadAsset<Sprite>("ElderDragonSS");
            Prefabs.flamethrowerProjectile.transform.localScale = Vector2.one * size;
            shootingSummon = summon.GetComponent<ShootingSummon>();
            shootingSummon.numProjectiles = 1;
            shootingSummon.projectilePrefab = Prefabs.flamethrowerProjectile;
            shootingSummon.baseDamage = 0;
            shootingSummon.attackCooldown = 0.65f;
            shootingSummon.projectileSpeed = 22;
            shootingSummon.inheritPlayerDamage = true;
            shootingSummon.OP.AddObject(shootingSummon.projectilePrefab.name, shootingSummon.projectilePrefab, 50, true);
            shootingSummon.pierce = 999;
        }
        public void FixedUpdate()
        {
            if (canEvolve)
            {
                evolutionSpeed += Time.fixedDeltaTime / evolutionTime;
                if (currentEvolution == 0)
                {
                    if (orbit)
                    {
                        orbit.radius = Mathf.Lerp(1, 1.5f, evolutionSpeed);
                    }
                    size = Mathf.Lerp(baseEvolutionSize, evolution1Size, evolutionSpeed);
                }
                else
                {
                    if (orbit)
                    {
                        orbit.radius = Mathf.Lerp(1.5f, 2, evolutionSpeed);
                    }
                    size = Mathf.Lerp(evolution1Size, evolution2Size, evolutionSpeed);
                }
                evolutionStopwatch += Time.fixedDeltaTime;
                if (evolutionStopwatch >= evolutionTime)
                {
                    evolutionStopwatch = 0;
                    currentEvolution++;
                    evolutionSpeed = 0;
                    if (currentEvolution == 1)
                    {
                        Destroy(Instantiate(Prefabs.fireTransformationEffect, base.transform.position, Quaternion.identity, ObjectPooler.SharedInstance.transform), 0.3f);
                        blackDragonSpawn.Play();
                        if (animator)
                        {
                            animator.runtimeAnimatorController = Prefabs.blackDragonAnimator;
                        }
                        shootingSummon.projectilePrefab = Prefabs.blackDragonProjectile;
                        shootingSummon.OP.AddObject(shootingSummon.projectilePrefab.name, shootingSummon.projectilePrefab, 50, true);
                        shootingSummon.shooter.onShoot.AddListener(delegate() 
                        { 
                            if (UnityEngine.Random.Range(0, 100) <= 35)
                            {
                                blackDragonAttack.Play();
                            }
                        });
                    }
                    else
                    {
                        var effect = Instantiate(Prefabs.fireTransformationEffect, base.transform.position, Quaternion.identity, ObjectPooler.SharedInstance.transform);
                        effect.transform.localScale = Vector2.one * 4;
                        Destroy(effect, 0.3f);
                        Instantiate(Prefabs.elderDragon, base.transform.position, Quaternion.identity);
                        Destroy(base.transform.parent.gameObject);
                    }
                }
            }
            if (shootingSummon)
            {
                base.transform.parent.localScale = Vector2.one * size;
            }
        }
        public void OnImpact(object sender, object args)
        {
            if ((sender as Summon) != summon)
            {
                return;
            }
            GameObject gameObject = args as GameObject;
            if (gameObject.tag.Contains("Enemy"))
            {
                BurnSystem.SharedInstance.Burn(gameObject, burnDamage);
            }
        }
        public void OnDestroy()
        {
            this.RemoveObserver(new Action<object, object>(this.OnImpact), Summon.SummonOnHitNotification);
        }
    }
}
