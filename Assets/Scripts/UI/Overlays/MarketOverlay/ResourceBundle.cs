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
        public class ResourceBundle : BaseOffer
        {
            private Image art;
            private Button buyButton;
            private TextMeshProUGUI timer;
            private Transform charactersGrid;
            private Transform currencyGrid;

            protected override void Awake()
            {
                base.Awake();
                var background = canvas.Find("Background");
                var mainGrid = background.Find("MainGrid");

                art = canvas.Find("Art").GetComponent<Image>();
                charactersGrid = mainGrid.Find("CharactersVerticalGrid");
                currencyGrid = mainGrid.Find("CurrencyVerticalGrid");
                buyButton = background.Find("BuyButton").GetComponent<Button>();
                timer = background.Find("TimerBack").Find("Counter").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                Characters.GetInstance(charactersGrid);
                Currencies.GetInstance(currencyGrid);
                Currencies.GetInstance(currencyGrid);
            }

            public static ResourceBundle GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ResourceBundle>(
                    "Prefabs/UI/Overlays/MarketOverlay/ResourceBundle", parent);
            }
        }
    }
}