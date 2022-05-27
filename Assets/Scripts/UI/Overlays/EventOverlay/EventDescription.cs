using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class EventDescription : MonoBehaviour
        {
            public int eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);

            private TextMeshProUGUI text;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                text = canvas.Find("Text").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var _eventData = eventData;
                text.text = _eventData.description;
            }

            public static EventDescription GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventDescription>
                    ("Prefabs/UI/Overlays/EventOverlay/EventDescription", parent);
            }
        }
    }
}
