using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace Overlewd
{
    public class MunicipalityScreen : BaseFullScreen
    {
        private Button municipalityButton;
        private GameObject municipalityMaxLevel;
        private TextMeshProUGUI municipalityName;

        private Button forgeButton;
        private GameObject forgeMaxLevel;
        private TextMeshProUGUI forgeName;

        private Button magicGuildButton;
        private GameObject magicGuildMaxLevel;
        private TextMeshProUGUI magicGuildName;

        private Button marketButton;
        private GameObject marketMaxLevel;
        private TextMeshProUGUI marketName;

        private Button portalButton;
        private GameObject portalMaxLevel;
        private TextMeshProUGUI portalName;

        private Button castleButton;
        private GameObject castleMaxLevel;
        private TextMeshProUGUI castleName;

        private Button cathedralButton;
        private GameObject cathedralMaxLevel;
        private TextMeshProUGUI cathedralName;

        private Button aerostatButton;
        private GameObject aerostatMaxLevel;
        private TextMeshProUGUI aerostatName;

        private Button catacombsButton;
        private GameObject catacombsMaxLevel;
        private TextMeshProUGUI catacombsName;

        private Button haremButton;
        private GameObject haremMaxLevel;
        private TextMeshProUGUI haremName;

        private Button backButton;

        private MunicipalityScreenInData inputData;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MunicipalityScreen/MunicipalityScreen",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var grid = canvas.Find("Grid");

            var municipality = grid.Find("Municipality");
            municipalityButton = municipality.Find("Button").GetComponent<Button>();
            municipalityMaxLevel = municipality.transform.Find("MaxLevel").gameObject;
            municipalityName = municipality.Find("Name").GetComponent<TextMeshProUGUI>();

            var forge = grid.Find("Forge");
            forgeButton = forge.Find("Button").GetComponent<Button>();
            forgeMaxLevel = forge.Find("MaxLevel").gameObject;
            forgeName = forge.Find("Name").GetComponent<TextMeshProUGUI>();

            var magicGuild = grid.Find("MagicGuild");
            magicGuildButton = magicGuild.Find("Button").GetComponent<Button>();
            magicGuildMaxLevel = magicGuild.Find("MaxLevel").gameObject;
            magicGuildName = magicGuild.Find("Name").GetComponent<TextMeshProUGUI>();

            var market = grid.Find("Market");
            marketButton = market.Find("Button").GetComponent<Button>();
            marketMaxLevel = market.Find("MaxLevel").gameObject;
            marketName = market.Find("Name").GetComponent<TextMeshProUGUI>();

            var portal = grid.Find("Portal");
            portalButton = grid.Find("Portal").Find("Button").GetComponent<Button>();
            portalMaxLevel = portal.Find("MaxLevel").gameObject;
            portalName = portal.Find("Name").GetComponent<TextMeshProUGUI>();

            var castle = grid.Find("Castle");
            castleButton = castle.Find("Button").GetComponent<Button>();
            castleMaxLevel = castle.Find("MaxLevel").gameObject;
            castleName = castle.Find("Name").GetComponent<TextMeshProUGUI>();

            var cathedral = grid.Find("Cathedral");
            cathedralButton = cathedral.Find("Button").GetComponent<Button>();
            cathedralMaxLevel = cathedral.Find("MaxLevel").gameObject;
            cathedralName = cathedral.Find("Name").GetComponent<TextMeshProUGUI>();

            var aerostat = grid.Find("Aerostat");
            aerostatButton = aerostat.Find("Button").GetComponent<Button>();
            aerostatMaxLevel = aerostat.Find("MaxLevel").gameObject;
            aerostatName = aerostat.Find("Name").GetComponent<TextMeshProUGUI>();

            var catacombs = grid.Find("Catacombs");
            catacombsButton = catacombs.Find("Button").GetComponent<Button>();
            catacombsMaxLevel = catacombs.Find("MaxLevel").gameObject;
            catacombsName = catacombs.Find("Name").GetComponent<TextMeshProUGUI>();

            var harem = grid.Find("Harem");
            haremButton = harem.Find("Button").GetComponent<Button>();
            haremMaxLevel = harem.Find("MaxLevel").gameObject;
            haremName = harem.Find("Name").GetComponent<TextMeshProUGUI>();

            backButton = canvas.Find("BackButton").GetComponent<Button>();

            backButton.onClick.AddListener(BackButtonClick);
            municipalityButton.onClick.AddListener(MunicipalityButtonClick);
            forgeButton.onClick.AddListener(ForgeButtonClick);
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            marketButton.onClick.AddListener(MarketButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            castleButton.onClick.AddListener(CastleButtonClick);
            cathedralButton.onClick.AddListener(CathedralButtonClick);
            aerostatButton.onClick.AddListener(AerostatButtonClick);
            catacombsButton.onClick.AddListener(CatacombsButtonClick);
            haremButton.onClick.AddListener(HaremButtonClick);
        }

        public MunicipalityScreen SetData(MunicipalityScreenInData data)
        {
            inputData = data;
            return this;
        }

        private TextMeshProUGUI NameByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castleName,
                AdminBRO.Building.Key_Catacombs => catacombsName,
                AdminBRO.Building.Key_Cathedral => cathedralName,
                AdminBRO.Building.Key_Aerostat => aerostatName,
                AdminBRO.Building.Key_Forge => forgeName,
                AdminBRO.Building.Key_Harem => haremName,
                AdminBRO.Building.Key_MagicGuild => magicGuildName,
                AdminBRO.Building.Key_Market => marketName,
                AdminBRO.Building.Key_Municipality => municipalityName,
                AdminBRO.Building.Key_Portal => portalName,
                _ => null
            };
        }

        private GameObject AvailableByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castleButton.gameObject,
                AdminBRO.Building.Key_Catacombs => catacombsButton.gameObject,
                AdminBRO.Building.Key_Cathedral => cathedralButton.gameObject,
                AdminBRO.Building.Key_Aerostat => aerostatButton.gameObject,
                AdminBRO.Building.Key_Forge => forgeButton.gameObject,
                AdminBRO.Building.Key_Harem => haremButton.gameObject,
                AdminBRO.Building.Key_MagicGuild => magicGuildButton.gameObject,
                AdminBRO.Building.Key_Market => marketButton.gameObject,
                AdminBRO.Building.Key_Municipality => municipalityButton.gameObject,
                AdminBRO.Building.Key_Portal => portalButton.gameObject,
                _ => null
            };
        }

        private GameObject MaxLevelByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castleMaxLevel,
                AdminBRO.Building.Key_Catacombs => catacombsMaxLevel,
                AdminBRO.Building.Key_Cathedral => cathedralMaxLevel,
                AdminBRO.Building.Key_Aerostat => aerostatMaxLevel,
                AdminBRO.Building.Key_Forge => forgeMaxLevel,
                AdminBRO.Building.Key_Harem => haremMaxLevel,
                AdminBRO.Building.Key_MagicGuild => magicGuildMaxLevel,
                AdminBRO.Building.Key_Market => marketMaxLevel,
                AdminBRO.Building.Key_Municipality => municipalityMaxLevel,
                AdminBRO.Building.Key_Portal => portalMaxLevel,
                _ => null
            };
        }

        private void Customize()
        {
            foreach (var buildingData in GameData.buildings)
            {
                NameByKey(buildingData.key).text = buildingData.name;

                if (!buildingData.isBuilt)
                    AvailableByKey(buildingData.key).SetActive(false);
                if (!buildingData.isMax)
                    MaxLevelByKey(buildingData.key).SetActive(false);
            }

            switch (GameData.ftue.activeChapter?.key)
            {
                case "chapter1":
                    switch (GameData.ftueStats.lastEndedStageData?.key)
                    {
                        case "battle4":
                            AvailableByKey(AdminBRO.Building.Key_Castle).SetActive(true);
                            break;
                    }
                    break;
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData?.type)
            {
                case GameDataEvent.Type.BuildingBuildNow:
                    Customize();
                    break;
                case GameDataEvent.Type.BuildingBuildStarted:
                    Customize();
                    break;
            }
        }

        public override void OnUIEvent(UIEvent eventData)
        {
            switch (eventData?.type)
            {
                case UIEvent.Type.RestoreScreenFocusAfterPopup:
                    if (eventData.uiSenderType == typeof(BuildingPopup))
                    {
                    }

                    break;
            }
        }

        private void MunicipalityButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Municipality)?.id
            }).RunShowPopupProcess();
        }

        private void ForgeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Forge)?.id
            }).RunShowPopupProcess();
        }

        private void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_MagicGuild)?.id
            }).RunShowPopupProcess();
        }

        private void MarketButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Market)?.id
            }).RunShowPopupProcess();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Portal)?.id
            }).RunShowPopupProcess();
        }

        private void CastleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Castle)?.id,
            }).RunShowPopupProcess();
        }

        private void CathedralButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Cathedral)?.id
            }).RunShowPopupProcess();
        }

        private void AerostatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Aerostat)?.id
            }).RunShowPopupProcess();
        }

        private void CatacombsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Catacombs)?.id
            }).RunShowPopupProcess();
        }

        private void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Harem)?.id
            }).RunShowPopupProcess();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class MunicipalityScreenInData : BaseScreenInData
    {
    }
}