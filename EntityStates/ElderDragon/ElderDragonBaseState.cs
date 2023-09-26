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
    class ElderDragonBaseState : AIWalkState
    {
        public float minActionTimer = 3;
        public int usedAction = 0;
        public int minActionForSpecial = 2;
        public Transform firePos;
        public int dragonBalls = 0;
        public float actionCD = 4;
        public float dragonballMult
        {
            get
            {
                return dragonBalls > 0 ? 1 + (dragonBalls * 0.1f) : 1;
            }
        }
        public override float size()
        {
            return 3;
        }
        public override void Enter()
        {
            base.Enter();
            firePos = base.transform.GetChild(0);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedDeltaTime >= actionCD)
            {
                if (!target)
                {
                    return;
                }
                if (usedAction < minActionForSpecial)
                {
                    usedAction++;
                    components.machine.ChangeState<ElderDragonFireFireFireballsState>();
                }
                else
                {
                    usedAction = 0;
                    components.machine.ChangeState<ElderDragonFireFireFirewaveState>();
                }
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void MoveObject()
        {
            base.MoveObject();
        }
    }
}
