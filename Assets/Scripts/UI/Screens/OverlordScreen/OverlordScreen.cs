using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class OverlordScreen : BaseFullScreenParent<OverlordScreenInData>
    {
        private const int TabWeapon = 0;
        private const int TabGloves = 1;
        private const int TabHelmet = 2;
        private const int TabHarness = 3;
        private const int TabTigh = 4;
        private const int TabBoots = 5;
        private const int TabsCount = 6;

        private int activeTabId;

        private Button[] tabs = new Button[TabsCount];
        private GameObject[] selectedTabs = new GameObject[TabsCount];
        private string[] tabNames = {"TabWeapon", "TabGloves", "TabHelmet", "TabHarness", "TabTigh", "TabBoots"};
        private int[] tabIds = {TabWeapon, TabGloves, TabHelmet, TabHarness, TabTigh, TabBoots};
        private GameObject[] scrolls = new GameObject[TabsCount];
        private Transform[] scrollContents = new Transform[TabsCount];

        private TextMeshProUGUI speed;
        private TextMeshProUGUI power;
        private TextMeshProUGUI constitution;
        private TextMeshProUGUI agility;

        private TextMeshProUGUI accuracy;
        private TextMeshProUGUI critChance;
        private TextMeshProUGUI dodgeChance;
        private TextMeshProUGUI damage;

        private TextMeshProUGUI potency;
        private TextMeshProUGUI health;
        private TextMeshProUGUI mana;

        private Button collectiblesButton;
        private Button forgeButton;
        private Button magicGuildButton;
        private Button backButton;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/OverlordScreen/OverlordScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var statsInfo = canvas.Find("StatsInfo");
            var mainStats = statsInfo.Find("MainStats");
            var secondaryStats = statsInfo.Find("SecondaryStats");
            var equipmentBack = canvas.Find("EquipmentBack");
            var tabArea = canvas.Find("TabArea");
            var selectedTabArea = canvas.Find("SelectedTabArea");
            var equipmentCells = canvas.Find("EquipmentCells");

            speed = mainStats.Find("Speed").Find("Stat").GetComponent<TextMeshProUGUI>();
            power = mainStats.Find("Power").Find("Stat").GetComponent<TextMeshProUGUI>();
            constitution = mainStats.Find("Constitution").Find("Stat").GetComponent<TextMeshProUGUI>();
            agility = mainStats.Find("Agility").Find("Stat").GetComponent<TextMeshProUGUI>();

            accuracy = secondaryStats.Find("Accuracy").Find("Stat").GetComponent<TextMeshProUGUI>();
            critChance = secondaryStats.Find("CritChance").Find("Stat").GetComponent<TextMeshProUGUI>();
            dodgeChance = secondaryStats.Find("DodgeChance").Find("Stat").GetComponent<TextMeshProUGUI>();
            damage = secondaryStats.Find("DamageDealt").Find("Stat").GetComponent<TextMeshProUGUI>();

            potency = statsInfo.Find("PotencyBack").Find("Potency").GetComponent<TextMeshProUGUI>();
            health = statsInfo.Find("Health").Find("Stat").GetComponent<TextMeshProUGUI>();
            mana = statsInfo.Find("Mana").Find("Stat").GetComponent<TextMeshProUGUI>();

            collectiblesButton = canvas.Find("CollectiblesButton").GetComponent<Button>();
            collectiblesButton.onClick.AddListener(CollectiblesButtonClick);

            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            forgeButton.onClick.AddListener(ForgeButtonClick);

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            foreach (var id in tabIds)
            {
                tabs[id] = tabArea.Find(tabNames[id]).GetComponent<Button>();
                tabs[id].onClick.AddListener(() => TabClick(id));
                selectedTabs[id] = selectedTabArea.Find(tabNames[id]).gameObject;
                selectedTabs[id].SetActive(false);
                scrolls[id] = equipmentBack.Find($"Scroll{tabNames[id]}").gameObject;
                scrollContents[id] = scrolls[id].transform.Find("Viewport").Find("Content");
            }

            TabClick(activeTabId);
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        private void Customize()
        {
            UITools.DisableButton(forgeButton);
            UITools.DisableButton(collectiblesButton);
        }

        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(tabId);
        }

        private void EnterTab(int tabId)
        {
            activeTabId = tabId;
            tabs[tabId].gameObject.SetActive(false);
            selectedTabs[tabId].SetActive(true);
            scrolls[tabId].SetActive(true);
        }

        private void LeaveTab(int tabId)
        {
            tabs[tabId].gameObject.SetActive(true);
            selectedTabs[tabId].SetActive(false);
            scrolls[tabId].SetActive(false);
        }


        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (inputData.prevScreenInData == null)
            {
                UIManager.ShowScreen<CastleScreen>();
            }
            else if (inputData.prevScreenInData.IsType<TeamEditScreenInData>())
            {
                UIManager.MakeScreen<TeamEditScreen>()
                    .SetData(inputData.prevScreenInData.As<TeamEditScreenInData>())
                    .RunShowScreenProcess();
            }
            else
            {
                UIManager.ShowScreen<CastleScreen>();
            }
        }

        private void ForgeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        private void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        private void CollectiblesButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }
    }

    public class OverlordScreenInData : BaseFullScreenInData
    {
    }
}