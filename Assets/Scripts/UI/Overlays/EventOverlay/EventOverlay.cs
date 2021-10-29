using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventOverlay : BaseOverlay
    {
        private static int TabWeekly = 0;
        private static int TabMonthly = 1;
        private static int TabDecade = 2;
        private static int TabActive = 3;
        private static int TabComingSoon = 4;
        private static int TabsCount = 5;

        private int[] tabIds = { TabWeekly, TabMonthly, TabDecade, TabActive, TabComingSoon };
        private string[] tabNames = { "Weekly", "Monthly", "Decade", "Active", "ComingSoon" };
        private Transform[] scrollView = new Transform[TabsCount];
        private Transform[] scrollViewContent = new Transform[TabsCount];
        private Button[] eventButton = new Button[TabsCount];
        private Image[] eventButtonImage = new Image[TabsCount];
        private Sprite[] eventButtonDefaultSprite = new Sprite[TabsCount];

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/EventOverlay"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.HideOverlay();
            });

            foreach (var tabId in tabIds)
            {
                scrollView[tabId] = screenRectTransform.Find("Canvas").Find("ScrollView_" + tabNames[tabId]);
                scrollViewContent[tabId] = scrollView[tabId].Find("Viewport").Find("Content");
                eventButton[tabId] = screenRectTransform.Find("Canvas").Find("EventButton_" + tabNames[tabId]).GetComponent<Button>();
                eventButtonImage[tabId] = screenRectTransform.Find("Canvas").Find("EventButton_" + tabNames[tabId]).GetComponent<Image>();
                eventButtonDefaultSprite[tabId] = eventButtonImage[tabId].sprite;

                var tabId_delegate = tabId;
                eventButton[tabId].onClick.AddListener(() =>
                {
                    TabClick(tabId_delegate);
                });
            }

            NSEventOverlay.Banner.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventDescription.GetInstance(scrollViewContent[0]);

            NSEventOverlay.ComingEvent.GetInstance(scrollViewContent[4]);

            eventButton[TabWeekly].onClick.Invoke();

            InitWeeklyTab();
        }

        private void TabClick(int tabId)
        {
            foreach (var _tabId in tabIds)
            {
                scrollView[_tabId].gameObject.SetActive(_tabId == tabId);
                eventButtonImage[_tabId].sprite = (_tabId == tabId) ? 
                                                     eventButton[_tabId].spriteState.selectedSprite :
                                                     eventButtonDefaultSprite[_tabId];

            }
        }

        private void InitWeeklyTab()
        {
            var q = GameData.quests;
            var e = GameData.events;
        }

        void Update()
        {

        }
    }
}
