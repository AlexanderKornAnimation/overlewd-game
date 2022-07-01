using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class ComingEvent : MonoBehaviour
        {
            public int eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);

            private TextMeshProUGUI timer;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                timer = canvas.Find("Timer").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                StopCoroutine("TimerUpd");
                StartCoroutine(TimerUpd());
            }

            private IEnumerator TimerUpd()
            {
                var dateStart = eventData.dateStart;
                var timeToStart = TimeTools.AvailableTimeToString(dateStart);
                while (!String.IsNullOrEmpty(timeToStart))
                {
                    timer.text = timeToStart;
                    yield return new WaitForSeconds(1.0f);
                    dateStart = TimeTools.AvailableTimeToString(dateStart);
                }
            }

            public static ComingEvent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ComingEvent>
                    ("Prefabs/UI/Overlays/EventOverlay/ComingEvent", parent);
            }
        }
    }
}
