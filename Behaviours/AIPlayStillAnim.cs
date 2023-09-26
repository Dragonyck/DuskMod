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
    public class AIPlayStillAnim : MonoBehaviour
    {
        public Animator animator;
        public MoveComponent2D move2D;
        private bool walk;

        public void Start()
        {
            animator = base.GetComponent<Animator>();
            move2D = base.GetComponent<MoveComponent2D>();
        }
        public void FixedUpdate()
        {
            if (move2D && animator)
            {
                var magnitude = move2D.vector.magnitude;
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
    }
}
