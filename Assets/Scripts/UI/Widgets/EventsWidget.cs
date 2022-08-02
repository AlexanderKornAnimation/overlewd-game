using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
    {
        protected Transform background;
        protected RectTransform backRect;
        
        protected Button weeklyEventButton;
        protected Button monthlyEventButton;
        protected Button quarterlyEventButton;

        protected TextMeshProUGUI weeklyEventTitle;
        protected TextMeshProUGUI monthlyEventTitle;
        protected TextMeshProUGUI quarterlyEventTitle;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();
            background = backRect.Find("Background");

            weeklyEventButton = background.Find("WeeklyEvent").GetComponent<Button>();
            weeklyEventTitle = weeklyEventButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            weeklyEventButton.onClick.AddListener(WeeklyEventClick);
            weeklyEventButton.gameObject.SetActive(false);

            monthlyEventButton = background.Find("MonthlyEvent").GetComponent<Button>();
            monthlyEventTitle = monthlyEventButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            monthlyEventButton.onClick.AddListener(MonthlyEventClick);
            monthlyEventButton.gameObject.SetActive(false);

            quarterlyEventButton = background.Find("QuarterlyEvent").GetComponent<Button>();
            quarterlyEventTitle = quarterlyEventButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            quarterlyEventButton.onClick.AddListener(QuarterlyEventClick);
            quarterlyEventButton.gameObject.SetActive(false);

            gameObject.AddComponent<BlendPulseSelector>();
        }

        void Start()
        {
            Customize();
        }
        
        void Customize()
        {
            var weekly = GameData.events.activeWeekly;
            if (weekly != null)
            {
                weeklyEventButton.gameObject.SetActive(true);
                weeklyEventTitle.text = weekly.name;
            }

            var monthly = GameData.events.activeMonthly;
            if (monthly != null)
            {
                monthlyEventButton.gameObject.SetActive(true);
                monthlyEventTitle.text = monthly.name;
            }

            var quarterly = GameData.events.activeQuarterly;
            if (quarterly != null)
            {
                quarterlyEventButton.gameObject.SetActive(true);
                quarterlyEventTitle.text = quarterly.name;
            }
        }
        
        protected virtual void WeeklyEventClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeOverlay<EventOverlay>().
                SetData(new EventOverlayInData
                {
                    activeTabId = EventOverlay.TabWeekly
                }).RunShowOverlayProcess();
        }

        protected virtual void MonthlyEventClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeOverlay<EventOverlay>().
                SetData(new EventOverlayInData
                {
                    activeTabId = EventOverlay.TabMonthly
                }).RunShowOverlayProcess();
        }
        
        protected virtual void QuarterlyEventClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeOverlay<EventOverlay>().
                SetData(new EventOverlayInData
                {
                    activeTabId = EventOverlay.TabQuarterly
                }).RunShowOverlayProcess();
        }
        
        public void Show()
        {
            UITools.RightShow(backRect);
        }

        public void Hide()
        {
            UITools.RightHide(backRect);
        }

        public async Task ShowAsync()
        {
            await UITools.RightShowAsync(backRect);
        }

        public async Task HideAsync()
        {
            await UITools.RightHideAsync(backRect);
        }
        
        public static EventsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<EventsWidget>
                ("Prefabs/UI/Widgets/EventsWidget/EventsWidget", parent);
        }
    }
}