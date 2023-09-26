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
    class StatPanelBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI gunName;
        public TextMeshProUGUI charName;
        public TextMeshProUGUI gunStats;
        public string _gunStats;
        public TextMeshProUGUI charStats;
        public string _charStats;
        public float stopwatch;

        public void Start()
        {
            _gunStats = gunStats.text;
            _charStats = charStats.text;
            UpdateStats();
        }
        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= 1)
            {
                UpdateStats();
            }
        }
        public void UpdateStats()
        {
            PlayerController player = PlayerController.Instance;
            Gun gun = PlayerController.Instance.gun;
            gunName.text = gun.gunData.nameString;
            gunStats.text = string.Format(_gunStats,
                Mathf.FloorToInt(gun.damage),
                Math.Round((decimal)gun.shotCooldown, 2),
                gun.numOfProjectiles,
                gun.reloadDuration,
                player.stats[StatType.Knockback].Modify(gun.gunData.knockback),
                Mathf.Max(0, (int)player.stats[StatType.Bounce].Modify((float)gun.gunData.bounce)),
                Mathf.Max(0, (int)player.stats[StatType.Piercing].Modify((float)gun.gunData.piercing)),
                player.stats[StatType.ProjectileSpeed].Modify(gun.gunData.projectileSpeed),
                gun.spread,
                gun.stats[StatType.ProjectileSize].Modify(1f),
                Math.Round((decimal)player.stats[Prefabs.criticalChance]._multiplierBonus * 100, 0),
                Math.Round((decimal)player.stats[Prefabs.criticalDamage]._multiplierBonus * 100, 0)
                );

            charName.text = player.loadedCharacter.nameString;
            charStats.text = string.Format(_charStats,
                player.stats[StatType.SummonDamage].Modify(1f),
                player.stats[StatType.SummonAttackSpeed].Modify(1f),
                player.playerHealth.maxHP,
                player.playerHealth.maxSHP,
                player.finalMoveSpeed,
                player.stats[StatType.WalkSpeed].Modify(1f),
                player.stats[StatType.Dodge].Modify(1f),
                player.stats[StatType.DodgeCapMod].Modify(1f),
                player.stats[StatType.PickupRange].Modify(1f),
                player.stats[StatType.VisionRange].Modify(1f),
                player.stats[StatType.CharacterSize].Modify(1f),
                ScoreCalculator.SharedInstance._enemiesKilled
                );
        }
        public void OnDisable()
        {
            if (!PlayerController.Instance)
            {
                return;
            }
        }
    }
}
