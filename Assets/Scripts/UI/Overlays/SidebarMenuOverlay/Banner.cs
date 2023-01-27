using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            private TextMeshProUGUI marketNotifs;

            public int? tabId { get; set; }
            private AdminBRO.MarketItem.Tab tabData => GameData.markets.mainMarket?.GetTabById(tabId);

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                title = canvas.Find("TitleBack/Title").GetComponent<TextMeshProUGUI>();
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                image = button.GetComponent<Image>();
                notifIsNew = canvas.Find("NotifIsNew").gameObject;
                marketNotifs = canvas.Find("TitleBack/MarketNotifications").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var tradableData = GameData.markets.GetTradableById(tabData.goods?.FirstOrDefault());
                if (tradableData != null)
                {
                    marketNotifs.text = tradableData.discount.HasValue ? TMPSprite.NotificationSale :
                        !string.IsNullOrEmpty(tradableData.dateStart) ? TMPSprite.NotificationTimeLimit : null;
                }
             
                title.text = tabData.bannerDescription;
                image.sprite = ResourceManager.LoadSprite(tabData.bannerArt);
                
            }

            private void ButtonClick()
            {
                UIManager.MakeOverlay<MarketOverlay>().
                    SetData(new MarketOverlayInData
                    {
                        tabId = tabId,
                    }).DoShow();
            }
            
            public static Banner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Banner>("Prefabs/UI/Overlays/SidebarMenuOverlay/Banner",
                    parent);
            }
        }
    }
}