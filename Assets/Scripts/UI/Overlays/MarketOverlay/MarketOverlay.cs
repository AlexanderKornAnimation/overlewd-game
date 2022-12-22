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
        private Button backButton;
        private Transform offerButtonsContent;
        private WalletWidget walletWidget;
        public Transform offerContentPos { get; private set; }

        public List<NSMarketOverlay.OfferButton> offers =>
            offerButtonsContent.GetComponentsInChildren<NSMarketOverlay.OfferButton>().ToList();
        public NSMarketOverlay.OfferButton selectedOffer => offers.Find(o => o.isSelected);

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/MarketOverlay/Market", transform);

            var canvas = screenInst.transform.Find("Canvas");
            offerButtonsContent = canvas.Find("OfferButtonsScroll").Find("Viewport").Find("Content");
            backButton = canvas.Find("CloseButton").GetComponent<Button>();
            backButton.onClick.AddListener(CloseButtonClick);
            offerContentPos = canvas.Find("OfferContentPos");

            walletWidget = WalletWidget.GetInstance(canvas.Find("WalletWidgetPos"));
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.id)
            {
                case GameDataEventId.BuyTradable:
                    selectedOffer?.Refresh();
                    break;
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            var mData = GameData.markets.mainMarket;
            var tabsData = mData.tabs.Where(t => t.isVisible).
                OrderBy(t => t.order).ToList();
            foreach (var tData in tabsData)
            {
                var offerButton = NSMarketOverlay.OfferButton.GetInstance(offerButtonsContent);
                offerButton.marketOverlay = this;
                offerButton.tabId = tData.tabId;
                offerButton.Deselect();
            }

            offers.FirstOrDefault()?.Select();

            await Task.CompletedTask;
        }

        public override async Task BeforeShowAsync()
        {
            selectedOffer?.Refresh();

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastStartedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_market);
                    break;
                case (FTUE.CHAPTER_2, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_market);
                    break;
                case (FTUE.CHAPTER_3, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_market);
                    break;
            }

            await Task.CompletedTask;
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (UIManager.currentState.prevState.OverlayTypeIs<EventOverlay>())
            {
                UIManager.ToPrevState();
            }
            else
            {
                UIManager.HideOverlay();
            }
        }
    }

    public class MarketOverlayInData : BaseOverlayInData
    {
    }
}