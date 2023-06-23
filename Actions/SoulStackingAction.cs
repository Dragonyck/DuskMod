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
    internal class SoulStackingAction : flanne.PerkSystem.Action
    {
        private int souls = 0;
        private int soulStack = 0;
        private int min = 800;
        private float statMult = 0.5f;
        public override void Init()
        {
            base.Init();
            Debug.LogWarning("SoulStackingAction Equipped");
        }
        public override void Activate(GameObject target)
        {
            souls++;
            soulStack++;
            if (soulStack >= 13)
            {
                soulStack = 0;
                if (UnityEngine.Random.Range(0f, 101f) <= souls / min)
                {
                    ModStat();
                }
            }
        }
        void ModStat()
        {
            StatsHolder componentInChildren = PlayerController.Instance.GetComponentInChildren<StatsHolder>();
            if (componentInChildren)
            {
                Array stats = Enum.GetValues(typeof(StatType));
                componentInChildren[(StatType)stats.GetValue(UnityEngine.Random.Range(0, stats.Length + 1))].AddMultiplierBonus(statMult);
            }
        }
    }
}
