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
            private TextMeshProUGUI buyingValue;
            private TextMeshProUGUI price;
            private Transform discountBack;
            private TextMeshProUGUI discount;
            private GameObject iconCrystalSmall;
            private GameObject iconCrystalMedium;
            private GameObject iconCrystalLarge;
            private GameObject iconCurrency;

            private Button button;
            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                buyingValue = button.transform.Find("BuyingValue").GetComponent<TextMeshProUGUI>();
                price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();
                discountBack = button.transform.Find("DiscountBack");
                discount = discountBack.Find("Discount").GetComponent<TextMeshProUGUI>();
                iconCrystalSmall = button.transform.Find("IconCrystalSmall").gameObject;
                iconCrystalMedium = button.transform.Find("IconCrystalMedium").gameObject;
                iconCrystalLarge = button.transform.Find("IconCrystalLarge").gameObject;
                iconCurrency = button.transform.Find("IconCurrency").gameObject;
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