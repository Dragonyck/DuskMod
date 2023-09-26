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
    class NoReloadTrigger : flanne.PerkSystem.Triggers.OnReloadTrigger
    {
        public override void OnEquip(PlayerController player)
        {
            base.OnEquip(player);
            if (player.ammo)
            {
                player.ammo.Reload();
            }
            if (!player.GetComponent<ReloadOverTimeBehaviour>())
            {
                player.gameObject.AddComponent<ReloadOverTimeBehaviour>();
            }
            On.flanne.Gun.StopShooting += Gun_StopShooting;
        }
        private void Gun_StopShooting(On.flanne.Gun.orig_StopShooting orig, Gun self)
        {
            orig(self);
            base.RaiseTrigger(PlayerController.Instance.gameObject);
        }
        public override void OnUnEquip(PlayerController player)
        {
            base.OnUnEquip(player);
            On.flanne.Gun.StopShooting -= Gun_StopShooting;
        }
    }
}
