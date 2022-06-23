using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryListScreen : BaseFullScreenParent<MemoryListScreenInData>
    {
        private List<NSMemoryListScreen.MemoryList> memoryLists = new List<NSMemoryListScreen.MemoryList>();
        
        private Button backButton;
        private TextMeshProUGUI backButtonGirlName;

        private const int tabUlvi = 0;
        private const int tabAdriel = 1;
        private const int tabFaye = 2;
        private const int tabIngie = 3;
        private const int tabLili = 4;
        private const int tabsCount = 5;
        private int activeTabId;

        
        private string[] tabNames = {"Ulvi", "Adriel", "Faye", "Ingie", "Lili"};
        private int[] tabIds =  {tabUlvi, tabAdriel, tabFaye, tabIngie, tabLili};
        private Button[] tabs = new Button[tabsCount];
        private GameObject[] pressedTabs = new GameObject[tabsCount];
        private GameObject[] scrolls = new GameObject[tabsCount];
        private Transform[] scrollsContent = new Transform[tabsCount];

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryListScreen/MemoryListScreen",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");

            foreach (var id in tabIds)
            {
                tabs[id] = tabArea.Find(tabNames[id]).GetComponent<Button>();
                tabs[id].onClick.AddListener(() =>
                {
                    TabClick(id);
                });
                pressedTabs[id] = tabs[id].transform.Find("SelectedTab").gameObject;
                pressedTabs[id].SetActive(false);
                scrolls[id] = canvas.Find(tabNames[id] + "MainScroll").gameObject;
                scrolls[id].SetActive(false);
                scrollsContent[id] = scrolls[id].transform.Find("Viewport").Find("Content");
            }
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButtonGirlName = backButton.transform.Find("GirlName").GetComponent<TextMeshProUGUI>();
            backButton.onClick.AddListener(BackButtonClick);


        }

        public override async Task BeforeShowMakeAsync()
        {
            activeTabId = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => tabUlvi,
                AdminBRO.MatriarchItem.Key_Adriel => tabAdriel,
                AdminBRO.MatriarchItem.Key_Faye => tabFaye,
                AdminBRO.MatriarchItem.Key_Ingie => tabIngie,
                AdminBRO.MatriarchItem.Key_Lili => tabLili,
                _ => tabUlvi
            };

            AddMemoryLists();
            EnterTab(activeTabId);
        }

        private void AddMemoryLists()
        {
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabUlvi]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabUlvi]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabUlvi]));

            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabAdriel]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabAdriel]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabAdriel]));
            
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabFaye]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabFaye]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabFaye]));
            
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabIngie]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabIngie]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabIngie]));
            
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabLili]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabLili]));
            memoryLists.Add(NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[tabLili]));
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
            backButtonGirlName.text = tabs[tabId].name + "`s";
            scrolls[tabId].SetActive(true);
            pressedTabs[tabId].SetActive(true);
        }


        private void LeaveTab(int tabId)
        {
            scrolls[tabId].SetActive(false);
            pressedTabs[tabId].SetActive(false);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<GirlScreen>();
            }
            else
            {
                UIManager.MakeScreen<GirlScreen>().
                    SetData(inputData.prevScreenInData as GirlScreenInData).
                    RunShowScreenProcess();
            }
        }
    }

    public class MemoryListScreenInData : BaseFullScreenInData
    {
        public string girlKey;

        public AdminBRO.MatriarchItem girlData =>
            GameData.matriarchs.GetMatriarchByKey(girlKey);
    }
}