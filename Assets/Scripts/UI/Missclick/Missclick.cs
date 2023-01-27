using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class Missclick : MonoBehaviour
    {
        public bool enabledClick { get; set; } = true;

        private BaseScreen _target;
        public BaseScreen target
        {
            get => _target;
            set
            {
                if (target != null)
                {
                    target.missclick = null;
                }
                _target = value;
                if (target != null)
                {
                    target.missclick = this;
                }
            }
        }
        
        protected Image image;

        void Awake()
        {
            image = gameObject.AddComponent<Image>();
            image.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);

            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(Click);
            eventTrigger.triggers.Add(entry);
        }

        private void Click(BaseEventData data)
        {
            if (!enabledClick)
                return;
            if (IsTransitionState())
                return;
            target?.OnMissclick();
        }

        public MissclickShow Show()
        {
            return gameObject.AddComponent<MissclickFadeShow>();
        }

        public MissclickHide Hide()
        {
            return gameObject.AddComponent<MissclickFadeHide>();
        }

        public bool IsTransitionState()
        {
            return GetComponent<MissclickTransition>() != null;
        }

        public MissclickTransition GetTransition()
        {
            return GetComponent<MissclickTransition>();
        }
    }
}
