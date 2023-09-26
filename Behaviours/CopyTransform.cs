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
    class CopyTransform : MonoBehaviour
    {
        public bool copyPosition;
        public bool copyLocalPosition;
        public bool copyRotation;
        public bool copyLocalRotation;
        public bool copyScale;

        public Transform target;

        public void FixedUpdate()
        {
            if (copyPosition)
            {
                base.transform.position = target.position;
            }
            if (copyLocalPosition)
            {
                base.transform.localPosition = target.localPosition;
            }
            if (copyRotation)
            {
                base.transform.rotation = target.rotation;
            }
            if (copyLocalRotation)
            {
                base.transform.localRotation = target.localRotation;
            }
            if (copyScale)
            {
                base.transform.localScale = target.localScale;
            }
        }
    }
}
