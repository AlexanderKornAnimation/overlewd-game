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
        public const int TabWeekly = 0;
        public const int TabMonthly = 1;
        public const int TabDecade = 2;
        public const int TabComingMonthly = 3;
        public const int TabComingDecade = 4;
        public const int TabsCount = 5;

        private int[] tabIds = { TabWeekly, TabMonthly, TabDecade, TabComingMonthly, TabComingDecade };
        private string[] tabNames = { "Weekly", "Monthly", "Decade", "ComingMonthly", "ComingDecade" };
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
        private TextMeshProUGUI[] eventButtonText = new TextMeshProUGUI[TabsCount];
        private Image[] eventButtonImage = new Image[TabsCount];
        private Sprite[] eventButtonDefaultSprite = new Sprite[TabsCount];

        private Button backButton;

        private int activeTabId = TabWeekly;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/EventOverlay/EventOverlay", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            foreach (var tabId in tabIds)
            {
                scrollView[tabId] = canvas.Find("ScrollView_" + tabNames[tabId]);
                scrollViewContent[tabId] = scrollView[tabId].Find("Viewport").Find("Content");
                eventButton[tabId] = canvas.Find("EventButton_" + tabNames[tabId]).GetComponent<Button>();
                eventButtonText[tabId] = eventButton[tabId].transform.Find("Text").GetComponent<TextMeshProUGUI>();
                eventButtonImage[tabId] = eventButton[tabId].GetComponent<Image>();
                eventButtonDefaultSprite[tabId] = eventButtonImage[tabId].sprite;

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
            eventButton[activeTabId].onClick.Invoke();

            StartCoroutine(TabContentVisibleOptimize());

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_eventbook);

            await Task.CompletedTask;
        }

        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            activeTabId = tabId;
            foreach (var _tabId in tabIds)
            {
                scrollView[_tabId].gameObject.SetActive(_tabId == tabId);
                eventButtonImage[_tabId].sprite = (_tabId == tabId) ? 
                                                     eventButton[_tabId].spriteState.selectedSprite :
                                                     eventButtonDefaultSprite[_tabId];
            }
        }

        private void InitEventTab(AdminBRO.EventItem eventData, int tabId)
        {
            var tabScrollViewContent = scrollViewContent[tabId];
            eventButtonText[tabId].text = eventData.name;

            var banner = NSEventOverlay.Banner.GetInstance(tabScrollViewContent);

            if (tabId == TabMonthly)
            {
                NSEventOverlay.BattlePass.GetInstance(scrollViewContent[tabId]);
            }
            
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
            eventButtonText[tabId].text = eventData.name;

            var comingWidget = NSEventOverlay.ComingEvent.GetInstance(tabScrollViewContent);
            comingWidget.eventId = eventData.id;
        }

        private void InitTabs()
        {
            var eventsData = new [] {
                GameData.events.activeWeekly,
                GameData.events.activeMonthly,
                GameData.events.activeQuarterly,
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
                        case TabComingDecade:
                            InitComingEventTab(eventsData[tabId], tabId);
                            break;
                        default:
                            InitEventTab(eventsData[tabId], tabId);
                            break;
                    }
                }
                else 
                {
                    eventButton[tabId].gameObject.SetActive(false);
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
        public int activeTabId = EventOverlay.TabWeekly;
    }
}
