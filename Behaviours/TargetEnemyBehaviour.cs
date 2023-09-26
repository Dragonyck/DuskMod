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
    public class TargetEnemyBehaviour : MonoBehaviour
    {
        public float cooldown = 5;
        public float stopwatch;
        public GameObject tagInstance;
        public void FixedUpdate()
        {
            if (!tagInstance || tagInstance && !tagInstance.transform.parent.gameObject.activeInHierarchy)
            {
                stopwatch += Time.fixedDeltaTime;
            }
            if (stopwatch >= cooldown)
            {
                stopwatch = 0;
                List<Collider2D> colliders = Physics2D.OverlapCircleAll(base.transform.position, 20, 1 << TagLayerUtil.Enemy).ToList();
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.IsBoss())
                    {
                        colliders.Add(c);
                    }
                }
                colliders.RemoveAll(x => x.gameObject.IsPassiveEnemy());
                Collider2D selection = colliders[UnityEngine.Random.Range(0, colliders.Count + 1)];
                if (!tagInstance)
                {
                    tagInstance = Instantiate(Prefabs.headhunterTag, selection.transform.position, Quaternion.identity, selection.transform);
                }
                else
                {
                    tagInstance.transform.position = selection.transform.position;
                    tagInstance.transform.SetParent(selection.transform);
                }
            }
        }
    }
}
