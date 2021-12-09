using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class BaseMissclick : MonoBehaviour
    {
        public bool missClickEnabled { get; set; } = true;
        
        protected Image image;

        protected virtual void Awake()
        {
            image = gameObject.AddComponent<Image>();
            image.color = Color.clear;

            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(ImageClick);
            eventTrigger.triggers.Add(entry);
        }

        private void ImageClick(BaseEventData data)
        {
            if (missClickEnabled)
            {
                OnClick();
            }
        }

        protected virtual void OnClick()
        {
            
        }

        public virtual void Show()
        {

        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }
    }
}
