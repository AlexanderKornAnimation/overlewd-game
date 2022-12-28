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
            private Transform discount;
            private TextMeshProUGUI discountTitle;
        
            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                icon = button.transform.Find("Icon").GetComponent<Image>();
                descriptionTitle = button.transform.Find("Description/Title").GetComponent<TextMeshProUGUI>();
                price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();
                discount = button.transform.Find("Discount");
                discountTitle = discount.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var trData = tradableData;

                icon.sprite = ResourceManager.LoadSprite(trData.imageUrl);
                descriptionTitle.text = trData.description;
                price.text = $"Buy for {UITools.PriceToString(trData.price)}";

                discount.gameObject.SetActive(false);
                if (!string.IsNullOrEmpty(trData.discount))
                {
                    discountTitle.text = trData.discount;
                    discount.gameObject.SetActive(true);
                }
                else if (!string.IsNullOrEmpty(trData.specialOfferLabel))
                {
                    discountTitle.text = trData.specialOfferLabel;
                    discount.gameObject.SetActive(true);
                }

                //CalcLockedState();
            }

            public void Refresh()
            {
                //CalcLockedState();
            }

            private void CalcLockedState()
            {
                var trData = tradableData;
                if (!trData.nutakuPriceValid)
                {
                    UITools.DisableButton(button, !trData.canBuy);
                }
            }

            private async void ButtonClick()
            {
                var trData = tradableData;
                if (trData.nutakuPriceValid)
                {
                    var payment = await NutakuApiHelper.PaymentAsync(this, trData);
                }
                else
                {
                    if (trData.canBuy)
                    {
                        var result = await GameData.markets.BuyTradable(GameData.markets.mainMarket.id, tradableId);
                    }
                    else
                    {
                        packOffer.offerButton.marketOverlay.ToCrystalOffersTab();
                    }
                }
            }

            public static CurrencyPack GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CurrencyPack>(
                    "Prefabs/UI/Overlays/MarketOverlay/CurrencyPack", parent);
            }
        }
    }
}