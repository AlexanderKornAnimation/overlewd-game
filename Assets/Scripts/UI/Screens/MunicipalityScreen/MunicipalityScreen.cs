using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace Overlewd
{
    public class MunicipalityScreen : BaseFullScreenParent<MunicipalityScreenInData>
    {
        private List<GameObject> municipalityUnavailables = new List<GameObject>();
        private List<GameObject> municipalityAvailables = new List<GameObject>();
        private Transform municipality;
        private Button municipalityButton;
        private GameObject municipalityMaxLevel;
        private TextMeshProUGUI municipalityName;

        private List<GameObject> forgeUnavailables = new List<GameObject>();
        private List<GameObject> forgeAvailables = new List<GameObject>();
        private Transform forge;
        private Button forgeButton;
        private GameObject forgeMaxLevel;
        private TextMeshProUGUI forgeName;

        private List<GameObject> magicGuildUnavailables = new List<GameObject>();
        private List<GameObject> magicGuildAvailables = new List<GameObject>();
        private Transform magicGuild;
        private Button magicGuildButton;
        private GameObject magicGuildMaxLevel;
        private TextMeshProUGUI magicGuildName;

        private Transform market;
        private Button marketButton;
        private GameObject marketMaxLevel;
        private TextMeshProUGUI marketName;

        private Transform portal;
        private Button portalButton;
        private GameObject portalMaxLevel;
        private TextMeshProUGUI portalName;

        private Transform castle;
        private Button castleButton;
        private GameObject castleMaxLevel;
        private TextMeshProUGUI castleName;

        private Transform laboratory;
        private Button laboratoryButton;
        private GameObject laboratoryMaxLevel;
        private TextMeshProUGUI laboratoryName;

        private Transform aerostat;
        private Button aerostatButton;
        private GameObject aerostatMaxLevel;
        private TextMeshProUGUI aerostatName;

        private Transform catacombs;
        private Button catacombsButton;
        private GameObject catacombsMaxLevel;
        private TextMeshProUGUI catacombsName;

        private List<GameObject> haremUnavailables = new List<GameObject>();
        private List<GameObject> haremAvailables = new List<GameObject>();
        private Transform harem;
        private Button haremButton;
        private GameObject haremMaxLevel;
        private TextMeshProUGUI haremName;

        private Button backButton;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MunicipalityScreen/MunicipalityScreen",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var grid = canvas.Find("Grid");
            
            municipality = grid.Find("Municipality");
            municipalityButton = municipality.Find("Button").GetComponent<Button>();
            municipalityMaxLevel = municipality.transform.Find("MaxLevel").gameObject;
            municipalityName = municipality.Find("Name").GetComponent<TextMeshProUGUI>();

            forge = grid.Find("Forge");
            forgeButton = forge.Find("Button").GetComponent<Button>();
            forgeMaxLevel = forge.Find("MaxLevel").gameObject;
            forgeName = forge.Find("Name").GetComponent<TextMeshProUGUI>();

            magicGuild = grid.Find("MagicGuild");
            magicGuildButton = magicGuild.Find("Button").GetComponent<Button>();
            magicGuildMaxLevel = magicGuild.Find("MaxLevel").gameObject;
            magicGuildName = magicGuild.Find("Name").GetComponent<TextMeshProUGUI>();

            market = grid.Find("Market");
            marketButton = market.Find("Button").GetComponent<Button>();
            marketMaxLevel = market.Find("MaxLevel").gameObject;
            marketName = market.Find("Name").GetComponent<TextMeshProUGUI>();

            portal = grid.Find("Portal");
            portalButton = grid.Find("Portal").Find("Button").GetComponent<Button>();
            portalMaxLevel = portal.Find("MaxLevel").gameObject;
            portalName = portal.Find("Name").GetComponent<TextMeshProUGUI>();

            castle = grid.Find("Castle");
            castleButton = castle.Find("Button").GetComponent<Button>();
            castleMaxLevel = castle.Find("MaxLevel").gameObject;
            castleName = castle.Find("Name").GetComponent<TextMeshProUGUI>();

            laboratory = grid.Find("Laboratory");
            laboratoryButton = laboratory.Find("Button").GetComponent<Button>();
            laboratoryMaxLevel = laboratory.Find("MaxLevel").gameObject;
            laboratoryName = laboratory.Find("Name").GetComponent<TextMeshProUGUI>();

            aerostat = grid.Find("Aerostat");
            aerostatButton = aerostat.Find("Button").GetComponent<Button>();
            aerostatMaxLevel = aerostat.Find("MaxLevel").gameObject;
            aerostatName = aerostat.Find("Name").GetComponent<TextMeshProUGUI>();

            catacombs = grid.Find("Catacombs");
            catacombsButton = catacombs.Find("Button").GetComponent<Button>();
            catacombsMaxLevel = catacombs.Find("MaxLevel").gameObject;
            catacombsName = catacombs.Find("Name").GetComponent<TextMeshProUGUI>();

            harem = grid.Find("Harem");
            haremButton = harem.Find("Button").GetComponent<Button>();
            haremMaxLevel = harem.Find("MaxLevel").gameObject;
            haremName = harem.Find("Name").GetComponent<TextMeshProUGUI>();

            foreach (var buildingData in GameData.buildings.buildings)
            {
                var levels = LevelsByKey(buildingData.key);
                var buildingTransform = BuildingTransformByKey(buildingData.key);
                var availableButton = AvailableByKey(buildingData.key);

                if (levels != (null, null) && buildingTransform != null && availableButton != null)
                {
                    for (int i = 1; i <= buildingData.levels.Count; i++)
                    {
                        levels.available.Add(availableButton.transform.Find($"AvailableLevel{i}").gameObject);
                        levels.unavailable.Add(buildingTransform.Find($"UnavailableLevel{i}").gameObject);
                    }
                }
            }

            backButton = canvas.Find("BackButton").GetComponent<Button>();

            backButton.onClick.AddListener(BackButtonClick);
            municipalityButton.onClick.AddListener(MunicipalityButtonClick);
            forgeButton.onClick.AddListener(ForgeButtonClick);
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            marketButton.onClick.AddListener(MarketButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            castleButton.onClick.AddListener(CastleButtonClick);
            laboratoryButton.onClick.AddListener(CathedralButtonClick);
            aerostatButton.onClick.AddListener(AerostatButtonClick);
            catacombsButton.onClick.AddListener(CatacombsButtonClick);
            haremButton.onClick.AddListener(HaremButtonClick);
        }

        private void Customize()
        {
            foreach (var buildingData in GameData.buildings.buildings)
            {
                var isAvailable = !buildingData.isMax;
                NameByKey(buildingData.key).text =
                    isAvailable ? $"{buildingData.name} \nlevel <size=38>{buildingData.currentLevel + 1}" : buildingData.name;

                AvailableByKey(buildingData.key).SetActive(isAvailable);
                
                MaxLevelByKey(buildingData.key).SetActive(buildingData.isMax);
                
                var levels = LevelsByKey(buildingData.key);

                if (levels.available != null && levels.unavailable != null) 
                {
                    for (var i = 0; i < levels.available.Count; i++)
                    {
                        levels.available[i].SetActive(buildingData.nextLevel == i);
                        levels.unavailable[i].SetActive(buildingData.nextLevel == i);
                    }
                }
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            //ftue part
            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    if (!GameData.buildings.castle.isBuilt)
                    {
                        GameData.ftue.info.chapter1.ShowNotifByKey("quickbuildtutor");
                    }

                    break;
            }

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

        private Transform BuildingTransformByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castle,
                AdminBRO.Building.Key_Catacombs => catacombs,
                AdminBRO.Building.Key_Laboratory => laboratory,
                AdminBRO.Building.Key_Aerostat => aerostat,
                AdminBRO.Building.Key_Forge => forge,
                AdminBRO.Building.Key_Harem => harem,
                AdminBRO.Building.Key_MagicGuild => magicGuild,
                AdminBRO.Building.Key_Market => market,
                AdminBRO.Building.Key_Municipality => municipality,
                AdminBRO.Building.Key_Portal => portal,
                _ => null
            };
        }
        
        private TextMeshProUGUI NameByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Castle => castleName,
                AdminBRO.Building.Key_Catacombs => catacombsName,
                AdminBRO.Building.Key_Laboratory => laboratoryName,
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
                AdminBRO.Building.Key_Laboratory => laboratoryButton.gameObject,
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
                AdminBRO.Building.Key_Laboratory => laboratoryMaxLevel,
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

        private (List<GameObject> available, List<GameObject> unavailable) LevelsByKey(string key)
        {
            return key switch
            {
                AdminBRO.Building.Key_Harem => (haremAvailables, haremUnavailables),
                AdminBRO.Building.Key_Municipality => (municipalityAvailables, municipalityUnavailables),
                AdminBRO.Building.Key_MagicGuild => (magicGuildAvailables, magicGuildUnavailables),
                AdminBRO.Building.Key_Forge => (forgeAvailables, forgeUnavailables),
                _ => (null, null)
            };
        }
        
        private void MunicipalityButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.municipality?.id
            }).RunShowPopupProcess();
        }

        private void ForgeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.forge?.id
            }).RunShowPopupProcess();
        }

        private void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.magicGuild?.id
            }).RunShowPopupProcess();
        }

        private void MarketButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.market?.id
            }).RunShowPopupProcess();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.portal?.id
            }).RunShowPopupProcess();
        }

        private void CastleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.castle?.id,
            }).RunShowPopupProcess();
        }

        private void CathedralButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.laboratory?.id
            }).RunShowPopupProcess();
        }

        private void AerostatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.aerostat?.id
            }).RunShowPopupProcess();
        }

        private void CatacombsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.catacombs?.id
            }).RunShowPopupProcess();
        }

        private void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BuildingPopup>().
                SetData(new BuildingPopupInData
            {
                buildingId = GameData.buildings.harem?.id
            }).RunShowPopupProcess();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class MunicipalityScreenInData : BaseFullScreenInData
    {
    }
}