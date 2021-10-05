using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
    {
        private List<ShortMarketData> shortDataItems;
        private List<FullMarketData> fullDataItems = new List<FullMarketData>();
        private List<CurrencyItem> currencies = new List<CurrencyItem>();

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

            yield return StartCoroutine(NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/markets", NetworkHelper.tokens.accessToken, (downloadHandler) =>
            {
                var shortItemsJson = "{ \"items\" : " + downloadHandler.text + " }";
                shortDataItems = JsonUtility.FromJson<ShortMarketsData>(shortItemsJson).items;
            },
            (errorMsg) => {
                var msg = errorMsg;
            }));

            foreach (var item in shortDataItems)
            {
                yield return StartCoroutine(NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/markets/" + item.id.ToString(),
                    NetworkHelper.tokens.accessToken, (downloadHandler) => {
                        var fullItemsJson = "{ \"items\" : " + downloadHandler.text + " }";
                        var fullItems = JsonUtility.FromJson<FullMarketsData>(fullItemsJson).items;
                        fullDataItems.AddRange(fullItems);
                    },
                    (errorMsg) =>
                    {

                    }));
            }

            yield return StartCoroutine(NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/currencies", NetworkHelper.tokens.accessToken, (downloadHandler) =>
            {
                var currenciesJson = "{ \"items\" : " + downloadHandler.text + " }";
                currencies = JsonUtility.FromJson<Currenies>(currenciesJson).items;
            },
            (errorMsg) => {
                var msg = errorMsg;
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

        [Serializable]
        public class ShortMarketData
        {
            public int id;
            public string name;
            public string backgroundUrl;
            public string bannerUrl;
            public string dateStart;
            public string dateEnd;
            public string createdAt;
            public string updatedAt;
        }

        [Serializable]
        public class ShortMarketsData
        {
            public List<ShortMarketData> items;
        }

        [Serializable]
        public class FullMarketData
        {
            public int id;
            public string name;
            public string imageUrl;
            public string description;
            public List<PriceItem> price;
            public string discount;
            public string specialOfferLabel;
            public string itemPack;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
        }

        [Serializable]
        public class FullMarketsData
        {
            public List<FullMarketData> items;
        }

        [Serializable]
        public class PriceItem
        {
            public int id;
            public string name;
            public string iconUrl;
            public string createdAt;
            public string updatedAt;
            public int count;
        }

        [Serializable]
        public class CurrencyItem
        { 
            public int id;
            public string name;
            public string iconUrl;
            public string createdAt;
            public string updatedAt;
        }

        [Serializable]
        public class Currenies
        {
            public List<CurrencyItem> items;
        }
    }
}
