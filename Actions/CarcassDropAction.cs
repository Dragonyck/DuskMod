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

namespace DuskMod
{
   public class CarcassDropAction : flanne.PerkSystem.Action
    {
        public int chance = 13;
        public override void Init()
        {
            base.Init();
        }
        public override void Activate(GameObject target)
        {
            if (UnityEngine.Random.Range(0, 100) <= chance)
            {
                bool isBurning = BurnSystem.SharedInstance.IsBurning(target);
                bool isCursed = CurseSystem.Instance.IsCursed(target);
                bool isFrozen = FreezeSystem.SharedInstance.IsFrozen(target);
                if (isBurning || isCursed || isFrozen)
                {
                    CarcassBehaviour c = UnityEngine.Object.Instantiate(Prefabs.carcass, target.transform.position, Quaternion.identity).GetComponent<CarcassBehaviour>();
                    c.isBurn = isBurning;
                    c.isCurse = isCursed;
                    c.isFreeze = isFrozen;

                    BoxCollider2D box = target.GetComponent<BoxCollider2D>();
                    CircleCollider2D circle = target.GetComponent<CircleCollider2D>();
                    float size = box ? Math.Max(box.size.x, box.size.y) / 2 : circle ? circle.radius : 1;
                    c.transform.localScale = Vector2.one * size / 0.16f;

                    UnityEngine.Object.Destroy(c.gameObject, 15);

                    c.transform.SetParent(ObjectPooler.SharedInstance.transform);
                }
            }
        }
    }
}
