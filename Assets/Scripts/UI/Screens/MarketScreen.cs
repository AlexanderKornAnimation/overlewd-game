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

        IEnumerator Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/ScreenRoot"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var texture = Resources.Load<Texture2D>("Ulvi");
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            var grid = screenPrefab.transform.Find("CanvasRoot").Find("Grid");
            for (int i = 0; i < 5; i++)
            {
                var resPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/ResourceItem"));
                var image = resPrefab.transform.Find("RootCanvas").Find("Image").GetComponent<Image>();
                image.sprite = sprite;
                resPrefab.transform.SetParent(grid, false);
            }

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

            bool doLoad = true;
        }

        void Update()
        {

        }

        void OnGUI()
        {
            GUI.depth = 2;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Market Screen");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Castle"))
                {
                    UIManager.ShowScreen<CastleScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Event Market"))
                {
                    UIManager.ShowScreen<EventMarketScreen>();
                }
            }
            GUI.EndGroup();
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
            public string price;
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
    }
}
