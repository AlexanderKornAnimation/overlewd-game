using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overlewd.NSMemoryListScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryListScreen : BaseFullScreen
    {
        private Dictionary<string, List<Transform>> scrolls = new Dictionary<string, List<Transform>>();
        private Dictionary<string, List<Transform>> scrollContents = new Dictionary<string, List<Transform>>();

        private Button backButton;
        private TextMeshProUGUI backButtonGirlName;

        private Button ulviTab;
        private Button adrielTab;
        private Button fayeTab;
        private Button ingieTab;
        private Button liliTab;
        private Button prevTab;

        private Transform closedEventMemoryScrollPos;
        private Transform openedEventMemoryScrollPos;
        private Transform mainMemoryScrollPos;

        private string[] girlNames = new string[5] {"Ulvi", "Adriel", "Faye", "Ingie", "Lili"};

        private MemoryListScreenInData inputData;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryListScreen/MemoryListScreen",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButtonGirlName = backButton.transform.Find("GirlName").GetComponent<TextMeshProUGUI>();
            backButton.onClick.AddListener(BackButtonClick);

            ulviTab = tabArea.Find("Ulvi").GetComponent<Button>();
            ulviTab.onClick.AddListener(UlviButtonClick);

            adrielTab = tabArea.Find("Adriel").GetComponent<Button>();
            adrielTab.onClick.AddListener(AdrielButtonClick);

            fayeTab = tabArea.Find("Faye").GetComponent<Button>();
            fayeTab.onClick.AddListener(FayeButtonClick);

            ingieTab = tabArea.Find("Ingie").GetComponent<Button>();
            ingieTab.onClick.AddListener(IngieButtonClick);

            liliTab = tabArea.Find("Lili").GetComponent<Button>();
            liliTab.onClick.AddListener(LiliButtonClick);
            
            closedEventMemoryScrollPos = canvas.Find("ClosedEventMemoryScrollPos");
            openedEventMemoryScrollPos = canvas.Find("OpenedEventMemoryScrollPos");
            mainMemoryScrollPos = canvas.Find("MainMemoryScrollPos");
        }

        private void Start()
        {
            Customize();
        }

        private void Customize()
        {
            for (int i = 0; i < girlNames.Length; i++)
            {
                if (!scrolls.ContainsKey(girlNames[i]))
                {
                    scrolls.Add(girlNames[i], new List<Transform>());
                    scrollContents.Add(girlNames[i], new List<Transform>());
                }

                var closedEventMemoryScroll = closedEventMemoryScrollPos.Find(girlNames[i] + "ClosedEventMemoryScroll");
                var closedEventMemoryContent = closedEventMemoryScroll.transform.Find("Viewport").Find("Content");
                
                var openedEventMemoryScroll = openedEventMemoryScrollPos.Find(girlNames[i] + "OpenedEventMemoryScroll");
                var openedEventMemoryContent = openedEventMemoryScroll.transform.Find("Viewport").Find("Content");
                
                var mainMemoryScroll = mainMemoryScrollPos.Find(girlNames[i] + "MainMemoryScroll");
                var mainMemoryContent = mainMemoryScroll.transform.Find("Viewport").Find("Content");

                scrolls[girlNames[i]].Add(closedEventMemoryScroll);
                scrollContents[girlNames[i]].Add(closedEventMemoryContent);
                
                scrolls[girlNames[i]].Add(openedEventMemoryScroll);
                scrollContents[girlNames[i]].Add(openedEventMemoryContent);
                
                scrolls[girlNames[i]].Add(mainMemoryScroll);
                scrollContents[girlNames[i]].Add(mainMemoryContent);

                var closedEventMemory = EventMemoryClosed.GetInstance(closedEventMemoryContent);

                closedEventMemory.screenInData = inputData;
                
                // closedEventMemory.inputData = new MemoryListScreenInData
                // {
                //    prevScreenInData = inputData.prevScreenInData,
                //    ftueStageId = inputData?.ftueStageId,
                //    eventStageId = inputData?.eventStageId
                // };
                
                closedEventMemoryScroll.gameObject.SetActive(false);
                openedEventMemoryScroll.gameObject.SetActive(false);
                mainMemoryScroll.gameObject.SetActive(false);
            }
            
            var selectedGirl = inputData?.girlName switch
            {
                "Ulvi" => ulviTab,
                "Adriel" => adrielTab,
                "Faye" => fayeTab,
                "Ingie" => ingieTab,
                "Lili" => liliTab,
                _ => ulviTab
            };
            
            EnterTab(selectedGirl);

        }
        
        public MemoryListScreen SetData(MemoryListScreenInData data)
        {
            inputData = data;
            return this;
        }
        
        private void EnterTab(Button girlTab)
        {
            var girlContent = scrolls[girlTab.name];

            foreach (var _content in girlContent)
            {
                _content.gameObject.SetActive(true);
            }

            Select(girlTab);
        }

        private void Select(Button girlTab)
        {
            prevTab = girlTab;
            girlTab.transform.Find("SelectedTab").gameObject.SetActive(true);

            backButtonGirlName.text = girlTab.name + "`s";
        }

        private void LeaveTab(Button girlTab)
        {
            Deselect();

            var girlContent = scrolls[girlTab.name];

            foreach (var _content in girlContent)
            {
                _content.gameObject.SetActive(false);
            }
        }

        private void Deselect()
        {
            var selectedTab = prevTab.transform.Find("SelectedTab");
            selectedTab.gameObject.SetActive(false);
        }

        private void UlviButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(ulviTab);
        }

        private void AdrielButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(adrielTab);
        }

        private void FayeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(fayeTab);
        }

        private void IngieButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(ingieTab);
        }

        private void LiliButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(liliTab);
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

    public class MemoryListScreenInData : BaseScreenInData
    {
        public string girlName;
    }
}