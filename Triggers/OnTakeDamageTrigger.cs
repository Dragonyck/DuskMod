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
    public class OnTakeDamageTrigger : Trigger
	{
		public DamageType damageType;
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(Trigger), string.Format(Prefabs.DamageTypeInflicted, damageType.ToString()));
		}
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(Trigger), string.Format(Prefabs.DamageTypeInflicted, damageType.ToString()));
		}
		private void Trigger(object sender, object args)
		{
			var h = args as Health;
			base.RaiseTrigger(h.gameObject);
		}
	}
}
