using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class CurrencyPack : MonoBehaviour
        {
            public CurrencyPacksOffer packOffer { get; set; }
            public int tradableId { get; set; }
            public AdminBRO.TradableItem tradableData =>
                GameData.markets.GetTradableById(tradableId);

            private Image icon;
            private TextMeshProUGUI description;
            private TextMeshProUGUI price;
            private TextMeshProUGUI discount;
            private GameObject specialOffer;
            private Button button;
            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                icon = button.transform.Find("Icon").GetComponent<Image>();
                description = button.transform.Find("DescriptionBack/Description").GetComponent<TextMeshProUGUI>();
                price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();
                
                var markerBack = button.transform.Find("MarkerBack");
                specialOffer = markerBack.Find("SpecialOffer").gameObject;
                discount = markerBack.Find("Discount/Amount").GetComponent<TextMeshProUGUI>();
                
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
                
            }

            public static CurrencyPack GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CurrencyPack>(
                    "Prefabs/UI/Overlays/MarketOverlay/CurrencyPack", parent);
            }
        }
    }
}