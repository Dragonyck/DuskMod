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
    class LaserSightBehaviour : MonoBehaviour
    {
        public LineRenderer laser;
        public Transform dot;
        public Transform origin;
        public float distance = 10;
        public bool disOnly = false;
        public void Awake()
        {
            dot = Instantiate(Prefabs.fmg9RedDot, base.transform).transform;
            origin = new GameObject("origin").transform;
            origin.SetParent(base.gameObject.transform);
            origin.localPosition = new Vector2(-0.2f, 0);
            laser = base.GetComponent<LineRenderer>();
        }
        public void Update()
        {
            if (PauseController.isPaused || !laser || !dot)
            {
                return;
            }
            Vector2 origin = this.origin.position;
            laser.SetPosition(0, origin);
            Vector2 direction = base.transform.right;
            Vector2 point = Vector2.zero;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, 1 << TagLayerUtil.Enemy);//LayerMask.GetMask("Player", "PlayerPickupper", "DespawnRange", "PlayerProjectile", "PlayerProjectileMod", "Pickup")
            if (hit.collider && !disOnly)
            {
                point = hit.point;
            }
            else
            {
                point = origin + direction * distance;
            }
            laser.SetPosition(1, point);
            dot.transform.position = point;
        }
        public void OnDestroy()
        {
            Destroy(dot.gameObject);
            Destroy(origin.gameObject);
        }
    }
}
