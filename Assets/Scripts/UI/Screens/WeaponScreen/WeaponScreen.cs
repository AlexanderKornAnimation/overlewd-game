using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class WeaponScreen : BaseFullScreenParent<WeaponScreenInData>
    {
        private const int TabAllUnits = 0;
        private const int TabAssassins = 1;
        private const int TabCasters = 2;
        private const int TabHealers = 3;
        private const int TabBruisers = 4;
        private const int TabTanks = 5;
        private const int TabsCount = 6;

        private int activeTabId;

        private string[] tabNames = {"AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks"};

        private int[] tabIds = {TabAllUnits, TabAssassins, TabCasters, TabHealers, TabBruisers, TabTanks};
        private Button[] tabs = new Button[TabsCount];
        private GameObject[] pressedTabs = new GameObject[TabsCount];
        private GameObject[] scrollViews = new GameObject[TabsCount];
        private Transform[] scrollContents = new Transform[TabsCount];

        private List<NSWeaponScreen.Weapon> weapons = new List<NSWeaponScreen.Weapon>();
        private NSWeaponScreen.Weapon selectedWeapon;

        private Button backButton;
        private NSWeaponScreen.BaseSlot slotEquipped;
        private NSWeaponScreen.BaseSlot slotSelected;
        private Transform canvas;
        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/WeaponScreen/WeaponsScreen", transform);

            canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");
            var weaponsBack = canvas.Find("WeaponsBack");

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
                pressedTabs[i].gameObject.SetActive(false);

                scrollViews[i] = weaponsBack.Find("ScrollView_" + tabNames[i]).gameObject;
                scrollContents[i] = scrollViews[i].transform.Find("Viewport").Find("Content");
                scrollViews[i].gameObject.SetActive(false);
            }

            EnterTab(activeTabId);
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }
        
        private void Customize()
        {
            foreach (var equip in GameData.equipment.equipment)
            {
                if (equip.characterClass == AdminBRO.Character.Class_Overlord)
                    continue;
                    
                var tabId = equip.characterClass switch
                {
                    AdminBRO.Equipment.Class_Bruiser => TabBruisers,
                    AdminBRO.Equipment.Class_Assassin => TabAssassins,
                    AdminBRO.Equipment.Class_Caster => TabCasters,
                    AdminBRO.Equipment.Class_Tank => TabTanks,
                    AdminBRO.Equipment.Class_Healer => TabHealers,
                    _=> TabAllUnits
                };
                
                var weaponAll = NSWeaponScreen.Weapon.GetInstance(scrollContents[TabAllUnits]);
                weaponAll.Initialize();
                weaponAll.characterId = inputData?.characterId;
                weaponAll.weaponId = equip.id;
                weaponAll.OnSelect += SelectWeapon;
                weapons.Add(weaponAll);

                var weaponClass = NSWeaponScreen.Weapon.GetInstance(scrollContents[tabId]);
                weaponClass.Initialize();
                weaponClass.characterId = inputData?.characterId;
                weaponClass.weaponId = equip.id;
                weaponClass.OnSelect += SelectWeapon;
                weapons.Add(weaponClass);
            }

            slotSelected = canvas.Find("SelectedWeaponSlot").gameObject.AddComponent<NSWeaponScreen.SelectedSlot>();
            slotEquipped = canvas.Find("EquippedWeaponSlot").gameObject.AddComponent<NSWeaponScreen.EquippedSlot>();
            
            if (inputData.characterData.hasEquipment)
            {
                InitEquippedSlot();
            }
            
            SortWeaponsInTabs();
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.eventId)
            {
                case GameDataEvent.EventId.EquipmentEquipped:
                    OnEquipOrUnequip();
                    InitEquippedSlot();
                    slotSelected.Hide();
                    break;
                case GameDataEvent.EventId.EquipmentUnequipped:
                    OnEquipOrUnequip();
                    slotEquipped.Hide();
                    break;
            }
            
            SortWeaponsInTabs();
        }
        
        private void OnEquipOrUnequip()
        {
            foreach (var equip in weapons)
            {
                equip.Customize(); 
            }
        }
        
        private void InitEquippedSlot()
        {
            slotEquipped.chId = inputData.characterId;
            slotEquipped.equipId = inputData.characterData?.characterEquipmentData?.id;
            slotEquipped.Customize();
            slotEquipped.Show();
        }
        
        private void InitSelectedSlot(int? weaponId)
        {
            slotSelected.chId = inputData.characterId;
            slotSelected.equipId = weaponId;
            slotSelected.Customize();
            slotSelected.Show();
        }
        
        private void SelectWeapon(NSWeaponScreen.Weapon weapon)
        {
            selectedWeapon?.Deselect();
            weapon?.Select();
            selectedWeapon = weapon;
            InitSelectedSlot(weapon?.weaponId);
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
            if (inputData.prevScreenInData.IsType<BattleGirlListScreenInData>())
            {
                UIManager.ShowScreen<BattleGirlListScreen>();
            }
            else if (inputData.prevScreenInData.IsType<TeamEditScreenInData>())
            {
                UIManager.MakeScreen<TeamEditScreen>().
                    SetData(inputData.prevScreenInData.As<TeamEditScreenInData>()).
                    DoShow();
            }
            else
            {
                UIManager.MakeScreen<BattleGirlScreen>().
                    SetData(inputData.prevScreenInData.As<BattleGirlScreenInData>()).
                    DoShow();
            }
        }

        private void SortWeaponsInTabs()
        {
            foreach (var tabId in tabIds)
            {
                var tabWeapon = scrollContents[tabId].GetComponentsInChildren<NSWeaponScreen.Weapon>().ToList();
                var tabWeaponSort = tabWeapon.OrderByDescending(w => w.weaponData.raritySortLevel + (w.weaponData.isEquipped ? 100 : 0));
                var wSiblingIndex = 0;
                foreach (var weapon in tabWeaponSort)
                {
                    weapon?.transform.SetSiblingIndex(wSiblingIndex);
                    wSiblingIndex++;
                }
            }
        }
    }

    public class WeaponScreenInData : BaseFullScreenInData
    {
        public int? characterId;
        public AdminBRO.Character characterData => GameData.characters.GetById(characterId);
    }
}
