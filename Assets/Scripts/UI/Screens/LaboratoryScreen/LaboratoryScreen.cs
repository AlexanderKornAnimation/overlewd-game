using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    public class LaboratoryScreen : BaseFullScreenParent<LaboratoryScreenInData>
    {
        private const int tabAllUnits = 0;
        private const int tabAssassins = 1;
        private const int tabCasters = 2;
        private const int tabHealers = 3;
        private const int tabBruisers = 4;
        private const int tabTanks = 5;
        private const int tabsCount = 6;

        private int activeTabId;

        private string[] tabNames = { "AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks" };

        private int[] tabIds = { tabAllUnits, tabAssassins, tabCasters, tabHealers, tabBruisers, tabTanks };
        private Button[] tabs = new Button[tabsCount];
        private GameObject[] pressedTabs = new GameObject[tabsCount];
        private GameObject[] scrollViews = new GameObject[tabsCount];
        private Transform[] scrollContents = new Transform[tabsCount];

        private Button marketButton;
        private Button portalButton;
        private Button mergeButton;
        private Button backButton;

        private GameObject hint;

        private GameObject slotFull;
        private Button slotButton;
        private GameObject girlInfo;
        private Image girlRarity;
        private Image girlImage;
        private TextMeshProUGUI girlName;
        private TextMeshProUGUI girlLevel;
        
        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/LaboratoryScreen/LaboratoryScreen",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");
            var charactersBack = canvas.Find("CharactersBack");
            var slot = canvas.Find("Slot");
            var headline = slot.Find("Headline");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);

            mergeButton = canvas.Find("MergeButton").GetComponent<Button>();
            mergeButton.onClick.AddListener(MergeButtonClick);

            hint = headline.Find("Hint").gameObject;

            slotFull = slot.Find("SlotFull").gameObject;
            girlInfo = slotFull.transform.Find("GirlInfo").gameObject;
            girlImage = slotFull.transform.Find("Girl").GetComponent<Image>();
            girlRarity = slotFull.transform.Find("GirlRarity").GetComponent<Image>();
            girlName = girlInfo.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            girlLevel = girlInfo.transform.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
            
            foreach (var i in tabIds)
            {
                tabs[i] = tabsArea.Find(tabNames[i]).GetComponent<Button>();
                tabs[i].onClick.AddListener(() => { TabClick(i); });

                pressedTabs[i] = pressedTabsArea.Find(tabNames[i]).GetComponent<Image>().gameObject;

                scrollViews[i] = charactersBack.Find("ScrollView_" + tabNames[i]).gameObject;
                scrollContents[i] = scrollViews[i].transform.Find("Viewport").Find("Content");
            }
        }

        private void Customize()
        {
            foreach (var i in tabIds)
            {
                pressedTabs[i].gameObject.SetActive(false);
                scrollViews[i].gameObject.SetActive(false);
            }
            
            var orderedCharactersById = GameData.characters.orderById;
            foreach (var ch in orderedCharactersById)
            {
                if (ch.characterClass == AdminBRO.Character.Class_Overlord)
                    continue;

                var tabId = ch.characterClass switch
                {
                    AdminBRO.Character.Class_Assassin => tabAssassins,
                    AdminBRO.Character.Class_Bruiser => tabBruisers,
                    AdminBRO.Character.Class_Caster => tabCasters,
                    AdminBRO.Character.Class_Healer => tabHealers,
                    AdminBRO.Character.Class_Tank => tabTanks,
                    _ => tabAllUnits
                };

                var newCh = NSLaboratoryScreen.Character.GetInstance(scrollContents[tabId]);
                newCh.characterId = ch.id.Value;

                var newChAll = NSLaboratoryScreen.Character.GetInstance(scrollContents[tabAllUnits]);
                newChAll.characterId = ch.id.Value;
            }
            
            slotFull.SetActive(false);
        }
        
        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            
            EnterTab(activeTabId);
            
            await Task.CompletedTask;
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
            pressedTabs[tabId].SetActive(true); 
            scrollViews[tabId].SetActive(true);
        }

        private void LeaveTab(int tabId)
        {
            tabs[tabId].gameObject.SetActive(true);
            pressedTabs[tabId].SetActive(false);
            scrollViews[tabId].SetActive(false);
        }

        private void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }

        private void MarketButtonClick()
        {
            UIManager.ShowPopup<MarketPopup>();
        }

        private void MergeButtonClick()
        {
            
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
        
    }

    public class LaboratoryScreenInData : BaseFullScreenInData
    {
        
    }
}
