using System.Collections;
using System.Collections.Generic;
using TMPro;
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

            private Image item;
            private TextMeshProUGUI description;

            private Button buyButton;
            private TextMeshProUGUI buyPrice;

            private Button buyWithCountButton;
            private TextMeshProUGUI buyWithCountPrice;
            private TextMeshProUGUI buyWithCountCount;

            private Transform soldOut;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                item = canvas.Find("Item").GetComponent<Image>();
                description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

                buyButton = canvas.Find("Buy").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                buyPrice = buyButton.transform.Find("Price").GetComponent<TextMeshProUGUI>();

                buyWithCountButton = canvas.Find("BuyWithCount").GetComponent<Button>();
                buyWithCountButton.onClick.AddListener(BuyWithCountButtonClick);
                buyWithCountPrice = buyWithCountButton.transform.Find("Price").GetComponent<TextMeshProUGUI>();
                buyWithCountCount = buyWithCountButton.transform.Find("Count").GetComponent<TextMeshProUGUI>();

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

                    buyWithCountCount.text = $"{tradableData.currentCount}/{tradableData.limit.Value}";
                }
                else
                {
                    soldOut.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(true);
                    buyWithCountButton.gameObject.SetActive(false);

                    buyPrice.text = tradableData.price[0].amount.ToString();
                }

                item.gameObject.SetActive(true);
                item.sprite = ResourceManager.LoadSpriteById(tradableData.imageUrl);
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
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/EventMarketScreen/Item"), parent);
                newItem.name = nameof(EventMarketItem);
                return newItem.AddComponent<EventMarketItem>();
            }
        }
    }
}