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
    class BaseSpawnState : EntityState
    {
        public bool startupFinished;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!startupFinished)
            {
                startupFinished = !components.animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
            }
            else
            {
                SetNextState();
                Destroy(this);
            }
        }
        public virtual void SetNextState()
        {
            components.machine.ChangeState<AIWalkState>();
        }
    }
}
