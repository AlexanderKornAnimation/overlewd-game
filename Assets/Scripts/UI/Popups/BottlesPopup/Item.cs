using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBottlesPopup
    {
        public class Item : MonoBehaviour
        {
            private Image icon;
            private Button buyButton;
            private TextMeshProUGUI price;
            private Button minusButton;
            private Button plusButton;
            private TextMeshProUGUI count;

            private void Awake()
            {
                icon = transform.Find("Icon").GetComponent<Image>();
                buyButton = transform.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                price = buyButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                minusButton = transform.Find("ButtonMinus").GetComponent<Button>();
                minusButton.onClick.AddListener(MinusButtonClick);
                plusButton = transform.Find("ButtonMinus").GetComponent<Button>();
                plusButton.onClick.AddListener(PlusButtonClick);
                count = transform.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();
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
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }
            
            private void PlusButtonClick()
            {
                ButtonClick();
            }

            private void MinusButtonClick()
            {
                ButtonClick();
            }

            private void BuyButtonClick()
            {
                ButtonClick();
            }
        }
    }
}