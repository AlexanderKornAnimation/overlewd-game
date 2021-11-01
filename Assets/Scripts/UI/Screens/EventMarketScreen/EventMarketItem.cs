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
            public AdminBRO.MarketProductItem marketProductData { get; set; }

            private Image girlImage;
            private Image itemImage;
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

                girlImage = canvas.Find("GirlImage").GetComponent<Image>();
                itemImage = canvas.Find("ItemImage").GetComponent<Image>();
                description = canvas.Find("Description").GetComponent<Text>();

                buyButton = canvas.Find("Buy").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                buyPrice = buyButton.transform.Find("Price").GetComponent<Text>();
                buyCurrency = buyButton.transform.Find("Currency").GetComponent<Image>();

                buyWithCountButton = canvas.Find("BuyWithCount").GetComponent<Button>();
                buyWithCountButton.onClick.AddListener(BuyWithCountButtonClick);
                buyWithCountPrice = buyWithCountButton.transform.Find("Price").GetComponent<Text>();
                buyWithCountCurrency = buyWithCountButton.transform.Find("Currency").GetComponent<Image>(); ;
                buyWithCountCount = buyWithCountButton.transform.Find("Count").GetComponent<Text>();

                soldOut = canvas.Find("SoldOut");

                CustomizeItem();
            }

            void Update()
            {

            }

            private void CustomizeItem()
            {
                soldOut.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                girlImage.gameObject.SetActive(false);

                itemImage.sprite = ResourceManager.LoadTextureAsSpriteById(marketProductData.imageUrl);

                buyWithCountPrice.text = marketProductData.price[0].amount.ToString();
                var currencyId = marketProductData.price[0].currencyId;
                var currencyData = GameData.GetCurrencyById(currencyId);
                buyWithCountCurrency.sprite = ResourceManager.LoadTextureAsSpriteById(currencyData.iconUrl);

                description.text = marketProductData.description;
            }

            private void BuyButtonClick()
            {
                UIManager.ShowNotification<BuyingNotification>();
            }

            private void BuyWithCountButtonClick()
            {
                UIManager.ShowNotification<BuyingNotification>();
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
