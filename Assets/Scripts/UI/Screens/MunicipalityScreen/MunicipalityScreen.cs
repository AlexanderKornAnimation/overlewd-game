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
        protected Button municipalityButton;
        protected GameObject municipalityMaxLevel;
        protected TextMeshProUGUI municipalityName;

        protected Button forgeButton;
        protected GameObject forgeMaxLevel;
        protected TextMeshProUGUI forgeName;

        protected Button magicGuildButton;
        protected GameObject magicGuildMaxLevel;
        protected TextMeshProUGUI magicGuildName;

        protected Button marketButton;
        protected GameObject marketMaxLevel;
        protected TextMeshProUGUI marketName;

        protected Button portalButton;
        protected GameObject portalMaxLevel;
        protected TextMeshProUGUI portalName;

        protected Button castleButton;
        protected GameObject castleMaxLevel;
        protected TextMeshProUGUI castleName;

        protected Button cathedralButton;
        protected GameObject cathedralMaxLevel;
        protected TextMeshProUGUI cathedralName;

        protected Button aerostatButton;
        protected GameObject aerostatMaxLevel;
        protected TextMeshProUGUI aerostatName;

        protected Button catacombsButton;
        protected GameObject catacombsMaxLevel;
        protected TextMeshProUGUI catacombsName;

        protected Button haremButton;
        protected GameObject haremMaxLevel;
        protected TextMeshProUGUI haremName;

        protected Button backButton;

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

        protected TextMeshProUGUI NameByKey(string key)
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

        protected GameObject AvailableByKey(string key)
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

        protected GameObject MaxLevelByKey(string key)
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

                if (!buildingData.isBuilded)
                    AvailableByKey(buildingData.key).SetActive(false);
                if (!buildingData.isMax)
                    MaxLevelByKey(buildingData.key).SetActive(false);
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        public override void UpdateGameData(GameDataEvent eventData)
        {
            switch (eventData?.type)
            {
                case GameDataEvent.Type.BuildingWasBuild:
                    Customize();
                    break;
            }
        }

        public override async Task FocusRestoredAsync()
        {
            
            await Task.CompletedTask;
        }

        protected virtual void MunicipalityButtonClick()
        {

        }

        protected virtual void ForgeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Forge)?.id
            }).RunShowPopupProcess();
        }

        protected virtual void MagicGuildButtonClick()
        {
        }

        protected virtual void MarketButtonClick()
        {
        }

        protected virtual void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().SetData(new BuildingPopupInData
            {
                buildingId = GameData.GetBuildingByKey(AdminBRO.Building.Key_Portal)?.id
            }).RunShowPopupProcess();
        }

        protected virtual void CastleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        protected virtual void CathedralButtonClick()
        {
        }

        protected virtual void AerostatButtonClick()
        {
        }

        protected virtual void CatacombsButtonClick()
        {
        }

        protected virtual void HaremButtonClick()
        {
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }
}