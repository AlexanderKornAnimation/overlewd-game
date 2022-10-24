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
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;
        private Transform offerPos;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/MarketOverlay/Market", transform);

            canvas = screenInst.transform.Find("Canvas");
            walletWidgetPos = canvas.Find("WalletWidgetPos");
            offerButtonsContent = canvas.Find("OfferButtonsScroll").Find("Viewport").Find("Content");
            backButton = canvas.Find("CloseButton").GetComponent<Button>();
            backButton.onClick.AddListener(CloseButtonClick);
            offerPos = canvas.Find("OfferPos");
        }

        public override async Task BeforeShowMakeAsync()
        {
            AddOffers();
            SelectOffer(offers.First());
            walletWidget = WalletWidget.GetInstance(walletWidgetPos);
                
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            GameData.ftue.DoLern(
                GameData.ftue.stats.lastStartedStageData,
                new FTUELernActions 
                {
                    ch1_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_market),
                    ch2_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_market),
                    ch3_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_market)
                });

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