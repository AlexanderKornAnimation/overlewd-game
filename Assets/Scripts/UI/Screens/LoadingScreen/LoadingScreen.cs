using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class LoadingScreen : BaseScreen
    {
        private string loadingLabel;
        private Texture2D screenTexture;

        void Awake()
        {
            screenTexture = Resources.Load<Texture2D>("Ulvi");
        }

        private async Task LoadResourcesAsync()
        {
            var serverResourcesMeta = await AdminBRO.resourcesAsync();
            if (serverResourcesMeta != null)
            {
                if (!ResourceManager.HasFreeSpaceForNewResources(serverResourcesMeta))
                {
                    UIManager.ShowDialogBox("Not enough free space", "", () => Game.Quit());
                }
                else
                {
                    ResourceManager.SaveLocalResourcesMeta(serverResourcesMeta);
                    ResourceManager.runtimeResourcesMeta = serverResourcesMeta;

                    await ResourceManager.ActualizeResourcesAsync(serverResourcesMeta,
                        (resourceItemMeta) =>
                        {
                            loadingLabel = "Download: " + resourceItemMeta.url;
                        });
                }
            }
            else
            {
                UIManager.ShowDialogBox("Server error", "No load resources", () => Game.Quit());
            }
        }

        private async Task LoadDialogsAsync()
        {
            var dialogTasks = new List<Task<AdminBRO.Dialog>>();
            var loadDialogsId = new List<int>();
            foreach (var eventItem in GameData.events)
            {
                foreach (var stage in eventItem.stages)
                {
                    if (stage.dialog != null)
                    {
                        if (!loadDialogsId.Exists(item => item == stage.dialog.id))
                        {
                            loadDialogsId.Add(stage.dialog.id);
                            dialogTasks.Add(AdminBRO.dialogAsync(stage.dialog.id));
                        }
                    }
                }
            }

            await Task.WhenAll(dialogTasks);

            await Task.Run(() =>
            {
                foreach (var dialog in dialogTasks)
                {
                    GameData.dialogs.Add(dialog.Result);
                }
            });
        }

        private async Task LoadQuestsAsync()
        {
            var questsTasks = new List<Task<List<AdminBRO.QuestItem>>>();
            foreach (var eventItem in GameData.events)
            {
                questsTasks.Add(AdminBRO.questsAsync(eventItem.id));
            }

            await Task.WhenAll(questsTasks);

            await Task.Run(() =>
            {
                foreach (var quests in questsTasks)
                {
                    foreach (var quest in quests.Result)
                    {
                        if (!GameData.quests.Exists(q => q.id == quest.id))
                        {
                            GameData.quests.Add(quest);
                        }
                    }
                }
            });
        }

        private async Task LoadMarketsAsync()
        {
            var marketTasks = new List<Task<AdminBRO.MarketItem>>();
            var loadMarketsId = new List<int>();
            foreach (var eventItem in GameData.events)
            {
                foreach (var marketId in eventItem.markets)
                {
                    if (!loadMarketsId.Exists(m_id => m_id == marketId))
                    {
                        loadMarketsId.Add(marketId);
                        marketTasks.Add(AdminBRO.marketsAsync(marketId));
                    }
                }
            }

            await Task.WhenAll(marketTasks);

            await Task.Run(() => 
            {
                foreach (var task in marketTasks)
                {
                    GameData.markets.Add(task.Result);
                }
            });
        }

        async void Start()
        {
            var tasks = new List<Task>();

            var taskMe = AdminBRO.meAsync();
            tasks.Add(taskMe);

            var taskLocale = AdminBRO.localizationAsync("en");
            tasks.Add(taskLocale);

            var taskCurrencies = AdminBRO.currenciesAsync();
            tasks.Add(taskCurrencies);

            GameData.events = await AdminBRO.eventsAsync();

            tasks.Add(LoadMarketsAsync());
            tasks.Add(LoadQuestsAsync());
            tasks.Add(LoadDialogsAsync());
            tasks.Add(LoadResourcesAsync());

            await Task.WhenAll(tasks);

            GameData.playerInfo = taskMe.Result;
            GameData.currenies = taskCurrencies.Result;

            UIManager.ShowScreen<CastleScreen>();
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
