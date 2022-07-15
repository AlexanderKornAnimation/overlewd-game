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

        private Button backButton;
        private Button overlordButton;

        private Transform slot1;
        private Transform slot1_SlotEmptyHint;
        private Transform slot1_SlotFull;
        private Image slot1_characterIcon;
        private Button slot1_Button;
        private TextMeshProUGUI slot1_title;
        private Transform slot1_titleChIcons;
        private TextMeshProUGUI slot1_Level;
        private TextMeshProUGUI slot1_Class;
        private Transform slot1_Potency;
        private TextMeshProUGUI slot1_PotencyValue;

        private Transform slot2;
        private Transform slot2_SlotEmptyHint;
        private Transform slot2_SlotFull;
        private Image slot2_characterIcon;
        private Button slot2_Button;
        private TextMeshProUGUI slot2_title;
        private Transform slot2_titleChIcons;
        private TextMeshProUGUI slot2_Level;
        private TextMeshProUGUI slot2_Class;
        private Transform slot2_Potency;
        private TextMeshProUGUI slot2_PotencyValue;

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

            overlordButton = canvas.Find("OverlordButton").GetComponent<Button>();
            overlordButton.onClick.AddListener(OverlordButtonClick);

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
            slot1_SlotEmptyHint = slot1.Find("SlotEmptyHint");
            slot1_SlotFull = slot1.Find("SlotFull");
            slot1_characterIcon = slot1_SlotFull.Find("CharacterPos").GetComponent<Image>();
            slot1_Button = slot1_SlotFull.Find("SlotButton").GetComponent<Button>();
            slot1_Button.onClick.AddListener(Slot1Click);
            slot1_title = slot1.Find("Headline").Find("Title").GetComponent<TextMeshProUGUI>();
            slot1_titleChIcons = slot1.Find("Headline").Find("CharacterIcons");
            slot1_Level = slot1_titleChIcons.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
            slot1_Class = slot1_titleChIcons.Find("Class").GetComponent<TextMeshProUGUI>();
            slot1_Potency = slot1.Find("PotencyBack");
            slot1_PotencyValue = slot1_Potency.Find("Value").GetComponent<TextMeshProUGUI>();

            slot2 = canvas.Find("Slot2");
            slot2_SlotEmptyHint = slot2.Find("SlotEmptyHint");
            slot2_SlotFull = slot2.Find("SlotFull");
            slot2_characterIcon = slot2_SlotFull.Find("CharacterPos").GetComponent<Image>();
            slot2_Button = slot2_SlotFull.Find("SlotButton").GetComponent<Button>();
            slot2_Button.onClick.AddListener(Slot2Click);
            slot2_title = slot2.Find("Headline").Find("Title").GetComponent<TextMeshProUGUI>();
            slot2_titleChIcons = slot2.Find("Headline").Find("CharacterIcons");
            slot2_Level = slot2_titleChIcons.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
            slot2_Class = slot2_titleChIcons.Find("Class").GetComponent<TextMeshProUGUI>();
            slot2_Potency = slot2.Find("PotencyBack");
            slot2_PotencyValue = slot2_Potency.Find("Value").GetComponent<TextMeshProUGUI>();
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

        private void Slot1Click()
        {
            var chId = GetCharacterInSlot1();
            if (chId.HasValue)
            {
                TryEquipOrUnequip(chId.Value);
            }
        }

        private void Slot2Click()
        {
            var chId = GetCharacterInSlot2();
            if(chId.HasValue)
            {
                TryEquipOrUnequip(chId.Value);
            }
        }

        private void CustomizeSlots()
        {
            var slot1_chId = GetCharacterInSlot1();
            if (slot1_chId.HasValue)
            {
                var chData = GameData.characters.GetById(slot1_chId.Value);
                slot1_SlotEmptyHint.gameObject.SetActive(false);
                slot1_SlotFull.gameObject.SetActive(true);
                slot1_characterIcon.sprite = ResourceManager.LoadSprite(chData.teamEditSlotPersIcon);
                slot1_title.text = chData.name;
                slot1_titleChIcons.gameObject.SetActive(true);
                slot1_Level.text = chData.level.ToString();
                slot1_Class.text = chData.classMarker;
                slot1_Potency.gameObject.SetActive(true);
                slot1_PotencyValue.text = chData.potency.HasValue ? chData.potency.ToString() : "_";;
            }
            else
            {
                slot1_SlotEmptyHint.gameObject.SetActive(true);
                slot1_SlotFull.gameObject.SetActive(false);
                slot1_title.text = "Empty slot";
                slot1_titleChIcons.gameObject.SetActive(false);
                slot1_Potency.gameObject.SetActive(false);
            }

            //
            var slot2_chId = GetCharacterInSlot2();
            if (slot2_chId.HasValue)
            {
                var chData = GameData.characters.GetById(slot2_chId.Value);
                slot2_SlotEmptyHint.gameObject.SetActive(false);
                slot2_SlotFull.gameObject.SetActive(true);
                slot2_characterIcon.sprite = ResourceManager.LoadSprite(chData.teamEditSlotPersIcon);
                slot2_title.text = chData.name;
                slot2_titleChIcons.gameObject.SetActive(true);
                slot2_Level.text = chData.level.ToString();
                slot2_Class.text = chData.classMarker;
                slot2_Potency.gameObject.SetActive(true);
                slot2_PotencyValue.text = chData.potency.HasValue ? chData.potency.ToString() : "_";
            }
            else
            {
                slot2_SlotEmptyHint.gameObject.SetActive(true);
                slot2_SlotFull.gameObject.SetActive(false);
                slot2_title.text = "Empty slot";
                slot2_titleChIcons.gameObject.SetActive(false);
                slot2_Potency.gameObject.SetActive(false);
            }
        }

        private void Customize()
        {
            foreach (var tabId in tabIds)
            {
                foreach (Transform child in scrollContents[tabId])
                {
                    var character = child.GetComponent<NSTeamEditScreen.Character>();
                    character?.Customize();
                }
            }

            CustomizeSlots();
        }

        public override async Task BeforeShowMakeAsync()
        {
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

                var newCh = NSTeamEditScreen.Character.GetInstance(scrollContents[tabId]);
                newCh.screen = this;
                newCh.characterId = ch.id.Value;
                newCh.inputData = inputData;

                var newChAll = NSTeamEditScreen.Character.GetInstance(scrollContents[tabAllUnits]);
                newChAll.screen = this;
                newChAll.characterId = ch.id.Value;
                newChAll.inputData = inputData;
            }

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

            if (inputData?.prevScreenInData != null)
            {
                if (inputData.prevScreenInData.IsType<HaremScreenInData>())
                {
                    UIManager.MakeScreen<HaremScreen>().
                        SetData(inputData.prevScreenInData as HaremScreenInData).
                        RunShowScreenProcess();
                } 
                else if (inputData.prevScreenInData.IsType<MapScreenInData>())
                {
                    UIManager.MakeScreen<MapScreen>().
                        SetData(new MapScreenInData 
                        { 
                            ftueStageId = inputData.ftueStageId 
                        }).RunShowScreenProcess();
                }
                else if (inputData.prevScreenInData.IsType<EventMapScreenInData>())
                {
                    UIManager.MakeScreen<EventMapScreen>().
                        SetData(new EventMapScreenInData
                        {
                            eventStageId = inputData.eventStageId
                        }).RunShowScreenProcess();
                }
            }
            else
            {
                UIManager.ShowScreen<HaremScreen>();
            }
        }

        private void OverlordButtonClick()
        {
            
        }
    }

    public class TeamEditScreenInData : BaseFullScreenInData
    {

    }
}