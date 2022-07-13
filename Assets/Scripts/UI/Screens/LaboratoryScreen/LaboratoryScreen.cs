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
        private Button backButton;
        
        private Button mergeButton;
        private Image[] mergePriceImage = new Image[2];
        private TextMeshProUGUI[] mergePriceAmount = new TextMeshProUGUI[2];

        private GameObject slotFull;
        private Image slotImage;
        private Button slotButton;
        private Image girlImage;
        private TextMeshProUGUI girlName;

        private Transform currencyBack;
        
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
        
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            
            mergeButton = canvas.Find("MergeButton").Find("Button").GetComponent<Button>();
            mergeButton.onClick.AddListener(MergeButtonClick);

            for (int i = 0; i < inputData?.characterData?.mergePrice?.Count; i++)
            {
                mergePriceImage[i] = mergeButton.transform.Find($"Resource{i + 1}").GetComponent<Image>();
                mergePriceAmount[i] = mergePriceImage[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
            }
            
            slotFull = slot.Find("SlotFull").gameObject;
            girlImage = slotFull.transform.Find("Girl").GetComponent<Image>();
            currencyBack = canvas.Find("CurrencyBack");
            
            foreach (var i in tabIds)
            {
                tabs[i] = tabsArea.Find(tabNames[i]).GetComponent<Button>();
                tabs[i].onClick.AddListener(() => TabClick(i));
        
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
            
            UITools.FillWallet(currencyBack);
            slotFull.SetActive(false);
            mergeButton.gameObject.SetActive(slotFull.activeSelf);
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
            UIManager.ShowOverlay<MarketOverlay>();
        }

        private void MergeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
        
    }

    public class LaboratoryScreenInData : BaseFullScreenInData
    {
        public int? characterId;
        public AdminBRO.Character characterData => GameData.characters.GetById(characterId);
    }
}
