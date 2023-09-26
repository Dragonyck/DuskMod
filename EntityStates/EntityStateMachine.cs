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
    public class EntityStateMachine : StateMachine
    {
        public struct Components
        {
            public Animator animator;
            public MoveComponent2D move;
            public Rigidbody2D rigidBody;
            public EntityStateMachine machine;
            public PlayerController player;
            public Components(GameObject obj)
            {
                machine = obj.GetComponent<EntityStateMachine>();
                animator = obj.GetComponent<Animator>();
                move = obj.GetComponent<MoveComponent2D>();
                rigidBody = obj.GetComponent<Rigidbody2D>();
                player = PlayerController.Instance;
            }
        }
        public Components components;
        public EntityState spawnState;
        public void Awake()
        {
        }
        public void Start()
        {
            components = new EntityStateMachine.Components(base.gameObject);
            spawnState.Enter();
            _currentState = spawnState;
        }
        public override void ChangeState<T>()
        {
            base.ChangeState<T>();
        }
    }
}
