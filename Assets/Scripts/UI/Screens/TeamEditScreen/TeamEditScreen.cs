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

        private Transform slotOverlord;
        private Image slotOverlord_characterIcon;
        private TextMeshProUGUI slotOverlord_potencyValue;
        private TextMeshProUGUI slotOverlord_name;
        private TextMeshProUGUI slotOverlord_level;
        
        private Transform slot1;
        private Transform slot1_SlotEmptyHint;
        private Transform slot1_SlotFull;
        private Image slot1_characterIcon;
        private Button slot1_Button;
        private TextMeshProUGUI slot1_name;
        private TextMeshProUGUI slot1_Level;
        private Transform slot1_Potency;
        private TextMeshProUGUI slot1_PotencyValue;

        private Transform slot2;
        private Transform slot2_SlotEmptyHint;
        private Transform slot2_SlotFull;
        private Image slot2_characterIcon;
        private Button slot2_Button;
        private TextMeshProUGUI slot2_name;
        private TextMeshProUGUI slot2_Level;
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

            slotOverlord = canvas.Find("SlotOverlord");
            var slotOverlordHeadline = slotOverlord.Find("Headline");
            slotOverlord_characterIcon = slotOverlord.Find("CharacterPos").GetComponent<Image>();
            slotOverlord_potencyValue = slotOverlordHeadline.Find("PotencyBack").Find("Value").GetComponent<TextMeshProUGUI>();
            slotOverlord_level = slotOverlordHeadline.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
            slotOverlord_name = slotOverlordHeadline.Find("Name").GetComponent<TextMeshProUGUI>();
            
            slot1 = canvas.Find("Slot1");
            slot1_SlotEmptyHint = slot1.Find("SlotEmptyHint");
            slot1_SlotFull = slot1.Find("SlotFull");
            slot1_characterIcon = slot1_SlotFull.Find("CharacterPos").GetComponent<Image>();
            slot1_Button = slot1_SlotFull.Find("SlotButton").GetComponent<Button>();
            slot1_Button.onClick.AddListener(Slot1Click);
            slot1_name = slot1_SlotFull.Find("Headline").Find("Name").GetComponent<TextMeshProUGUI>();
            slot1_Level = slot1_SlotFull.Find("Headline").Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
            slot1_Potency = slot1_SlotFull.Find("Headline").Find("PotencyBack");
            slot1_PotencyValue = slot1_Potency.Find("Value").GetComponent<TextMeshProUGUI>();

            slot2 = canvas.Find("Slot2");
            slot2_SlotEmptyHint = slot2.Find("SlotEmptyHint");
            slot2_SlotFull = slot2.Find("SlotFull");
            slot2_characterIcon = slot2_SlotFull.Find("CharacterPos").GetComponent<Image>();
            slot2_Button = slot2_SlotFull.Find("SlotButton").GetComponent<Button>();
            slot2_Button.onClick.AddListener(Slot2Click);
            slot2_name = slot2_SlotFull.Find("Headline").Find("Name").GetComponent<TextMeshProUGUI>();
            slot2_Level = slot2_SlotFull.Find("Headline").Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
            slot2_Potency = slot2_SlotFull.Find("Headline").Find("PotencyBack");
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
                slot1_name.text = chData.name;
                slot1_Level.text = chData.level.ToString();
                slot1_PotencyValue.text = chData.potency.HasValue ? chData.potency.ToString() : "_";;
            }
            else
            {
                slot1_SlotEmptyHint.gameObject.SetActive(true);
                slot1_SlotFull.gameObject.SetActive(false);
            }

            //
            var slot2_chId = GetCharacterInSlot2();
            if (slot2_chId.HasValue)
            {
                var chData = GameData.characters.GetById(slot2_chId.Value);
                slot2_SlotEmptyHint.gameObject.SetActive(false);
                slot2_SlotFull.gameObject.SetActive(true);
                slot2_characterIcon.sprite = ResourceManager.LoadSprite(chData.teamEditSlotPersIcon);
                slot2_name.text = chData.name;
                slot2_Level.text = chData.level.ToString();
                slot2_PotencyValue.text = chData.potency.HasValue ? chData.potency.ToString() : "_";
            }
            else
            {
                slot2_SlotEmptyHint.gameObject.SetActive(true);
                slot2_SlotFull.gameObject.SetActive(false);
            }
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
            slotOverlord_characterIcon.sprite = ResourceManager.LoadSprite(overlordData.teamEditSlotPersIcon);
            slotOverlord_level.text = overlordData.level.ToString();
            slotOverlord_name.text = overlordData.name;
            slotOverlord_potencyValue.text = overlordData.potency.ToString();
            teamPotency.text = GameData.characters.myTeamPotency.ToString();
            
            var orderedCharacters = GameData.characters.orderByLevel;
            foreach (var ch in orderedCharacters)
            {
                if (ch.characterClass == AdminBRO.Character.Class_Overlord)
                    continue;

                var tabId = ch.characterClass switch
                {
                    AdminBRO.Character.Class_Assassin => TabAssassins,
                    AdminBRO.Character.Class_Bruiser => TabBruisers,
                    AdminBRO.Character.Class_Caster => TabCasters,
                    AdminBRO.Character.Class_Healer => TabHealers,
                    AdminBRO.Character.Class_Tank => TabTanks,
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
            UIManager.MakeScreen<OverlordScreen>().
                SetData(new OverlordScreenInData
            {
                prevScreenInData = inputData,
            }).RunShowScreenProcess();
        }
    }

    public class TeamEditScreenInData : BaseFullScreenInData
    {

    }
}