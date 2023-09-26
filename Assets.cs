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
    public class Assets
    {
        public static AssetBundle MainAssetBundle = null;

        public static T Load<T>(string name)
        {
            object o = MainAssetBundle.LoadAsset(name, typeof(T));
            T e = (T)o;
            return e;
        }
        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(MainPlugin.MODNAME + "." + "assets"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }
            /*using (var bankStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(MainPlugin.MODNAME + "." + "Bomber.bnk"))
            {
                var bytes = new byte[bankStream.Length];
                bankStream.Read(bytes, 0, bytes.Length);
                SoundAPI.SoundBanks.Add(bytes);
            }*/
        }
    }
}
