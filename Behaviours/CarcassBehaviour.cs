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
   public class CarcassBehaviour : MonoBehaviour
    {
        public bool isBurn = false;
        public bool isCurse = false;
        public bool isFreeze = false;
        public int burnDamage = 3;
        public void OnCollisionEnter2D(Collision2D collider)
		{
			if (collider.gameObject.IsEnemyOrBoss())
            {
                foreach (Collider2D c in Physics2D.OverlapCircleAll(base.transform.position, 2, 1 << TagLayerUtil.Enemy))
                {
                    if (isBurn)
                    {
                        BurnSystem.SharedInstance.Burn(c.gameObject, burnDamage);
                    }
                    if (isCurse)
                    {
                        CurseSystem.Instance.Curse(c.gameObject);
                    }
                    if (isFreeze)
                    {
                        FreezeSystem.SharedInstance.Freeze(c.gameObject);
                    }
                }
                Destroy(base.gameObject);
            }
		}
	}
}
