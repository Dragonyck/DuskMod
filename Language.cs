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
    public class Language
    {
        public static Dictionary<string, string> localizedEN;
        public static readonly string modKey = "duskmod";
        public static readonly string PU1Key = "_powerup1";
        public static readonly string PU2Key = "_powerup2";
        public static readonly string PU3Key = "_powerup3";
        public static readonly string PU4Key = "_powerup4";
        public static readonly string SynKey = "_synergy";
        public static readonly string StatKey = "_stat";

        public static readonly string reaperKey = "_reaper";
        public static readonly string reaperNameToken = modKey + reaperKey + "_name";
        public static readonly string reaperDescriptionToken = modKey + reaperKey + "_description";
        public static readonly string reaperPU1NameToken = modKey + reaperKey + PU1Key + "_name";
        public static readonly string reaperPU1DescriptionToken = modKey + reaperKey + PU1Key + "_description";
        public static readonly string reaperPU2NameToken = modKey + reaperKey + PU2Key + "_name";
        public static readonly string reaperPU2DescriptionToken = modKey + reaperKey + PU2Key + "_description";
        public static readonly string reaperPU3NameToken = modKey + reaperKey + PU3Key + "_name";
        public static readonly string reaperPU3DescriptionToken = modKey + reaperKey + PU3Key + "_description";

        public static readonly string catacombsKey = "_catacombs";
        public static readonly string catacombsNameToken = modKey + catacombsKey + "_name";
        public static readonly string catacombsDescriptionToken = modKey + catacombsKey + "_description";

        public static readonly string fmg9EvoKey = "_fmg9_evo";
        public static readonly string fmg9Key = "_fmg9";
        public static readonly string fmg9NameToken = modKey + fmg9Key + "_name";
        public static readonly string fmg9DescriptionToken = modKey + fmg9Key + "_description";
        public static readonly string fmg9ExMagNameToken = modKey + fmg9EvoKey + "_exmag_name";
        public static readonly string fmg9ExMagDescriptionToken = modKey + fmg9EvoKey + "_exmag_description";
        public static readonly string fmg9RedDotNameToken = modKey + fmg9EvoKey + "_reddot_name";
        public static readonly string fmg9RedDotDescriptionToken = modKey + fmg9EvoKey + "_reddot_description";
        public static readonly string fmg9ExRoundNameToken = modKey + fmg9EvoKey + "_exround_name";
        public static readonly string fmg9ExRoundDescriptionToken = modKey + fmg9EvoKey + "_exround_description";

        public static readonly string bleedKey = "_bleed";
        public static readonly string bleedPU1NameToken = modKey + bleedKey + PU1Key + "_name";
        public static readonly string bleedPU1DescriptionToken = modKey + bleedKey + PU1Key + "_description";
        public static readonly string bleedPU2NameToken = modKey + bleedKey + PU2Key + "_name";
        public static readonly string bleedPU2DescriptionToken = modKey + bleedKey + PU2Key + "_description";
        public static readonly string bleedPU3NameToken = modKey + bleedKey + PU3Key + "_name";
        public static readonly string bleedPU3DescriptionToken = modKey + bleedKey + PU3Key + "_description";
        public static readonly string bleedPU4NameToken = modKey + bleedKey + PU4Key + "_name";
        public static readonly string bleedPU4DescriptionToken = modKey + bleedKey + PU4Key + "_description";

        public static readonly string poisonKey = "_poison";
        public static readonly string poisonPU1NameToken = modKey + poisonKey + PU1Key + "_name";
        public static readonly string poisonPU1DescriptionToken = modKey + poisonKey + PU1Key + "_description";
        public static readonly string poisonPU2NameToken = modKey + poisonKey + PU2Key + "_name";
        public static readonly string poisonPU2DescriptionToken = modKey + poisonKey + PU2Key + "_description";
        public static readonly string poisonPU3NameToken = modKey + poisonKey + PU3Key + "_name";
        public static readonly string poisonPU3DescriptionToken = modKey + poisonKey + PU3Key + "_description";
        public static readonly string poisonPU4NameToken = modKey + poisonKey + PU4Key + "_name";
        public static readonly string poisonPU4DescriptionToken = modKey + poisonKey + PU4Key + "_description";

        public static readonly string critKey = "_critical";
        public static readonly string critPU1NameToken = modKey + critKey + PU1Key + "_name";
        public static readonly string critPU1DescriptionToken = modKey + critKey + PU1Key + "_description";
        public static readonly string critPU2NameToken = modKey + critKey + PU2Key + "_name";
        public static readonly string critPU2DescriptionToken = modKey + critKey + PU2Key + "_description";
        public static readonly string critPU3NameToken = modKey + critKey + PU3Key + "_name";
        public static readonly string critPU3DescriptionToken = modKey + critKey + PU3Key + "_description";
        public static readonly string critPU4NameToken = modKey + critKey + PU4Key + "_name";
        public static readonly string critPU4DescriptionToken = modKey + critKey + PU4Key + "_description";

        public static readonly string headHunterKey = "_headhunter";
        public static readonly string headHunterNameToken = modKey + headHunterKey + SynKey + "_name";
        public static readonly string headHunterDescriptionToken = modKey + headHunterKey + SynKey + "_description";

        public static readonly string critChanceNameToken = modKey + critKey + StatKey + "_name";
        public static readonly string critDamageNameToken = modKey + critKey + "_damage" + StatKey + "_name";

        public static void Init()
        {
            Language.localizedEN = LocalizationSystem.GetDictionaryForEditor();
            Language.localizedEN["vengeful_ghost_description"] = Language.localizedEN["vengeful_ghost_description"].Replace("1", "2");
            Language.localizedEN["dual_smg_name"] = "P226 Akimbo";
            Language.localizedEN["dual_smg_description"] = Language.localizedEN["dual_smg_description"].Replace(" Triggers on-shoot effects twice.", "");
            Language.localizedEN["dragon_egg_description"] = "Summon a Baby Dragon.";
            Language.localizedEN["aged_dragon_name"] = "Eternal Dragon";
            Language.localizedEN["aged_dragon_description"] = "Enemies killed by your Dragon have a " + ColorText("7%") + " chance to drop a " + ColorText("Dragon Ball") + ". Share 5% of your stats with your Dragon for each " + ColorText("Dragon Ball") + " collected. Collecting all 7 summons a " + ColorText("Eternal Dragon", "fd5161") + ", that grants you a wish.";
            AddLanguage();
        }
        public static void AddLanguage()
        {
            AddKey(reaperNameToken, "Reaper");
            AddKey(reaperDescriptionToken, "Start with " + ColorText("+3 Soul Hearts", "fd5161") + ". Trigger on kill effects " + ColorText("twice", "fd5161") + ".");
            AddKey(reaperPU1NameToken, "Soul Harvester");
            AddKey(reaperPU1DescriptionToken, "Collect your enemies' " + ColorText("souls") + ". Every " + ColorText("13th") + " kill, there's a small chance to gain " + ColorText("+50%") + " on a random stat for " + ColorText("1") + " second. The chance is influenced by " + ColorText("souls") + ".");
            AddKey(reaperPU2NameToken, "Putrid Carcass");
            AddKey(reaperPU2DescriptionToken, "Enemies have a " + ColorText("13%") + " chance drop a " + ColorText("carcass") + " on death if they were afflicted by a " + ColorText("debuff") + ". It " + ColorText("bursts") + " when stepped on, " + ColorText("inflicting") + " all nearby enemies with the same " + ColorText("debuffs") + ".");
            AddKey(reaperPU3NameToken, "Dead Heart");
            AddKey(reaperPU3DescriptionToken, "Kill " + ColorText("666") + " enemies to " + ColorText("activate") + ". When active, prevents death " + ColorText("once") + ", instantly " + ColorText("kills") + " nearby non-boss enemies and gain " + ColorText("base") + " Soul Hearts. " + ColorText("Deactivates") + " when triggered.");

            AddKey(catacombsNameToken, "Catacombs");
            AddKey(catacombsDescriptionToken, "");

            AddKey(fmg9NameToken, "FMG9 Akimbo");
            AddKey(fmg9DescriptionToken, "High fire rate submachine guns.");
            AddKey(fmg9ExMagNameToken, "Extended Magazines");
            AddKey(fmg9ExMagDescriptionToken, "Ammo is reloaded over time instead, scaling with your " + ColorText("Reload Speed") + " bonus multipliers.");
            AddKey(fmg9RedDotNameToken, "Laser Sights");
            AddKey(fmg9RedDotDescriptionToken, "Gain Laser Sight attachments for precise shots.");
            AddKey(fmg9ExRoundNameToken, "Explosive Rounds");
            AddKey(fmg9ExRoundDescriptionToken, "Bullets explode on contact, dealing " + ColorText("35%") + " of your Bullet Damage as " + ColorText("Burn") + " Damage to nearby enemies.");

            AddKey(bleedPU1NameToken, "Open Wounds");
            AddKey(bleedPU1DescriptionToken, "Your bullets have a " + ColorText("20%") + " chance to " + ColorText("permanently") + " inflict " + ColorText("Bleed", "b03838") + ", dealing " + ColorText("5%") + " of the enemy's " + ColorText("Max HP") + " per second.");
            AddKey(bleedPU2NameToken, "Rupture");
            AddKey(bleedPU2DescriptionToken, "Bullets that pierce a " + ColorText("bleeding", "b03838") + " enemy will inflict " + ColorText("Bleed", "b03838") + " on every subsequent enemy, while also increasing the " + ColorText("Bleed", "b03838") + " damage by " + ColorText("50%") + ".");
            AddKey(bleedPU3NameToken, "Hemorrhage");
            AddKey(bleedPU3DescriptionToken, ColorText("Bleed", "b03838") + "now stacks, reducing it's damage interval by half. " + "Bullets deal " + ColorText("+25%") + "damage as "+ ColorText("Bleed", "b03838") + " damage to bleeding enemies.");
            AddKey(bleedPU4NameToken, "Life Leech");
            AddKey(bleedPU4DescriptionToken, "Heal " + ColorText("1") + " HP for every 500 enemies killed by " + ColorText("Bleed", "b03838") + ". If you're full HP, increase your Max HP instead.");

            AddKey(poisonPU1NameToken, "Blight");
            AddKey(poisonPU1DescriptionToken, "Summon damage has a " + ColorText("40%") + " chance to inflict " + ColorText("Poison", "59fd51") + " for " + ColorText("6") + " seconds. " + ColorText("Poison", "59fd51") + " deals " + ColorText("10") + " summon damage over a second, ticking" + ColorText("8") + " times.");
            AddKey(poisonPU2NameToken, "Corrosion");
            AddKey(poisonPU2DescriptionToken, ColorText("Poisoned", "59fd51") + " enemies receive " + ColorText("+50%") + " summon damage. ");
            AddKey(poisonPU3NameToken, "Acid Downpour");
            AddKey(poisonPU3DescriptionToken, "Double " + ColorText("Poison", "59fd51") + " duration and damage.");
            AddKey(poisonPU4NameToken, "Smog");
            AddKey(poisonPU4DescriptionToken, "Summon damage has a " + ColorText("60%") + " chance to " + ColorText("instantly") + " deal the remaining " + ColorText("Poison", "59fd51") + " damage. Enemies killed by " + ColorText("Poison", "59fd51") + " leave behind a cloud of smog.");

            AddKey(critPU1NameToken, "Critical Strike");
            AddKey(critPU1DescriptionToken, "Your bullets have a " + ColorText("5%") + " chance to deal double damage.");
            AddKey(critPU2NameToken, "BuckShot");
            AddKey(critPU2DescriptionToken, "Critical hits deal " + ColorText("+50%") + " damage.");
            AddKey(critPU3NameToken, "Artillery Shrapnel");
            AddKey(critPU3DescriptionToken, "Critical hits inflict " + ColorText("Bleed", "b03838") + ". " + ColorText("Bleed", "b03838") + " damage increases critical chance by " + ColorText("5%") + ", resetting after landing a critical hit.");
            AddKey(critPU4NameToken, "Jacketed Hollow Point");
            AddKey(critPU4DescriptionToken, "Critical hits on " + ColorText("bleeding", "b03838") + " enemies cause a " + ColorText("Bleed", "b03838") + " explosion for " + ColorText("100%") + " bullet damage.");

            AddKey(headHunterNameToken, "Headhunter");
            AddKey(headHunterDescriptionToken, "Every " + ColorText("5") + " seconds a random enemy is tagged, bosses have a higher chance to be tagged." + Environment.NewLine + " Bullets have " + ColorText("100%") + " chance to crit tagged enemies. Gain " + ColorText("1%") + " critical damage after killing a tagged enemy.");

            AddKey(critChanceNameToken, "Critical Chance");
            AddKey(critDamageNameToken, "Critical Damage");
        }
        public static void AddKey(string key, string value)
        {
            LocalizationSystem.localizedEN.Add(key, value);
            LocalizationSystem.localizedJP.Add(key, value);
            LocalizationSystem.localizedCH.Add(key, value);
            LocalizationSystem.localizedBR.Add(key, value);
            LocalizationSystem.localizedTC.Add(key, value);
            LocalizationSystem.localizedRU.Add(key, value);
            LocalizationSystem.localizedSP.Add(key, value);
            LocalizationSystem.localizedGR.Add(key, value);
            LocalizationSystem.localizedPL.Add(key, value);
            LocalizationSystem.localizedIT.Add(key, value);
            LocalizationSystem.localizedTR.Add(key, value);
            LocalizationSystem.localizedFR.Add(key, value);
            LocalizationSystem.localizedKR.Add(key, value);
            LocalizationSystem.localizedHU.Add(key, value);
        }
        public static string ColorText(string textToColor, string color = "f5d6c1")
        {
            string newString = "<color=#" + color + ">" + textToColor + "</color>";
            return newString;
        }
    }
}
