using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class ShardContent : MonoBehaviour
        {
            public const int TabShardBasic = 0;
            public const int TabShardAdvanced = 1;
            public const int TabShardEpic = 2;
            public const int TabShardHeroic = 3;
            public const int TabShardsCount = 4;

            public int activeTabId { get; private set; } = TabShardBasic;

            private Button[] tabs = new Button[TabShardsCount];
            private GameObject[] selectedTabs = new GameObject[TabShardsCount];
            private int[] tabIds = {TabShardBasic, TabShardAdvanced, TabShardEpic, TabShardHeroic};
            private string[] tabNames = {"TabShardBasic", "TabShardAdvanced", "TabShardEpic", "TabShardHeroic"};

            public bool isMergeMode = false;

            private string[] matriarchNames = {"Ulvi", "Adriel", "Ingie", "Faye", "Lili"};
            private List<Matriarch> matriarchies = new List<Matriarch>();
            
            private void Awake()
            {
                var tabArea = transform.Find("TabArea");
                var selectedTabsArea = transform.Find("SelectedTabs");
                var matriarchiesArea = transform.Find("Matriarchies");

                foreach (var i in tabIds)
                {
                    tabs[i] = tabArea.Find(tabNames[i]).GetComponent<Button>();
                    tabs[i].onClick.AddListener(() => TabClick(i));
                    selectedTabs[i] = selectedTabsArea.Find(tabNames[i]).gameObject;
                    selectedTabs[i].SetActive(false);
                }
                
                for (int i = 0; i < matriarchNames.Length; i++)
                {
                    var matriarch = matriarchiesArea.Find(matriarchNames[i]).gameObject;
                    matriarchies.Add(matriarch.AddComponent<Matriarch>());
                    matriarchies[i].matriarchKey = matriarchNames[i];
                }
            }

            private void Start()
            {
                Customize();
            }
            
            public void OnModeChanged()
            {
                tabs[TabShardHeroic].gameObject.SetActive(!isMergeMode);
                selectedTabs[TabShardHeroic].SetActive(!isMergeMode && activeTabId == TabShardHeroic);
            }
            
            private void Customize()
            {
                EnterTab(activeTabId);
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
            }

            private void LeaveTab(int tabId)
            {
                selectedTabs[tabId].SetActive(false);
            }
        }
    }
}
