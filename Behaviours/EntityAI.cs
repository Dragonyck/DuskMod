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
using UnityEngine.TextCore;
using TMPro;

namespace DuskMod
{
    class EntityAI : MonoBehaviour
    {
        public GameObject target;
        public MoveComponent2D move;
        public Rigidbody2D rigidBody;
        public Animator animator;
        public float speed = 3.5f;
        public float size = 2;
        public float pushForce = 3;
        public bool enemy = false;
        public bool customTarget = false;
        public bool focusBosses = true;
        public SoundEffectSO spawnSound = Prefabs.elderDragonSpawn;
        public float distance
        {
            get
            {
                return target ? Vector2.Distance(base.transform.position, target.transform.position) : -999;
            }
        }
        public bool startupFinished;
        public bool inAction;
        public Transform firePos;
        public PlayerController player;
        public bool walk;

        protected virtual void Awake()
        {
            move = base.GetComponent<MoveComponent2D>();
            rigidBody = base.GetComponent<Rigidbody2D>();
            animator = base.GetComponent<Animator>();
            firePos = base.transform.GetChild(0);
            player = PlayerController.Instance;
        }
        protected virtual void Start()
        {
            CameraShake.CameraShaker.ShakeOn = true;
            base.transform.localScale = Vector2.one * size;
            if (spawnSound)
            {
                spawnSound.Play();
            }
            if (customTarget)
            {
                return;
            }
            if (!enemy)
            {
                FindTarget();
            }
            else
            {
                target = PlayerController.Instance.gameObject;
            }
        }
        public void Collide(Collision2D collision)
        {
            MoveComponent2D move2D = collision.collider.GetComponent<MoveComponent2D>();
            if (!move2D)
            {
                return;
            }
            if (collision.collider.gameObject.IsPassiveEnemy())
            {
                move.vector = move2D.vectorLastFrame.normalized * 3;
                return;
            }
            if (!enemy)
            {
                if (collision.collider.gameObject.IsEnemyOrBoss())
                {
                    move2D.vector = move.vectorLastFrame.normalized * pushForce;
                }
            }
        }
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Collide(collision);
        }
        protected virtual void OnBecameInvisible()
        {
            if (!enemy)
            {
                TeleportToPlayer();
            }
        }
        public void TeleportToPlayer(float radius = 3)
        {
            base.transform.position = (UnityEngine.Random.insideUnitCircle * radius + (Vector2)PlayerController.Instance.transform.position);
            target = null;
        }
        protected virtual void FixedUpdate()
        {
            if (!startupFinished)
            {
                startupFinished = !animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
                if (startupFinished)
                {
                    CameraShake.CameraShaker.ShakeOn = true;
                }
            }
            if (!target || target && !target.activeInHierarchy)
            {
                FindTarget();
            }
            MoveObject();
        }
        protected virtual void FindTarget()
        {
            if (!enemy && focusBosses)
            {
                if (BossHealthBarBehaviour.instance && BossHealthBarBehaviour.instance.bossHealthList.Count > 0)
                {
                    GameObject t = BossHealthBarBehaviour.instance.bossHealthList.FirstOrDefault().Item1.gameObject;
                    if (target != t)
                    {
                        target = t;
                    }
                    return;
                }
            }
            target = AIController.SharedInstance.GetNearestEnemy(base.transform.position);
        }
        protected virtual void SetTarget(GameObject t)
        {
            target = t;
        }
        protected virtual void MoveObject()
        {
            if (!startupFinished)
            {
                return;
            }
            if (!enemy)
            {
                if (customTarget)
                {
                    return;
                }
                FindTarget();
            }
            if (!move || !animator)
            {
                return;
            }
            if (target)
            {
                Vector2 direction = target.transform.position - base.transform.position;
                FlipDirection(direction);
                if (Vector3.Dot(move.vector, direction.normalized) < speed)
                {
                    move.vector += direction.normalized * speed * Time.fixedDeltaTime;
                }
                var magnitude = move.vector.magnitude;
                if (magnitude > 0.5f)
                {
                    if (!walk)
                    {
                        walk = true;
                        animator.SetBool("walk", walk);
                    }
                }
                else
                {
                    if (walk)
                    {
                        walk = false;
                        animator.SetBool("walk", walk);
                    }
                }
            }
        }
        protected virtual void FlipDirection(Vector2 direction)
        {
            if (direction.x < 0f)
            {
                base.transform.localScale = new Vector2(-size, size);
            }
            if (direction.x > 0f)
            {
                base.transform.localScale = Vector2.one * size;
            }
            if (inAction)
            {
                return;
            }
        }
    }
}
