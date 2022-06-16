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
        private Transform haremBuildingPos;
        private Transform marketBuildingPos;
        private Transform forgeBuildingPos;
        private Transform magicGuildBuildingPos;
        private Transform portalBuildingPos;
        private Transform castleBuildingPos;
        private Transform municipalityBuildingPos;
        private Transform laboratoryBuildingPos;
        private Transform catacombsBuildingPos;
        private Transform aerostatBuildingPos;

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

        private NSCastleScreen.BaseBuilding haremBuilding;
        private NSCastleScreen.BaseBuilding marketBuilding;
        private NSCastleScreen.BaseBuilding forgeBuilding;
        private NSCastleScreen.BaseBuilding magicGuildBuilding;
        private NSCastleScreen.BaseBuilding portalBuilding;
        private NSCastleScreen.BaseBuilding castleBuilding;
        private NSCastleScreen.BaseBuilding municipalityBuilding;
        private NSCastleScreen.BaseBuilding laboratoryBuilding;
        private NSCastleScreen.BaseBuilding catacombsBuilding;
        private NSCastleScreen.BaseBuilding aerostatBuilding;
        
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

            haremBuildingPos = canvas.Find("HaremBuildingPos");
            portalBuildingPos = canvas.Find("PortalBuildingPos");
            marketBuildingPos = canvas.Find("MarketBuildingPos");
            forgeBuildingPos = canvas.Find("ForgeBuildingPos");
            magicGuildBuildingPos = canvas.Find("MagicGuildBuildingPos");
            castleBuildingPos = canvas.Find("CastleBuildingPos");
            municipalityBuildingPos = canvas.Find("MunicipalityBuildingPos");
            laboratoryBuildingPos = canvas.Find("LaboratoryBuildingPos");
            catacombsBuildingPos = canvas.Find("CatacombsBuildingPos");
            aerostatBuildingPos = canvas.Find("AerostatBuildingPos");
        }

        public override async Task BeforeShowMakeAsync()
        {
            foreach (var buildingData in GameData.buildings.buildings)
            {
                if (buildingData.isBuilt)
                {
                    switch (buildingData.key)
                    {
                        case AdminBRO.Building.Key_Harem:
                            haremButton = NSCastleScreen.HaremButton.GetInstance(harem);
                            haremBuilding = NSCastleScreen.HaremBuilding.GetInstance(haremBuildingPos);
                            haremBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Market:
                            marketButton = NSCastleScreen.MarketButton.GetInstance(market);
                            marketBuilding = NSCastleScreen.MarketBuilding.GetInstance(marketBuildingPos);
                            marketBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Forge:
                            forgeButton = NSCastleScreen.ForgeButton.GetInstance(forge);
                            forgeBuilding = NSCastleScreen.ForgeBuilding.GetInstance(forgeBuildingPos);
                            forgeBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_MagicGuild:
                            magicGuildButton = NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
                            magicGuildBuilding = NSCastleScreen.MagicGuildBuilding.GetInstance(magicGuildBuildingPos);
                            magicGuildBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Portal:
                            portalButton = NSCastleScreen.PortalButton.GetInstance(portal);
                            portalBuilding = NSCastleScreen.PortalBuilding.GetInstance(portalBuildingPos);
                            portalBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Castle:
                            castleButton = NSCastleScreen.CastleButton.GetInstance(castle);
                            castleBuilding = NSCastleScreen.CastleBuilding.GetInstance(castleBuildingPos);
                            castleBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Municipality:
                            municipalityButton = NSCastleScreen.MunicipalityButton.GetInstance(municipality);
                            municipalityBuilding = NSCastleScreen.MunicipalityBuilding.GetInstance(municipalityBuildingPos);
                            municipalityBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Laboratory:
                            laboratoryButton = NSCastleScreen.LaboratoryButton.GetInstance(laboratory);
                            laboratoryBuilding = NSCastleScreen.LaboratoryBuilding.GetInstance(laboratoryBuildingPos);
                            laboratoryBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Catacombs:
                            catacombsButton = NSCastleScreen.CatacombsButton.GetInstance(catacombs);
                            catacombsBuilding = NSCastleScreen.CatacombsBuilding.GetInstance(catacombsBuildingPos);
                            catacombsBuilding.buildingId = buildingData.id;
                            break;
                        case AdminBRO.Building.Key_Aerostat:
                            aerostatButton = NSCastleScreen.AerostatButton.GetInstance(aerostat);
                            aerostatBuilding = NSCastleScreen.AerostatBuilding.GetInstance(aerostatBuildingPos);
                            aerostatBuilding.buildingId = buildingData.id;
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
            DevWidget.GetInstance(transform);

            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    if (!GameData.buildings.castle.isBuilt)
                    {
                        UITools.DisableButton(sidebarButton);
                    }
                    break;
            }

            var building = GetBuildingByKey(inputData?.buildedBuildingKey);

            if (building.building != null && building.button != null)
            {
                var buildingData = GameData.buildings.GetBuildingById(building.building.buildingId.Value);
                building.building.Hide();
                
                if (buildingData.currentLevel == 0)
                {
                    building.button.Hide();
                }
            }
           

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

            var building = GetBuildingByKey(inputData?.buildedBuildingKey);

            if (building.building != null && building.button != null)
            {
                var buildingData = GameData.buildings.GetBuildingById(building.building.buildingId.Value);
                await building.building.ShowAsync();
                
                if (buildingData.currentLevel == 0)
                {
                    await building.button.ShowAsync();
                }
            }
            
            await Task.CompletedTask;
        }

        private (NSCastleScreen.BaseBuilding building, NSCastleScreen.BaseButton button) GetBuildingByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => (castleBuilding, castleButton),
                AdminBRO.Building.Key_Aerostat => (aerostatBuilding, aerostatButton),
                AdminBRO.Building.Key_Municipality => (municipalityBuilding, municipalityButton),
                AdminBRO.Building.Key_Catacombs => (catacombsBuilding, catacombsButton),
                AdminBRO.Building.Key_Harem => (haremBuilding, haremButton),
                AdminBRO.Building.Key_MagicGuild => (magicGuildBuilding, magicGuildButton),
                AdminBRO.Building.Key_Forge => (forgeBuilding, forgeButton),
                AdminBRO.Building.Key_Portal => (portalBuilding, portalButton),
                AdminBRO.Building.Key_Laboratory => (laboratoryBuilding, laboratoryButton),
                AdminBRO.Building.Key_Market => (marketBuilding, marketButton),
                _ => (null, null)
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