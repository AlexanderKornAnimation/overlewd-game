using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketPopup
    {
        public abstract class BaseCurrencyPack : MonoBehaviour
        {
            private TextMeshProUGUI buyingValue;
            private TextMeshProUGUI price;
            private Transform discountBack;
            private TextMeshProUGUI discount;

            private Button button;
            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                buyingValue = button.transform.Find("BuyingValue").GetComponent<TextMeshProUGUI>();
                price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();
                discountBack = button.transform.Find("DiscountBack");
                discount = discountBack.Find("Discount").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void ButtonClick()
            {
                
            }
        }
    }
}