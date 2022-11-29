using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    public class TeamEditScreen : BaseFullScreenParent<TeamEditScreenInData>
    {
        private const int TabAllUnits = 0;
        private const int TabAssassins = 1;
        private const int TabCasters = 2;
        private const int TabHealers = 3;
        private const int TabBruisers = 4;
        private const int TabTanks = 5;
        private const int TabsCount = 6;

        private int activeTabId;

        private string[] tabNames = { "AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks" };

        private int[] tabIds = { TabAllUnits, TabAssassins, TabCasters, TabHealers, TabBruisers, TabTanks };
        private Button[] tabs = new Button[TabsCount];
        private GameObject[] pressedTabs = new GameObject[TabsCount];
        private GameObject[] scrollViews = new GameObject[TabsCount];
        private Transform[] scrollContents = new Transform[TabsCount];

        private Button backButton;
        private Button overlordButton;
        private TextMeshProUGUI teamPotency;

        private TextMeshProUGUI overlordPotency;
        
        private NSTeamEditScreen.Slot slot1;
        private NSTeamEditScreen.Slot slot2;
       
        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/TeamEditScreen/TeamEditScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");
            var charactersBack = canvas.Find("CharactersBack");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            teamPotency = canvas.Find("TeamPotencyBack").Find("Potency").GetComponent<TextMeshProUGUI>();
            
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

            overlordPotency = canvas.Find("Overlord").Find("PotencyInfo").Find("PotencyBack").Find("Potency").GetComponent<TextMeshProUGUI>();
            overlordButton = canvas.Find("Overlord/PotencyInfo/OverlordButton").GetComponent<Button>();
            overlordButton.onClick.AddListener(OverlordButtonClick);

            slot1 = canvas.Find("Slot1").gameObject.AddComponent<NSTeamEditScreen.Slot>();
            slot1.chTeamPos = AdminBRO.Character.TeamPosition_Slot1;
            slot1.OnSlotClick += SlotClick;

            slot2 = canvas.Find("Slot2").gameObject.AddComponent<NSTeamEditScreen.Slot>();
            slot2.chTeamPos = AdminBRO.Character.TeamPosition_Slot2;
            slot2.OnSlotClick += SlotClick;
        }
        private int? GetCharacterInSlot1() =>
            GameData.characters.slot1Ch?.id;

        private int? GetCharacterInSlot2() =>
            GameData.characters.slot2Ch?.id;

        public async void TryEquipOrUnequip(int chId)
        {
            var chData = GameData.characters.GetById(chId);
            if (chData.teamPosition != AdminBRO.Character.TeamPosition_None)
            {
                await GameData.characters.ToSlotNone(chId);
                Customize();
            }
            else
            {
                if (!GetCharacterInSlot1().HasValue)
                {
                    await GameData.characters.ToSlot1(chId);
                    Customize();
                }
                else if (!GetCharacterInSlot2().HasValue)
                {
                    await GameData.characters.ToSlot2(chId);
                    Customize();
                }
            }
        }

        private void SlotClick(int? chId)
        {
            if (chId.HasValue)
            {
                TryEquipOrUnequip(chId.Value);
            }
        }
        
        private void CustomizeSlots()
        {
            slot1?.Customize();
            slot2?.Customize();
        }

        private void SortCharactersInTabs()
        {
            foreach (var tabId in tabIds)
            {
                var tabChars = scrollContents[tabId].GetComponentsInChildren<NSTeamEditScreen.Character>().ToList();
                var tabCharsSort = tabChars.OrderByDescending(ch => ch.characterData.level + (ch.characterData.inTeam ? 100 : 0));
                var chSiblingIndex = 0;
                foreach (var character in tabCharsSort)
                {
                    character?.transform.SetSiblingIndex(chSiblingIndex);
                    chSiblingIndex++;
                }
            }
        }

        private void Customize()
        {
            foreach (var tabId in tabIds)
            {
                foreach (var character in scrollContents[tabId].GetComponentsInChildren<NSTeamEditScreen.Character>())
                {
                    character?.Customize();
                }
            }
            
            teamPotency.text = GameData.characters.myTeamPotency.ToString();
            CustomizeSlots();
            SortCharactersInTabs();
        }

        public override async Task BeforeShowMakeAsync()
        {
            var overlordData = GameData.characters.overlord;
            overlordPotency.text = overlordData.potency.ToString();
            teamPotency.text = GameData.characters.myTeamPotency.ToString();
            
            var orderedCharacters = GameData.characters.orderByLevel;
            foreach (var ch in orderedCharacters)
            {
                if (ch.characterClass == AdminBRO.CharacterClass.Overlord)
                    continue;

                var tabId = ch.characterClass switch
                {
                    AdminBRO.CharacterClass.Assassin => TabAssassins,
                    AdminBRO.CharacterClass.Bruiser => TabBruisers,
                    AdminBRO.CharacterClass.Caster => TabCasters,
                    AdminBRO.CharacterClass.Healer => TabHealers,
                    AdminBRO.CharacterClass.Tank => TabTanks,
                    _ => TabAllUnits
                };

                var newCh = NSTeamEditScreen.Character.GetInstance(scrollContents[tabId]);
                newCh.screen = this;
                newCh.characterId = ch.id.Value;
                newCh.inputData = inputData;

                var newChAll = NSTeamEditScreen.Character.GetInstance(scrollContents[TabAllUnits]);
                newChAll.screen = this;
                newChAll.characterId = ch.id.Value;
                newChAll.inputData = inputData;
            }

            SortCharactersInTabs();
            CustomizeSlots();

            //
            foreach (var i in tabIds)
            {
                pressedTabs[i].gameObject.SetActive(false);
                scrollViews[i].gameObject.SetActive(false);
            }
            EnterTab(activeTabId);

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                    GameData.ftue.chapter2.ShowNotifByKey("ch2teamtutor1");
                    await UIManager.WaitHideNotifications();
                    break;
            }
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

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            var prevState = UIManager.currentState.prevState;
            if (prevState.ScreenTypeIs<BattleScreen>() || prevState.ScreenTypeIs<BossFightScreen>())
            {
                UIManager.ToPrevState(prevState.prevScreenState);
            }
            else
            {
                UIManager.ToPrevScreen();
            }
        }

        private void OverlordButtonClick()
        {
            UIManager.ShowScreen<OverlordScreen>();
        }
    }

    public class TeamEditScreenInData : BaseFullScreenInData
    {

    }
}