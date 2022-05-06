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
        protected GameObject municipalityUnaviable;
        protected GameObject municipalityMaxLevel;
        protected TextMeshProUGUI municipalityName;

        protected Button forgeButton;
        protected GameObject forgeUnaviable;
        protected GameObject forgeMaxLevel;
        protected TextMeshProUGUI forgeName;

        protected Button magicGuildButton;
        protected GameObject magicGuildUnaviable;
        protected GameObject magicGuildMaxLevel;
        protected TextMeshProUGUI magicGuildName;

        protected Button marketButton;
        protected GameObject marketUnaviable;
        protected GameObject marketMaxLevel;
        protected TextMeshProUGUI marketName;

        protected Button portalButton;
        protected GameObject portalUnaviable;
        protected GameObject portalMaxLevel;
        protected TextMeshProUGUI portalName;

        protected Button castleButton;
        protected GameObject castleUnaviable;
        protected GameObject castleMaxLevel;
        protected TextMeshProUGUI castleName;

        protected Button cathedralButton;
        protected GameObject cathedralUnaviable;
        protected GameObject cathedralMaxLevel;
        protected TextMeshProUGUI cathedralName;

        protected Button aerostatButton;
        protected GameObject aerostatUnaviable;
        protected GameObject aerostatMaxLevel;
        protected TextMeshProUGUI aerostatName;

        protected Button catacombsButton;
        protected GameObject catacombsUnaviable;
        protected GameObject catacombsMaxLevel;
        protected TextMeshProUGUI catacombsName;

        protected Button haremButton;
        protected GameObject haremUnaviable;
        protected GameObject haremMaxLevel;
        protected TextMeshProUGUI haremName;

        protected Button backButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MunicipalityScreen/MunicipalityScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var grid = canvas.Find("Grid");

            municipalityButton = grid.Find("MunicipalityButton").GetComponent<Button>();
            municipalityUnaviable = municipalityButton.transform.Find("Unaviable").gameObject;
            municipalityMaxLevel =  municipalityButton.transform.Find("MaxLevel").gameObject;
            municipalityName = municipalityButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            forgeButton = grid.Find("ForgeButton").GetComponent<Button>();
            forgeUnaviable = forgeButton.transform.Find("Unaviable").gameObject;
            forgeMaxLevel = forgeButton.transform.Find("MaxLevel").gameObject;
            forgeName = forgeButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            magicGuildButton = grid.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildUnaviable = magicGuildButton.transform.Find("Unaviable").gameObject;
            magicGuildMaxLevel = magicGuildButton.transform.Find("MaxLevel").gameObject;
            magicGuildName = magicGuildButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            marketButton = grid.Find("MarketButton").GetComponent<Button>();
            marketUnaviable = marketButton.transform.Find("Unaviable").gameObject;
            marketMaxLevel = marketButton.transform.Find("MaxLevel").gameObject;
            marketName = marketButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            portalButton = grid.Find("PortalButton").GetComponent<Button>();
            portalUnaviable = portalButton.transform.Find("Unaviable").gameObject;
            portalMaxLevel = portalButton.transform.Find("MaxLevel").gameObject;
            portalName = portalButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            castleButton = grid.Find("CastleButton").GetComponent<Button>();
            castleUnaviable = castleButton.transform.Find("Unaviable").gameObject;
            castleMaxLevel = castleButton.transform.Find("MaxLevel").gameObject;
            castleName = castleButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            cathedralButton = grid.Find("CathedralButton").GetComponent<Button>();
            cathedralUnaviable = cathedralButton.transform.Find("Unaviable").gameObject;
            cathedralMaxLevel = cathedralButton.transform.Find("MaxLevel").gameObject;
            cathedralName = cathedralButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            aerostatButton = grid.Find("AerostatButton").GetComponent<Button>();
            aerostatUnaviable = aerostatButton.transform.Find("Unaviable").gameObject;
            aerostatMaxLevel = aerostatButton.transform.Find("MaxLevel").gameObject;
            aerostatName = aerostatButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            catacombsButton = grid.Find("CatacombsButton").GetComponent<Button>();
            catacombsUnaviable = catacombsButton.transform.Find("Unaviable").gameObject;
            catacombsMaxLevel = catacombsButton.transform.Find("MaxLevel").gameObject;
            catacombsName = catacombsButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            haremButton = grid.Find("HaremButton").GetComponent<Button>();
            haremUnaviable = haremButton.transform.Find("Unaviable").gameObject;
            haremMaxLevel = haremButton.transform.Find("MaxLevel").gameObject;
            haremName = haremButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

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

        protected GameObject UnaviableByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castleUnaviable,
                AdminBRO.Building.Key_Catacombs => catacombsUnaviable,
                AdminBRO.Building.Key_Cathedral => cathedralUnaviable,
                AdminBRO.Building.Key_Aerostat => aerostatUnaviable,
                AdminBRO.Building.Key_Forge => forgeUnaviable,
                AdminBRO.Building.Key_Harem => haremUnaviable,
                AdminBRO.Building.Key_MagicGuild => magicGuildUnaviable,
                AdminBRO.Building.Key_Market => marketUnaviable,
                AdminBRO.Building.Key_Municipality => municipalityUnaviable,
                AdminBRO.Building.Key_Portal => portalUnaviable,
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

        public override async Task BeforeShowMakeAsync()
        {
            foreach (var buildingData in GameData.buildings)
            {
                NameByKey(buildingData.key).text = buildingData.name;
            }

            await Task.CompletedTask;
        }

        public override void UpdateGameData(GameDataEvent eventData)
        {
            switch (eventData?.type)
            {
                case GameDataEvent.Type.BuildingWasBuild:

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
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
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
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
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
