using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketOverlay : BaseOverlayParent<MarketOverlayInData>
    {
        private List<NSMarketOverlay.OfferButton> offers = new List<NSMarketOverlay.OfferButton>();
        private NSMarketOverlay.OfferButton selectedOffer;

        private Button backButton;
        private Transform canvas;
        private Transform offerButtonsContent;
        private Transform currencyBack;
        private Transform offerPos;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/MarketOverlay/Market", transform);

            canvas = screenInst.transform.Find("Canvas");
            currencyBack = canvas.Find("CurrencyBack");
            offerButtonsContent = canvas.Find("OfferButtonsScroll").Find("Viewport").Find("Content");
            backButton = canvas.Find("CloseButton").GetComponent<Button>();
            backButton.onClick.AddListener(CloseButtonClick);
            offerPos = canvas.Find("OfferPos");
        }

        public override async Task BeforeShowMakeAsync()
        {
            AddOffers();
            SelectOffer(offers.First());
            UITools.FillWallet(currencyBack);
            
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedState)
            {
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_market);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_market);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_market);
                            break;
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        private void AddOffers()
        {
            for (int i = 0; i < 3; i++)
            {
                var offerButton = NSMarketOverlay.OfferButton.GetInstance(offerButtonsContent);
                offerButton.offerPos = offerPos;
                offerButton.Customize();
                offerButton.selectButton += SelectOffer;
                offers.Add(offerButton);
            }
        }

        private void SelectOffer(NSMarketOverlay.OfferButton offer)
        {
            selectedOffer?.Deselect();
            offer?.Select();
            selectedOffer = offer;
        }

        private void CloseButtonClick()
        {
            UIManager.HideOverlay();
        }
    }

    public class MarketOverlayInData : BaseOverlayInData
    {
    }
}