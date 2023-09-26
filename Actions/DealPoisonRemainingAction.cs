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
   public class DealPoisonRemainingAction : flanne.PerkSystem.Action
    {
        public override void Init()
        {
            base.Init();
        }
        public override void Activate(GameObject target)
        {
            var health = target.GetComponent<Health>();
            PoisonBehaviour poison = null;
            if (health && PoisonManager.instance.IsPoisoned(health, out poison))
            {
                poison.DealRemainingDamage();
            }
        }
    }
}
