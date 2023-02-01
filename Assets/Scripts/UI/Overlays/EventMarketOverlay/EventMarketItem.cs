using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMarketScreen
    {
        public class EventMarketItem : MonoBehaviour
        {
            public int? marketId { get; set; }
            public AdminBRO.MarketItem marketData =>
                GameData.markets.GetMarketById(marketId.Value);
            public int? tradableId { get; set; }
            public AdminBRO.TradableItem tradableData =>
                GameData.markets.GetTradableById(tradableId);

            private Image item;
            private TextMeshProUGUI itemAmount;
            private TextMeshProUGUI description;
            private TextMeshProUGUI discount;

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
                itemAmount = item.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();
                discount = canvas.Find("DiscountBack/Discount").GetComponent<TextMeshProUGUI>();

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
                var _tradableData = tradableData;

                if (_tradableData.soldOut)
                {
                    soldOut.gameObject.SetActive(true);
                    buyButton.gameObject.SetActive(false);
                    buyWithCountButton.gameObject.SetActive(false);
                }
                else if (_tradableData.limit.HasValue)
                {
                    soldOut.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                    buyWithCountButton.gameObject.SetActive(true);

                    //buyWithCountPrice.text = _tradableData.price[0].amount.ToString();

                    buyWithCountCount.text = $"{_tradableData.currentCount}/{_tradableData.limit.Value}";
                }
                else
                {
                    soldOut.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(true);
                    buyWithCountButton.gameObject.SetActive(false);

                    //buyPrice.text = _tradableData.price[0].amount.ToString();
                }

                itemAmount.gameObject.SetActive(false);
                item.sprite = ResourceManager.LoadSprite(_tradableData.imageUrl);
                description.text = _tradableData.description;
            }

            private async void BuyButtonClick()
            {
                await Buy();
            }

            private async void BuyWithCountButtonClick()
            {
                await Buy();
            }

            private async Task Buy()
            {
                var trData = tradableData;
                if (trData.nutakuPriceValid)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    var payment = await GameData.markets.NutakuPayment(this, trData);
                    if (payment.isSucceess)
                    {
                        SoundManager.PlayOneShot(FMODEventPath.SFX_UI_Shop_Buy_Success);
                    }
                    else
                    {
                        SoundManager.PlayOneShot(FMODEventPath.SFX_UI_Shop_Buy_Fail);
                    }
                }
                else
                {
                    if (trData.canBuy)
                    {
                        SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                        var result = await GameData.markets.Payment(marketId, tradableId);
                        SoundManager.PlayOneShot(FMODEventPath.SFX_UI_Shop_Buy_Success);
                    }
                    else
                    {
                        SoundManager.PlayOneShot(FMODEventPath.SFX_UI_Shop_Buy_Fail);
                    }
                }
            }

            public static EventMarketItem GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventMarketItem>
                    ("Prefabs/UI/Overlays/EventMarketOverlay/Item", parent);
            }
        }
    }
}