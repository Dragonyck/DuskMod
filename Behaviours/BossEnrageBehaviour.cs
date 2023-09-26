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
using HarmonyLib;
using UnityEngine.TextCore;
using TMPro;
using UnityEngine.Events;
using flanne.PerkSystem.Actions;

namespace DuskMod
{
    class BossEnrageBehaviour : MonoBehaviour
    {
        public int useCount = 0;
        public bool addedUse = false;
        public AIComponent ai;
        public ElderBuffSpecial elder;
        public void Awake()
        {
            ai = base.GetComponent<AIComponent>();
            elder = base.GetComponent<ElderBuffSpecial>();
            if (elder)
            {
                elder.healthMult = 1;
                elder.speedMult = 0.4f;
                elder.scale = 1.8f;
                elder.radius = 3.5f;
            }
        }
        public void Update()
        {
            if (!ai || elder)
            {
                return;
            }
            if (ai.specialTimer > 0)
            {
                if (!addedUse)
                {
                    addedUse = true;
                    useCount++;
                }
            }
            else if (addedUse)
            {
                addedUse = false;
            }
            if (useCount == 2)
            {
                useCount = 0;
                ai.specialTimer = 0;
            }
        }
        public void OnDisable()
        {
            Destroy(this);
        }
    }
}
