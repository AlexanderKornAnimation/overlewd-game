using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
    {
        protected Button eventsButton;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");

            eventsButton = canvas.Find("EventsButton").GetComponent<Button>();

            eventsButton.onClick.AddListener(OnEventButtonClick);
        }
        
        protected virtual void OnEventButtonClick()
        {
            UIManager.ShowOverlay<EventOverlay>();
        }
        
        public static EventsWidget GetInstance(Transform parent)
        {
            var prefab =
                (GameObject) Instantiate(Resources.Load("Prefabs/UI/Widgets/EventsWidget/EventsWidget"), parent);
            prefab.name = nameof(EventsWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<EventsWidget>();
        }
    }
}