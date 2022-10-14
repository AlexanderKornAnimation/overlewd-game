using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

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
                ActualizeTabsContent();
                RefreshState();
                TabClick(TabAll, false);
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
                Merge();
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
                infoBlock.RefreshState();

                SortAndRefreshTabContent(TabAll);
                SortAndRefreshTabContent(TabAssassin);
                SortAndRefreshTabContent(TabCaster);
                SortAndRefreshTabContent(TabHealer);
                SortAndRefreshTabContent(TabBruiser);
                SortAndRefreshTabContent(TabTank);

                //merge button
                if (infoBlock.isFilled)
                {
                    mergeButton.gameObject.SetActive(infoBlock.isFilled);
                    SetMergeBtnPrice(infoBlock.mergePrice);
                    UITools.DisableButton(mergeButton, !infoBlock.canMerge);
                }
                else
                {
                    mergeButton.gameObject.SetActive(false);
                }
            }

            private void ActualizeTabsContent()
            {
                ActualizeTabContent(TabAll, GameData.equipment.chAll);
                ActualizeTabContent(TabAssassin, GameData.equipment.chAssassins);
                ActualizeTabContent(TabCaster, GameData.equipment.chCasters);
                ActualizeTabContent(TabHealer, GameData.equipment.chHealers);
                ActualizeTabContent(TabBruiser, GameData.equipment.chBruisers);
                ActualizeTabContent(TabTank, GameData.equipment.chTanks);
                infoBlock.ActualizeConsumeEquips();
            }

            private void ActualizeTabContent(int tabId, List<AdminBRO.Equipment> actualEq)
            {
                var tabEq = contents[tabId].GetComponentsInChildren<EquipmentBattleGirls>().ToList();
                var removedTabEq = tabEq.FindAll(te => !actualEq.Exists(ae => ae.id == te.equipId));
                var newEq = actualEq.FindAll(ae => !tabEq.Exists(te => te.equipId == ae.id));

                foreach (var re in removedTabEq)
                {
                    DestroyImmediate(re.gameObject);
                }

                foreach (var e in newEq)
                {
                    var equip = EquipmentBattleGirls.GetInstance(contents[tabId]);
                    equip.equipId = e.id;
                    equip.equipCtrl = this;
                    equip.ctrl_InfoBlock = infoBlock;
                }
            }

            private void SortAndRefreshTabContent(int tabId)
            {
                var tabSortEqs = contents[tabId].GetComponentsInChildren<EquipmentBattleGirls>().
                    OrderBy(e => (e.equipData.raritySortLevel * 10 + e.equipData.classSortLevel) * (e.IsConsume ? 0 : 1));
                var siblingIndex = 0;
                foreach (var e in tabSortEqs)
                {
                    e.transform.SetSiblingIndex(siblingIndex);
                    siblingIndex++;
                    e.RefreshState();
                }
            }

            private async void Merge()
            {
                await GameData.buildings.forge.MergeEquipment(
                    infoBlock.mergeSettings.mergeType,
                    infoBlock.mergeIds);
                ActualizeTabsContent();
                RefreshState();
            }
        }
    }
}