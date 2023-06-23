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
            localizedEN.Add(reaperDescriptionToken, "Start with +3 Soul Hearts. Trigger on kill effects twice.");

            localizedEN.Add(reaperPU1NameToken, "Soul Harvester");
            localizedEN.Add(reaperPU1DescriptionToken, "Collect your enemies' souls. Every 13th kill, there's a small chance to gain +50% on a random stat for 1 second. The chance is influenced by souls.");
        }
    }
}
