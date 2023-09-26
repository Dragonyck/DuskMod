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
    public class AITargetState : EntityState
    {
        public GameObject target;
        public bool enemy = false;
        public bool customTarget = false;
        public bool focusBosses = true;
        public float distance
        {
            get
            {
                return target ? Vector2.Distance(base.transform.position, target.transform.position) : -999;
            }
        }
        public virtual float size() { return 1; }
        public override void Enter()
        {
            base.Enter();
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
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!target || target && !target.activeInHierarchy)
            {
                FindTarget();
            }
        }
        public virtual void FindTarget()
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
        public virtual void FlipDirection(Vector2 direction)
        {
            if (direction.x < 0f)
            {
                base.transform.localScale = new Vector2(-size(), size());
            }
            if (direction.x > 0f)
            {
                base.transform.localScale = Vector2.one * size();
            }
        }
    }
}
