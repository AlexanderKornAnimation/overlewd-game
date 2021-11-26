using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMarketScreen : BaseScreen
    {
        private Button backButton;
        private Button portalButton;
        private Button marketButton;

        private Button moneyBackButton;
        private Text moneyBackValue;
        private Image eventMoneyImage;

        private Transform scrollViewContent;

        private AdminBRO.TradableItem promoTradable;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMarketScreen/EventMarket"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);

            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);

            moneyBackButton = canvas.Find("MoneyBack").GetComponent<Button>();
            moneyBackButton.onClick.AddListener(MoneyBackButtonClick);
            moneyBackValue = moneyBackButton.transform.Find("Value").GetComponent<Text>();
            eventMoneyImage = moneyBackButton.transform.Find("EventMoney").GetComponent<Image>();

            scrollViewContent = canvas.Find("ScrollView").Find("Viewport").Find("Content");

            Customize();
        }

        public void Customize()
        {
            var marketData = GameGlobalStates.eventShop_MarketData;
            var tradables = new List<AdminBRO.TradableItem>(marketData.tradable);

            tradables.Sort((x, y) =>
            {
                return x.promo ? -1 : 1;
            }
            );
            foreach (var tradableData in tradables)
            {
                var eventMarketItem = NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
                eventMarketItem.tradableId = tradableData.id;
            }

            promoTradable = tradables.Find(t => t.promo);

            moneyBackValue.text = GameData.GetCurencyCatEarsCount().ToString();
            eventMoneyImage.sprite = ResourceManager.LoadSpriteById(GameData.GetCurencyCatEars().iconUrl);
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        private void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }

        private void MarketButtonClick()
        {
            UIManager.ShowScreen<MarketScreen>();
        }

        private void MoneyBackButtonClick()
        {
            if (promoTradable != null)
            {
                if (GameData.CanTradableBuy(promoTradable))
                {
                    GameGlobalStates.bannerNotifcation_TradableId = promoTradable.id;
                    UIManager.ShowNotification<BannerNotification>();
                }
            }
        }
    }
}
