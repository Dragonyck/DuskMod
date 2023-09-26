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
    class ReloadOverTimeBehaviour : MonoBehaviour
    {
        public PlayerController player;
        public Ammo ammo;
        public bool reload = false;
        public float stopwatch;
        public void Awake()
        {
            player = PlayerController.Instance;
            ammo = player.ammo;
        }
        public void FixedUpdate()
        {
            if (!reload || !player || !ammo)
            {
                return;
            }
            stopwatch += Time.fixedDeltaTime;
            if (ammo && stopwatch >= 0.1f / (1 + player.stats[StatType.ReloadRate]._multiplierBonus))
            {
                stopwatch = 0;
                ammo.GainAmmo(1);
            }
        }
    }
}
