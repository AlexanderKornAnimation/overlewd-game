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
        public class EventQuest : MonoBehaviour
        {
            public int eventId { get; set; }
            public int questId { get; set; }

            private Button mapButton;

            private TextMeshProUGUI eventName;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;
            private TextMeshProUGUI eventMark;

            private List<Image> rewards = new List<Image>();

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                var backWithClock = canvas.Find("BackWithClock");

                eventName = canvas.Find("EventName").GetComponent<TextMeshProUGUI>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();
                eventMark = backWithClock.Find("EventMark").GetComponent<TextMeshProUGUI>();

                mapButton = canvas.Find("MapButton").GetComponent<Button>();
                mapButton.onClick.AddListener(ToMapClick);
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var eventData = GameData.GetEventById(eventId);
                var questData = GameData.GetQuestById(questId);

                eventName.text = eventData?.name;
                title.text = questData?.name;
                description.text = questData?.description;

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
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                GameGlobalStates.eventData = GameData.GetEventById(eventId);
                UIManager.ShowScreen<EventMapScreen>();
            }

            public static EventQuest GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventQuest>("Prefabs/UI/Overlays/EventOverlay/EventQuest", parent);
            }
        }
    }
}