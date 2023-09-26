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
    public class NotificationTrigger : Trigger
    {
		public string notification;
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(Trigger), notification, PlayerController.Instance.gameObject);
		}
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(Trigger), notification, PlayerController.Instance.gameObject);
		}
		private void Trigger(object sender, object args)
		{
			Debug.LogError("NOTIFICATION TRIGGERED");
			base.RaiseTrigger(args as GameObject);
		}
	}
}
