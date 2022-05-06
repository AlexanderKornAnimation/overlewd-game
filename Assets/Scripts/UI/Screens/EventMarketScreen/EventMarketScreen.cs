using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    public class EventMarketScreen : BaseFullScreen
    {
        private Button backButton;
        private Button portalButton;
        private Button marketButton;

        private Button moneyBackButton;
        private TextMeshProUGUI moneyBackValue;

        private Transform scrollViewContent;

        private List<NSEventMarketScreen.EventMarketItem> marketItems = 
            new List<NSEventMarketScreen.EventMarketItem>();

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/EventMarketScreen/EventMarket", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);

            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);

            moneyBackButton = canvas.Find("MoneyBack").GetComponent<Button>();
            moneyBackButton.onClick.AddListener(MoneyBackButtonClick);
            moneyBackValue = moneyBackButton.transform.Find("EventCurrency").GetComponent<TextMeshProUGUI>();

            scrollViewContent = canvas.Find("ScrollView").Find("Viewport").Find("Content");
        }

        void Start()
        {
            var marketData = GameGlobalStates.eventShop_MarketData;
            var tradables = new List<AdminBRO.TradableItem>();
            foreach (var tId in marketData.tradables)
            {
                var tradableData = GameData.GetTradableById(tId);
                if (tradableData != null)
                {
                    tradables.Add(tradableData);
                }
            }

            tradables.Sort((x, y) =>
                {
                    return x.promo ? -1 : 1;
                }
            );

            foreach (var tradableData in tradables)
            {
                var eventMarketItem = NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
                eventMarketItem.tradableId = tradableData.id;
                eventMarketItem.eventMarketId = marketData.id;

                marketItems.Add(eventMarketItem);
            }

            Customize();
        }

        public override void UpdateGameData(GameDataEvent eventData)
        {
            switch (eventData?.type)
            {
                case GameDataEvent.Type.BuyTradable:
                    Customize();
                    foreach (var marketItem in marketItems)
                    {
                        marketItem.Customize();
                    }
                    break;
            }
        }

        private void Customize()
        {
            moneyBackValue.text = GameData.GetCurencyCatEarsCount().ToString();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<EventMapScreen>();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }

        private void MarketButtonClick()
        {
            // UIManager.ShowScreen<MarketScreen>();
        }

        private void MoneyBackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        
            foreach (var tId in GameGlobalStates.eventShop_MarketData.tradables)
            {
                var tData = GameData.GetTradableById(tId);
                if (tData != null)
                {
                    if (tData.promo)
                    {
                        if (GameData.CanTradableBuy(tData))
                        {
                            GameGlobalStates.bannerNotification_EventMarketId = GameGlobalStates.eventShop_MarketId;
                            GameGlobalStates.bannerNotification_TradableId = tId;
                            UIManager.ShowNotification<BannerNotification>();
                        }
                    }
                }
            }
        }
    }
}
