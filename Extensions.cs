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
    static internal class Extensions
    {
        internal static OnKillTrigger NewKillTrigger(this OnKillTrigger trigger, int killsToTrigger, bool anyDamageType = true, DamageType damageType = DamageType.None)
        {
            trigger.killsToTrigger = killsToTrigger;
            trigger.anyDamageType = anyDamageType;
            trigger.damageType = damageType;
            return trigger;
        }
        internal static Vector2 AimDirection(this Gun gun)
        {
            Vector2 a = Camera.main.ScreenToWorldPoint(gun.SC.cursorPosition);
            Vector2 b = gun.transform.position;
            Vector2 pointDirection = a - b;
            return pointDirection;
        }
        internal static void Shoot(this Gun gun, Shooter shooter, Vector2 direction)
        {
            gun.SetAnimationTrigger("Attack");

            gun._shotTimer += gun.shotCooldown;

            ProjectileRecipe projectileRecipe = gun.GetProjectileRecipe();
            gun.PostNotification(Gun.ShootEvent, projectileRecipe);
            shooter.Shoot(projectileRecipe, direction, gun.numOfProjectiles, gun.spread, gun.gunData.inaccuracy);

            if (gun.gunData.gunshotSFX)
            {
                gun.gunData.gunshotSFX.Play();
            }
        }
    }
}
