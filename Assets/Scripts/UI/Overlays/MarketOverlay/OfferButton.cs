using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd.NSMarketOverlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class OfferButton : MonoBehaviour
        {
            public MarketOverlay marketOverlay { get; set; }
            public int tabId { get; set; }
            public AdminBRO.MarketItem.Tab tabData =>
                GameData.markets.mainMarket?.GetTabById(tabId);

            private TextMeshProUGUI title;
            private TextMeshProUGUI notification;
            private Button button;
            private GameObject buttonSelected;

            private BaseOffer offer;

            public bool isSelected => buttonSelected.activeSelf;

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                buttonSelected = button.transform.Find("Selected").gameObject;
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                notification = button.transform.Find("Notification").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                var tData = tabData;
                offer = tData.viewType switch
                {
                    AdminBRO.MarketItem.Tab.ViewType_Bundle => Bundle.GetInstance(marketOverlay.offerContentPos),
                    AdminBRO.MarketItem.Tab.ViewType_Pack => Bundle.GetInstance(marketOverlay.offerContentPos),
                    AdminBRO.MarketItem.Tab.ViewType_GoodsList => CurrencyPacksOffer.GetInstance(marketOverlay.offerContentPos),
                    _ => null
                };

                offer.offerButton = this;
                title.text = tData.title;

                if (!isSelected)
                {
                    offer?.Hide();
                }
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                Select();
            }

            public void Select()
            {
                marketOverlay?.selectedOffer?.Deselect();
                buttonSelected?.SetActive(true);
                offer?.Show();
            }
            
            public void Deselect()
            {
                buttonSelected?.SetActive(false);
                offer?.Hide();
            }

            public static OfferButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<OfferButton>(
                    "Prefabs/UI/Overlays/MarketOverlay/OfferButton", parent);
            }
        }
    }
}
