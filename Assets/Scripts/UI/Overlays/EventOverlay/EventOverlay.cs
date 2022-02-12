using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private Button backButton;

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
                eventButtonImage[tabId] = canvas.Find("EventButton_" + tabNames[tabId]).GetComponent<Image>();
                eventButtonDefaultSprite[tabId] = eventButtonImage[tabId].sprite;

                var tabId_delegate = tabId;
                eventButton[tabId].onClick.AddListener(() =>
                {
                    TabClick(tabId_delegate);
                });
            }

            eventButton[TabWeekly].onClick.Invoke();

            InitTabs();
        }

        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            foreach (var _tabId in tabIds)
            {
                scrollView[_tabId].gameObject.SetActive(_tabId == tabId);
                eventButtonImage[_tabId].sprite = (_tabId == tabId) ? 
                                                     eventButton[_tabId].spriteState.selectedSprite :
                                                     eventButtonDefaultSprite[_tabId];

            }
        }

        private void InitTabs()
        {
            NSEventOverlay.Banner.GetInstance(scrollViewContent[TabWeekly]);

            foreach (var eventData in GameData.events)
            {
                foreach (var questId in eventData.quests)
                {
                    var eventItem = NSEventOverlay.EventItem.GetInstance(scrollViewContent[TabWeekly]);
                    eventItem.eventId = eventData.id;
                    eventItem.eventQuestId = questId;
                }
            }

            NSEventOverlay.EventDescription.GetInstance(scrollViewContent[TabWeekly]);

            NSEventOverlay.ComingEvent.GetInstance(scrollViewContent[TabComingSoon]);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.HideOverlay();
        }
    }
}
