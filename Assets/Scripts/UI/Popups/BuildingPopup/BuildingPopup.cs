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

        private Button buildButton;
        private TextMeshProUGUI buildButtonText;
        private Button crystalBuildButton;
        private TextMeshProUGUI crystalBuildButtonText;
        private Transform currencyBack;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BuildingPopup/BuildingPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            currencyBack = canvas.Find("CurrencyBack");
            
            background = canvas.Find("Background");
            imageSpawnPoint = background.Find("ImageSpawnPoint");

            buildingName = canvas.Find("BuildingName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            resourcesGrid = canvas.Find("Grid");
            for (var i = 0; i < resource.Length; i++)
            {
                resource[i] = resourcesGrid.Find($"Resource{i + 1}").GetComponent<Image>();
                count[i] = resource[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                resource[i].gameObject.SetActive(false);
            }

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            buildButton = canvas.Find("BuildButton").GetComponent<Button>();
            buildButton.onClick.AddListener(BuildButtonClick);
            buildButtonText = buildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            crystalBuildButton = canvas.Find("CrystalBuildButton").GetComponent<Button>();
            crystalBuildButton.onClick.AddListener(CrystalBuildButtonClick);
            crystalBuildButtonText = crystalBuildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            var buildingData = inputData?.buildingData;
            if (buildingData != null)
            {
                var building =
                    ResourceManager.InstantiateWidgetPrefab($"Prefabs/UI/Popups/BuildingPopup/{buildingData.name}",
                        imageSpawnPoint);

                for (int i = 1; i <= buildingData.levels.Count; i++)
                {
                    if (buildingData.nextLevel.HasValue)
                    {
                        building.transform.Find($"Level{i}").gameObject.
                            SetActive(buildingData.nextLevel + 1 == i);
                    }
                }
                
                var nextLevelData = buildingData.nextLevelData;
                var prices = nextLevelData.price;
                
                for (var i = 0; i < prices.Count; i++)
                {
                    var currencyData = GameData.currencies.GetById(prices[i].currencyId);
                    resource[i].gameObject.SetActive(true);
                    resource[i].sprite = ResourceManager.LoadSprite(currencyData.icon256Url);
                    count[i].text = prices[i].amount.ToString();
                    count[i].color = buildingData.canUpgrade ? Color.white : Color.red;
                }

                var crystalPriceAmount = nextLevelData?.crystalPrice?.FirstOrDefault()?.amount.ToString() ?? "-";
                crystalBuildButtonText.text = buildingData.canUpgradeCrystal
                    ? $"Summon building\nfor <color=white>{crystalPriceAmount}</color> crystals"
                    : $"Summon building\nfor <color=red>{crystalPriceAmount}</color> crystals";

                description.text = buildingData.description ?? "EMPTY";
                buildingName.text = buildingData.name ?? "EMPTY";
                
                UITools.FillWallet(currencyBack);
            }

            await Task.CompletedTask;
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual async void BuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_FreeBuildButton);
            await Build();
        }

        protected virtual async void CrystalBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await BuildCrystals();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }

        private async Task Build()
        {
            var buildingData = inputData?.buildingData;
            if (buildingData != null)
            {
                if (buildingData.canUpgrade)
                {
                    await GameData.buildings.Build(buildingData.id);
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

        private async Task BuildCrystals()
        {
            var buildingData = inputData?.buildingData;
            if (buildingData != null)
            {
                if (buildingData.canUpgradeCrystal)
                {
                    await GameData.buildings.BuildCrystals(buildingData.id);
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