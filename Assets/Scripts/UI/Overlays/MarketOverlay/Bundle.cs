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
        public class Bundle : BaseOffer
        {
            private Image girlArt;
            private TextMeshProUGUI profitAmount;
            private TextMeshProUGUI description;
            private TextMeshProUGUI discount;
            private Transform promoItem1;
            private Transform promoItem2;
            private Image promoBackground;
            private Image[] currenciesImage = new Image[5];
            private TextMeshProUGUI[] currenciesAmount = new TextMeshProUGUI[5];
            private Button buyButton;
            
            protected override void Awake()
            {
                base.Awake();
                
                var background = canvas.Find("Background");
                promoBackground = background.Find("Promo/Background").GetComponent<Image>();
                description = background.Find("Promo/DescriptionBack/Description").GetComponent<TextMeshProUGUI>();
                profitAmount = background.Find("ProfitBack/Title").GetComponent<TextMeshProUGUI>();
                discount = background.Find("DiscountBack/Discount").GetComponent<TextMeshProUGUI>();
                promoItem1 = promoBackground.transform.Find("HorizontalGrid/Item1");
                promoItem1 = promoBackground.transform.Find("HorizontalGrid/Item2");
                girlArt = canvas.Find("GirlArt").GetComponent<Image>();
                buyButton = background.Find("BuyButton").GetComponent<Button>();

                var currencies = background.Find("Currencies");
                
                for (int i = 0; i < currenciesImage.Length; i++)
                {
                    currenciesImage[i] = currencies.Find($"Currency{i + 1}").GetComponent<Image>();
                    currenciesAmount[i] = currenciesImage[i].transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                    currenciesImage[i].gameObject.SetActive(false);
                }
            }

            protected override void Customize()
            {
                
            }

            public static Bundle GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Bundle>("Prefabs/UI/Overlays/MarketOverlay/Bundle",
                    parent);
            }
        }
    }
}