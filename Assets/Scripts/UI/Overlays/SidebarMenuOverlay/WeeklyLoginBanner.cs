using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSidebarMenuOverlay
    {
        public class WeeklyLoginBanner : MonoBehaviour
        {
            private Image background;
            private GameObject rewardsAvailable;
            private Button button;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                rewardsAvailable = canvas.Find("RewardsAvailable").gameObject;
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                background = button.GetComponent<Image>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowPopup<WeeklyLoginPopup>();
            }

            public static WeeklyLoginBanner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<WeeklyLoginBanner>(
                    "Prefabs/UI/Overlays/SidebarMenuOverlay/WeeklyLoginBanner", parent);
            }
        }
    }
}
