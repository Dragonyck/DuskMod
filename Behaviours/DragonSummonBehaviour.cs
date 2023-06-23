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
    internal class DragonSummonBehaviour : MonoBehaviour
    {
        public Summon summon;
        public SoundEffectSO sound;
        public ParticleSystem hatchParticles;
        private ShootingSummon shootingSummon;
        private bool isElder = false;
        private float size = 0.75f;
        private Sprite sprite;
        private void Start()
        {
            this.AddObserver(new Action<object, object>(this.OnImpact), Summon.SummonOnHitNotification);
            //base.GetComponent<SpriteRenderer>().sprite = Assets.MainAssetBundle.LoadAsset<Sprite>("ElderDragonSS");
            Prefabs.flamethrowerProjectile.transform.localScale = Vector2.one * size;
            shootingSummon = summon.GetComponent<ShootingSummon>();
            shootingSummon.numProjectiles = 1;
            shootingSummon.projectilePrefab = Prefabs.flamethrowerProjectile;
            shootingSummon.baseDamage = 0;
            shootingSummon.attackCooldown = 0.65f;
            shootingSummon.projectileSpeed = 22;
            shootingSummon.inheritPlayerDamage = true;
            shootingSummon.OP.AddObject(shootingSummon.projectilePrefab.name, shootingSummon.projectilePrefab, 50, true);
            shootingSummon.pierce = 999;
        }
        private void FixedUpdate()
        {
            if (shootingSummon)
            {
                shootingSummon.transform.localScale = Vector2.one * size;
            }
        }
        private void OnImpact(object sender, object args)
        {
            if ((sender as Summon) != summon)
            {
                return;
            }
            GameObject gameObject = args as GameObject;
            if (gameObject.tag.Contains("Enemy"))
            {
                BurnSystem.SharedInstance.Burn(gameObject, 3);
            }
        }
        private void OnDestroy()
        {
            this.RemoveObserver(new Action<object, object>(this.OnImpact), Summon.SummonOnHitNotification);
        }
    }
}
