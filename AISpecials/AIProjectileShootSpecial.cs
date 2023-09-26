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
    public class AIProjectileShootSpecial : AISpecial
	{
		public GameObject projectile;
		public float projectileSpeed = 4;
		public float projectileDelay = 0;
		public int projectileCount = 1;
		public SoundEffectSO sound;
		public Animator animator;
		public override void Use(AIComponent ai, Transform target)
		{
			animator = ai.GetComponent<Animator>();
			if (animator)
            {
				animator.SetTrigger("Special");
            }
			Vector3 direction = target.position - ai.specialPoint.position;
			ai.StartCoroutine(ShootCR(direction, ai.specialPoint.transform));
		}
		private IEnumerator ShootCR(Vector3 direction, Transform spawn)
		{
			int num;
			for (int i = 0; i < projectileCount; i = num + 1)
			{
				GameObject projectileInstance = Instantiate(projectile, ObjectPooler.SharedInstance.transform);
				projectileInstance.transform.position = spawn.position;
				projectileInstance.GetComponent<MoveComponent2D>().vector = projectileSpeed * direction.normalized;

				SoundEffectSO soundEffectSO = sound;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
				yield return new WaitForSeconds(projectileDelay);
				num = i;
			}
			yield break;
		}
	}
}
