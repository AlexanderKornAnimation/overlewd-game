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

        public override async Task BeforeShowDataAsync()
        {
            await GameData.buildings.municipality.GetTimeLeft();

            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            foreach (var buildingData in GameData.buildings.buildingsMeta)
            {
                if (buildingData.isBuilt)
                {
                    switch (buildingData.key)
                    {
                        case AdminBRO.Building.Key_Harem:
                            haremButton = NSCastleScreen.HaremButton.GetInstance(harem);
                            haremButton.buildingId = buildingData.id;
                            haremBuilding = NSCastleScreen.HaremBuilding.GetInstance(haremBuildingPos);
                            haremBuilding.buildingId = buildingData.id;
                            haremBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Market:
                            marketButton = NSCastleScreen.MarketButton.GetInstance(market);
                            marketButton.buildingId = buildingData.id;
                            marketBuilding = NSCastleScreen.MarketBuilding.GetInstance(marketBuildingPos);
                            marketBuilding.buildingId = buildingData.id;
                            marketBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Forge:
                            forgeButton = NSCastleScreen.ForgeButton.GetInstance(forge);
                            forgeButton.buildingId = buildingData.id;
                            forgeBuilding = NSCastleScreen.ForgeBuilding.GetInstance(forgeBuildingPos);
                            forgeBuilding.buildingId = buildingData.id;
                            forgeBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_MagicGuild:
                            magicGuildButton = NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
                            magicGuildButton.buildingId = buildingData.id;
                            magicGuildBuilding = NSCastleScreen.MagicGuildBuilding.GetInstance(magicGuildBuildingPos);
                            magicGuildBuilding.buildingId = buildingData.id;
                            magicGuildBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Portal:
                            portalButton = NSCastleScreen.PortalButton.GetInstance(portal);
                            portalButton.buildingId = buildingData.id;
                            portalBuilding = NSCastleScreen.PortalBuilding.GetInstance(portalBuildingPos);
                            portalBuilding.buildingId = buildingData.id;
                            portalBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Castle:
                            castleButton = NSCastleScreen.CastleButton.GetInstance(castle);
                            castleButton.buildingId = buildingData.id;
                            castleBuilding = NSCastleScreen.CastleBuilding.GetInstance(castleBuildingPos);
                            castleBuilding.buildingId = buildingData.id;
                            castleBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Municipality:
                            municipalityButton = NSCastleScreen.MunicipalityButton.GetInstance(municipality);
                            municipalityButton.buildingId = buildingData.id;
                            municipalityBuilding = NSCastleScreen.MunicipalityBuilding.GetInstance(municipalityBuildingPos);
                            municipalityBuilding.buildingId = buildingData.id;
                            municipalityBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Laboratory:
                            laboratoryButton = NSCastleScreen.LaboratoryButton.GetInstance(laboratory);
                            laboratoryButton.buildingId = buildingData.id;
                            laboratoryBuilding = NSCastleScreen.LaboratoryBuilding.GetInstance(laboratoryBuildingPos);
                            laboratoryBuilding.buildingId = buildingData.id;
                            laboratoryBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Catacombs:
                            catacombsButton = NSCastleScreen.CatacombsButton.GetInstance(catacombs);
                            catacombsButton.buildingId = buildingData.id;
                            catacombsBuilding = NSCastleScreen.CatacombsBuilding.GetInstance(catacombsBuildingPos);
                            catacombsBuilding.buildingId = buildingData.id;
                            catacombsBuilding.Customize();
                            break;
                        case AdminBRO.Building.Key_Aerostat:
                            aerostatButton = NSCastleScreen.AerostatButton.GetInstance(aerostat);
                            aerostatButton.buildingId = buildingData.id;
                            aerostatBuilding = NSCastleScreen.AerostatBuilding.GetInstance(aerostatBuildingPos);
                            aerostatBuilding.buildingId = buildingData.id;
                            aerostatBuilding.Customize();
                            break;
                    }
                }
            }

            eventsPanel = EventsWidget.GetInstance(transform);
            if (!GameData.progressFlags.eventsWidgetEnabled)
            {
                eventsPanel.Hide();
            }
            questsPanel = QuestsWidget.GetInstance(transform);
            buffPanel = BuffWidget.GetInstance(transform);
            DevWidget.GetInstance(transform);

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_4):
                    UITools.DisableButton(sidebarButton);
                    marketButton?.DisableButton();
                    if (GameData.buildings.castle.meta.isBuilt)
                    {
                        municipalityButton?.DisableButton();
                    }
                    break;
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                    UITools.DisableButton(sidebarButton);
                    castleButton?.DisableButton();
                    marketButton?.DisableButton();
                    if (GameData.buildings.portal.meta.isBuilt)
                    {
                        municipalityButton?.DisableButton();
                    }
                    break;
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_2):
                    UITools.DisableButton(sidebarButton);
                    portalButton?.DisableButton();
                    castleButton?.DisableButton();
                    marketButton?.DisableButton();
                    if (GameData.buildings.harem.meta.isBuilt)
                    {
                        municipalityButton?.DisableButton();
                    }
                    break;
                case (FTUE.CHAPTER_2, FTUE.SEX_2):
                    UITools.DisableButton(sidebarButton);
                    portalButton?.DisableButton();
                    castleButton?.DisableButton();
                    marketButton?.DisableButton();
                    haremButton?.DisableButton();
                    if (GameData.buildings.laboratory.meta.isBuilt)
                    {
                        municipalityButton?.DisableButton();
                    }
                    break;
                case (FTUE.CHAPTER_3, FTUE.DIALOGUE_1):
                    UITools.DisableButton(sidebarButton);
                    portalButton?.DisableButton();
                    castleButton?.DisableButton();
                    marketButton?.DisableButton();
                    haremButton?.DisableButton();
                    laboratoryButton?.DisableButton();
                    if (GameData.buildings.magicGuild.meta.isBuilt)
                    {
                        municipalityButton?.DisableButton();
                    }
                    break;
                case (FTUE.CHAPTER_3, FTUE.DIALOGUE_4):
                    UITools.DisableButton(sidebarButton);
                    portalButton?.DisableButton();
                    castleButton?.DisableButton();
                    marketButton?.DisableButton();
                    haremButton?.DisableButton();
                    laboratoryButton?.DisableButton();
                    magicGuildButton?.DisableButton();
                    if (GameData.buildings.forge.meta.isBuilt)
                    {
                        municipalityButton?.DisableButton();
                    }
                    break;
                case (FTUE.CHAPTER_3, FTUE.DIALOGUE_5):
                    if (!GameData.buildings.aerostat.meta.isBuilt)
                    {
                        UITools.DisableButton(sidebarButton);
                        portalButton?.DisableButton();
                        castleButton?.DisableButton();
                        marketButton?.DisableButton();
                        haremButton?.DisableButton();
                        laboratoryButton?.DisableButton();
                        magicGuildButton?.DisableButton();
                        forgeButton?.DisableButton();
                    }
                    break;
            }

            var building = GetBuildingByKey(inputData?.buildedBuildingKey);

            if (building.building != null && building.button != null)
            {
                var buildingData = GameData.buildings.GetBuildingMetaById(building.building.buildingId.Value);
                
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
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_4):
                    {
                        if (GameData.buildings.castle.meta.isBuilt)
                        {
                            await castleButton.ShowAsync();
                            GameData.ftue.chapter1.ShowNotifByKey("barrackstutor2");
                        }
                        else
                        {
                            GameData.ftue.chapter1.ShowNotifByKey("barrackstutor1");
                        }
                    }
                    break;
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                    if (GameData.buildings.portal.meta.isBuilt)
                    {
                        GameData.ftue.chapter2.ShowNotifByKey("ch2portaltutor2");
                    }
                    break;
                case (FTUE.CHAPTER_2, FTUE.SEX_2):
                    if (!GameData.buildings.laboratory.meta.isBuilt)
                    {
                        GameData.ftue.chapter2.ShowNotifByKey("ch2labtutor1");
                    }
                    break;
                case (FTUE.CHAPTER_3, FTUE.DIALOGUE_1):
                    if (!GameData.buildings.magicGuild.meta.isBuilt)
                    {
                        GameData.ftue.chapter3.ShowNotifByKey("ch3guildtutor1");
                    }
                    else
                    {
                        GameData.ftue.chapter3.ShowNotifByKey("ch3guildtutor2");
                    }
                    break;
                default:
                    break;
            }

            var building = GetBuildingByKey(inputData?.buildedBuildingKey);

            if (building.building != null && building.button != null)
            {
                if (building.building.buildingId != null)
                {
                    var buildingData = GameData.buildings.GetBuildingMetaById(building.building.buildingId.Value);
                    await building.building.ShowAsync();
                
                    if (buildingData.currentLevel == 0)
                    {
                        await building.button.ShowAsync();
                    }
                }
            }
            
            await Task.CompletedTask;
        }

        public override void OnUIEvent(UIEvent eventData)
        {
            switch (eventData.id)
            {
                case UIEventId.ChangeScreenComplete:
                    switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
                    {
                        case (FTUE.CHAPTER_3, FTUE.DIALOGUE_4):
                            if (GameData.buildings.forge.meta.isBuilt)
                            {
                                UIManager.MakeScreen<ForgeScreen>().
                                    SetData(new ForgeScreenInData
                                    {
                                        activeTabId = ForgeScreen.TabBattleGirlsEquip
                                    }).DoShow();
                            }
                            break;
                        case (FTUE.CHAPTER_3, FTUE.DIALOGUE_5):
                            if (GameData.buildings.aerostat.meta.isBuilt)
                            {
                                UIManager.ShowOverlay<EventOverlay>();
                            }
                            break;
                    }
                    break;
            }
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
            SoundManager.PlayBGMusic(FMODEventPath.Music_CastleScreen);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_CastleWindowHide);
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