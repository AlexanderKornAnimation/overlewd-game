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
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);
            public int questId { get; set; }
            public AdminBRO.QuestItem questData =>
                GameData.GetQuestById(questId);

            private Transform canvas;
            private bool canvasActive = true;

            private Button mapButton;

            private TextMeshProUGUI eventName;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;
            private TextMeshProUGUI eventMark;

            private List<Image> rewards = new List<Image>();

            void Awake()
            {
                canvas = transform.Find("Canvas");
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
                var _eventData = eventData;
                var _questData = questData;

                eventName.text = _eventData?.name;
                title.text = _questData?.name;
                description.text = _questData?.description;

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
                eventData.SetAsMapEvent();
                UIManager.ShowScreen<EventMapScreen>();
            }

            public void SetCanvasActive(bool value)
            {
                if (value != canvasActive)
                {
                    canvasActive = value;
                    canvas.gameObject.SetActive(canvasActive);
                }
            }

            public static EventQuest GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventQuest>("Prefabs/UI/Overlays/EventOverlay/EventQuest", parent);
            }
        }
    }
}