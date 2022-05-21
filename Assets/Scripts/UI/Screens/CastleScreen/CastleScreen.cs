using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseFullScreen
    {
        private Transform harem;
        private Transform market;
        private Transform forge;
        private Transform magicGuild;
        private Transform portal;
        private Transform castle;
        private Transform municipality;
        private Transform cathedral;
        private Transform catacombs;
        private Transform aerostat;

        private NSCastleScreen.HaremButton haremButton;
        private NSCastleScreen.MarketButton marketButton;
        private NSCastleScreen.ForgeButton forgeButton;
        private NSCastleScreen.MagicGuildButton magicGuildButton;
        private NSCastleScreen.PortalButton portalButton;
        private NSCastleScreen.CastleButton castleButton;
        private NSCastleScreen.MunicipalityButton municipalityButton;
        private NSCastleScreen.CathedralButton cathedralButton;
        private NSCastleScreen.CatacombsButton catacombsButton;
        private NSCastleScreen.AerostatButton aerostatButton;

        private EventsWidget eventsPanel;
        private QuestsWidget questsPanel;
        private BuffWidget buffPanel;
        private SidebarButtonWidget sidebarButton;

        private FMODEvent music;

        private CastleScreenInData inputData;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/CastleScreen/CastleScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            harem = canvas.Find("Harem");
            portal = canvas.Find("Portal");
            market = canvas.Find("Market");
            forge = canvas.Find("Forge");
            magicGuild = canvas.Find("MagicGuild");
            castle = canvas.Find("Castle");
            municipality = canvas.Find("Municipality");
            cathedral = canvas.Find("Cathedral");
            catacombs = canvas.Find("Catacombs");
            aerostat = canvas.Find("Aerostat");
        }

        public CastleScreen SetData(CastleScreenInData data)
        {
            inputData = data;
            return this;
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
                            if (GameData.ftue.stats.IsLastEnededStage("battle4", "chapter1")) {
                                castleButton.Hide();
                            }
                            break;
                        case AdminBRO.Building.Key_Municipality:
                            municipalityButton = NSCastleScreen.MunicipalityButton.GetInstance(municipality);
                            break;
                        case AdminBRO.Building.Key_Cathedral:
                            cathedralButton = NSCastleScreen.CathedralButton.GetInstance(cathedral);
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
            sidebarButton = SidebarButtonWidget.GetInstance(transform);

            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    if (!GameData.buildings.castle.isBuilt)
                    {
                        sidebarButton.DisableButton();
                    }
                    break;
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
            
            await Task.CompletedTask;
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
    }

    public class CastleScreenInData : BaseScreenInData
    {
    }
}