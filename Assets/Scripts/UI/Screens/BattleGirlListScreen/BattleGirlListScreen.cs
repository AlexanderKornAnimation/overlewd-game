using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BattleGirlListScreen : BaseFullScreenParent<BattleGirlListScreenInData>
    {
        private const int TabAllUnits = 0;
        private const int TabAssassins = 1;
        private const int TabCasters = 2;
        private const int TabHealers = 3;
        private const int TabBruisers = 4;
        private const int TabTanks = 5;
        private const int TabsCount = 6;

        private int activeTabId = TabAllUnits;

        private string[] tabNames = { "AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks" };

        private int[] tabIds = { TabAllUnits, TabAssassins, TabCasters, TabHealers, TabBruisers, TabTanks };
        private Button[] tabs = new Button[TabsCount];
        private GameObject[] pressedTabs = new GameObject[TabsCount];
        private GameObject[] scrollViews = new GameObject[TabsCount];
        private Transform[] scrollContents = new Transform[TabsCount];

        private Button backButton;
        
        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/BattleGirlListScreen/BattleGirlListScreen",
                    transform);
            var canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            foreach (var i in tabIds)
            {
                tabs[i] = tabsArea.Find(tabNames[i]).GetComponent<Button>();
                tabs[i].onClick.AddListener(() => TabClick(i));
        
                pressedTabs[i] = pressedTabsArea.Find(tabNames[i]).GetComponent<Image>().gameObject;
        
                scrollViews[i] = canvas.Find("ScrollView_" + tabNames[i]).gameObject;
                scrollContents[i] = scrollViews[i].transform.Find("Viewport").Find("Content");
                scrollViews[i].gameObject.SetActive(false);
                pressedTabs[i].gameObject.SetActive(false);
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            TabClick(activeTabId);
            await Task.CompletedTask;
        }

        private void Customize()
        {
            var characters = SortCharacters(GameData.characters.orderByLevel);
            foreach (var chData in characters)
            {
                var tabId = chData.characterClass switch
                {
                    AdminBRO.Character.Class_Assassin => TabAssassins,
                    AdminBRO.Character.Class_Bruiser => TabBruisers,
                    AdminBRO.Character.Class_Caster => TabCasters,
                    AdminBRO.Character.Class_Healer => TabHealers,
                    AdminBRO.Character.Class_Tank => TabTanks,
                    _ => TabAllUnits
                };

                var newCh = NSBattleGirlListScreen.Character.GetInstance(scrollContents[tabId]);
                newCh.characterId = chData.id;
                newCh.inputData = inputData;

                var newChAll = NSBattleGirlListScreen.Character.GetInstance(scrollContents[TabAllUnits]);
                newChAll.characterId = chData.id;
                newChAll.inputData = inputData;
            }
            
            SortCharactersInTabs();
        }
        
        private List<AdminBRO.Character> SortCharacters(List<AdminBRO.Character> characters)
        {
            return characters.Where(ch => ch.characterClass != AdminBRO.Character.Class_Overlord).ToList();
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
            pressedTabs[tabId].SetActive(true); 
            scrollViews[tabId].SetActive(true);
        }

        private void LeaveTab(int tabId)
        {
            tabs[tabId].gameObject.SetActive(true);
            pressedTabs[tabId].SetActive(false);
            scrollViews[tabId].SetActive(false);
        }
        
        private void BackButtonClick()
        {
            if (inputData != null)
            {
                UIManager.MakeScreen<HaremScreen>().
                    SetData(inputData.As<HaremScreenInData>()).
                    RunShowScreenProcess();
            }
            else
            {
                UIManager.ShowScreen<HaremScreen>();
            }
        }
        
        private void SortCharactersInTabs()
        {
            foreach (var tabId in tabIds)
            {
                var tabChars = scrollContents[tabId].GetComponentsInChildren<NSBattleGirlListScreen.Character>().ToList();
                var tabCharsSort = tabChars.OrderByDescending(ch => ch.characterData.level + (ch.characterData.inTeam ? 100 : 0));
                var chSiblingIndex = 0;
                foreach (var character in tabCharsSort)
                {
                    character?.transform.SetSiblingIndex(chSiblingIndex);
                    chSiblingIndex++;
                }
            }
        }
    }
    
    public class BattleGirlListScreenInData : BaseFullScreenInData
    {
        
    }
}
