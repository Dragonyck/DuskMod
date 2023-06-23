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

namespace DuskMod
{
    internal class ReaperBehaviour : MonoBehaviour
    {
        private int shpIncrease = 3;
        private UnityEvent onLethalDeath;
        private bool deadDead = false;
        private GameController gameController;
        private void Start()
        {
            if (MainPlugin.debug)
            {
                PlayerController.Instance.playerPerks.Equip(Prefabs.reaperPU1);
            }
            this.AddObserver(new Action<object, object>(this.OnKill), Health.DeathEvent);

            PlayerController.Instance.playerHealth.maxSHP += shpIncrease;
            PlayerController.Instance.playerHealth.shp += shpIncrease;
            PlayerController.Instance.playerHealth.onHurt.AddListener(new UnityAction(OnHurt));

            Debug.LogWarning("1");
            PersistentCallGroup persistentCalls = PlayerController.Instance.playerHealth.onDeath.m_PersistentCalls;
            Debug.LogWarning("2");
            onLethalDeath.m_PersistentCalls = new PersistentCallGroup();
            onLethalDeath.m_PersistentCalls.m_Calls = new List<PersistentCall>();
            Debug.LogWarning("3");
            onLethalDeath.m_PersistentCalls.m_Calls.AddRange(persistentCalls.m_Calls);
            Debug.LogWarning("4");
            onLethalDeath.AddListener(new UnityAction(VeryveryDeathActualDeath));
            Debug.LogWarning("5");
            PlayerController.Instance.playerHealth.onDeath.m_PersistentCalls.Clear();
            Debug.LogWarning("6");

            gameController = FindObjectOfType<GameController>();
            Debug.LogWarning("7");

        }
        private void Update()
        {
            if (PlayerController.Instance && !PauseController.isPaused && !deadDead)
            {
                PlayerController.Instance.MovePlayer();
                PlayerController.Instance.UpdateSprite();
            }
        }
        void VeryveryDeathActualDeath()
        {
            deadDead = true;
            if (gameController)
            {
                CombatState state = gameController.CurrentState as CombatState;
                if (state)
                {
                    state.OnDeath();
                }
            }
        }
        void OnHurt()
        {
            if (PlayerController.Instance.playerHealth.shp == 0)
            {
                onLethalDeath.Invoke();
            }
            if (gameController)
            {
                CombatState state = gameController.CurrentState as CombatState;
                if (state)
                {
                    PlayerController.Instance.playerHealth.onDeath.RemoveListener(state.OnDeath);
                }
            }
        }
        private void OnDestroy()
        {
            this.RemoveObserver(new Action<object, object>(this.OnKill), Health.DeathEvent);
		}
		private void OnKill(object sender, object args)
        {
            if ((sender as Health).gameObject.tag == "Enemy" && args == null)
            {
                (sender as Health).PostNotification(Health.DeathEvent, this);
            }
        }
	}
}
