using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSidebarMenuOverlay
    {
        public class Banner : MonoBehaviour
        {
            private TextMeshProUGUI title;
            private Image image;
            private Button button;
            private GameObject notifIsNew;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                title = canvas.Find("TitleBack/Title").GetComponent<TextMeshProUGUI>();
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                image = button.GetComponent<Image>();
                notifIsNew = canvas.Find("NotifIsNew").gameObject;
            }

            private void ButtonClick()
            {
                UIManager.ShowOverlay<MarketOverlay>();
            }
            
            public static Banner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Banner>("Prefabs/UI/Overlays/SidebarMenuOverlay/Banner",
                    parent);
            }
        }
    }
}