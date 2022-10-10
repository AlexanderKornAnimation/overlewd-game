using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class BattleGirlsEquipContent : BaseContent
        {
            public const int TabAll = 0;
            public const int TabAssassin = 1;
            public const int TabCaster = 2;
            public const int TabHealer = 3;
            public const int TabBruiser = 4;
            public const int TabTank = 5;
            public const int TabsCount = 6;

            private Button[] tabs = new Button[TabsCount];
            private GameObject[] selectedTabs = new GameObject[TabsCount];
            private GameObject[] scrolls = new GameObject[TabsCount];
            private Transform[] contents = new Transform[TabsCount];
            private int[] tabIds = {TabAll, TabAssassin, TabCaster, TabHealer, TabBruiser, TabTank};
            private string[] tabNames = {"AllUnits", "Assassins", "Casters", "Healers", "Bruisers", "Tanks"};

            private InfoBlockBattleGirlEquip infoBlock;

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
                }

                infoBlock = transform.Find("InfoBlock").GetComponent<InfoBlockBattleGirlEquip>();
                infoBlock.equipCtrl = this;
            }

            private void Start()
            {
                TabClick(TabAll, false);
                RefreshState();
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

            }

            protected override void PortalButtonClick()
            {
                UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
                {
                    activeButtonId = PortalScreen.TabBattleGirlsEquip
                }).RunShowScreenProcess();
            }

            protected override void MarketButtonClick()
            {
                UIManager.ShowOverlay<MarketOverlay>();
            }

            public void RefreshState()
            {
                RefreshTabContent(TabAll, GameData.equipment.chAll);
                RefreshTabContent(TabAssassin, GameData.equipment.chAssassins);
                RefreshTabContent(TabCaster, GameData.equipment.chCasters);
                RefreshTabContent(TabHealer, GameData.equipment.chHealers);
                RefreshTabContent(TabBruiser, GameData.equipment.chBruisers);
                RefreshTabContent(TabTank, GameData.equipment.chTanks);

                infoBlock.RefreshState();
            }

            private void RefreshTabContent(int tabId, List<AdminBRO.Equipment> actualEq)
            {
                var tabEq = contents[tabId].GetComponentsInChildren<EquipmentBattleGirls>().ToList();
                var removedTabEq = tabEq.FindAll(te => !actualEq.Exists(ae => ae.id == te.equipId));
                var newEq = actualEq.FindAll(ae => !tabEq.Exists(te => te.equipId == ae.id));
                
                foreach (var re in removedTabEq)
                {
                    DestroyImmediate(re.gameObject);
                }

                foreach (var ne in newEq)
                {
                    var equip = EquipmentBattleGirls.GetInstance(contents[tabId]);
                    equip.equipCtrl = this;
                    equip.ctrl_InfoBlock = infoBlock;
                }

                var tabEqNew = contents[tabId].GetComponentsInChildren<EquipmentBattleGirls>().ToList();
                foreach (var te in tabEqNew)
                {
                    te.RefreshState();
                }
            }
        }
    }
}