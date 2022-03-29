using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
    {
        protected Transform background;
        
        protected Button weeklyEventButton;
        protected Button monthlyEventButton;
        protected Button quarterlyEventButton;

        protected TextMeshProUGUI weeklyEventTitle;
        protected TextMeshProUGUI monthlyEventTitle;
        protected TextMeshProUGUI quarterlyEventTitle;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            background = canvas.Find("Background");

            weeklyEventButton = background.Find("WeeklyEvent").GetComponent<Button>();
            weeklyEventTitle = weeklyEventButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            weeklyEventButton.onClick.AddListener(WeeklyEventClick);
            
            monthlyEventButton = background.Find("MonthlyEvent").GetComponent<Button>();
            monthlyEventTitle = monthlyEventButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            monthlyEventButton.onClick.AddListener(MonthlyEventClick);
            
            quarterlyEventButton = background.Find("QuarterlyEvent").GetComponent<Button>();
            quarterlyEventTitle = quarterlyEventButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            quarterlyEventButton.onClick.AddListener(QuarterlyEventClick);

            gameObject.AddComponent<BlendPulseSelector>();
        }

        private void Start()
        {
            Customize();
        }
        
        protected virtual void Customize()
        {
            
        }
        
        protected virtual void WeeklyEventClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Destroy(GetComponent<Selector>());
            UIManager.ShowOverlay<EventOverlay>();
        }

        protected virtual void MonthlyEventClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Destroy(GetComponent<Selector>());
            UIManager.ShowOverlay<EventOverlay>();
        }
        
        protected virtual void QuarterlyEventClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Destroy(GetComponent<Selector>());
            UIManager.ShowOverlay<EventOverlay>();
        }
        
        public static EventsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<EventsWidget>
                ("Prefabs/UI/Widgets/EventsWidget/EventsWidget", parent);
        }
    }
}