using System.Collections;
using System.Collections.Generic;
using TMPro;
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

            private TextMeshProUGUI eventName;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;

            private List<Image> rewards = new List<Image>();

            void Start()
            {
                var canvas = transform.Find("Canvas");

                eventName = canvas.Find("EventName").GetComponent<TextMeshProUGUI>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

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
                
                for (int i = 1; i <= 5; i++)
                {
                    rewards.Add(transform.Find("Canvas").Find("BackWithClock").Find($"Reward{i}").Find("Item").GetComponent<Image>());
                }

                rewards[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
                rewards[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
                rewards[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Wood");
                rewards[3].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
                rewards[4].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Copper");
            }

            private void ToMapClick()
            {
                GameGlobalStates.eventMapScreen_EventId = eventId;
                UIManager.ShowScreen<EventMapScreen>();
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
