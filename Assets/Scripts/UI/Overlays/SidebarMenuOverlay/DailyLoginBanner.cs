using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSidebarMenuOverlay
    {
        public class DailyLoginBanner : MonoBehaviour
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
                background.sprite = ResourceManager.LoadSprite(GameData.dailyLogin.info.bannerArt);
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowOverlay<DailyLoginOverlay>();
            }

            public static DailyLoginBanner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DailyLoginBanner>(
                    "Prefabs/UI/Overlays/SidebarMenuOverlay/DailyLoginBanner", parent);
            }
        }
    }
}