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
    public class OverlordScreen : BaseFullScreenParent<OverlordScreenInData>
    {
        private const int TabWeapon = 0;
        private const int TabGloves = 1;
        private const int TabHelmet = 2;
        private const int TabHarness = 3;
        private const int TabThigh = 4;
        private const int TabBoots = 5;
        private const int TabsCount = 6;

        private int activeTabId = TabWeapon;

        private Button[] tabs = new Button[TabsCount];
        private GameObject[] selectedTabs = new GameObject[TabsCount];
        private string[] tabNames = {"TabWeapon", "TabGloves", "TabHelmet", "TabHarness", "TabTigh", "TabBoots"};
        private int[] tabIds = {TabWeapon, TabGloves, TabHelmet, TabHarness, TabThigh, TabBoots};
        private GameObject[] scrolls = new GameObject[TabsCount];
        private Transform[] scrollContents = new Transform[TabsCount];
        private Transform canvas;
        
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
        private Button backButton;

        private Button cellWeapon;
        private Button cellGloves;
        private Button cellHelmet;
        private Button cellHarness;
        private Button cellThigh;
        private Button cellBoots;

        private Transform infoPopupPos;
        private NSOverlordScreen.EquipInfoPopup equipPopup;

        private Button basicAttack;
        private NSOverlordScreen.Skill passiveSkill1;
        private NSOverlordScreen.Skill passiveSkill2;
        private NSOverlordScreen.Skill enhancedAttack;
        private NSOverlordScreen.Skill ultimate;

        public AdminBRO.Character overlordData => GameData.characters.overlord;
        private List<NSOverlordScreen.Equipment> equippedItems = new List<NSOverlordScreen.Equipment>();
        private List<NSOverlordScreen.Equipment> equipments = new List<NSOverlordScreen.Equipment>();

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/OverlordScreen/OverlordScreen", transform);

            canvas = screenInst.transform.Find("Canvas");
            var statsInfo = canvas.Find("StatsInfo");
            var mainStats = statsInfo.Find("MainStats");
            var secondaryStats = statsInfo.Find("SecondaryStats");
            var equipmentBack = canvas.Find("EquipmentBack");
            var tabArea = canvas.Find("TabArea");
            var selectedTabArea = canvas.Find("SelectedTabArea");

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

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            infoPopupPos = canvas.Find("InfoPopupPos");

            var equipmentCells = canvas.Find("EquipmentCells");
            cellWeapon = equipmentCells.Find("CellWeapon").GetComponent<Button>();
            cellGloves = equipmentCells.Find("CellGloves").GetComponent<Button>();
            cellHelmet = equipmentCells.Find("CellHelmet").GetComponent<Button>();
            cellHarness = equipmentCells.Find("CellHarness").GetComponent<Button>();
            cellThigh = equipmentCells.Find("CellTigh").GetComponent<Button>();
            cellBoots = equipmentCells.Find("CellBoots").GetComponent<Button>();

            foreach (var id in tabIds)
            {
                tabs[id] = tabArea.Find(tabNames[id]).GetComponent<Button>();
                tabs[id].onClick.AddListener(() => TabClick(id));
                selectedTabs[id] = selectedTabArea.Find(tabNames[id]).gameObject;
                selectedTabs[id].SetActive(false);
                scrolls[id] = equipmentBack.Find($"Scroll{tabNames[id]}").gameObject;
                scrollContents[id] = scrolls[id].transform.Find("Viewport").Find("Content");
                scrolls[id].gameObject.SetActive(false);
            }

            EnterTab(activeTabId);
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {

            await Task.CompletedTask;
        }

        public override async void OnUIEvent(UIEvent eventData)
        {
            switch (eventData.id)
            {
                case UIEventId.ChangeScreenComplete:
                    switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
                    {
                        case (FTUE.CHAPTER_3, FTUE.SEX_1):
                            if (UIManager.currentState.prevState.ScreenTypeIs<SummoningScreen>())
                            {
                                GameData.ftue.chapter3.ShowNotifByKey("ch3overequiptutor4");
                            }
                            else
                            {
                                GameData.ftue.chapter3.ShowNotifByKey("ch3overequiptutor3");
                                await UIManager.WaitHideNotifications();
                                UIManager.MakeScreen<PortalScreen>().
                                    SetData(new PortalScreenInData
                                    {
                                        activeButtonId = PortalScreen.TabOverlordEquip
                                    }).DoShow();
                            }
                            return;
                    }
                    break;
            }
        }

        private void Customize()
        {
            UITools.DisableButton(collectiblesButton);

            CustomizeStats();
            
            foreach (var equipment in GameData.equipment.equipment)
            {
                var content = GetContentByType(equipment.equipmentType);
                if (content != null)
                {
                    var equip = NSOverlordScreen.Equipment.GetInstance(content);
                    equip.equipId = equipment.id;
                    equip.OnClick += ShowInfoPopup;
                    equipments.Add(equip);
                    
                    if (equipment.isEquipped)
                    {
                        equip.transform.SetAsFirstSibling();
                        equippedItems.Add(equip);
                        CustomizeCell(equip);
                    }
                }
            }

            var skills = canvas.Find("Skills");
            passiveSkill1 = skills.Find("PassiveSkill1").gameObject.AddComponent<NSOverlordScreen.Skill>();
            passiveSkill1.skillType = AdminBRO.MagicGuildSkill.Type_PassiveSkill1;
            passiveSkill1.infoWidgetPos = infoPopupPos;
            
            passiveSkill2 = skills.Find("PassiveSkill2").gameObject.AddComponent<NSOverlordScreen.Skill>();
            passiveSkill2.skillType = AdminBRO.MagicGuildSkill.Type_PassiveSkill2;
            passiveSkill2.infoWidgetPos = infoPopupPos;

            basicAttack = skills.Find("BasicAttack").GetComponent<Button>();
            basicAttack.onClick.AddListener(BasicAttackClick);
            
            enhancedAttack = skills.Find("EnhancedSkill").gameObject.AddComponent<NSOverlordScreen.Skill>();
            enhancedAttack.skillType = AdminBRO.MagicGuildSkill.Type_ActiveSkill;
            enhancedAttack.infoWidgetPos = infoPopupPos;
            
            ultimate = skills.Find("UltimateSkill").gameObject.AddComponent<NSOverlordScreen.Skill>();
            ultimate.skillType = AdminBRO.MagicGuildSkill.Type_UltimateSkill;
            ultimate.infoWidgetPos = infoPopupPos;
            
            SortEquipInTabs();
        }
        
        private void UpdateCell(int equipId, int newEquipId)
        {
            var newEquip = GetEquipById(newEquipId);
            var equip = GetEquippedItemByType(newEquip.equipData.equipmentType);
            
            newEquip.Select();
            equippedItems.Add(newEquip);
            CustomizeCell(newEquip);

            equip?.Deselect();
            equippedItems.Remove(equip);
            
            CustomizeStats();
            SortEquipInTabs();
        }

        private void CustomizeStats()
        {
            health.text = overlordData.health.ToString();
            mana.text = overlordData.mana.ToString();
            speed.text = overlordData.speed.ToString();
            power.text = overlordData.power.ToString();
            constitution.text = overlordData.constitution.ToString();
            agility.text = overlordData.agility.ToString();

            accuracy.text = overlordData.accuracy * 100 + "%";
            critChance.text = overlordData.critrate * 100 + "%";
            dodgeChance.text = overlordData.dodge * 100 + "%";
            damage.text = overlordData.damage.ToString();
            potency.text = overlordData.potency.ToString();
        }

        private void BasicAttackClick()
        {
            var skillInfo = NSOverlordScreen.BasicAttackInfo.GetInstance(transform);
            skillInfo.skillType = AdminBRO.CharacterSkill.Type_OverlordAttack;
        }
        
        private void CustomizeCell(NSOverlordScreen.Equipment equip)
        {
            var type = equip.equipData.equipmentType;
            var cell = GetCellByType(type);
            
            var cellButton = cell.GetComponent<Button>();
            cellButton.onClick.RemoveAllListeners();
            cellButton.onClick.AddListener(() => CellClick(equip));
            
            var icon = cell.Find("Equip").GetComponent<Image>();
            var notification = cell.Find("Notification").gameObject;

            icon.sprite = ResourceManager.LoadSprite(equip.equipData.icon);
        }

        private void CellClick(NSOverlordScreen.Equipment equip)
        {
            var tabId = GetTabIdByEquipType(equip.equipData.equipmentType);
            
            if (tabId != activeTabId || equipPopup == null)
            {
                TabClick(tabId);
                InstantiateInfoPopup(equip);
            }
        }

        private void ShowInfoPopup(NSOverlordScreen.Equipment newEquip)
        {
            var equip = GetEquippedItemByType(newEquip.equipData.equipmentType);
            InstantiateInfoPopup(equip, newEquip);
        }

        private void InstantiateInfoPopup(NSOverlordScreen.Equipment equip, NSOverlordScreen.Equipment newEquip = null)
        {
            equipPopup = NSOverlordScreen.EquipInfoPopup.GetInstance(infoPopupPos);
            equipPopup.equipId = equip.equipId;
            equipPopup.selectedEquipId = newEquip?.equipId;
            equipPopup.OnEquip += UpdateCell;
        }
        
        private NSOverlordScreen.Equipment GetEquipById(int id) => 
            equipments.FirstOrDefault(eq => eq.equipId == id);
        
        private NSOverlordScreen.Equipment GetEquippedItemByType(string type) => 
            equippedItems.FirstOrDefault(eq => eq.equipData.equipmentType == type);

        private int GetTabIdByEquipType(string type)
        {
            return type switch
            {
                AdminBRO.EquipmentType.OverlordWeapon => TabWeapon,
                AdminBRO.EquipmentType.OverlordGloves => TabGloves,
                AdminBRO.EquipmentType.OverlordHelmet => TabHelmet,
                AdminBRO.EquipmentType.OverlordHarness => TabHarness,
                AdminBRO.EquipmentType.OverlordThighs => TabThigh,
                AdminBRO.EquipmentType.OverlordBoots => TabBoots,
                _ => TabWeapon
            };
        }
        
        private Transform GetContentByType(string type)
        {
            return type switch
            {
                AdminBRO.EquipmentType.OverlordWeapon => scrollContents[TabWeapon],
                AdminBRO.EquipmentType.OverlordGloves => scrollContents[TabGloves],
                AdminBRO.EquipmentType.OverlordHelmet => scrollContents[TabHelmet],
                AdminBRO.EquipmentType.OverlordHarness => scrollContents[TabHarness],
                AdminBRO.EquipmentType.OverlordThighs => scrollContents[TabThigh],
                AdminBRO.EquipmentType.OverlordBoots => scrollContents[TabBoots],
                _ => null
            };
        }

        private Transform GetCellByType(string type)
        {
            return type switch
            {
                AdminBRO.EquipmentType.OverlordWeapon => cellWeapon.transform,
                AdminBRO.EquipmentType.OverlordGloves => cellGloves.transform,
                AdminBRO.EquipmentType.OverlordHelmet => cellHelmet.transform,
                AdminBRO.EquipmentType.OverlordHarness => cellHarness.transform,
                AdminBRO.EquipmentType.OverlordThighs => cellThigh.transform,
                AdminBRO.EquipmentType.OverlordBoots => cellBoots.transform,
                _ => null
            };
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
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_3, FTUE.SEX_1):
                    UIManager.ShowScreen<MapScreen>();
                    break;
                default:
                    var prevState = UIManager.currentState.prevState;
                    if (prevState.ScreenTypeIs<BattleScreen>() || prevState.ScreenTypeIs<BossFightScreen>())
                    {
                        UIManager.ToPrevState(prevState.prevScreenState);
                    }
                    else
                    {
                        UIManager.ToPrevScreen();
                    }
                    break;
            }
        }

        private void CollectiblesButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        private void SortEquipInTabs()
        {
            foreach (var tabId in tabIds)
            {
                var tabEquip = scrollContents[tabId].GetComponentsInChildren<NSOverlordScreen.Equipment>().ToList();
                var tabEquipSort = tabEquip.OrderByDescending(e => e.equipData.raritySortLevel + (e.equipData.isEquipped ? 100 : 0));
                var eSiblingIndex = 0;
                foreach (var equip in tabEquipSort)
                {
                    equip?.transform.SetSiblingIndex(eSiblingIndex);
                    eSiblingIndex++;
                }
            }
        }
    }

    public class OverlordScreenInData : BaseFullScreenInData
    {
    }
}