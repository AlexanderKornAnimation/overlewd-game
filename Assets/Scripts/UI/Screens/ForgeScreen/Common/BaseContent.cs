using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public abstract class BaseContent : MonoBehaviour
        {
            protected Transform buttons;
            protected Button marketButton;
            protected TextMeshProUGUI marketButtonText;
            protected Button portalButton;
            protected TextMeshProUGUI portalButtonText;
            protected Button mergeButton;
            protected Image[] mergePrice = new Image[2];
            protected TextMeshProUGUI[] mergePriceAmount = new TextMeshProUGUI[2];

            protected virtual void Awake()
            {
                buttons = transform.Find("Buttons");
                marketButton = buttons.Find("MarketButton").GetComponent<Button>();
                marketButton.onClick.AddListener(MarketButtonClick);
                marketButtonText = marketButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                portalButton = buttons.Find("PortalButton").GetComponent<Button>();
                portalButton.onClick.AddListener(PortalButtonClick);
                portalButtonText = portalButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                mergeButton = buttons.Find("MergeButton").GetComponent<Button>();
                mergeButton.onClick.AddListener(MergeButtonClick);

                for (int i = 0; i < mergePrice.Length; i++)
                {
                    mergePrice[i] = mergeButton.transform.Find($"Resource{i + 1}").GetComponent<Image>();
                    mergePriceAmount[i] = mergePrice[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                }
            }

            protected virtual void MergeButtonClick()
            {

            }

            protected virtual void PortalButtonClick()
            {
               
            }

            protected virtual void MarketButtonClick()
            {
                UIManager.ShowOverlay<MarketOverlay>();
            }

            protected void SetMergeBtnPrice(List<AdminBRO.PriceItem> price)
            {
                mergePrice[0].gameObject.SetActive(false);
                mergePrice[1].gameObject.SetActive(false);
                var i = 0;
                foreach (var p in price)
                {
                    mergePrice[i].gameObject.SetActive(true);
                    mergePrice[i].sprite = ResourceManager.LoadSprite(p.icon);
                    mergePriceAmount[i].text = p.amount.ToString();
                    i++;
                }
            }
        }
    }
}
