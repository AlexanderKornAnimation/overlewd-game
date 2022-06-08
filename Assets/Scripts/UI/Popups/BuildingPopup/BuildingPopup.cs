using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuildingPopup : BasePopupParent<BuildingPopupInData>
    {
        private Transform background;
        private Transform imageSpawnPoint;

        private TextMeshProUGUI fullPotentialDescription;
        private TextMeshProUGUI fullPotentialTitle;
        private TextMeshProUGUI buildingName;
        private TextMeshProUGUI description;

        private Transform resourcesGrid;
        private Image[] resource = new Image[4];
        private TextMeshProUGUI[] count = new TextMeshProUGUI[4];

        private Button backButton;

        private Button freeBuildButton;
        private TextMeshProUGUI freeBuildButtonText;

        private Button paidBuildingButton;
        private TextMeshProUGUI paidBuildingButtonText;
        private Image paidBuildingButtonIcon;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BuildingPopup/BuildingPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            background = canvas.Find("Background");
            imageSpawnPoint = background.Find("ImageSpawnPoint");

            fullPotentialDescription = canvas.Find("FullPotentialDescription").GetComponent<TextMeshProUGUI>();
            fullPotentialTitle = canvas.Find("FullPotentialTitle").GetComponent<TextMeshProUGUI>();
            buildingName = canvas.Find("BuildingName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            resourcesGrid = canvas.Find("Grid");
            for (var i = 0; i < resource.Length; i++)
            {
                resource[i] = resourcesGrid.Find($"Recource{i + 1}").GetComponent<Image>();

                count[i] = resource[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
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

        public override async Task BeforeShowMakeAsync()
        {
            var buildingData = inputData?.buildingData;
            if (buildingData != null)
            {
                switch (buildingData.key)
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

                var nextLevelData = buildingData.nextLevelData;
                var prices = nextLevelData.price;
                
                for (var i = 0; i < prices.Count; i++)
                {
                    var currencyData = GameData.currencies.GetById(prices[i].currencyId);
                    resource[i].sprite = ResourceManager.LoadSprite(currencyData.icon186Url);
                    count[i].text = prices[i].amount.ToString();
                    count[i].color = buildingData.canUpgrade ? Color.white : Color.red;
                }

                var momentPrice = nextLevelData.momentPrice;
                var momentPriceAmount = momentPrice.Count > 0 ? momentPrice.First().amount : 0;

                paidBuildingButtonText.text = $"Summon building\nfor <color=red>{momentPriceAmount}</color> crystals";

                description.text = buildingData.description ?? "EMPTY";
                buildingName.text = buildingData.name ?? "EMPTY";
                fullPotentialDescription.text = nextLevelData.description ?? "EMPTY";
                fullPotentialTitle.text = nextLevelData.title ?? "EMPTY";
            }

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
        }

        protected virtual async void PaidBuildingButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await BuildNow();
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
                    UIManager.MakeScreen<CastleScreen>().SetData(new CastleScreenInData
                    {
                        buildedBuildingKey = buildingData.key
                    }).RunShowScreenProcess();
                }
                else
                {
                    UIManager.ShowPopup<DeclinePopup>();
                }
            }
        }
    }

    public class BuildingPopupInData : BasePopupInData
    {
        public int? buildingId;

        public AdminBRO.Building buildingData =>
            GameData.buildings.GetBuildingById(buildingId);
    }
}