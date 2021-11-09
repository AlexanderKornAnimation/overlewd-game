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

        private float alphaMax = 0.8f;
        private Image image;

        void Awake()
        {
            image = gameObject.AddComponent<Image>();
            image.color = Color.black;

            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(OnClick);
            eventTrigger.triggers.Add(entry);
        }

        protected virtual void OnClick(BaseEventData data)
        {
            
        }

        public void Show()
        {
            gameObject.AddComponent<MissclickShow>();
        }

        public void Hide()
        {
            gameObject.AddComponent<MissclickHide>();
        }

        public void UpdateShow(float showPercent)
        {
            var newColor = Color.black;
            newColor.a = alphaMax * showPercent;
            image.color = newColor;
        }

        public void UpdateHide(float hidePercent)
        {
            var newColor = Color.black;
            newColor.a = alphaMax * (1.0f - hidePercent);
            image.color = newColor;
        }
    }
}
