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

        private Button moneyBackButton;
        private TextMeshProUGUI moneyBackValue;

        private Transform scrollViewContent;
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/EventMarketOverlay/EventMarket", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            moneyBackButton = canvas.Find("MoneyBack").GetComponent<Button>();
            moneyBackButton.onClick.AddListener(MoneyBackButtonClick);
            moneyBackValue = moneyBackButton.transform.Find("CurrencyAmount").GetComponent<TextMeshProUGUI>();

            scrollViewContent = canvas.Find("ScrollView").Find("Viewport").Find("Content");
            walletWidgetPos = canvas.Find("WalletWidgetPos");
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

            Customize();

            await Task.CompletedTask;
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.id)
            {
                case GameDataEventId.BuyTradable:
                    Customize();
                    foreach (var marketItem in scrollViewContent.GetComponentsInChildren<NSEventMarketScreen.EventMarketItem>())
                    {
                        marketItem.Customize();
                    }
                    walletWidget.Customize();
                    break;
            }
        }

        private void Customize()
        {
            moneyBackValue.text = $"{GameData.player.info.CatEars?.amount ?? 0}";
            walletWidget = WalletWidget.GetInstance(walletWidgetPos);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideOverlay();
        }

        private void MoneyBackButtonClick()
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
        public int? marketId;
        public AdminBRO.MarketItem marketData =>
            GameData.markets.GetMarketById(marketId);
    }
}
