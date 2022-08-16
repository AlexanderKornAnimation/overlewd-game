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
        public class OverlordEquipContent : MonoBehaviour
        {
            public const int TabWeapon = 0;
            public const int TabGloves = 1;
            public const int TabHelmet = 2;
            public const int TabHarness = 3;
            public const int TabTigh = 4;
            public const int TabBoots = 5;
            public const int TabsCount = 6;

            public int activeTabId { get; private set; }= TabWeapon;

            private Button[] tabs = new Button[TabsCount];
            private GameObject[] selectedTabs = new GameObject[TabsCount];
            private GameObject[] scrolls = new GameObject[TabsCount];
            private Transform[] contents = new Transform[TabsCount];
            private int[] tabIds = {TabWeapon, TabGloves, TabHelmet, TabHarness, TabTigh, TabBoots};
            private string[] tabNames = {"Weapon", "Gloves", "Helmet", "Harness", "Tigh", "Boots"};

            private void Awake()
            {
                var tabArea = transform.Find("TabArea");
                var selectedTabArea = transform.Find("SelectedTabArea");
                var bottomSubstrate = transform.Find("BottomSubstrate");
                
                foreach (var i in tabIds)
                {
                    tabs[i] = tabArea.Find(tabNames[i]).GetComponent<Button>();
                    tabs[i].onClick.AddListener(() => TabClick(i));
                    selectedTabs[i] = selectedTabArea.Find(tabNames[i]).gameObject;
                    scrolls[i] = bottomSubstrate.Find("ScrollView_" + tabNames[i]).gameObject;
                    contents[i] = scrolls[i].transform.Find("Viewport").Find("Content");

                    selectedTabs[i].SetActive(false);
                }
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                EnterTab(activeTabId);

                for (int i = 0; i <= 5; i++)
                {
                    Equipment.GetInstance(contents[0]);
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
                selectedTabs[tabId].SetActive(true);
                scrolls[tabId].SetActive(true);
            }

            private void LeaveTab(int tabId)
            {
                selectedTabs[tabId].SetActive(false);
                scrolls[tabId].SetActive(false);
            }
        }
    }
}
