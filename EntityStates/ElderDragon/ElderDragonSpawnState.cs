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
    class ElderDragonSpawnState : BaseSpawnState
    {
        public SoundEffectSO spawnSound = Prefabs.elderDragonSpawn;
        public override void Enter()
        {
            base.Enter();
            if (spawnSound)
            {
                spawnSound.Play();
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }
        public override void SetNextState()
        {
            components.machine.ChangeState<ElderDragonBaseState>();
        }
    }
}
