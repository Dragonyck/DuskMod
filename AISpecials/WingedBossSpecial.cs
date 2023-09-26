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
    public class WingedBossSpecial : MonoBehaviour
	{
		public GameObject projectile;
		public float projectileSpeed = 4f;
		public float projectileDelay = 0;
		public int projectileCount = 4;
		public SoundEffectSO sound;
		public Animator animator;
		public float stopwatch;
		public void Start()
		{
			animator = base.GetComponent<Animator>();
		}
		public void FixedUpdate()
        {
			stopwatch += Time.fixedDeltaTime;
			if (stopwatch >= 5)
			{
				stopwatch = 0;
				Vector3 direction = PlayerController.Instance.transform.position - base.transform.position;
				Shoot(direction);
			}
        }
		public void Shoot(Vector3 direction)
		{
			if (animator)
			{
				animator.SetTrigger("Special");
			}
			float angle = 360f / (float)projectileCount;
			for (int i = 0; i < projectileCount; i++)
			{
				Vector3 forward = Quaternion.AngleAxis(angle * (float)i, Vector3.forward) * direction;

				GameObject projectileInstance = Instantiate(projectile, ObjectPooler.SharedInstance.transform);
				projectileInstance.transform.position = base.transform.position;
				projectileInstance.GetComponent<MoveComponent2D>().vector = projectileSpeed * forward.normalized;

				SoundEffectSO soundEffectSO = sound;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
			}
		}
	}
}
