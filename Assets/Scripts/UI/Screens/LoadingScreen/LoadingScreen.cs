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

        private IEnumerator LoadDialogs()
        {
            while (!eventsRouteEnd)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount++;

            var dialogRoutes = 0;
            var loadDialogsId = new List<int>();
            foreach (var eventItem in GameData.events)
            {
                foreach (var stage in eventItem.stages)
                {
                    if (stage.dialog != null)
                    {
                        if (!loadDialogsId.Exists(item => item == stage.dialog.id))
                        {
                            dialogRoutes++;
                            loadDialogsId.Add(stage.dialog.id);
                            StartCoroutine(AdminBRO.dialog(stage.dialog.id, (dialog) =>
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

        private IEnumerator LoadQuests()
        {
            while (!eventsRouteEnd)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount++;

            var questsRoutes = 0;
            foreach (var eventItem in GameData.events)
            {
                questsRoutes++;
                StartCoroutine(AdminBRO.quests(eventItem.id, (quests) =>
                {
                    foreach (var quest in quests)
                    {
                        if (!GameData.quests.Exists(q => q.id == quest.id))
                        {
                            GameData.quests.Add(quest);
                        }
                    }
                    questsRoutes--;
                }));
            }

            while (questsRoutes > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount--;
        }

        private IEnumerator LoadMarkets()
        {
            while (!eventsRouteEnd)
            {
                yield return new WaitForSeconds(waitTime);
            }

            runningRoutesCount++;

            var marketRoutes = 0;
            var loadMarketsId = new List<int>();
            foreach (var eventItem in GameData.events)
            {
                foreach (var marketId in eventItem.markets)
                {
                    if (!loadMarketsId.Exists(m_id => m_id == marketId))
                    {
                        marketRoutes++;
                        loadMarketsId.Add(marketId);
                        StartCoroutine(AdminBRO.markets(marketId, (market) =>
                        {
                            GameData.markets.Add(market);
                            marketRoutes--;
                        }));
                    }
                }
            }

            while (marketRoutes > 0)
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
            StartCoroutine(AdminBRO.currencies((currencies) => 
            {
                GameData.currenies = currencies;

                runningRoutesCount--;
            }));

            StartCoroutine(LoadMarkets());

            StartCoroutine(LoadQuests());

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
