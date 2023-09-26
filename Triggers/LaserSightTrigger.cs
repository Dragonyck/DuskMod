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
    public class LaserSightTrigger : Trigger
    {
        public override void OnEquip(PlayerController player)
        {
            if (!player)
            {
                player = PlayerController.Instance;
            }
            if (player.gun)
            {
                foreach (Shooter s in player.gun.shooters)
                {
                    if (s.gameObject.GetComponent<LineRenderer>())
                    {
                        return;
                    }
                    var line = s.gameObject.AddComponent<LineRenderer>();
                    line.material = Prefabs.spriteMaterial;
                    line.startColor = Prefabs.colorDict["red"].Item1;
                    line.endColor = Prefabs.colorDict["red"].Item1;
                    line.widthMultiplier = 0.03f;
                    s.gameObject.AddComponent<LaserSightBehaviour>();
                }
            }
        }
        public override void OnUnEquip(PlayerController player)
        {
            if (!player)
            {
                player = PlayerController.Instance;
            }
            if (player.gun)
            {
                foreach (Shooter s in player.gun.shooters)
                {
                    if (!s.GetComponent<LineRenderer>())
                    {
                        return;
                    }
                    UnityEngine.Object.Destroy(s.GetComponent<LineRenderer>());
                    UnityEngine.Object.Destroy(s.GetComponent<LaserSightBehaviour>());
                }
            }
        }
    }
    public class EmptyAction : flanne.PerkSystem.Action
    {
        public override void Activate(GameObject target)
        {
        }
    }
}
