using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class DebugLoadingScreen : BaseScreen
    {
        private string loadingLabel;
        private Texture2D screenTexture;

        private bool meRouteEnd;
        private bool i18nRouteEnd;
        private bool eventsRouteEnd;
        private bool questsRouteEnd;
        private bool marketsRouteEnd;
        private bool marketProductsRouteEnd;
        private bool currenciesRouteEnd;

        void Awake()
        {
            screenTexture = Resources.Load<Texture2D>("Ulvi");
        }

        private IEnumerator LoadResources()
        {
            while (!meRouteEnd || !i18nRouteEnd || !eventsRouteEnd ||
                   !questsRouteEnd || !marketsRouteEnd || !marketProductsRouteEnd ||
                   !currenciesRouteEnd)
            {
                yield return new WaitForSeconds(0.5f);
            }

            StartCoroutine(AdminBRO.resources(serverResourcesMeta =>
            {
                if (!ResourceManager.HasFreeSpaceForNewResources(serverResourcesMeta))
                {
                    UIManager.ShowDialogBox("Not enough free space", "", () => Game.Quit());
                }
                else
                {
                    ResourceManager.SaveLocalResourcesMeta(serverResourcesMeta);
                    ResourceManager.runtimeResourcesMeta = serverResourcesMeta;

                    StartCoroutine(ResourceManager.ActualizeResources(
                        serverResourcesMeta,
                        (resourceItemMeta) =>
                        {
                            loadingLabel = "Download: " + resourceItemMeta.url;
                        },
                        () =>
                        {
                            UIManager.ShowScreen<CastleScreen>();
                        }
                    ));
                }
            },
            (errorMsg) =>
            {
                UIManager.ShowDialogBox("Server error", errorMsg, () => Game.Quit());
            }));
        }

        private IEnumerator LoadMarketProducts()
        {
            while (!marketsRouteEnd)
            {
                yield return new WaitForSeconds(0.5f);
            }

            var productRoutes = 0;
            foreach (var market in PlayerData.markets)
            {
                productRoutes++;
                StartCoroutine(AdminBRO.markets(market.id, (products) =>
                {
                    var marketProducts = new PlayerData.MarketProducts { marketId = market.id, products = products.items };
                    PlayerData.marketProducts.Add(marketProducts);
                    productRoutes--;
                }));
            }

            while (productRoutes > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            marketProductsRouteEnd = true;
        }

        void Start()
        {
            StartCoroutine(AdminBRO.me(e =>
            {
                PlayerData.playerInfo = e;

                /*StartCoroutine(AdminBRO.me("NewName", e =>
                {

                }));*/

                meRouteEnd = true;
            }));

            StartCoroutine(AdminBRO.i18n("en", (dict) =>
            {
                var d = dict;

                i18nRouteEnd = true;
            }));

            StartCoroutine(AdminBRO.events((events) =>
            {
                PlayerData.events = events.items;

                eventsRouteEnd = true;
            }));

            StartCoroutine(AdminBRO.quests((quests) =>
            {
                PlayerData.quests = quests.items;

                questsRouteEnd = true;
            }));

            StartCoroutine(AdminBRO.markets((markets) =>
            {
                PlayerData.markets = markets.items;

                marketsRouteEnd = true;
            }));

            StartCoroutine(AdminBRO.currencies((currencies) => 
            {
                PlayerData.currenies = currencies.items;

                currenciesRouteEnd = true;
            }));

            StartCoroutine(LoadMarketProducts());

            StartCoroutine(LoadResources());
        }

        void OnGUI()
        {
            GUI.depth = 2;
            var rect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(rect, screenTexture);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = (int)(Screen.height * 0.08);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.black;
            int labelHeight = (int)(labelStyle.fontSize * 1.5);
            GUI.Label(new Rect(0, Screen.height - labelHeight, Screen.width, labelHeight), loadingLabel, labelStyle);
        }
    }
}
