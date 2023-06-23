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
using HarmonyLib;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace DuskMod
{
    [BepInPlugin(MODUID, MODNAME, VERSION)]
    internal class MainPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.Dragonyck.DuskMod";
        public const string MODNAME = "DuskMod";
        public const string VERSION = "1.0.0";

        private GunData gun;
        private bool akimboShot = false;
        private GameController gameController;
        private GameObject rerollButtonObject;

        internal static readonly bool debug = true;

        private void Awake()
        {
            Assets.PopulateAssets();
            Language.Init();
            Prefabs.Init();
            GunChanges();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            On.flanne.PowerupGenerator.InitPowerupPool += PowerupGenerator_InitPowerupPool;
            On.flanne.SummonEgg.Start += SummonEgg_Start;
        }

        private void Update()
        {
            if (!debug)
            {
                return;
            }
            if (rerollButtonObject)
            {
                rerollButtonObject.SetActive(true);
            }
        }

        private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode arg1)
        {
            if (debug)
            {
                Debug.LogWarning("Scene Loaded: " + scene.name);
            }
            GameObject[] rootObjects = scene.GetRootGameObjects();
            switch (scene.name)
            {
                case "TitleScreen":
                    GameObject titleScreenControllerObject = Array.Find<GameObject>(rootObjects, x => x.name == "TitleScreenController");
                    if (titleScreenControllerObject)
                    {
                        TitleScreenController titleController = titleScreenControllerObject.GetComponent<TitleScreenController>();

                        CharacterMenu characterMenu = titleController ? titleController.characterMenu : null;
                        if (characterMenu)
                        {
                           CharacterIconUI characterIconUI = characterMenu.gameObject.GetComponentInChildren<CharacterIconUI>();
                            if (characterIconUI)
                            {
                                GameObject reaper = Instantiate(characterIconUI.gameObject, characterIconUI.transform.parent);
                                reaper.name = "Reaper";
                                reaper.transform.Find("ShanaVictories").gameObject.SetActive(false);
                                var ui = reaper.GetComponent<CharacterIconUI>();
                                ui.animator.gameObject.SetActive(false);
                                ui.animator = Instantiate(Prefabs.reaperAnimator, ui.transform).GetComponent<Animator>();
                                ui.animator.transform.localScale = Vector2.one * 40;
                                int CharacterIndex = ui.transform.GetSiblingIndex();

                                ui.GetComponent<Button>().onClick.AddListener(delegate ()
                                {
                                    Loadout.CharacterSelection = Prefabs.reaperData;
                                    Loadout.CharacterIndex = CharacterIndex;
                                    characterMenu.Hide();
                                    titleController.ChangeState<GunSelectState>();
                                    PlayerPrefs.SetInt("LastCharacterSelected", CharacterIndex);
                                });

                                CharacterDescription characterDescription = characterMenu.GetComponentInChildren<CharacterDescription>();
                                if (characterDescription)
                                {
                                    ButtonExtension buttonExtension = ui.GetComponent<ButtonExtension>();
                                    buttonExtension.onPointerEnter.AddListener(delegate ()
                                    {
                                        ui.animator.SetTrigger("Run");
                                        characterDescription.SetProperties(Prefabs.reaperData);
                                    });
                                    buttonExtension.onPointerExit.AddListener(delegate ()
                                    {
                                        ui.animator.SetTrigger("Idle");
                                    });
                                }

                                if (titleController.selectPanel)
                                {
                                    var characterPortrait = titleController.selectPanel.transform.Find("CharacterPortrait");
                                    if (characterPortrait)
                                    {
                                    }
                                }
                            }
                        }

                        Transform canvasTransform = characterMenu ? titleController.characterMenu.transform.parent : null;
                        Transform scelectScreenBG = canvasTransform ? canvasTransform.Find("SelectScreenBG") : null;
                        Transform botBG = (scelectScreenBG ? scelectScreenBG.Find("BGBot") : null);
                        if (botBG)
                        {
                            botBG.gameObject.SetActive(false);
                        }
                    }
                    break;
                case "Battle":
                    if (debug)
                    {
                        GameObject gameControllerObject = Array.Find<GameObject>(rootObjects, x => x.name == "GameController");
                        gameController = gameControllerObject ? gameControllerObject.GetComponent<GameController>() : null;
                        rerollButtonObject = gameController ? gameController.powerupRerollButton ? gameController.powerupRerollButton.gameObject : null : null;
                    }
                    //UnityEngine.Object.Instantiate(GameObject.Find("PF_Dragon"), PlayerController.Instance.transform);
                    break;
            }
        }

        private void SummonEgg_Start(On.flanne.SummonEgg.orig_Start orig, SummonEgg self)
        {
            self.summon.gameObject.SetActive(true);

            DragonSummonBehaviour summonBehaviour = self.summon.gameObject.AddComponent<DragonSummonBehaviour>();
            summonBehaviour.summon = self.summon;
            summonBehaviour.sound = self.soundFX;
            summonBehaviour.hatchParticles = self.hatchParticles;

            self.gameObject.SetActive(false);
        }

        void GunChanges()
        {
            Language.localizedEN["dual_smg_name"] = "Akimbo";
            Language.localizedEN["dragon_egg_description"] = "Summon a Baby Dragon.";
            foreach (GunData p in Prefabs.guns)
            {
                switch (p.name)
                {
                    case "DualSMGsData":
                        gun = p;
                        p.shotCooldown = 0.125f;
                        break;
                    case "FlameCannon":
                        Prefabs.flamethrowerProjectile = Prefabs.Instantiate(p.bullet);
                        break;
                }
            }

            On.flanne.Gun.Update += Gun_Update;
        }

        private void PowerupGenerator_InitPowerupPool(On.flanne.PowerupGenerator.orig_InitPowerupPool orig, PowerupGenerator self, int numTimesRepeatable)
        {
            orig(self, numTimesRepeatable);
            foreach (PowerupPoolItem p in self.powerupPool)
            {
                switch (p.powerup.name)
                {
                    case "":
                        break;
                }
            }
        }

        private void Gun_Update(On.flanne.Gun.orig_Update orig, Gun self)
        {
            if (self.gunData == gun)
            {
				if (!PauseController.isPaused)
				{
					if (self._shotTimer > 0f)
					{
						self._shotTimer -= Time.deltaTime;
						return;
					}
					if (self.isShooting)
                    {
                        int shooterIndex = akimboShot ? 1 : 0;
                        self.Shoot(self.shooters[shooterIndex], self.AimDirection());
                        akimboShot ^= true;
					}
				}
			}
            else
            {
                orig(self);
            }
        }
    }
}
