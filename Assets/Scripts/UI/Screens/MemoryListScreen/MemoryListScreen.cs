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

        public const int TabUlvi = 0;
        public const int TabAdriel = 1;
        public const int TabFaye = 2;
        public const int TabIngie = 3;
        public const int TabLili = 4;
        public const int TabsCount = 5;
        private int activeTabId;

        private string[] tabNames = {"Ulvi", "Adriel", "Faye", "Ingie", "Lili"};
        private int[] tabIds =  {TabUlvi, TabAdriel, TabFaye, TabIngie, TabLili};
        private Button[] tabs = new Button[TabsCount];
        private GameObject[] pressedTabs = new GameObject[TabsCount];
        private GameObject[] scrolls = new GameObject[TabsCount];
        private Transform[] scrollsContent = new Transform[TabsCount];

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
                var matriarch = GameData.matriarchs.GetMatriarchByKey(tabNames[id]);
                tabs[id].gameObject.SetActive(matriarch.isOpen);
            }
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButtonGirlName = backButton.transform.Find("GirlName").GetComponent<TextMeshProUGUI>();
            backButton.onClick.AddListener(BackButtonClick);
        }

        private int TabIdByGirlKey(string key) =>
            key switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => TabUlvi,
                AdminBRO.MatriarchItem.Key_Adriel => TabAdriel,
                AdminBRO.MatriarchItem.Key_Faye => TabFaye,
                AdminBRO.MatriarchItem.Key_Ingie => TabIngie,
                AdminBRO.MatriarchItem.Key_Lili => TabLili,
                _ => TabUlvi
            };


        public override async Task BeforeShowMakeAsync()
        {
            activeTabId = TabIdByGirlKey(inputData?.girlKey);

            AddMemoryLists();
            EnterTab(activeTabId);

            await Task.CompletedTask;
        }

        private void AddMemoryLists()
        {
            foreach (var matriarch in GameData.matriarchs.matriarchs)
            {
                if (matriarch.isOpen)
                {
                    var memList = NSMemoryListScreen.MemoryList.GetInstance(scrollsContent[TabIdByGirlKey(matriarch.key)]);
                    memList.girlKey = matriarch.key;
                }
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
            UIManager.ToPrevScreen();
        }
    }

    public class MemoryListScreenInData : BaseFullScreenInData
    {
        public string girlKey;

        public AdminBRO.MatriarchItem girlData =>
            GameData.matriarchs.GetMatriarchByKey(girlKey);
    }
}