using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class TeamEditScreen : BaseFullScreen
    {
        private const int tabAllUnits = 0;
        private const int tabAssassins = 1;
        private const int tabCasters = 2;
        private const int tabHealers = 3;
        private const int tabBruisers = 4;
        private const int tabTanks = 5;
        private const int tabsCount = 6;

        private int activeTabId;

        private Button backButton;

        private string[] tabNames = { "AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks" };

        private int[] tabIds = { tabAllUnits, tabAssassins, tabCasters, tabHealers, tabBruisers, tabTanks };
        private Button[] tabs = new Button[tabsCount];
        private GameObject[] pressedTabs = new GameObject[tabsCount];
        private GameObject[] scrollViews = new GameObject[tabsCount];
        private Transform[] scrollContents = new Transform[tabsCount];

        private Transform slot1;
        private NSTeamEditScreen.SlotOneDrop slot1_drop;
        private Transform slot2;
        private NSTeamEditScreen.SlotTwoDrop slot2_drop;

        private int? eventMapStageId;
        private AdminBRO.FTUEStageItem mapStageData;

        private Transform canvas;

        public NSTeamEditScreen.CharacterDrag chDragObj { get; private set; }

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/TeamEditScreen/TeamEditScreen", transform);

            canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");
            var charactersBack = canvas.Find("CharactersBack");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            foreach (var i in tabIds)
            {
                tabs[i] = tabsArea.Find(tabNames[i]).GetComponent<Button>();
                tabs[i].onClick.AddListener(() =>
                {
                    TabClick(i);
                });

                pressedTabs[i] = pressedTabsArea.Find(tabNames[i]).GetComponent<Image>().gameObject;

                scrollViews[i] = charactersBack.Find("ScrollView_" + tabNames[i]).gameObject;
                scrollContents[i] = scrollViews[i].transform.Find("Viewport").Find("Content");
            }

            slot1 = canvas.Find("Slot1");
            slot1_drop = slot1.Find("DropArea").GetComponent<NSTeamEditScreen.SlotOneDrop>();
            slot1_drop.screen = this;
            slot2 = canvas.Find("Slot2");
            slot2_drop = slot2.Find("DropArea").GetComponent<NSTeamEditScreen.SlotTwoDrop>();
            slot2_drop.screen = this;
        }

        public TeamEditScreen SetDataFromMapScreen(AdminBRO.FTUEStageItem stageData)
        {
            mapStageData = stageData;
            return this;
        }

        public TeamEditScreen SetDataFromEventMapScreen(int stageId)
        {
            eventMapStageId = stageId;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            foreach (var tabId in tabIds)
            {
                for (int i = 0; i < 3; i++)
                {
                    var ch = NSTeamEditScreen.Character.GetInstance(scrollContents[tabId]);
                    ch.dragDetector.scrollRect = scrollViews[tabId].GetComponent<ScrollRect>();
                    ch.dragDetector.screen = this;
                }
            }


            foreach (var i in tabIds)
            {
                pressedTabs[i].gameObject.SetActive(false);
                scrollViews[i].gameObject.SetActive(false);
            }
            EnterTab(activeTabId);

            await Task.CompletedTask;
        }

        public void SlotOneDrop()
        {
            if (chDragObj != null)
            {
                Debug.Log("slot 1 drop");
            }
        }

        public void SlotTwoDrop()
        {
            if (chDragObj != null)
            {
                Debug.Log("slot 2 drop");
            }
        }

        public NSTeamEditScreen.CharacterDrag MakeDragCharacterObject()
        {
            if (chDragObj == null)
            {
                chDragObj = NSTeamEditScreen.CharacterDrag.GetInstance(canvas);
                chDragObj.screen = this;
            }
            return chDragObj;
        }

        public void DestroyDragCharacterObject()
        {
            Destroy(chDragObj?.gameObject);
            chDragObj = null;
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

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (mapStageData != null)
            {
                UIManager.MakeScreen<MapScreen>().
                    SetDataFromTeamEdit(mapStageData).RunShowScreenProcess();
            }
            else if (eventMapStageId.HasValue)
            {
                UIManager.MakeScreen<EventMapScreen>().
                    SetDataFromTeamEdit(eventMapStageId.Value).RunShowScreenProcess();
            }
        }
    }
}