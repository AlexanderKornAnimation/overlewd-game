using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    public class EventOverlay : BaseOverlayParent<EventOverlayInData>
    {
        public const int TabQuarterly = 0;
        public const int TabMonthly = 1;
        public const int TabWeekly = 2;
        public const int TabComingMonthly = 3;
        public const int TabsCount = 4;

        private int[] tabIds = { TabQuarterly, TabMonthly, TabWeekly, TabComingMonthly };
        private string[] tabNames = { "Quarterly", "Monthly", "Weekly", "ComingMonthly" };
        private Transform[] scrollView = new Transform[TabsCount];
        private Transform[] scrollViewContent = new Transform[TabsCount];
        private List<NSEventOverlay.BaseQuest>[] tabEventQuests = 
            {
                new List<NSEventOverlay.BaseQuest>(),
                new List<NSEventOverlay.BaseQuest>(),
                new List<NSEventOverlay.BaseQuest>(),
                new List<NSEventOverlay.BaseQuest>(),
                new List<NSEventOverlay.BaseQuest>()
            };
        private Button[] eventButton = new Button[TabsCount];
        private Image[] eventButtonBanner = new Image[TabsCount];
        private TextMeshProUGUI[] eventButtonTitle = new TextMeshProUGUI[TabsCount];
        private GameObject[] eventButtonPressed = new GameObject[TabsCount];
        private Image[] eventButtonPressedBanner = new Image[TabsCount];
        private TextMeshProUGUI[] eventButtonPressedTitle = new TextMeshProUGUI[TabsCount];

        private Button backButton;
        private Transform tabArea;

        private int activeTabId = TabWeekly;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/EventOverlay/EventOverlay", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var background = canvas.Find("Background");
            tabArea = canvas.Find("TabArea");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            foreach (var tabId in tabIds)
            {
                scrollView[tabId] = background.Find("ScrollView_" + tabNames[tabId]);
                scrollViewContent[tabId] = scrollView[tabId].Find("Viewport").Find("Content");
                
                var eventButtonTr = tabArea.Find("EventButton_" + tabNames[tabId]);
                var unpressed = eventButtonTr.Find("Unpressed");
                eventButton[tabId] = eventButtonTr.Find("Button").GetComponent<Button>();
                eventButtonTitle[tabId] = unpressed.Find("Title").GetComponent<TextMeshProUGUI>();
                eventButtonBanner[tabId] = unpressed.Find("Banner").GetComponent<Image>();
                
                eventButtonPressed[tabId] = eventButtonTr.Find("Pressed").gameObject;
                eventButtonPressedTitle[tabId] = eventButtonPressed[tabId].transform.Find("Title").GetComponent<TextMeshProUGUI>();
                eventButtonPressedBanner[tabId] = eventButtonPressed[tabId].transform.Find("Banner").GetComponent<Image>();

                eventButton[tabId].onClick.AddListener(() =>
                {
                    TabClick(tabId);
                });
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            activeTabId = inputData != null ? inputData.activeTabId : activeTabId;
            InitTabs();
            
            foreach (var scroll in scrollView)
            {
                scroll.gameObject.SetActive(false);
            }
            
            EnterTab(activeTabId);
            StartCoroutine(TabContentVisibleOptimize());
            

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch(GameData.ftue.stats.lastEndedState)
            {
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_eventbook);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_eventbook);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_eventbook);
                            break;
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(tabId);
        }

        private void EnterTab(int tabId)
        {
            eventButtonPressed[tabId].SetActive(true);
            scrollView[tabId].gameObject.SetActive(true);
            activeTabId = tabId;
            
            var tab = tabArea.Find("EventButton_" + tabNames[tabId]);
            tab.GetComponent<RectTransform>().sizeDelta =  new Vector2(313, 235);
            
            MoveTabs();
        }

        private void LeaveTab(int tabId)
        {
            eventButtonPressed[tabId].SetActive(false);
            scrollView[tabId].gameObject.SetActive(false);
            
            var tab = tabArea.Find("EventButton_" + tabNames[tabId]);
            tab.GetComponent<RectTransform>().sizeDelta = new Vector2(303, 184);
        }

        private void MoveTabs()
        {
            var prevTabId = 0;

            for (int i = 1; i < tabIds.Length; i++)
            {
                var prevTabRectTr = tabArea.Find("EventButton_" + tabNames[prevTabId]).GetComponent<RectTransform>();
                var tabRectTr = tabArea.Find("EventButton_" + tabNames[i]).GetComponent<RectTransform>();
                var prevTabPos = prevTabRectTr.anchoredPosition;
                tabRectTr.anchoredPosition = new Vector2(0, prevTabPos.y - (23 + prevTabRectTr.rect.height));
                prevTabId++;
            }
        }
        
        private void InitEventTab(AdminBRO.EventItem eventData, int tabId)
        {
            var tabScrollViewContent = scrollViewContent[tabId];
            eventButtonTitle[tabId].text = eventData.name;
            eventButtonPressedTitle[tabId].text = eventData.name;

            var banner = NSEventOverlay.Banner.GetInstance(tabScrollViewContent);
            
            foreach (var questId in eventData.quests)
            {
                var questData = GameData.quests.GetById(questId);

                var eventQuest = questData.hasDescription 
                    ?  (NSEventOverlay.BaseQuest) NSEventOverlay.EventQuest.GetInstance(tabScrollViewContent)
                    :  NSEventOverlay.EventShortQuest.GetInstance(tabScrollViewContent);
                
                eventQuest.eventId = eventData.id;
                eventQuest.questId = questId;
                eventQuest.SetCanvasActive(false);

                tabEventQuests[tabId].Add(eventQuest);
            }
            var descr = NSEventOverlay.EventDescription.GetInstance(tabScrollViewContent);
            descr.eventId = eventData.id;
        }

        private void InitComingEventTab(AdminBRO.EventItem eventData, int tabId)
        {
            var tabScrollViewContent = scrollViewContent[tabId];
            eventButtonTitle[tabId].text = eventData.name;
            eventButtonPressedTitle[tabId].text = eventData.name;

            var comingWidget = NSEventOverlay.ComingEvent.GetInstance(tabScrollViewContent);
            comingWidget.eventId = eventData.id;
        }

        private void InitTabs()
        {
            var eventsData = new [] {
                GameData.events.activeQuarterly,
                GameData.events.activeMonthly,
                GameData.events.activeWeekly,
                GameData.events.comingSoonMonthly,
                GameData.events.comingSoonQuarterly
            };

            foreach (var tabId in tabIds)
            {
                if (eventsData[tabId] != null)
                {
                    switch (tabId)
                    {
                        case TabComingMonthly:
                            InitComingEventTab(eventsData[tabId], tabId);
                            break;
                        default:
                            InitEventTab(eventsData[tabId], tabId);
                            break;
                    }
                }
                else
                {
                    tabArea.Find("EventButton_" + tabNames[tabId]).gameObject.SetActive(false);
                }
            }
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideOverlay();
        }

        private IEnumerator TabContentVisibleOptimize()
        {
            while (true)
            {
                var screenRect = UIManager.GetScreenWorldRect();
                foreach (var eventQuest in tabEventQuests[activeTabId])
                {
                    var itemRect = eventQuest.transform as RectTransform;
                    eventQuest.SetCanvasActive(itemRect.WorldRect().Overlaps(screenRect));
                }
                yield return null;
            }
        }
    }

    public class EventOverlayInData : BaseOverlayInData
    {
        public int activeTabId = EventOverlay.TabQuarterly;
    }
}
