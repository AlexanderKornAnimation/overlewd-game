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

        void Awake()
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

        public virtual void UpdateShow(float showPercent)
        {

        }

        public virtual void UpdateHide(float hidePercent)
        {

        }

        public static T GetInstance<T>(Transform parent) where T : BaseMissclick
        {
            var newItem = new GameObject(typeof(T).Name);
            var screenRectTransform = newItem.AddComponent<RectTransform>();
            screenRectTransform.SetParent(parent, false);
            screenRectTransform.SetAsFirstSibling();
            UIManager.SetStretch(screenRectTransform);

            return newItem.AddComponent<T>();
        }
    }
}
