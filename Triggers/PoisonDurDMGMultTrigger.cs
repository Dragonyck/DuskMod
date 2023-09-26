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
using flanne.PerkSystem.Triggers;

namespace DuskMod
{
    class PoisonDurDMGMultTrigger : Trigger
    {
        public override void OnEquip(PlayerController player)
        {
            if (PoisonManager.instance)
            {
                PoisonManager.instance.baseDamageMult = 2;
                PoisonManager.instance.baseDuration = 12;
            }
        }
        public override void OnUnEquip(PlayerController player)
        {
            if (PoisonManager.instance)
            {
                PoisonManager.instance.baseDamageMult = 1;
                PoisonManager.instance.baseDuration = 6;
            }
        }
    }
}
