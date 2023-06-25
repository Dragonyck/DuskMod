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
        private UnityEvent onLethalDeath = new UnityEvent();
        internal UnityEvent onDeathPrevented = new UnityEvent();
        private bool deadDead = false;
        private bool giveUp = false;
        internal bool preventDeath = false;
        private GameController gameController;
        private SoundEffectSO deathPreventSound = Prefabs.preventDeathSFX;

        private void Start()
        {
            onDeathPrevented.AddListener(new UnityAction(VeryveryDeathActualDeath));

            this.AddObserver(new Action<object, object>(this.OnKill), Health.DeathEvent);

            PlayerController.Instance.playerHealth.maxSHP += shpIncrease;
            PlayerController.Instance.playerHealth.shp += shpIncrease;
            PlayerController.Instance.playerHealth.onHurt.AddListener(new UnityAction(OnHurt));

            PersistentCallGroup persistentCalls = PlayerController.Instance.playerHealth.onDeath.m_PersistentCalls;
            onLethalDeath.m_PersistentCalls.m_Calls.AddRange(persistentCalls.m_Calls);
            onLethalDeath.AddListener(new UnityAction(VeryveryDeathActualDeath));

            PlayerController.Instance.playerHealth.onDeath.m_PersistentCalls.Clear();

            gameController = FindObjectOfType<GameController>();
            if (gameController)
            {
                gameController.giveupButton.onClick.AddListener(delegate () { giveUp = true; });
            }
            On.flanne.Core.PlayerDeadState.Enter += PlayerDeadState_Enter;

            if (MainPlugin.debug)
            {
                PlayerController.Instance.playerPerks.Equip(Prefabs.reaperPU3);
            }
        }
        void PreventDeath()
        {
            preventDeath = false;
            deathPreventSound.Play();
            PlayerController.Instance.playerHealth.shp += 3;
            foreach (Collider2D c in Physics2D.OverlapCircleAll(base.transform.position, 8))
            {
                if (c.gameObject.IsEnemy() && !c.gameObject.IsBoss())
                {
                    var cursePrefab = CurseSystem.Instance.curseFXPrefab;
                    if (cursePrefab)
                    {
                        SpawnPrefabFromObjectPool sp = cursePrefab.GetComponent<SpawnPrefabFromObjectPool>();
                        if (sp)
                        {
                            Instantiate(sp.prefab, c.transform.position, Quaternion.identity, null);
                        }
                    }
                    c.GetComponent<Health>().AutoKill();
                }
            }
        }
        private void PlayerDeadState_Enter(On.flanne.Core.PlayerDeadState.orig_Enter orig, PlayerDeadState self)
        {
            if (!deadDead && !giveUp)
            {
                self.owner.ChangeState<CombatState>();
            }
            else
            {
                orig(self);
            }
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
                gameController.ChangeState<PlayerDeadState>();
            }
        }
        void OnHurt()
        {
            if (PlayerController.Instance.playerHealth.shp == 0)
            {
                if (preventDeath)
                {
                    PreventDeath();
                    return;
                }
                onLethalDeath.Invoke();
            }
            /*if (!deadDead && gameController)
            {
                CombatState state = gameController.CurrentState as CombatState;
                if (state)
                {
                    PlayerController.Instance.playerHealth.onDeath.RemoveListener(state.OnDeath);
                }
            }*/
        }
        private void OnDestroy()
        {
            this.RemoveObserver(new Action<object, object>(this.OnKill), Health.DeathEvent);
            On.flanne.Core.PlayerDeadState.Enter -= PlayerDeadState_Enter;
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
