using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class OverlordEquipContent : BaseContent
        {
            public const int TabWeapon = 0;
            public const int TabGloves = 1;
            public const int TabHelmet = 2;
            public const int TabHarness = 3;
            public const int TabTigh = 4;
            public const int TabBoots = 5;
            public const int TabsCount = 6;

            private Button[] tabs = new Button[TabsCount];
            private GameObject[] selectedTabs = new GameObject[TabsCount];
            private GameObject[] scrolls = new GameObject[TabsCount];
            private Transform[] contents = new Transform[TabsCount];
            private int[] tabIds = {TabWeapon, TabGloves, TabHelmet, TabHarness, TabTigh, TabBoots};
            private string[] tabNames = {"Weapon", "Gloves", "Helmet", "Harness", "Tigh", "Boots"};

            private InfoBlockOverlordEquip infoBlock;

            protected override void Awake()
            {
                base.Awake();

                var tabArea = transform.Find("TabArea");
                var selectedTabArea = transform.Find("SelectedTabArea");
                var bottomSubstrate = transform.Find("BottomSubstrate");
                
                foreach (var tabId in tabIds)
                {
                    tabs[tabId] = tabArea.Find(tabNames[tabId]).GetComponent<Button>();
                    tabs[tabId].onClick.AddListener(() => TabClick(tabId));
                    selectedTabs[tabId] = selectedTabArea.Find(tabNames[tabId]).gameObject;
                    scrolls[tabId] = bottomSubstrate.Find("ScrollView_" + tabNames[tabId]).gameObject;
                    contents[tabId] = scrolls[tabId].transform.Find("Viewport").Find("Content");

                    selectedTabs[tabId].SetActive(false);
                }

                infoBlock = transform.Find("InfoBlock").GetComponent<InfoBlockOverlordEquip>();
                infoBlock.equipCtrl = this;
            }

            private void Start()
            {
                ActualizeTabsContent();
                RefreshState();
                TabClick(TabWeapon, false);
            }
            
            private void TabClick(int tabId, bool playSound = true)
            {
                if (playSound)
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                foreach (var _tabId in tabIds)
                {
                    selectedTabs[_tabId].SetActive(_tabId == tabId);
                    scrolls[_tabId].SetActive(_tabId == tabId);
                }
            }

            protected override void MergeButtonClick()
            {

                ActualizeTabsContent();
                RefreshState();
            }

            protected override void PortalButtonClick()
            {
                UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
                {
                    activeButtonId = PortalScreen.TabOverlordEquip
                }).RunShowScreenProcess();
            }

            protected override void MarketButtonClick()
            {
                UIManager.ShowOverlay<MarketOverlay>();
            }

            public void RefreshState()
            {
                infoBlock.RefreshState();

                foreach (var tabContent in contents)
                {
                    var tabEqs = tabContent.GetComponentsInChildren<EquipmentOverlord>();
                    foreach (var e in tabEqs)
                    {
                        e.RefreshState();
                    }
                }
            }

            private void ActualizeTabsContent()
            {
                ActializeTabContent(TabWeapon, GameData.equipment.ovWeapons);
                ActializeTabContent(TabGloves, GameData.equipment.ovGloves);
                ActializeTabContent(TabHelmet, GameData.equipment.ovHelmets);
                ActializeTabContent(TabHarness, GameData.equipment.ovHarness);
                ActializeTabContent(TabTigh, GameData.equipment.ovThighs);
                ActializeTabContent(TabBoots, GameData.equipment.ovBoots);
            }

            private void ActializeTabContent(int tabId, List<AdminBRO.Equipment> actualEq)
            {
                var tabEq = contents[tabId].GetComponentsInChildren<EquipmentOverlord>().ToList();
                var removedTabEq = tabEq.FindAll(te => !actualEq.Exists(ae => ae.id == te.equipId));
                var newEq = actualEq.FindAll(ae => !tabEq.Exists(te => te.equipId == ae.id));

                foreach (var re in removedTabEq)
                {
                    DestroyImmediate(re.gameObject);
                }

                foreach (var ne in newEq)
                {
                    var equip = EquipmentOverlord.GetInstance(contents[tabId]);
                    equip.equipCtrl = this;
                    equip.ctrl_InfoBlock = infoBlock;
                }
            }
        }
    }
}
