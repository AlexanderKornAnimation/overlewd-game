using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseFullScreenParent<CastleScreenInData>
    {
        private Button sidebarButton;

        private Transform harem;
        private Transform market;
        private Transform forge;
        private Transform magicGuild;
        private Transform portal;
        private Transform castle;
        private Transform municipality;
        private Transform laboratory;
        private Transform catacombs;
        private Transform aerostat;

        private NSCastleScreen.BaseButton haremButton;
        private NSCastleScreen.BaseButton marketButton;
        private NSCastleScreen.BaseButton forgeButton;
        private NSCastleScreen.BaseButton magicGuildButton;
        private NSCastleScreen.BaseButton portalButton;
        private NSCastleScreen.BaseButton castleButton;
        private NSCastleScreen.BaseButton municipalityButton;
        private NSCastleScreen.BaseButton laboratoryButton;
        private NSCastleScreen.BaseButton catacombsButton;
        private NSCastleScreen.BaseButton aerostatButton;

        private EventsWidget eventsPanel;
        private QuestsWidget questsPanel;
        private BuffWidget buffPanel;

        private FMODEvent music;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/CastleScreen/CastleScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            sidebarButton = canvas.Find("SidebarButton").GetComponent<Button>();
            sidebarButton.onClick.AddListener(SidebarButtonClick);

            harem = canvas.Find("Harem");
            portal = canvas.Find("Portal");
            market = canvas.Find("Market");
            forge = canvas.Find("Forge");
            magicGuild = canvas.Find("MagicGuild");
            castle = canvas.Find("Castle");
            municipality = canvas.Find("Municipality");
            laboratory = canvas.Find("Laboratory");
            catacombs = canvas.Find("Catacombs");
            aerostat = canvas.Find("Aerostat");
        }

        public override async Task BeforeShowMakeAsync()
        {
            foreach (var building in GameData.buildings.buildings)
            {
                var showBuilding = GameData.progressMode ? building.isBuilt : true;
                
                if (showBuilding)
                {
                    switch (building.key)
                    {
                        case AdminBRO.Building.Key_Harem:
                            haremButton = NSCastleScreen.HaremButton.GetInstance(harem);
                            break;
                        case AdminBRO.Building.Key_Market:
                            marketButton = NSCastleScreen.MarketButton.GetInstance(market);
                            break;
                        case AdminBRO.Building.Key_Forge:
                            forgeButton = NSCastleScreen.ForgeButton.GetInstance(forge);
                            break;
                        case AdminBRO.Building.Key_MagicGuild:
                            magicGuildButton = NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
                            break;
                        case AdminBRO.Building.Key_Portal:
                            portalButton = NSCastleScreen.PortalButton.GetInstance(portal);
                            break;
                        case AdminBRO.Building.Key_Castle:
                            castleButton = NSCastleScreen.CastleButton.GetInstance(castle);
                            break;
                        case AdminBRO.Building.Key_Municipality:
                            municipalityButton = NSCastleScreen.MunicipalityButton.GetInstance(municipality);
                            break;
                        case AdminBRO.Building.Key_Laboratory:
                            laboratoryButton = NSCastleScreen.LaboratoryButton.GetInstance(laboratory);
                            break;
                        case AdminBRO.Building.Key_Catacombs:
                            catacombsButton = NSCastleScreen.CatacombsButton.GetInstance(catacombs);
                            break;
                        case AdminBRO.Building.Key_Aerostat:
                            aerostatButton = NSCastleScreen.AerostatButton.GetInstance(aerostat);
                            break;
                    }
                }
            }

            eventsPanel = EventsWidget.GetInstance(transform);
            eventsPanel.Hide();
            questsPanel = QuestsWidget.GetInstance(transform);
            questsPanel.Hide();
            buffPanel = BuffWidget.GetInstance(transform);
            buffPanel.Hide();

            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    if (!GameData.buildings.castle.isBuilt)
                    {
                        UITools.DisableButton(sidebarButton);
                    }
                    break;
            }

            var button = GetBuildingButtonByKey(inputData?.buildedBuildingKey);
            button?.Hide();
            
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            //ftue part
            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    {
                        var showPanelTasks = new List<Task>();
                        showPanelTasks.Add(questsPanel.ShowAsync());
                        showPanelTasks.Add(buffPanel.ShowAsync());
                        await Task.WhenAll(showPanelTasks);
                    }

                    if (GameData.buildings.castle.isBuilt)
                    {
                        await castleButton.ShowAsync();
                        GameData.ftue.info.chapter1.ShowNotifByKey("barrackstutor2");   
                    }
                    else
                    {
                        GameData.ftue.info.chapter1.ShowNotifByKey("barrackstutor1");
                    }
                    break;

                default:
                    {
                        var showPanelTasks = new List<Task>();
                        showPanelTasks.Add(questsPanel.ShowAsync());
                        showPanelTasks.Add(buffPanel.ShowAsync());
                        showPanelTasks.Add(eventsPanel.ShowAsync());
                        await Task.WhenAll(showPanelTasks);
                    }
                    break;
            }

            var buildingButton = GetBuildingButtonByKey(inputData.buildedBuildingKey);

            if (buildingButton != null)
            { 
                await buildingButton.ShowAsync();
            }
            
            await Task.CompletedTask;
        }

        private NSCastleScreen.BaseButton GetBuildingButtonByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castleButton,
                AdminBRO.Building.Key_Aerostat => aerostatButton,
                AdminBRO.Building.Key_Municipality => municipalityButton,
                AdminBRO.Building.Key_Catacombs => catacombsButton,
                AdminBRO.Building.Key_Harem => haremButton,
                AdminBRO.Building.Key_MagicGuild => magicGuildButton,
                AdminBRO.Building.Key_Forge => forgeButton,
                AdminBRO.Building.Key_Portal => portalButton,
                AdminBRO.Building.Key_Laboratory => laboratoryButton,
                AdminBRO.Building.Key_Market => marketButton,
                _ => null
            };
        }
        
        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_CastleWindowShow);
            music = SoundManager.GetEventInstance(FMODEventPath.Music_CastleScreen);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_CastleWindowHide);
            music?.Stop();
        }

        private void SidebarButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<SidebarMenuOverlay>();
        }
    }

    public class CastleScreenInData : BaseFullScreenInData
    {
        public string buildedBuildingKey;
    }
}