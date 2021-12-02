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

        async void Start()
        {
            await AdminBRO.eventStagesResetAsync();
            
            //
            GameData.playerInfo = await AdminBRO.meAsync();

            var locale = await AdminBRO.localizationAsync("en");

            GameData.currenies = await AdminBRO.currenciesAsync();

            GameData.events = await AdminBRO.eventsAsync();

            GameData.eventMarkets = await AdminBRO.eventMarketsAsync();

            GameData.eventQuests = await AdminBRO.eventQuestsAsync();

            GameData.eventStages = await AdminBRO.eventStagesAsync();

            GameData.dialogs = await AdminBRO.dialogsAsync();

            GameData.battles = await AdminBRO.battlesAsync();

            await LoadResourcesAsync();

            UIManager.ShowScreen<StartingScreen>();
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
