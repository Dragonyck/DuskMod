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
    class AIWalkState : AITargetState
    {
        public virtual float speed() { return 3.5f; }
        public virtual float pushForce() { return 3; }
        public bool walk = false;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            MoveObject();
        }
        public virtual void MoveObject()
        {
            if (!components.move || !components.animator)
            {
                return;
            }
            if (target)
            {
                Vector2 direction = target.transform.position - base.transform.position;
                FlipDirection(direction);
                if (Vector3.Dot(components.move.vector, direction.normalized) < speed())
                {
                    components.move.vector += direction.normalized * speed() * Time.fixedDeltaTime;
                }
            }
            var magnitude = components.move.vector.magnitude;
            if (magnitude > 0.5f)
            {
                if (!walk)
                {
                    walk = true;
                    components.animator.SetBool("walk", walk);
                }
            }
            else
            {
                if (walk)
                {
                    walk = false;
                    components.animator.SetBool("walk", walk);
                }
            }
        }
    }
}
