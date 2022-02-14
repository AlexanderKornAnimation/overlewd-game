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

        void Awake()
        {
            var canvas = transform.Find("Canvas");

            eventsButton = canvas.Find("EventsButton").GetComponent<Button>();

            eventsButton.onClick.AddListener(OnEventButtonClick);
        }
        
        protected virtual void OnEventButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowOverlay<EventOverlay>();
        }
        
        public static EventsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<EventsWidget>
                ("Prefabs/UI/Widgets/EventsWidget/EventsWidget", parent);
        }
    }
}