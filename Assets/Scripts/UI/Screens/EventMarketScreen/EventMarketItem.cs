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
            public int tradableId;
            private AdminBRO.TradableItem tradableData;
            private AdminBRO.CurrencyItem currencyData;

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

            void Start()
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

                tradableData = GameData.GetTradableById(tradableId);
                var currencyId = tradableData.price[0].currencyId;
                currencyData = GameData.GetCurrencyById(currencyId);

                CustomizeItem();
            }

            private void CustomizeItem()
            {
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

                    buyWithCountCount.text = $"{tradableData.limit.Value}/{tradableData.limit.Value}";
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
                if (GameData.CanTradableBuy(tradableData))
                {
                    if (!currencyData.nutaku)
                    {
                        await GameData.BuyTradableAsync(tradableData);
                        UIManager.ShowNotification<BuyingNotification>();
                    }
                    else
                    {
                        GameGlobalStates.bannerNotifcation_TradableId = tradableId;
                        UIManager.ShowNotification<BannerNotification>();
                    }
                }
            }

            private async void BuyWithCountButtonClick()
            {
                if (GameData.CanTradableBuy(tradableData))
                {
                    if (!currencyData.nutaku)
                    {
                        await GameData.BuyTradableAsync(tradableData);
                        UIManager.ShowNotification<BuyingNotification>();
                    }
                    else
                    {
                        GameGlobalStates.bannerNotifcation_TradableId = tradableId;
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
