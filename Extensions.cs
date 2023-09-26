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
using System.Collections;
using TMPro;

namespace DuskMod
{
    static public class Extensions
    {
        public static GameObject SpawnDamagePopup(this Health h, float damage, string color, int spriteIndex)
        {
            GameObject popup = UnityEngine.Object.Instantiate(Prefabs.statusDamagePopup, h.transform.position, Quaternion.identity, null);
            var tmpro = popup.GetComponent<TextMeshPro>();
            string dmgColor = Prefabs.colorDict[color].Item2;
            string sprite = "  <sprite=" + spriteIndex + ", tint=1></color>";
            tmpro.text = "<color=#" + dmgColor + ">" + Mathf.FloorToInt(damage) + sprite;
            return popup;
        }
        public static GameObject SpawnDamagePopup(this Health h, float damage, DamageType damageType)
        {
            bool regularDmg = damageType == DamageType.None || damageType == DamageType.Bullet;
            GameObject prefab = regularDmg ? Prefabs.regularDamagePopup : Prefabs.statusDamagePopup;
            GameObject popup = UnityEngine.Object.Instantiate(prefab, h.transform.position, Quaternion.identity, null);
            var tmpro = popup.GetComponent<TextMeshPro>();
            string dmgColor = regularDmg ? Prefabs.colorDict["white"].Item2 : Prefabs.colorDict[damageType].Item2;
            int spriteIndex = 0;
            switch (damageType)
            {
                case DamageType.None:
                case DamageType.Bullet:
                    spriteIndex = -1;
                    break;
                case DamageType.Burn:
                    spriteIndex = 3;
                    break;
                case DamageType.Curse:
                    spriteIndex = 6;
                    break;
                case DamageType.Gale:
                    spriteIndex = 7;
                    break;
                case DamageType.Glare:
                    spriteIndex = 8;
                    break;
                case DamageType.Smite:
                    spriteIndex = 4;
                    break;
                case DamageType.Summon:
                    spriteIndex = 9;
                    break;
                case DamageType.Thunder:
                    spriteIndex = 5;
                    break;
            }
            if (damageType == Prefabs.bleed)
            {
                spriteIndex = 2;
            }
            else if (damageType == Prefabs.poison)
            {
                spriteIndex = 1;
            }
            string sprite = regularDmg ? "</color>" : "  <sprite=" + spriteIndex + ", tint=1></color>";
            tmpro.text = "<color=#" + dmgColor + ">" + Mathf.FloorToInt(damage) + sprite;
            return popup;
        }
        public static GameObject RecolorProjectile(this GameObject obj, string name, Color color)
        {
            GameObject o = Prefabs.Instantiate(obj);
            o.name = name; 
            foreach (ParticleSystem p in o.GetComponentsInChildren<ParticleSystem>())
            {
                var main = p.GetComponent<ParticleSystem>().main;
                main.startColor = color;
            }
            return o;
        }
        public static GameObject GetNearestEnemy(this AIController finder, Vector2 center)
        {
            List<AIComponent> enemies = finder.enemies;
            GameObject result = null;
            float num = float.PositiveInfinity;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!(enemies[i].tag.Contains("Passive")) && enemies[i].gameObject.activeInHierarchy)
                {
                    Vector2 b = new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y);
                    float sqrMagnitude = (center - b).sqrMagnitude;
                    if (sqrMagnitude < num)
                    {
                        num = sqrMagnitude;
                        result = enemies[i].gameObject;
                    }
                }
            }
            return result;
        }
        public static void ResizeSprite(this SpriteRenderer s, Vector2 vector2, Sprite sprite = null)
        {
            s.drawMode = SpriteDrawMode.Sliced;
            s.size = vector2;
            if (sprite != null)
            {
                s.sprite = sprite;
            }
        }
        public static GameObject InstantiateProjectile(this GameObject obj,string name, float colliderScale, Sprite sprite, float spriteScale, Color particleColor1, Color particleColor2)
        {
            GameObject newGameObject = Prefabs.Instantiate(obj);
            newGameObject.name = name;
            var spriteRenderer = newGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.size = Vector2.one * spriteScale;
            newGameObject.transform.localScale = Vector2.one;
            UnityEngine.Object.Destroy(newGameObject.GetComponent<DisableOnInvisible>());
            UnityEngine.Object.Destroy(newGameObject.GetComponent<SpawnOnCollision>());
            newGameObject.AddComponent<DestroyOnCollision>();
            newGameObject.AddComponent<DestroyOnInvisible>();
            newGameObject.GetComponent<MoveComponent2D>().drag = 0;
            foreach (ParticleSystem p in newGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                var main = p.GetComponent<ParticleSystem>().main;
                main.startColor = particleColor1;
            }
            newGameObject.GetComponent<CircleCollider2D>().radius = colliderScale;
            return newGameObject;
        }
        public static GameObject CreateEnemyProjectile(this GameObject obj, string name, float radius, Vector2 offset, float lifetime = 10)
        {
            GameObject newGameObject = Prefabs.Instantiate(obj);
            newGameObject.name = name;
            newGameObject.layer = 15;
            newGameObject.tag = "EProjectile";
            UnityEngine.Object.Destroy(newGameObject.GetComponent<Projectile>());
            UnityEngine.Object.Destroy(newGameObject.GetComponent<DisableOnInvisible>());
            newGameObject.AddComponent<DestroyOnCollision>();
            newGameObject.AddComponent<DestroyOnInvisible>();
            newGameObject.GetComponent<MoveComponent2D>().drag = 0;
            foreach (ParticleSystem p in newGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                var main = p.GetComponent<ParticleSystem>().main;
                main.startColor = Prefabs.colorDict["red"].Item1;
            }
            foreach (SpriteRenderer s in newGameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                s.color = Prefabs.colorDict["red"].Item1;
            }
            CircleCollider2D circle = newGameObject.GetComponent<CircleCollider2D>();
            if (circle)
            {
                circle.radius = radius;
                circle.offset = offset;
            }
            TimeToLive time = newGameObject.GetComponent<TimeToLive>();
            if (!time)
            {
                time = newGameObject.AddComponent<TimeToLive>();
            }
            time.lifetime = lifetime;
            return newGameObject;
        }
        public static GameObject CreateEnemy(this GameObject g, string name, RuntimeAnimatorController animator, Vector2 scale, float speed = 0)
        {
            GameObject newGameObject = Prefabs.Instantiate(g);
            newGameObject.name = name;
            newGameObject.GetComponent<Animator>().runtimeAnimatorController = animator;
            SpriteRenderer sprite = newGameObject.GetComponent<SpriteRenderer>();
            sprite.drawMode = SpriteDrawMode.Sliced;
            sprite.size = Vector2.one * scale;
            var childSprite = newGameObject.GetComponentsInChildren<SpriteRenderer>()[1];
            if (childSprite)
            {
                UnityEngine.Object.Destroy(childSprite);
            }
            if (speed != 0)
            {
                newGameObject.GetComponent<MoveComponent2D>().drag = speed;
            }
            return newGameObject;
        }
        public static bool IsCharacterPowerup(this PlayerController c, Powerup p)
        {
            if (c.loadedCharacter && c.loadedCharacter.exclusivePowerups && !c.loadedCharacter.exclusivePowerups.powerupPool.IsNullOrEmpty())
            {
                return c.loadedCharacter.exclusivePowerups.powerupPool.Any(x => x.nameString == p.nameString);
            }
            return false;
        }
        public static string BossName(this GameObject obj)
        {
            string name = obj.name.Replace("(Clone)", "").Replace("Monster", "").Replace("PF_", "").Replace("Boss", "");
            int upperIndex = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]))
                {
                    upperIndex = i;
                    break;
                }
            }
            string firstName = name.Substring(0, upperIndex);
            string lastName = name.Substring(upperIndex, name.Length - upperIndex);
            return (firstName + " " + lastName).ToUpper();
        }
        public static float CurrentHealthPercentage(this Health health)
        {
            return (float)health.HP / (float)health.maxHP;
        }
        public static bool IsBurning(this BurnSystem b, GameObject obj, out int damage)
        {
            BurnSystem.BurnTarget target = b._currentTargets.Find((BurnSystem.BurnTarget bt) => bt.target == obj);
            bool burn = target != null;
            damage = burn ? Mathf.FloorToInt(target.damage / b.burnDuration) : 0;
            Debug.LogWarning(damage);
            return burn;
        }
        public static bool IsPassiveEnemy(this GameObject obj)
        {
            return obj.tag.Contains("Passive");
        }
        public static bool IsEnemyOrBoss(this GameObject obj, bool ignorePassive = true)
        {
            bool anyEnemy = obj.tag.Contains("Enemy");
            if (ignorePassive)
            {
                return anyEnemy && !obj.tag.Contains("Passive");
            }
            return anyEnemy;
        }
        public static bool IsEnemy(this GameObject obj, bool ignorePassive = true)
        {
            bool enemy = obj.tag == "Enemy";
            if (ignorePassive)
            {
                return enemy && !obj.tag.Contains("Passive");
            }
            return enemy;
        }
        public static bool IsBoss(this GameObject obj)
        {
            return obj.tag.Contains("Champion");
        }
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return (list == null || list.Count == 0);
        }
        public static bool IsNullOrEmpty(this Array array)
        {
            return (array == null || array.Length == 0);
        }
        public static Vector2 AimDirection(this Gun gun)
        {
            Vector2 a = Camera.main.ScreenToWorldPoint(gun.SC.cursorPosition);
            Vector2 b = gun.transform.position;
            Vector2 pointDirection = a - b;
            return pointDirection;
        }
        public static void Shoot(this Gun gun, Shooter shooter, Vector2 direction)
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
