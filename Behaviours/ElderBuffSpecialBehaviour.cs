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
    class ElderBuffSpecial : MonoBehaviour
    {
        public GameObject scaler;
        public float radius = 2;
        public float scale = 1.4f;
        public float healthMult = 0.5f;
        public float speedMult = 0.15f;
        public float stopwatch;
        public void Start()
        {
            scaler = new GameObject("scaler");
            scaler.transform.SetParent(ObjectPooler.SharedInstance.transform);
            scaler.transform.localPosition = Vector2.zero;
            scaler.transform.localScale = Vector2.one * scale;
        }
		public void FixedUpdate()
		{
			stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= 1)
			{
				stopwatch = 0;
				BuffNearby();
            }
            scaler.transform.localScale = Vector2.one * scale;
        }
		public void BuffNearby()
        {
            foreach (Collider2D c in Physics2D.OverlapCircleAll(base.transform.position, radius, 1 << TagLayerUtil.Enemy))
            {
                if (c.gameObject != base.gameObject)
                {
                    EnemyTimedBuffBehaviour buff = c.gameObject.GetComponent<EnemyTimedBuffBehaviour>();
                    if (!buff)
                    {
                        buff = c.gameObject.AddComponent<EnemyTimedBuffBehaviour>();
                        buff.scalerTransform = scaler.transform;
                        buff.healthMult = healthMult;
                        buff.speedMult = speedMult;
                    }
                    else
                    {
                        buff.speedMult = speedMult;
                        buff.healthMult = healthMult;
                        buff.duration++;
                        buff.UpdateBuff();
                    }
                }
            }
        }
        public void OnDisable()
        {
            Destroy(this);
        }
    }
}
