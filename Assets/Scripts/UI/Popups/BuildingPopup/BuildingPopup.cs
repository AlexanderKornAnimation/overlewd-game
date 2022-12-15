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
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BuildingPopup/BuildingPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            walletWidgetPos = canvas.Find("WalletWidgetPos");
            
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
            buildButton = canvas.Find("BuildButtons/BuildButton").GetComponent<Button>();
            buildButton.onClick.AddListener(BuildButtonClick);
            buildButtonText = buildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            crystalBuildButton = canvas.Find("BuildButtons/CrystalBuildButton").GetComponent<Button>();
            crystalBuildButton.onClick.AddListener(CrystalBuildButtonClick);
            crystalBuildButtonText = crystalBuildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            var buildingData = inputData?.buildingData;
            
            if (buildingData != null)
            {
                var picPrefabName = buildingData.key switch
                {
                    AdminBRO.Building.Key_Castle => "Castle",
                    AdminBRO.Building.Key_Catacombs => "Catacombs",
                    AdminBRO.Building.Key_Laboratory => "Laboratory",
                    AdminBRO.Building.Key_Aerostat => "Aerostat",
                    AdminBRO.Building.Key_Forge => "Forge",
                    AdminBRO.Building.Key_Harem => "Harem",
                    AdminBRO.Building.Key_MagicGuild => "MagicGuild",
                    AdminBRO.Building.Key_Market => "Market",
                    AdminBRO.Building.Key_Municipality => "Municipality",
                    AdminBRO.Building.Key_Portal => "Portal",
                    _ => "Castle"
                };
                var building = ResourceManager.InstantiateWidgetPrefab($"Prefabs/UI/Popups/BuildingPopup/{picPrefabName}",
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
                    resource[i].sprite = ResourceManager.LoadSprite(currencyData.iconUrl);
                    count[i].text = prices[i].amount.ToString();
                    count[i].color = buildingData.canUpgrade ? Color.white : Color.red;
                }

                var crystalPriceAmount = nextLevelData?.crystalPrice?.FirstOrDefault()?.amount.ToString() ?? "-";
                var amountColor = buildingData.canUpgradeCrystal ? "white" : "red";
                crystalBuildButtonText.text =
                    $"Summon building\nfor <color={amountColor}>{crystalPriceAmount}</color> crystals";
                   
                description.text = buildingData.description ?? "EMPTY";
                buildingName.text = buildingData.name ?? "EMPTY";

                walletWidget = WalletWidget.GetInstance(walletWidgetPos);
                crystalBuildButton.gameObject.SetActive(nextLevelData.crystalPrice?.Count != 0);
                
                UITools.DisableButton(buildButton, !buildingData.canUpgrade);
            }
            
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_4):
                    if (inputData?.buildingData?.key == AdminBRO.Building.Key_Castle &&
                        !GameData.buildings.castle.meta.isBuilt)
                    {
                        GameData.ftue.chapter1.ShowNotifByKey("quickbuildtutor");
                    }
                    break;
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
                    }).DoShow();
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
                    }).DoShow();
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