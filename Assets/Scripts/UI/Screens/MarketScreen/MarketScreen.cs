using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
    {
        private List<AdminBRO.MarketItem> markets;
        private List<AdminBRO.MarketProductItem> products = new List<AdminBRO.MarketProductItem>();
        private List<AdminBRO.CurrencyItem> currencies = new List<AdminBRO.CurrencyItem>();

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Market"));
            //screenPrefab.SetActive(false);
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);


            screenRectTransform.Find("CanvasRoot").Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            var bundlesGrid = screenRectTransform.Find("CanvasRoot").Find("BottomGrid");
            NSMarketScreen.BundleTypeA.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeB.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeC.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeD.GetInstance(bundlesGrid);

            /*screenRectTransform.Find("CanvasRoot").Find("EventMarket").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMarketScreen>();
            });

            var gridMarkets = screenRectTransform.Find("CanvasRoot").Find("GridMarkets");
            var gridCurrencies = screenRectTransform.Find("CanvasRoot").Find("GridCurrencies");

            yield return StartCoroutine(AdminBRO.markets((marketsData) =>
            {
                markets = marketsData.items;
            }));

            foreach (var item in markets)
            {
                yield return StartCoroutine(AdminBRO.markets(item.id, (productsData) =>
                {
                    products.AddRange(productsData.items);
                }));
            }

            yield return StartCoroutine(AdminBRO.currencies((currenciesData) =>
            {
                currencies = currenciesData.items;
            }));

            //add markets to grid
            foreach (var marketItem in markets)
            {
                yield return StartCoroutine(ResourceManager.LoadTextureById(marketItem.backgroundUrl, (texture) => 
                {
                    AddResourceToGrid(texture, gridMarkets);
                }));

                foreach (var productItem in products)
                {
                    yield return StartCoroutine(ResourceManager.LoadTextureById(productItem.imageUrl, (texture) =>
                    {
                        AddResourceToGrid(texture, gridMarkets);
                    }));

                    foreach (var priceItem in productItem.price)
                    {
                        var currency = currencies.Find(item => item.id == priceItem.currencyId);
                        if (currency != null)
                        {
                            yield return StartCoroutine(ResourceManager.LoadTextureById(currency.iconUrl, (texture) =>
                            {
                                AddResourceToGrid(texture, gridMarkets);
                            }));
                        }
                    }
                }
            }

            //add currencies
            foreach (var currency in currencies)
            {
                yield return StartCoroutine(ResourceManager.LoadTextureById(currency.iconUrl, (texture) =>
                {
                    AddResourceToGrid(texture, gridCurrencies, currency.name);
                }));
            }

            //activate screen
            screenPrefab.SetActive(true);*/
        }

        private void AddResourceToGrid(Texture2D texture, Transform grid, string name = "")
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            var resPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/ResourceItem"));
            var image = resPrefab.transform.Find("RootCanvas").Find("Image").GetComponent<Image>();
            image.sprite = sprite;
            var text = resPrefab.transform.Find("RootCanvas").Find("Image").Find("Text").GetComponent<Text>();
            text.text = name;
            resPrefab.transform.SetParent(grid, false);
        }

        void Update()
        {

        }
    }
}
