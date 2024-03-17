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
    public class EntityState : State
    {
        public float deltaTime = 0;
        public float fixedDeltaTime = 0;
        public EntityStateMachine.Components components;
        public virtual float duration => 0;
        public override void Enter()
        {
            base.Enter();
            this.enabled = true;
            components = base.GetComponent<EntityStateMachine>().components;
            deltaTime = 0;
            fixedDeltaTime = 0;
        }
        public override void Exit()
        {
            deltaTime = 0;
            fixedDeltaTime = 0;
            this.enabled = false;
            base.Exit();
        }
        public virtual void FixedUpdate()
        {
            fixedDeltaTime += Time.fixedDeltaTime;
        }
        public virtual void Update()
        {
            deltaTime += Time.deltaTime;
        }
    }
}
