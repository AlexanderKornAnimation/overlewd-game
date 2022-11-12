using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace Overlewd
{
    public class EventMarketScreen : BaseFullScreenParent<EventMarketScreenInData>
    {
        private Button backButton;

        private Button moneyBackButton;
        private TextMeshProUGUI moneyBackValue;

        private Transform scrollViewContent;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/EventMarketScreen/EventMarket", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            moneyBackButton = canvas.Find("MoneyBack").GetComponent<Button>();
            moneyBackButton.onClick.AddListener(MoneyBackButtonClick);
            moneyBackValue = moneyBackButton.transform.Find("EventCurrency").GetComponent<TextMeshProUGUI>();

            scrollViewContent = canvas.Find("ScrollView").Find("Viewport").Find("Content");
        }

        public override async Task BeforeShowMakeAsync()
        {
            var _marketData = inputData.eventMarketData;
            var tradables = _marketData.tradablesData;

            foreach (var tradableData in tradables)
            {
                var eventMarketItem = NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
                eventMarketItem.tradableId = tradableData.id;
                eventMarketItem.eventMarketId = _marketData.id;
            }

            Customize();

            await Task.CompletedTask;
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData?.eventId)
            {
                case GameDataEvent.EventId.BuyTradable:
                    Customize();
                    foreach (var marketItem in scrollViewContent.GetComponentsInChildren<NSEventMarketScreen.EventMarketItem>())
                    {
                        marketItem.Customize();
                    }
                    break;
            }
        }

        private void Customize()
        {
            moneyBackValue.text = $"{GameData.player.CatEars?.amount ?? 0}";
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ToPrevScreen();
        }

        private void MoneyBackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            var _marketData = inputData.eventMarketData;
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
                                eventMarketId = _marketData.id,
                                tradableId = tData.id
                            }).DoShow();
                    }
                }
            }
        }
    }

    public class EventMarketScreenInData : BaseFullScreenInData
    {
        public int? eventMarketId;
        public AdminBRO.EventMarketItem eventMarketData =>
            GameData.markets.GetEventMarketById(eventMarketId.Value);
    }
}
