using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class EventItem : MonoBehaviour
        {
            public int eventId;
            public int eventQuestId;

            private Button mapButton;

            private Text eventName;
            private Text title;
            private Text description;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                eventName = canvas.Find("EventName").GetComponent<Text>();
                title = canvas.Find("Title").GetComponent<Text>();
                description = canvas.Find("Description").GetComponent<Text>();

                mapButton = canvas.Find("MapButton").GetComponent<Button>();
                mapButton.onClick.AddListener(ToMapClick);

                CustomizeItem();
            }

            private void CustomizeItem()
            {
                var eventData = GameData.GetEventById(eventId);
                var eventQuestData = GameData.GetEventQuestById(eventQuestId);

                eventName.text = eventData?.name;
                title.text = eventQuestData?.name;
                description.text = eventQuestData?.description;
            }

            private void ToMapClick()
            {
                GameGlobalStates.eventMapScreen_EventId = eventId;
                UIManager.ShowScreen<EventMapScreen>();
            }

            void Update()
            {

            }

            public static EventItem GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/EventItem"), parent);
                newItem.name = nameof(EventItem);
                return newItem.AddComponent<EventItem>();
            }
        }
    }
}
