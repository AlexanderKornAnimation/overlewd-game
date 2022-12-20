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

            private Button button;
            private Image icon;
            private TextMeshProUGUI descriptionTitle;
            private TextMeshProUGUI price;
            private Transform mark;
            private TextMeshProUGUI markTitle;
        
            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                icon = button.transform.Find("Icon").GetComponent<Image>();
                descriptionTitle = button.transform.Find("Description/Title").GetComponent<TextMeshProUGUI>();
                price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();
                mark = button.transform.Find("Mark");
                markTitle = mark.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var trData = tradableData;
                icon.sprite = ResourceManager.LoadSprite(trData.imageUrl);
                descriptionTitle.text = trData.name;
                price.text = $"Buy for {UITools.PriceToString(trData.price)}";

                mark.gameObject.SetActive(false);
                if (!string.IsNullOrEmpty(trData.discount))
                {
                    markTitle.text = trData.discount;
                    mark.gameObject.SetActive(true);
                }
                else if (!string.IsNullOrEmpty(trData.specialOfferLabel))
                {
                    markTitle.text = trData.specialOfferLabel;
                    mark.gameObject.SetActive(true);
                }

                UITools.DisableButton(button, !trData.canBuy);
            }

            private async void ButtonClick()
            {
                var result = await GameData.markets.BuyTradable(GameData.markets.mainMarket.id, tradableId);
            }

            public static CurrencyPack GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CurrencyPack>(
                    "Prefabs/UI/Overlays/MarketOverlay/CurrencyPack", parent);
            }
        }
    }
}