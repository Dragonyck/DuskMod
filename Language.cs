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

namespace DuskMod
{
    internal class Language
    {
        internal static Dictionary<string, string> localizedEN;
        internal static readonly string modKey = "duskmod";

        internal static readonly string reaperKey = "_reaper";
        internal static readonly string reaperNameToken = modKey + reaperKey + "_name";
        internal static readonly string reaperDescriptionToken = modKey + reaperKey + "_description";

        internal static readonly string reaperPU1Key = "powerup1";
        internal static readonly string reaperPU1NameToken = modKey + reaperKey + reaperPU1Key + "_name";
        internal static readonly string reaperPU1DescriptionToken = modKey + reaperKey + reaperPU1Key + "_description";

        internal static readonly string reaperPU2Key = "powerup2";
        internal static readonly string reaperPU2NameToken = modKey + reaperKey + reaperPU2Key + "_name";
        internal static readonly string reaperPU2DescriptionToken = modKey + reaperKey + reaperPU2Key + "_description";

        internal static readonly string reaperPU3Key = "powerup3";
        internal static readonly string reaperPU3NameToken = modKey + reaperKey + reaperPU3Key + "_name";
        internal static readonly string reaperPU3DescriptionToken = modKey + reaperKey + reaperPU3Key + "_description";

        internal static void Init()
        {
            Language.localizedEN = LocalizationSystem.GetDictionaryForEditor();
            AddLanguage();
        }
        internal static void AddLanguage()
        {
            localizedEN.Add(reaperNameToken, "Reaper");
            localizedEN.Add(reaperDescriptionToken, "Start with " + ColorText("+3 Soul Hearts", "fd5161") + ". Trigger on kill effects " + ColorText("twice", "fd5161") + ".");

            localizedEN.Add(reaperPU1NameToken, "Soul Harvester");
            localizedEN.Add(reaperPU1DescriptionToken, "Collect your enemies' " + ColorText("souls") + ". Every " + ColorText("13th") + " kill, there's a small chance to gain " + ColorText("+50%") + " on a random stat for " + ColorText("1") + " second. The chance is influenced by " + ColorText("souls") + ".");

            localizedEN.Add(reaperPU2NameToken, "Putrid Carcass");
            localizedEN.Add(reaperPU2DescriptionToken, "Enemies drop a " + ColorText("carcass") + " on death if they were afflicted by a " + ColorText("debuff") + ". It " + ColorText("bursts") + " when stepped on, " + ColorText("inflicting") + " all nearby enemies with the same " + ColorText("debuffs") + ".");

            localizedEN.Add(reaperPU3NameToken, "Dead Heart");
            localizedEN.Add(reaperPU3DescriptionToken, "Kill " + ColorText("666") + " enemies to " + ColorText("activate") + ". When active, prevents death " + ColorText("once") + ", instantly " + ColorText("kills") + " nearby non-Boss enemies and gain " + ColorText("base") + " Soul Hearts. " + ColorText("Deactivates") + " when triggered.");



        }
        internal static string ColorText(string textToColor, string color = "f5d6c1")
        {
            string newString = "<color=#" + color + ">" + textToColor + "</color>";
            return newString;
        }
    }
}
