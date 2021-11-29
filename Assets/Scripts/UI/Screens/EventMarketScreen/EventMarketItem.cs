using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMarketScreen
    {
        public class EventMarketItem : MonoBehaviour
        {
            public int eventMarketId;
            public int tradableId;

            private Image promoIcon;
            private Transform item;
            private Image itemBack;
            private Image itemIcon;
            private Text description;

            private Button buyButton;
            private Text buyPrice;
            private Image buyCurrency;

            private Button buyWithCountButton;
            private Text buyWithCountPrice;
            private Image buyWithCountCurrency;
            private Text buyWithCountCount;

            private Transform soldOut;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                promoIcon = canvas.Find("PromoIcon").GetComponent<Image>();
                item = canvas.Find("Item");
                itemBack = item.Find("Back").GetComponent<Image>();
                itemIcon = item.Find("Icon").GetComponent<Image>();
                description = canvas.Find("Description").GetComponent<Text>();

                buyButton = canvas.Find("Buy").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                buyPrice = buyButton.transform.Find("Price").GetComponent<Text>();
                buyCurrency = buyButton.transform.Find("Currency").GetComponent<Image>();

                buyWithCountButton = canvas.Find("BuyWithCount").GetComponent<Button>();
                buyWithCountButton.onClick.AddListener(BuyWithCountButtonClick);
                buyWithCountPrice = buyWithCountButton.transform.Find("Price").GetComponent<Text>();
                buyWithCountCurrency = buyWithCountButton.transform.Find("Currency").GetComponent<Image>();
                buyWithCountCount = buyWithCountButton.transform.Find("Count").GetComponent<Text>();

                soldOut = canvas.Find("SoldOut");
            }

            void Start()
            {
                Customize();
            }

            public void Customize()
            {
                var tradableData = GameData.GetTradableById(eventMarketId, tradableId);
                var currencyId = tradableData.price[0].currencyId;
                var currencyData = GameData.GetCurrencyById(currencyId);

                if (tradableData.soldOut)
                {
                    soldOut.gameObject.SetActive(true);
                    buyButton.gameObject.SetActive(false);
                    buyWithCountButton.gameObject.SetActive(false);
                }
                else if (tradableData.limit.HasValue)
                {
                    soldOut.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                    buyWithCountButton.gameObject.SetActive(true);

                    buyWithCountPrice.text = tradableData.price[0].amount.ToString();
                    buyWithCountCurrency.sprite = ResourceManager.LoadSpriteById(currencyData.iconUrl);

                    buyWithCountCount.text = $"{tradableData.currentCount}/{tradableData.limit.Value}";
                }
                else
                {
                    soldOut.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(true);
                    buyWithCountButton.gameObject.SetActive(false);

                    buyPrice.text = tradableData.price[0].amount.ToString();
                    buyCurrency.sprite = ResourceManager.LoadSpriteById(currencyData.iconUrl);
                }

                if (tradableData.promo)
                {
                    item.gameObject.SetActive(false);
                    promoIcon.gameObject.SetActive(true);

                    promoIcon.sprite = ResourceManager.LoadSpriteById(tradableData.imageUrl);
                }
                else
                {
                    item.gameObject.SetActive(true);
                    promoIcon.gameObject.SetActive(false);

                    itemIcon.sprite = ResourceManager.LoadSpriteById(tradableData.imageUrl);
                }

                description.text = tradableData.description;
            }

            private async void BuyButtonClick()
            {
                var tradableData = GameData.GetTradableById(eventMarketId, tradableId);
                var currencyId = tradableData.price[0].currencyId;
                var currencyData = GameData.GetCurrencyById(currencyId);

                if (GameData.CanTradableBuy(tradableData))
                {
                    if (!currencyData.nutaku)
                    {
                        await GameData.BuyTradableAsync(eventMarketId, tradableId);
                        UIManager.ShowNotification<BuyingNotification>();
                    }
                    else
                    {
                        GameGlobalStates.bannerNotification_EventMarketId = eventMarketId;
                        GameGlobalStates.bannerNotification_TradableId = tradableId;
                        UIManager.ShowNotification<BannerNotification>();
                    }
                }
            }

            private async void BuyWithCountButtonClick()
            {
                var tradableData = GameData.GetTradableById(eventMarketId, tradableId);
                var currencyId = tradableData.price[0].currencyId;
                var currencyData = GameData.GetCurrencyById(currencyId);

                if (GameData.CanTradableBuy(tradableData))
                {
                    if (!currencyData.nutaku)
                    {
                        await GameData.BuyTradableAsync(eventMarketId, tradableId);
                        UIManager.ShowNotification<BuyingNotification>();
                    }
                    else
                    {
                        GameGlobalStates.bannerNotification_EventMarketId = eventMarketId;
                        GameGlobalStates.bannerNotification_TradableId = tradableId;
                        UIManager.ShowNotification<BannerNotification>();
                    }
                }
            }

            public static EventMarketItem GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMarketScreen/Item"), parent);
                newItem.name = nameof(EventMarketItem);
                return newItem.AddComponent<EventMarketItem>();
            }
        }
    }
}
