using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
    {
        private Button eventsButton;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");

            eventsButton = canvas.Find("EventsButton").GetComponent<Button>();
            
            eventsButton.onClick.AddListener(OnEventButtonClick);
        }

        private void OnEventButtonClick()
        {
            UIManager.ShowOverlay<EventOverlay>();
        }
        
        public static EventsWidget CreateInstance(Transform parent)
        {
            var prefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Widgets/EventsWidget/EventsWidget"));
            prefab.name = nameof(EventsWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<EventsWidget>();
        }
    }
}