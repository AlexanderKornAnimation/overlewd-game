using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketPopup : BasePopupParent<MarketPopupInData>
    {
        private List<NSMarketPopup.OfferButton> offers = new List<NSMarketPopup.OfferButton>();
        private NSMarketPopup.OfferButton selectedOffer;

        private Button backButton;
        private Transform canvas;
        private Transform offersContent;
        private Transform currencyBack;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/MarketPopup/Market", transform);

            canvas = screenInst.transform.Find("Canvas");
            currencyBack = canvas.Find("CurrencyBack");
            offersContent = canvas.Find("OffersScroll").Find("Viewport").Find("Content");
            backButton = canvas.Find("CloseButton").GetComponent<Button>();
            backButton.onClick.AddListener(CloseButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            AddOffers();

            foreach (var offer in offers)
            {
                offer.Deselect();
            }
            
            SelectOffer(offers.First());
            UITools.FillWallet(currencyBack);
            await Task.CompletedTask;
        }

        private void AddOffers()
        {
            for (int i = 0; i < 3; i++)
            {
                var offerButton = NSMarketPopup.OfferButton.GetInstance(offersContent);
                offerButton.offerPos = canvas;
                offerButton.Customize();
                offerButton.selectButton += SelectOffer;
                offers.Add(offerButton);
            }
        }

        private void SelectOffer(NSMarketPopup.OfferButton offer)
        {
            selectedOffer?.Deselect();
            offer?.Select();
            selectedOffer = offer;
        }

        private void CloseButtonClick()
        {
            UIManager.HidePopup();
        }
    }

    public class MarketPopupInData : BasePopupInData
    {
    }
}