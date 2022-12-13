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
                if (transform.GetSiblingIndex() % 2 == 0)
                    offer = CurrencyPacksOffer.GetInstance(marketOverlay.offerContentPos);
                else
                    offer = Bundle.GetInstance(marketOverlay.offerContentPos);

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
