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
using System.Collections;

namespace DuskMod
{
    class PointAtCursor : MonoBehaviour
    {
        public ShootingCursor cursor;
        public void Start()
        {
            cursor = ShootingCursor.Instance;
        }
        public void Update()
        {
            if (cursor && !PauseController.isPaused)
            {
                Vector2 a = Camera.main.ScreenToWorldPoint(cursor.cursorPosition);
                Vector2 b = base.transform.position;
                Vector2 vector = a - b;
                float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
                base.transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
            }
        }
    }
}
