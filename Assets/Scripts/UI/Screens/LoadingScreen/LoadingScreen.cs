using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class LoadingScreen : BaseScreen
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/LoadingScreen/LoadingScreen", transform);
        }

        private async Task LoadResourcesAsync()
        {
            var serverResourcesMeta = await AdminBRO.resourcesAsync();
            if (serverResourcesMeta != null)
            {
                if (!ResourceManager.HasFreeSpaceForNewResources(serverResourcesMeta))
                {
                    UIManager.ShowDialogBox("Not enough free space", "", /*() => Game.Quit()*/ null);
                }
                else
                {
                    ResourceManager.SaveLocalResourcesMeta(serverResourcesMeta);
                    ResourceManager.runtimeResourcesMeta = serverResourcesMeta;

                    await ResourceManager.ActualizeResourcesAsync(serverResourcesMeta,
                        (resourceItemMeta) =>
                        {
                            //loadingLabel = "Download: " + resourceItemMeta.url;
                        });
                }
            }
            else
            {
                UIManager.ShowDialogBox("Server error", "No load resources", /*() => Game.Quit()*/ null);
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
    }
}
