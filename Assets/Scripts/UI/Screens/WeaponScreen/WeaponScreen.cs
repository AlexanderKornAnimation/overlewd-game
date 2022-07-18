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
        private const int tabAllUnits = 0;
        private const int tabAssassins = 1;
        private const int tabCasters = 2;
        private const int tabHealers = 3;
        private const int tabBruisers = 4;
        private const int tabTanks = 5;
        private const int tabsCount = 6;

        private int activeTabId;

        private Button backButton;
        private Button portalButton;
        private Button slotButton;

        private string[] tabNames = {"AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks"};

        private int[] tabIds = {tabAllUnits, tabAssassins, tabCasters, tabHealers, tabBruisers, tabTanks};
        private Button[] tabs = new Button[tabsCount];
        private GameObject[] pressedTabs = new GameObject[tabsCount];
        private GameObject[] scrollViews = new GameObject[tabsCount];
        private Transform[] scrollContents = new Transform[tabsCount];

        private GameObject slotFull;
        private GameObject slotEmptyHint;
        private Image weaponIcon;
        private TextMeshProUGUI speed;
        private TextMeshProUGUI accuracy;
        private TextMeshProUGUI power;
        private TextMeshProUGUI critChance;
        private TextMeshProUGUI effectDescription;
        private Image weaponEffectRarity;

        private List<NSWeaponScreen.Weapon> weapons = new List<NSWeaponScreen.Weapon>();

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/WeaponScreen/WeaponsScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabsArea = canvas.Find("TabsArea");
            var pressedTabsArea = canvas.Find("PressedTabsArea");
            var weaponsBack = canvas.Find("WeaponsBack");
            var weaponSlot = canvas.Find("WeaponSlot");

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            slotFull = weaponSlot.Find("SlotFull").gameObject;
            slotEmptyHint = weaponSlot.Find("SlotEmptyHint").gameObject;
            
            slotButton = slotFull.GetComponent<Button>();
            slotButton.onClick.AddListener(SlotButtonClick);
            
            weaponIcon = slotFull.transform.Find("WeaponIcon").GetComponent<Image>();
            weaponEffectRarity = slotFull.transform.Find("WeaponEffect").GetComponent<Image>();
            effectDescription = weaponEffectRarity.transform.Find("EffectDescription").GetComponent<TextMeshProUGUI>();

            var stats = slotFull.transform.Find("Stats");
            speed = stats.Find("Speed").Find("IncreaseValue").GetComponent<TextMeshProUGUI>();
            power = stats.Find("Power").Find("IncreaseValue").GetComponent<TextMeshProUGUI>();
            accuracy = stats.Find("Accuracy").Find("IncreaseValue").GetComponent<TextMeshProUGUI>();
            critChance = stats.Find("CritChance").Find("IncreaseValue").GetComponent<TextMeshProUGUI>();            

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

        
        private void CustomizeSlot(NSWeaponScreen.Weapon weapon)
        {
            if (weapon == null)
            {
                slotFull.SetActive(false);
                slotEmptyHint.SetActive(!slotFull.activeSelf);
                slotButton.gameObject.SetActive(slotFull.activeSelf); 
            }
            else
            {
                var weaponData = weapon.weaponData;
                var isMy = weaponData.IsMy(inputData?.characterId);
                
                slotFull.SetActive(isMy);
                slotEmptyHint.SetActive(!slotFull.activeSelf);
                slotButton.gameObject.SetActive(slotFull.activeSelf);
                weaponIcon.sprite = ResourceManager.LoadSprite(weaponData.icon);

                if (isMy)
                {
                    speed.text = "+" + weaponData.speed;
                    power.text = "+" + weaponData.power;
                    accuracy.text = "+" + weaponData.accuracy + "%";
                    critChance.text = "+" + weaponData.critrate + "%";
                }
            }
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.type)
            {
                case GameDataEvent.Type.EquipmentEquipped:
                case GameDataEvent.Type.EquipmentUnequipped:
                    foreach (var weapon in weapons)
                    {
                        weapon.Customize();
                    }

                    var equipped = weapons.FirstOrDefault(w => w.weaponData.IsMy(inputData?.characterId));
                    CustomizeSlot(equipped);
                    break;
            }
        }

        private void Customize()
        {
            foreach (var equip in GameData.equipment.equipment)
            {
                var weaponData = GameData.equipment.GetById(equip.id);

                var tabId = weaponData.characterClass switch
                {
                    AdminBRO.Equipment.Class_Bruiser => tabBruisers,
                    AdminBRO.Equipment.Class_Assassin => tabAssassins,
                    AdminBRO.Equipment.Class_Caster => tabCasters,
                    AdminBRO.Equipment.Class_Tank => tabTanks,
                    AdminBRO.Equipment.Class_Healer => tabHealers,
                    _=> tabAllUnits
                };
                
                var weaponAll = NSWeaponScreen.Weapon.GetInstance(scrollContents[tabAllUnits]);
                weaponAll.characterId = inputData?.characterId;
                weaponAll.weaponId = equip.id;
                weapons.Add(weaponAll);

                var weaponClass = NSWeaponScreen.Weapon.GetInstance(scrollContents[tabId]);
                weaponClass.characterId = inputData?.characterId;
                weaponClass.weaponId = equip.id;
            }
            
            CustomizeSlot(weapons.FirstOrDefault(w => w.weaponData.IsMy(inputData?.characterId)));

        }

        private async void SlotButtonClick()
        {
            var charData = inputData?.characterData;
            if (charData != null)
            {
                var weapon = weapons.FirstOrDefault(w => w.weaponData.isEquipped);
                await weapon?.Unequip(charData.id.Value, charData.equipment.FirstOrDefault());
            }
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
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<TeamEditScreen>();
            }
            else
            {
                UIManager.MakeScreen<BattleGirlScreen>().
                    SetData(inputData.prevScreenInData as BattleGirlScreenInData).
                    RunShowScreenProcess();
            }
        }
    }

    public class WeaponScreenInData : BaseFullScreenInData
    {
        public int? characterId;
        public AdminBRO.Character characterData => GameData.characters.GetById(characterId);
    }
}
