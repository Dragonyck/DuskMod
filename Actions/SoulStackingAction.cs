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
using flanne.Player.Buffs;

namespace DuskMod
{
    public class SoulStackingAction : flanne.PerkSystem.Action
    {
        public int souls = 0;
        public int soulStack = 0;
        public int min = 800;
        public float statMult = 0.5f;
        public float buffDuration = 1;
        public override void Init()
        {
            base.Init();
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
            if (!PlayerController.Instance)
            {
                return;
            }
            StatsHolder statsHolder = PlayerController.Instance.GetComponentInChildren<StatsHolder>();
            if (statsHolder)
            {
                Array stats = Enum.GetValues(typeof(StatType));
                StatType stat = (StatType)stats.GetValue(UnityEngine.Random.Range(0, stats.Length - 1));
                PlayerBuffs playerBuffs = PlayerController.Instance.GetComponentInChildren<PlayerBuffs>();
                if (playerBuffs)
                {
                    TemporaryStatBuff buff = new TemporaryStatBuff();
                    buff.statChanges = new StatChange[]
                    {
                        new StatChange
                        {
                            type = stat,
                            value = statMult,
                            isFlatMod = false
                        }
                    };
                    buff.duration = buffDuration;
                    playerBuffs.Add(buff);
                }
            }
        }
    }
}
