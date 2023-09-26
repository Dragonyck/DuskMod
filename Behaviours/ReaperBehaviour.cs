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
    public class ReaperBehaviour : MonoBehaviour
    {
        public int shpIncrease = 3;
        public UnityEvent onLethalDeath = new UnityEvent();
        public UnityEvent onDeathPrevented = new UnityEvent();
        public UnityAction onGiveUp;
        public bool deadDead = false;
        public bool giveUp = false;
        public bool preventDeath = false;
        public GameController gameController;
        public SoundEffectSO deathPreventSound = Prefabs.preventDeathSFX;

        public void Start()
        {
            onDeathPrevented.AddListener(new UnityAction(VeryveryDeathActualDeath));
            onGiveUp = () => giveUp = true;

            this.AddObserver(new Action<object, object>(OnKill), Health.DeathEvent);

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
                gameController.giveupButton.onClick.AddListener(onGiveUp);
            }
            On.flanne.Core.PlayerDeadState.Enter += PlayerDeadState_Enter;
        }
        void PreventDeath()
        {
            preventDeath = false;
            deathPreventSound.Play();
            PlayerController.Instance.playerHealth.shp += 3;
            foreach (Collider2D c in Physics2D.OverlapCircleAll(base.transform.position, 8, 1 << TagLayerUtil.Enemy))
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
        public void PlayerDeadState_Enter(On.flanne.Core.PlayerDeadState.orig_Enter orig, PlayerDeadState self)
        {
            if (!deadDead && !giveUp)
            {
                self.owner.ChangeState<CombatState>();
            }
            else
            {
                if (giveUp)
                {
                    onLethalDeath.Invoke();
                }
                orig(self);
            }
        }
        public void Update()
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
        public void OnDestroy()
        {
            if (gameController)
            {
                gameController.giveupButton.onClick.RemoveListener(onGiveUp);
            }
            this.RemoveObserver(new Action<object, object>(OnKill), Health.DeathEvent);
            On.flanne.Core.PlayerDeadState.Enter -= PlayerDeadState_Enter;
        }
		public void OnKill(object sender, object args)
        {
            if ((sender as Health).gameObject.tag == "Enemy" && args == null)
            {
                (sender as Health).PostNotification(Health.DeathEvent, this);
            }
        }
	}
}
