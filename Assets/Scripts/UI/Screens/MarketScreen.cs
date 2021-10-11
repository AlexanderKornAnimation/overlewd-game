using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
    {
        private List<AdminBRO.ShortMarketData> shortDataItems;
        private List<AdminBRO.FullMarketData> fullDataItems = new List<AdminBRO.FullMarketData>();
        private List<AdminBRO.CurrencyItem> currencies = new List<AdminBRO.CurrencyItem>();

        IEnumerator Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Market"));
            screenPrefab.SetActive(false);
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);


            screenRectTransform.Find("CanvasRoot").Find("ToThroneRoom").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            screenRectTransform.Find("CanvasRoot").Find("EventMarket").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMarketScreen>();
            });

            var gridMarkets = screenRectTransform.Find("CanvasRoot").Find("GridMarkets");
            var gridCurrencies = screenRectTransform.Find("CanvasRoot").Find("GridCurrencies");

            yield return StartCoroutine(AdminBRO.markets((shortMarketsData) =>
            {
                shortDataItems = shortMarketsData.items;
            }));

            foreach (var item in shortDataItems)
            {
                yield return StartCoroutine(AdminBRO.markets(item.id, (fullItems) =>
                {
                    fullDataItems.AddRange(fullItems.items);
                }));
            }

            yield return StartCoroutine(AdminBRO.currencies((currenciesData) =>
            {
                currencies = currenciesData.items;
            }));

            //add markets to grid
            foreach (var shortItem in shortDataItems)
            {
                yield return StartCoroutine(ResourceManager.LoadTextureById(shortItem.backgroundUrl, (texture) => 
                {
                    AddResourceToGrid(texture, gridMarkets);
                }));

                foreach (var fullItem in fullDataItems)
                {
                    yield return StartCoroutine(ResourceManager.LoadTextureById(fullItem.imageUrl, (texture) =>
                    {
                        AddResourceToGrid(texture, gridMarkets);
                    }));

                    foreach (var priceItem in fullItem.price)
                    {
                        yield return StartCoroutine(ResourceManager.LoadTextureById(priceItem.iconUrl, (texture) =>
                        {
                            AddResourceToGrid(texture, gridMarkets);
                        }));
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
            screenPrefab.SetActive(true);
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
