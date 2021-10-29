using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class LoadingScreen : BaseScreen
    {
        private string loadingLabel;
        private Texture2D screenTexture;

        private bool marketRouteEnd;
        private bool eventsRouteEnd;
        private int runningRoutesCount;
        private float waitTime = 0.1f;

        void Awake()
        {
            screenTexture = Resources.Load<Texture2D>("Ulvi");
        }

        private IEnumerator WaitAllRoutes()
        {
            while (runningRoutesCount > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            UIManager.ShowScreen<CastleScreen>();
        }

        private IEnumerator LoadResources()
        {
            while (runningRoutesCount > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount++;

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
                            runningRoutesCount--;
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
            while (!marketRouteEnd)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount++;

            var productRoutes = 0;
            foreach (var market in GameData.markets)
            {
                productRoutes++;
                StartCoroutine(AdminBRO.markets(market.id, (products) =>
                {
                    var marketProducts = new GameData.MarketProducts { marketId = market.id, marketProducts = products };
                    GameData.marketProducts.Add(marketProducts);
                    productRoutes--;
                }));
            }

            while (productRoutes > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount--;
        }

        private IEnumerator LoadDialogs()
        {
            while (!eventsRouteEnd)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount++;

            var dialogRoutes = 0;
            var loadDialogId = new List<int>();
            foreach (var eventItem in GameData.events)
            {
                foreach (var stage in eventItem.stages)
                {
                    if (stage.dialogId.HasValue)
                    {
                        if (!loadDialogId.Exists(item => item == stage.dialogId.Value))
                        {
                            dialogRoutes++;
                            loadDialogId.Add(stage.dialogId.Value);
                            StartCoroutine(AdminBRO.dialog(stage.dialogId.Value, (dialog) =>
                            {
                                GameData.dialogs.Add(dialog);
                                dialogRoutes--;
                            }));
                        }
                    }
                }
            }

            while (dialogRoutes > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount--;
        }

        void Start()
        {
            runningRoutesCount++;
            StartCoroutine(AdminBRO.me(e =>
            {
                GameData.playerInfo = e;

                /*StartCoroutine(AdminBRO.me("NewName", e =>
                {

                }));*/

                runningRoutesCount--;
            }));

            runningRoutesCount++;
            StartCoroutine(AdminBRO.i18n("en", (dict) =>
            {
                var d = dict;

                runningRoutesCount--;
            }));

            runningRoutesCount++;
            StartCoroutine(AdminBRO.events((events) =>
            {
                GameData.events = events;

                eventsRouteEnd = true;
                runningRoutesCount--;
            }));

            runningRoutesCount++;
            StartCoroutine(AdminBRO.quests((quests) =>
            {
                GameData.quests = quests;

                runningRoutesCount--;
            }));

            runningRoutesCount++;
            StartCoroutine(AdminBRO.markets((markets) =>
            {
                GameData.markets = markets;

                marketRouteEnd = true;
                runningRoutesCount--;
            }));

            runningRoutesCount++;
            StartCoroutine(AdminBRO.currencies((currencies) => 
            {
                GameData.currenies = currencies;

                runningRoutesCount--;
            }));

            StartCoroutine(LoadMarketProducts());

            StartCoroutine(LoadDialogs());

            StartCoroutine(LoadResources());

            StartCoroutine(WaitAllRoutes());
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
