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
    class DragonBallDropAction : flanne.PerkSystem.Action
    {
        public int chance = 7;
        public override void Init()
        {
            base.Init();
            if (!DragonBallBehaviour.instance)
            {
                PlayerController.Instance.gameObject.AddComponent<DragonBallBehaviour>();
            }
        }
        public override void Activate(GameObject target)
        {
            if (DragonBallBehaviour.instance.collectedDragonballs >= 7)
            {
                return;
            }
            if (UnityEngine.Random.Range(0, 100) <= chance)
            {
            }
            var dragonball = UnityEngine.Object.Instantiate(Prefabs.dragonball, target.transform.position, Quaternion.identity, ObjectPooler.SharedInstance.transform);
            dragonball.transform.GetChild(DragonBallBehaviour.instance.collectedDragonballs).gameObject.SetActive(true);
        }
    }
}
