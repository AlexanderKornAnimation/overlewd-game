using System;
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

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                eventName = canvas.Find("EventName").GetComponent<TextMeshProUGUI>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

                mapButton = canvas.Find("MapButton").GetComponent<Button>();
                mapButton.onClick.AddListener(ToMapClick);
            }

            void Start()
            {
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
                    rewards.Add(transform.Find("Canvas").Find("BackWithClock").Find($"Reward{i}").Find("Item")
                        .GetComponent<Image>());
                }

                rewards[0].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Crystal");
                rewards[1].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gem");
                rewards[2].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Wood");
                rewards[3].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Stone");
                rewards[4].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Copper");

                mapButton.gameObject.AddComponent<BlendPulseSelector>();
            }

            private void ToMapClick()
            {
                Destroy(mapButton.gameObject.GetComponent<Selector>());
                GameGlobalStates.eventMapScreen_EventId = eventId;
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<EventMapScreen>();
            }

            public static EventItem GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventItem>("Prefabs/UI/Overlays/EventOverlay/EventItem", parent);
            }
        }
    }
}