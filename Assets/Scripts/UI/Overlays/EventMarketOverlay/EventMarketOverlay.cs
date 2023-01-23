using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace Overlewd
{
    public class EventMarketOverlay : BaseOverlayParent<EventMarketOverlayInData>
    {
        private Button backButton;

        private Button moreButton;
        private TextMeshProUGUI moreCountTitle;

        private Transform scrollViewContent;
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/EventMarketOverlay/EventMarket", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            moreButton = canvas.Find("MoreCurrenciesButton").GetComponent<Button>();
            moreButton.onClick.AddListener(MoreButtonClick);
            moreCountTitle = moreButton.transform.Find("CountTitle").GetComponent<TextMeshProUGUI>();

            scrollViewContent = canvas.Find("ScrollView").Find("Viewport").Find("Content");
            walletWidgetPos = canvas.Find("WalletWidgetPos");

            walletWidget = WalletWidget.GetInstance(walletWidgetPos);
        }

        public override async Task BeforeShowMakeAsync()
        {
            var _marketData = inputData.marketData;
            var tradables = _marketData.tradablesData;

            foreach (var tradableData in tradables)
            {
                var eventMarketItem = NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
                eventMarketItem.tradableId = tradableData.id;
                eventMarketItem.marketId = _marketData.id;
            }

            SetMoreCountTitle();

            await Task.CompletedTask;
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.id)
            {
                case GameDataEventId.BuyTradable:
                    foreach (var marketItem in scrollViewContent.GetComponentsInChildren<NSEventMarketScreen.EventMarketItem>())
                    {
                        marketItem.Customize();
                    }
                    walletWidget.Customize();
                    break;
                case GameDataEventId.WalletChangeState:
                    SetMoreCountTitle();
                    break;
            }
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideOverlay();
        }

        private void SetMoreCountTitle()
        {
            var title = "You have";
            foreach (var cd in inputData.eventData.currenciesData)
            {
                title = $"{title} {cd?.tmpSprite}{cd?.walletInfo?.amount}";
            }
            moreCountTitle.text = title;
        }

        private void MoreButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            var _marketData = inputData.marketData;
            var tradables = _marketData.tradablesData;

            foreach (var tData in tradables)
            {
                if (tData.promo)
                {
                    if (tData.canBuy)
                    {
                        UIManager.MakeNotification<BannerNotification>().
                            SetData(new BannerNotificationInData
                            {
                                marketId = _marketData.id,
                                tradableId = tData.id
                            }).DoShow();
                    }
                }
            }
        }
    }

    public class EventMarketOverlayInData : BaseOverlayInData
    {

    }
}
