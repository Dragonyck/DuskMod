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
    class DragonBallPickupBehaviour : MonoBehaviour
    {
        public SoundEffectSO pickupSFX = Prefabs.dragonballPickupSFX;
        public void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.gameObject.GetComponent<PlayerHealth>())
            {
                pickupSFX.Play();
                MinimapBehaviour.instance.minimapDragonballs[DragonBallBehaviour.instance.collectedDragonballs].gameObject.SetActive(true);
                DragonBallBehaviour.instance.collectedDragonballs++;
                Destroy(Instantiate(Prefabs.dragonballPickupFX, base.transform.position, Quaternion.identity, ObjectPooler.SharedInstance.transform), 0.3f);
                Destroy(base.gameObject);
            }
        }
    }
}
