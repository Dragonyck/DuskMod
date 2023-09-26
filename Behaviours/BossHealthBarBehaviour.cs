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
using flanne.PerkSystem.Triggers;
using UnityEngine.TextCore;
using TMPro;

namespace DuskMod
{
    class BossHealthBarBehaviour : MonoBehaviour
    {
        public static BossHealthBarBehaviour instance;
        public RectTransform bossHealthBarRoot;
        public List<Tuple<Health, Image>> bossHealthList = new List<Tuple<Health, Image>>();
        public TMP_FontAsset font = Prefabs.lantern;
        public void Awake()
        {
            BossHealthBarBehaviour.instance = this;
            bossHealthBarRoot = new GameObject("BossHealthBarRoot", typeof(RectTransform)).GetComponent<RectTransform>();
            bossHealthBarRoot.transform.SetParent(base.transform);
            bossHealthBarRoot.localPosition = new Vector2(0, -28);
            bossHealthBarRoot.localScale = Vector2.one;
            var layout = bossHealthBarRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 28;
            this.AddObserver(new Action<object, object>(UpdateHealthBar), Health.TookDamageEvent);
            this.AddObserver(new Action<object, object>(UpdateHealthBar), Health.DeathEvent);
        }
        public void NewHealthBar(Health targetHealth)
        {
            var bossBar = Instantiate(Prefabs.bossHealthUI, bossHealthBarRoot);
            bossBar.GetComponent<RectTransform>().localPosition = Vector2.zero;
            var tmp = bossBar.GetComponentInChildren<TextMeshProUGUI>();
            tmp.font = font;
            tmp.text = targetHealth.gameObject.BossName();
            tmp.transform.localScale = Vector2.one * 0.4f;
            bossHealthList.Add(new Tuple<Health, Image>(targetHealth, bossBar.GetComponentsInChildren<Image>()[1]));
        }
        public void UpdateHealthBar(object sender, object args)
        {
            Health health = sender as Health;
            if (health.gameObject.IsBoss())
            {
                var t = bossHealthList.Find(h => h.Item1 == health);
                if (t != null)
                {
                    Image image = t.Item2;
                    if (health.HP == 0)
                    {
                        Destroy(t.Item2.transform.parent.gameObject);
                        return;
                    }
                    float currentHealth = health.CurrentHealthPercentage();
                    image.fillAmount = currentHealth;
                    if (currentHealth <= 0.5f && !health.gameObject.GetComponent<BossEnrageBehaviour>())
                    {
                        Image[] images = t.Item2.transform.parent.GetComponentsInChildren<Image>();
                        images[1].color = Prefabs.colorDict["darkRed"].Item1;
                        images[2].color = Prefabs.colorDict["red"].Item1;
                        health.gameObject.AddComponent<BossEnrageBehaviour>();
                    }
                }
            }
        }
        public void Update()
        {
            if (bossHealthBarRoot)
            {
                bossHealthBarRoot.gameObject.SetActive(!PauseController.isPaused);
            }
        }
        public void OnDisable()
        {
            this.RemoveObserver(new Action<object, object>(this.UpdateHealthBar), Health.TookDamageEvent);
            this.RemoveObserver(new Action<object, object>(this.UpdateHealthBar), Health.DeathEvent);
        }
    }
}
