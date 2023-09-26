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
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace DuskMod
{
    public class EscToQuitPauseState : MonoBehaviour
    {
        public GameController controller;
        public float stopwatch;
        public void Update()
        {
            if (controller && controller.playerInput && controller.CurrentState == base.GetComponent<PauseState>())
            {
                controller.playerInput.actions["Move"].started += EscToQuitPauseState_started;
                stopwatch += 0.013f;
                if (stopwatch >= 0.6f)
                {
                    controller.playerInput.actions["Pause"].started += Unpause;
                    //controller.playerInput.actions.AddActionMap("U").;
                }
            }
        }
        private void EscToQuitPauseState_started(InputAction.CallbackContext obj)
        {
            Debug.LogError("Key");
            KeyControl key = (KeyControl)obj.action.activeControl;
            if (key != null && key.keyCode == Key.S)
            {
                Debug.LogError("SSSSSSSSSSSSSSSUSSSSSSSSSSSSSSSSSSSSS");
                Debug.LogError("SUSSY"); 
            }
        }
        public void Unpause(InputAction.CallbackContext obj)
        {
            base.GetComponent<PauseState>().OnResume();
            stopwatch = 0;
        }
    }
}
