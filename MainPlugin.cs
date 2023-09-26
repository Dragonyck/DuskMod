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
using UnityEngine.TextCore;
using TMPro;
using UnityEngine.Events;
using flanne.PerkSystem.Actions;
using System.Collections;
using MonoMod.Cil;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace DuskMod
{
    [BepInPlugin(MODUID, MODNAME, VERSION)]
    public class MainPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.Dragonyck.DuskMod";
        public const string MODNAME = "DuskMod";
        public const string VERSION = "1.0.0";

        public bool akimboShot = false;
        public static GameController gameController;
        public GameObject rerollButtonObject;
        public static bool createdPrefabs = false;

        public static readonly bool debug = true;

        public void Awake()
        {
            Assets.PopulateAssets();
            Language.Init();
            Prefabs.Init();
            GunChanges();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            On.flanne.SummonEgg.Start += SummonEgg_Start;
            On.flanne.Core.GameController.Start += GameController_Start;
            On.flanne.Health.OnEnable += Health_OnEnable;
            On.flanne.UI.PowerupListUI.OnPowerupApplied += PowerupListUI_OnPowerupApplied;
            On.flanne.UI.PowerupWidget.SetProperties += PowerupWidget_SetProperties;
            On.flanne.Shooter.Shoot += Shooter_Shoot;
            On.flanne.CurseSystem.Curse += CurseSystem_Curse;
            On.flanne.Health.TakeDamage += Health_TakeDamage;
            On.flanne.ObjectPooler.GetPooledObject += ObjectPooler_GetPooledObject;
            On.flanne.Core.GameController.Awake += GameController_Awake;
            On.flanne.PowerupGenerator.InitPowerupPool += PowerupGenerator_InitPowerupPool;
            On.flanne.StatsHolder.Awake += StatsHolder_Awake;
            On.flanne.DamagePopupSpawner.OnDamageTaken += DamagePopupSpawner_OnDamageTaken;
            IL.flanne.Core.CameraRig.Update += CameraRig_Update;
        }
        private void CameraRig_Update(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(x => x.MatchRet());
            c.RemoveRange(1);
        }   
        private void DamagePopupSpawner_OnDamageTaken(On.flanne.DamagePopupSpawner.orig_OnDamageTaken orig, DamagePopupSpawner self, int amount)
        {
            return;
        }
        private void StatsHolder_Awake(On.flanne.StatsHolder.orig_Awake orig, StatsHolder self)
        {
            orig(self);
            if (MainPlugin.debug)
            {
                Debug.LogWarning("DefaultStatModCount: " + self._data.Length);
            }
            var list = self._data.ToList();
            list.AddRange(Prefabs.statMods);
            self._data = list.ToArray();
            if (MainPlugin.debug)
            {
                Debug.LogWarning("StatModCountModded: " + self._data.Length);
            }
        }
        private void PowerupGenerator_InitPowerupPool(On.flanne.PowerupGenerator.orig_InitPowerupPool orig, PowerupGenerator self, int numTimesRepeatable)
        {
            orig(self, numTimesRepeatable);
            self.AddToPool(Prefabs.powerupsToPool, 1);
        }
        private void GameController_Awake(On.flanne.Core.GameController.orig_Awake orig, GameController self)
        {
            self.gameObject.AddComponent<MinimapBehaviour>();
            orig(self);
        }
        private GameObject ObjectPooler_GetPooledObject(On.flanne.ObjectPooler.orig_GetPooledObject orig, ObjectPooler self, string tag)
        {
            foreach (GameObject g in self.pooledObjectsDictionary["Chest"])
            {
                if (!MinimapBehaviour.instance.minimapIcons.ContainsKey(g))
                {
                    MinimapBehaviour.instance.minimapIcons.Add(g, Instantiate(Prefabs.minimapChestIcon, g.transform.position, Quaternion.identity, g.transform));
                }
            }
            foreach (GameObject g in self.pooledObjectsDictionary["DevilDeal"])
            {
                if (!MinimapBehaviour.instance.minimapIcons.ContainsKey(g))
                {
                    MinimapBehaviour.instance.minimapIcons.Add(g, Instantiate(Prefabs.minimapDevilPickupIcon, g.transform.position, Quaternion.identity, g.transform));
                }
            }
            foreach (GameObject g in self.pooledObjectsDictionary["SmallXP"])
            {
                if (!MinimapBehaviour.instance.minimapIcons.ContainsKey(g))
                {
                    MinimapBehaviour.instance.minimapIcons.Add(g, Instantiate(Prefabs.minimapXPIcon, g.transform.position, Quaternion.identity, g.transform));
                }
            }
            return orig(self, tag);
        }
        private void Health_TakeDamage(On.flanne.Health.orig_TakeDamage orig, Health self, DamageType damageType, int damage, float finalMultiplier)
        {
            CurseStackHolder stackHolder = self.GetComponent<CurseStackHolder>();
            if (stackHolder && damageType == DamageType.Curse)
            {
                damage *= stackHolder.stacks;
                Destroy(stackHolder);
            }
            if (self.gameObject.GetComponent<BossEnrageBehaviour>())
            {
                damage = Mathf.FloorToInt(damage * 0.8f);
            }
            orig(self, damageType, damage, finalMultiplier);
            self.PostNotification(string.Format(Prefabs.DamageTypeInflicted, damageType.ToString()), self);
            self.SpawnDamagePopup(damage * finalMultiplier, damageType);
        }
        private void CurseSystem_Curse(On.flanne.CurseSystem.orig_Curse orig, CurseSystem self, GameObject target)
        {
            Health component = target.GetComponent<Health>();
            if (component == null || self._cursedTargets.Contains(component))
            {
                CurseStackHolder stackHolder = target.GetComponent<CurseStackHolder>();
                if (!stackHolder)
                {
                    stackHolder = target.AddComponent<CurseStackHolder>();
                }
                stackHolder.stacks++;
            }
            orig(self, target);
        }
        private void Shooter_Shoot(On.flanne.Shooter.orig_Shoot orig, Shooter self, ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
        {
            if (recipe.objectPoolTag == "GhostPetProjectile_PF")
            {
                spread = spread / numProjectiles - 1;
            }
            orig(self, recipe, pointDirection, numProjectiles, spread, inaccuracy);
        }
        public void Update()
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
        private void PowerupWidget_SetProperties(On.flanne.UI.PowerupWidget.orig_SetProperties orig, PowerupWidget self, PowerupProperties properties)
        {
            orig(self, properties);
            if (PlayerController.Instance.IsCharacterPowerup(properties.powerup))
            {
                GameObject icon = self.icon.gameObject;
                Image border = Instantiate(icon, icon.transform.parent).GetComponent<Image>();
                border.sprite = Prefabs.powerupSprite;
            }
        }
        private void PowerupListUI_OnPowerupApplied(On.flanne.UI.PowerupListUI.orig_OnPowerupApplied orig, PowerupListUI self, object sender, object args)
        {
            orig(self, sender, args);
            if (PlayerController.Instance.IsCharacterPowerup(sender as Powerup))
            {
                if (self.transform.childCount > 0)
                {
                    var child = self.transform.GetChild(self.transform.childCount - 1);
                    if (child)
                    {
                        Image[] images = child.GetComponentsInChildren<Image>();
                        Image image = images.IsNullOrEmpty() ? null : images[1];
                        if (image)
                        {
                            Image border = Instantiate(image.gameObject, image.transform.parent).GetComponent<Image>();
                            border.sprite = Prefabs.powerupSprite;
                        }
                    }
                }
            }

        }
        private void GameController_Start(On.flanne.Core.GameController.orig_Start orig, GameController self)
        {
            orig(self);
            self.gameObject.AddComponent<EscToQuitPauseState>().controller = self;
            if (PlayerController.Instance)
            {
                PlayerController.Instance.gameObject.AddComponent<BleedManager>();
                PlayerController.Instance.gameObject.AddComponent<PoisonManager>();
            }
            if (self.hud)
            {
                self.hud.gameObject.AddComponent<BossHealthBarBehaviour>();
            }
        }
        private void Health_OnEnable(On.flanne.Health.orig_OnEnable orig, Health self)
        {
            orig(self);
            if (self.gameObject.IsBoss() && BossHealthBarBehaviour.instance)
            {
                if (MinimapBehaviour.instance)
                {
                    if (!MinimapBehaviour.instance.minimapIcons.ContainsKey(self.gameObject))
                    {
                        MinimapBehaviour.instance.minimapIcons.Add(self.gameObject, Instantiate(Prefabs.minimapBossIcon, self.transform.position, Quaternion.identity, self.transform));
                    }
                }

                BossHealthBarBehaviour.instance.NewHealthBar(self);

                var name = self.name;
                switch (name)
                {
                    case "Winged":
                        if (!self.gameObject.GetComponent<WingedBossSpecial>())
                        {
                            self.gameObject.AddComponent<WingedBossSpecial>().projectile = Prefabs.wingedBossProjectile;
                            self.gameObject.GetComponent<MoveComponent2D>().drag = 1.4f;
                        }
                        break;
                    case "Elder":
                        if (!self.gameObject.GetComponent<ElderBuffSpecial>())
                        {
                            self.gameObject.AddComponent<ElderBuffSpecial>();
                            self.maxHP *= 2;
                            var property = self.GetType().GetProperty("HP");
                            if (property != null)
                            {
                                property.SetValue(self, self.maxHP);
                            }
                        }
                        break;
                    case "Spawner":
                        var ai = self.GetComponent<AIComponent>();
                        ai.specialCooldown = 4;
                        ai.specialRangeSqr = 150;
                        break;
                }
            }
        }
        public void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode arg1)
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

                        if (titleController.gunMenu)
                        {
                            var gunMenu = titleController.gunMenu.GetComponent<GunMenu>();
                            var gun = gunMenu.GetComponentInChildren<GunIconUI>();
                            var fmg9 = Instantiate(gun.gameObject, gun.transform.parent);
                            fmg9.name = "FMG9";
                            var ui = fmg9.GetComponent<GunIconUI>();
                            ui.transform.Find("RevolverVictories").gameObject.SetActive(false);
                            int gunIndex = ui.transform.GetSiblingIndex();
                            Destroy(ui.GetComponentInChildren<Animator>().gameObject);
                            var fmg9Prefab = Instantiate(Prefabs.fmg9DisplayPrefab, ui.transform);
                            fmg9Prefab.transform.localPosition = Vector2.zero;
                            fmg9Prefab.GetComponent<RectTransform>().sizeDelta = Vector2.one * 30;
                            var animator = fmg9Prefab.GetComponent<Animator>();

                            ui.GetComponent<Button>().onClick.AddListener(delegate ()
                            {
                                Loadout.GunSelection = Prefabs.fmg9;
                                Loadout.GunIndex = gunIndex;
                                gunMenu.Hide();
                                titleController.ChangeState<ModeSelectState>();
                                PlayerPrefs.SetInt("LastGunSelected", gunIndex);
                            });

                            GunDescription gunDescription = gunMenu.GetComponentInChildren<GunDescription>();
                            if (gunDescription)
                            {
                                ButtonExtension buttonExtension = ui.GetComponent<ButtonExtension>();
                                buttonExtension.onSelect.AddListener(delegate ()
                                {
                                    animator.SetTrigger("Display");
                                    gunDescription.SetProperties(Prefabs.fmg9);
                                });
                                buttonExtension.onDeselect.AddListener(delegate ()
                                {
                                    animator.SetTrigger("Still");
                                });
                            }

                        }

                        if (titleController.mapSelectPanel)
                        {
                            HorizontalLayoutGroup mapGroup = titleController.mapSelectPanel.GetComponentInChildren<HorizontalLayoutGroup>();
                            if (mapGroup)
                            {
                                mapGroup.spacing = 6;
                                foreach (RectTransform t in mapGroup.GetComponentsInChildren<RectTransform>())
                                {
                                    string name = t.name;
                                    if (t.parent == mapGroup.transform)
                                    {
                                        t.sizeDelta = new Vector2(110, 90);
                                    }
                                    if (name == "Label")
                                    {
                                        t.localScale = Vector2.one * 0.85f;
                                    }
                                    else if (name == "Panel")
                                    {
                                        t.sizeDelta = Vector2.one;
                                    }
                                }

                            }
                            MapEntry entry = mapGroup.GetComponentInChildren<MapEntry>();
                            foreach (SpawnSession s in entry.data.spawnSessions)
                            {
                                Prefabs.catacombsData.spawnSessions.Add(s.Copy());
                            }
                            Prefabs.catacombsData.bossSpawns.AddRange(entry.data.bossSpawns);
                            if (entry && entry.data && entry.data.mapPrefab)
                            {
                                Prefabs.catacombsMapPrefab = Prefabs.Instantiate(entry.data.mapPrefab);
                                GameObject map = Prefabs.catacombsMapPrefab;
                                map.AddComponent<FogTintBehaviour>();
                                Destroy(map.GetComponent<ForestMapRhogogSpawner>());
                                map.name = "CatacombsMap";
                                foreach (Animator a in map.GetComponentsInChildren<Animator>(true))
                                {
                                    var ai = a.GetComponent<AIComponent>();
                                    ai.specialRangeSqr = 6.5f;

                                    a.runtimeAnimatorController = Prefabs.handAnimator;
                                    var tomb = Instantiate(Prefabs.tombstone, a.transform);
                                    tomb.transform.localPosition = new Vector2(0, 0.17f); ;
                                    a.transform.localScale = Vector2.one * 3;
                                    var box = a.GetComponent<BoxCollider2D>();
                                    box.size = new Vector2(0.7f, 0.7f);
                                    box.offset = new Vector2(0, 0.23f);
                                }
                                foreach (Grid g in map.GetComponentsInChildren<Grid>())
                                {
                                    Destroy(g.gameObject);
                                }
                                foreach (Transform t in map.GetComponent<WorldScroller>().tiles)
                                {
                                    var grid = Instantiate(Prefabs.catacombsGrid, t);
                                    grid.transform.localPosition = Vector2.zero;
                                }
                                foreach (SpawnSession s in Prefabs.catacombsData.spawnSessions)
                                {
                                    GameObject prefab = s.monsterPrefab;
                                    switch (s.monsterPrefab.name)
                                    {
                                        case "BrainMonster":
                                            if (!Prefabs.skellyEnemy)
                                            {
                                                Prefabs.skellyEnemy = prefab.CreateEnemy("Skelly", Prefabs.skellyAnimator, Vector2.one * 1.25f);
                                            }
                                            s.monsterPrefab = Prefabs.skellyEnemy;
                                            break;
                                        case "Lamprey":
                                            if (!Prefabs.wheelyEnemy)
                                            {
                                                Prefabs.wheelyEnemy = Prefabs.skellyEnemy.CreateEnemy("Wheely", Prefabs.wheelyAnimator, Vector2.one * 1.25f, 1.5f);
                                            }
                                            s.monsterPrefab = Prefabs.wheelyEnemy;
                                            break;
                                        case "EyeMonster":
                                            if (!Prefabs.skellyMageEnemy)
                                            {
                                                Prefabs.skellyMageEnemy = prefab.CreateEnemy("SkellyMage", Prefabs.skellyMageAnimator, new Vector2(0.8f, 1.5f));
                                                var ai = Prefabs.skellyMageEnemy.GetComponent<AIComponent>();
                                                ai.rotateTowardsPlayer = false;
                                                Prefabs.skellyMageEnemy.AddComponent<AIPlayStillAnim>();
                                                Prefabs.skellyMageSpecialProjectile = Prefabs.flamethrowerProjectile.CreateEnemyProjectile("SkellyMageProjectile", 0.15f, Vector2.zero);
                                                Prefabs.skellyMageSpecial.sound = Prefabs.skellyProjectileFire;
                                                Destroy(Prefabs.skellyMageSpecialProjectile.GetComponent<SpawnOnCollision>());
                                                Destroy(Prefabs.skellyMageSpecialProjectile.GetComponent<Projectile>());
                                                Destroy(Prefabs.skellyMageSpecialProjectile.GetComponent<Knockback>());
                                                Prefabs.skellyMageSpecial.projectile = Prefabs.skellyMageSpecialProjectile;
                                                ai.specialSO = Prefabs.skellyMageSpecial;
                                            }
                                            s.monsterPrefab = Prefabs.skellyMageEnemy;
                                            break;
                                    }
                                }
                                Prefabs.catacombsData.mapPrefab = map;
                            }
                            GameObject newEntry = Instantiate(entry.gameObject, entry.transform.parent);
                            newEntry.name = "Catacombs";
                            MapEntry mapEntry = newEntry.GetComponent<MapEntry>();
                            mapEntry.data = Prefabs.catacombsData;
                            Destroy(newEntry.GetComponentInChildren<TextLocalizerUI>());
                            newEntry.GetComponentInChildren<TextMeshProUGUI>().text = "Catacombs";
                            PointerDetector pointer = newEntry.GetComponent<PointerDetector>();
                            Toggle toggle = newEntry.GetComponent<Toggle>();
                            toggle.isOn = true;
                            GameModeMenu gameModemenu = titleController.mapSelectMenu;
                            Array.Resize<Toggle>(ref gameModemenu.toggles, gameModemenu.toggles.Length + 1);
                            gameModemenu.toggles[gameModemenu.toggles.Length - 1] = toggle;
                            int index = gameModemenu.toggles.Length + 1;
                            pointer.onEnter.AddListener(delegate () { gameModemenu.OnPointerEnter(index); });
                            pointer.onSelect.AddListener(delegate () { gameModemenu.OnPointerEnter(index); Debug.LogWarning("SELECT"); });
                            pointer.onExit.AddListener(new UnityAction(gameModemenu.OnPointerExit));
                            pointer.onDeselect.AddListener(new UnityAction(gameModemenu.OnPointerExit));
                            toggle.onValueChanged.AddListener(delegate (bool b) { gameModemenu.OnToggleChanged(index); });
                            newEntry.GetComponentsInChildren<Image>()[1].sprite = Prefabs.catacombsPreview;

                        }

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
                                    buttonExtension.onSelect.AddListener(delegate ()
                                    {
                                        ui.animator.SetTrigger("Run");
                                        characterDescription.SetProperties(Prefabs.reaperData);
                                    });
                                    buttonExtension.onDeselect.AddListener(delegate ()
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
                    GameObject gameControllerObject = Array.Find<GameObject>(rootObjects, x => x.name == "GameController");
                    gameController = gameControllerObject ? gameControllerObject.GetComponent<GameController>() : null;
                    if (debug)
                    {
                        rerollButtonObject = gameController ? gameController.powerupRerollButton ? gameController.powerupRerollButton.gameObject : null : null;
                    }
                    if (gameController)
                    {
                        if (gameController.hud)
                        {
                            var timer = gameController.hud.GetComponentInChildren<TimerUI>();
                            if (timer)
                            {
                                timer.transform.localPosition = new Vector2(390, -110);
                                MinimapBehaviour.instance.minimap = Instantiate(Prefabs.minimap, timer.transform);
                                MinimapBehaviour.instance.minimap.transform.localPosition = new Vector2(-55, 70);
                                var camera = MinimapBehaviour.instance.minimap.GetComponentInChildren<Camera>();
                                camera.transform.SetParent(PlayerController.Instance.transform);
                                camera.transform.localPosition = new Vector3(0, 0, -1);
                                MinimapBehaviour.instance.minimapDragonballs = MinimapBehaviour.instance.minimap.transform.Find("dragonballs").GetComponentsInChildren<SpriteRenderer>(true);

                                MinimapBehaviour.instance.minimapIcons.Add(PlayerController.Instance.gameObject, Instantiate(Prefabs.minimapPlayerArrowIcon, PlayerController.Instance.transform.position, Quaternion.identity, PlayerController.Instance.transform));
                            }
                        }
                        if (gameController.powerupListUI)
                        {
                            var statPanel = Instantiate(Prefabs.statPanel, gameController.powerupListUI.transform.GetChild(0));
                            statPanel.name = "StatPanel";
                            statPanel.transform.localPosition = new Vector2(-16, 0);
                            var statsPanelBehaviour = statPanel.GetComponent<StatPanelBehaviour>();
                            foreach (TextMeshProUGUI t in statPanel.GetComponentsInChildren<TextMeshProUGUI>())
                            {
                                t.font = Prefabs.lantern;
                            }

                            var powerupListUI = gameController.powerupListUI.GetComponentInChildren<PowerupListUI>();
                            if (powerupListUI)
                            {
                                powerupListUI.transform.localPosition = new Vector2(70, -135.7582f);
                            }
                            powerupListUI.transform.parent.gameObject.AddComponent<ForceLocalPosition>().localPos = new Vector2(70, 143.7418f);
                            var scrollbar = powerupListUI.transform.parent.GetComponentInChildren<Scrollbar>();
                            if (scrollbar)
                            {
                                scrollbar.gameObject.AddComponent<ForceLocalPosition>().localPos = new Vector2(70, -143.7418f);
                            }
                        }
                    }
                    if (!createdPrefabs)
                    {
                        createdPrefabs = true;  

                        var powerups = (Powerup[])Resources.FindObjectsOfTypeAll(typeof(Powerup));
                        foreach (Powerup p in powerups)
                        {
                            var name = p.nameString;
                            
                            if (!Prefabs.powerups.ContainsKey(name))
                            {
                                Prefabs.powerups.Add(name, p);
                            }
                        }
                        Prefabs.wingedBossProjectile = ((flanne.PerkSystem.Actions.ThrowProjectileTowardsCursorAction)Prefabs.powerups["Aero Magic"].effects[0].action).projectilePrefab.CreateEnemyProjectile("WingedBossProjectile", 0.8f, Vector2.zero);
                        var fireBallAction = ((flanne.PerkSystem.Actions.ThrowProjectileTowardsCursorAction)Prefabs.powerups["Fire Starter"].effects[0].action);
                        //Prefabs.elderDragonProjectileFireballExplosion = Prefabs.Instantiate(fireBallAction.projectilePrefab.GetComponent<SpawnOnCollision>().lastImpactFX);
                        Prefabs.Instantiate(fireBallAction.projectilePrefab);
                        Prefabs.elderDragonProjectileFireball = fireBallAction.projectilePrefab.InstantiateProjectile("ElderDragonProjectileFireball", 0.65f, Assets.Load<Sprite>("elderDragonFireball"), 1.5f, Prefabs.colorDict["red"].Item1, Prefabs.colorDict["red"].Item1);
                        Prefabs.elderDragonProjectileFireball.AddComponent<ExplosionEffectOnDisable>().effectScale = 2f;
                        Prefabs.powerups["Vengeful Ghost"].effects[0].action = new ModShootingSummonNumProjectilesAction() { SummonTypeID = "GhostFriend", addNumProjectiles = 2 };

                        Prefabs.powerups["Aged Dragon"].stackedEffects = null;
                        Prefabs.powerups["Aged Dragon"].effects = new PerkEffect[] { Prefabs.dragonBallDrop };
                        Prefabs.powerups["Aged Dragon"].icon = Assets.Load<Sprite>("Icon_EternalDragon");

                        Prefabs.headhunterSyn.prereqs = Prefabs.CreateList<Powerup>(Prefabs.critPU1, Prefabs.powerups["Take Aim"]);
                    }
                    break;
            }
        }
        public void SummonEgg_Start(On.flanne.SummonEgg.orig_Start orig, SummonEgg self)
        {
            self.summon.gameObject.SetActive(true);

            DragonSummonBehaviour summonBehaviour = self.summon.gameObject.AddComponent<DragonSummonBehaviour>();
            summonBehaviour.summon = self.summon;
            //summonBehaviour.sound = self.soundFX;
            summonBehaviour.hatchParticles = self.hatchParticles;

            self.gameObject.SetActive(false);
        }
        void GunChanges()
        {
            foreach (GunData p in Prefabs.guns)
            {
                switch (p.name)
                {
                    case "DualSMGsData":
                        Prefabs.dualSMG = p;
                        p.shotCooldown = 0.125f;

                        Prefabs.fmg9Prefab = Prefabs.Instantiate(p.model);
                        Prefabs.fmg9Prefab.name = "PF_FMG9";
                        foreach (Animator a in Prefabs.fmg9Prefab.GetComponentsInChildren<Animator>())
                        {
                            if (a.name == "GunSprite")
                            {
                                a.runtimeAnimatorController = Prefabs.fmg9Animator;
                                var sprite = a.GetComponent<SpriteRenderer>();
                                sprite.drawMode = SpriteDrawMode.Sliced;
                                sprite.size = Vector2.one * 0.6f;
                                sprite.transform.localPosition = new Vector2(0.25f, -0.1f);
                                a.gameObject.AddComponent<AnimEventReload>();
                            }
                            else
                            {
                                Destroy(a.gameObject);
                            }
                        }
                        foreach (Shooter s in Prefabs.fmg9Prefab.GetComponentsInChildren<Shooter>())
                        {
                            var sprite = s.muzzleFlashObject.GetComponent<SpriteRenderer>();
                            sprite.sprite = Prefabs.fmg9MuzzleFlash;
                            sprite.drawMode = SpriteDrawMode.Sliced;
                            sprite.transform.localScale = Vector2.one;
                            sprite.size = Vector2.one * 0.4f;
                            if (s.name == "Firepoint")
                            {
                                s.transform.localPosition = new Vector2(0.72f, 0.05f);
                            }
                            else
                            {
                                s.transform.localPosition = new Vector2(0.72f, -0.1f);
                            }
                        }
                        foreach (TimeToLive t in Prefabs.fmg9Prefab.GetComponentsInChildren<TimeToLive>())
                        {
                            t.lifetime = 0.05f;
                        }

                        Prefabs.fmg9Bullet = Prefabs.Instantiate(p.bullet);
                        Prefabs.fmg9Bullet.name = "PF_FMG9Projectile";
                        Prefabs.fmg9Bullet.transform.localScale = Vector2.one * 0.6f;
                        Prefabs.fmg9Bullet.GetComponent<TrailRenderer>().widthMultiplier = 0.6f;


                        Prefabs.fmg9.model = Prefabs.fmg9Prefab;
                        Prefabs.fmg9.bullet = Prefabs.fmg9Bullet;

                        break;
                    case "FlameCannon":
                        Prefabs.flamethrowerProjectile = Prefabs.Instantiate(p.bullet);
                        Prefabs.skellyProjectileFire = p.gunshotSFX;
                        break;
                    case "GrenadeLauncherData":
                        Prefabs.redExplosionEffect = Prefabs.Instantiate(p.bullet.GetComponent<ExplosiveProjectile>().explosionPrefab);
                        Prefabs.redExplosionEffect.name = "RedExplosionEffect";
                        Destroy(Prefabs.redExplosionEffect.GetComponent<HarmfulOnContact>());
                        Destroy(Prefabs.redExplosionEffect.GetComponent<CircleCollider2D>());
                        Destroy(Prefabs.redExplosionEffect.GetComponent<TriggerOnHitEffects>());
                        Destroy(Prefabs.redExplosionEffect.GetComponent<TimeToLive>());

                        Prefabs.whiteExplosionEffect = Prefabs.redExplosionEffect.RecolorProjectile("WhiteExplosionEffect", Prefabs.colorDict["white"].Item1);
                        break;
                }
            }

            On.flanne.Gun.Update += Gun_Update;
        }
        public void Gun_Update(On.flanne.Gun.orig_Update orig, Gun self)
        {
            if (self.gunData == Prefabs.dualSMG || self.gunData == Prefabs.fmg9)
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
