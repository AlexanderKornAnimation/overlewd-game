using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuildingPopup : BasePopup
    {
        private Transform background;
        private Transform imageSpawnPoint;

        private TextMeshProUGUI fullPotentialDescription;
        private TextMeshProUGUI buildingName;
        private TextMeshProUGUI description;

        private Transform resourcesGrid;
        private Transform[] resource = new Transform[4];
        private GameObject[] notEnough = new GameObject[4];
        private TextMeshProUGUI[] count = new TextMeshProUGUI[4];
        private Image[] recourceIcon = new Image[4];

        private Button backButton;

        private Button freeBuildButton;
        private TextMeshProUGUI freeBuildButtonText;

        private Button paidBuildingButton;
        private TextMeshProUGUI paidBuildingButtonText;
        private Image paidBuildingButtonIcon;

        private BuildingPopupInData inputData;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BuildingPopup/BuildingPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            background = canvas.Find("Background");
            imageSpawnPoint = background.Find("ImageSpawnPoint");

            fullPotentialDescription = canvas.Find("FullPotentialDescription").GetComponent<TextMeshProUGUI>();
            buildingName = canvas.Find("BuildingName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            resourcesGrid = canvas.Find("Grid");
            for (var i = 0; i < resource.Length; i++)
            {
                resource[i] = resourcesGrid.Find($"Recource{i + 1}");

                notEnough[i] = resource[i].Find("NotEnough").gameObject;
                count[i] = resource[i].Find("Count").GetComponent<TextMeshProUGUI>();
                recourceIcon[i] = resource[i].Find("RecourceIcon").GetComponent<Image>();
            }

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            freeBuildButton = canvas.Find("FreeBuildButton").GetComponent<Button>();
            freeBuildButton.onClick.AddListener(FreeBuildButtonClick);
            freeBuildButtonText = freeBuildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            paidBuildingButton = canvas.Find("PaidBuildingButton").GetComponent<Button>();
            paidBuildingButton.onClick.AddListener(PaidBuildingButtonClick);
            paidBuildingButtonText = paidBuildingButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            paidBuildingButtonIcon = paidBuildingButton.transform.Find("Icon").GetComponent<Image>();
        }

        public BuildingPopup SetData(BuildingPopupInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            var buildindData = inputData?.buildingData;
            if (buildindData != null)
            {
                switch (buildindData.key)
                {
                    case AdminBRO.Building.Key_Aerostat:

                        break;
                    case AdminBRO.Building.Key_Castle:

                        break;
                    case AdminBRO.Building.Key_Catacombs:

                        break;
                    case AdminBRO.Building.Key_Laboratory:

                        break;
                    case AdminBRO.Building.Key_Forge:
                        ResourceManager.InstantiateWidgetPrefab("Prefabs/UI/Popups/BuildingPopup/ForgeImage",
                            imageSpawnPoint);
                        break;
                    case AdminBRO.Building.Key_Harem:
                        ResourceManager.InstantiateWidgetPrefab("Prefabs/UI/Popups/BuildingPopup/HaremImage",
                            imageSpawnPoint);
                        break;
                    case AdminBRO.Building.Key_MagicGuild:

                        break;
                    case AdminBRO.Building.Key_Market:

                        break;
                    case AdminBRO.Building.Key_Municipality:

                        break;
                    case AdminBRO.Building.Key_Portal:
                        ResourceManager.InstantiateWidgetPrefab("Prefabs/UI/Popups/BuildingPopup/PortalImage",
                            imageSpawnPoint);
                        break;
                }
            }

            recourceIcon[0].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gold");
            recourceIcon[1].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Stone");
            recourceIcon[2].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Wood");
            recourceIcon[3].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gem");

            await Task.CompletedTask;
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual async void FreeBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_FreeBuildButton);
            await BuildNow();

            UIManager.ShowScreen<CastleScreen>();
        }

        protected virtual async void PaidBuildingButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await BuildNow();

            UIManager.ShowScreen<CastleScreen>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }

        private async Task BuildNow()
        {
            var buildingData = inputData?.buildingData;
            if (buildingData != null)
            {
                if (buildingData.canUpgradeNow)
                {
                    await GameData.buildings.BuildNow(buildingData.id);
                }
            }
        }
    }

    public class BuildingPopupInData : BaseScreenInData
    {
        public int? buildingId;

        public AdminBRO.Building buildingData =>
            buildingId.HasValue ? GameData.buildings.GetBuildingById(buildingId.Value) : null;
    }
}