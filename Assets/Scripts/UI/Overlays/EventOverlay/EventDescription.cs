using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            private Image girlPic;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                text = canvas.Find("Text").GetComponent<TextMeshProUGUI>();
                girlPic = canvas.Find("Girl").GetComponent<Image>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var _eventData = eventData;
                text.text = _eventData.description;
                //girlPic.sprite = ResourceManager.LoadSprite(_eventData?.narratorMatriarch?.icon);
            }

            public static EventDescription GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventDescription>
                    ("Prefabs/UI/Overlays/EventOverlay/EventDescription", parent);
            }
        }
    }
}
