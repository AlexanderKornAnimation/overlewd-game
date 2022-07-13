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
                GameData.quests.GetById(questId);

            private Transform canvas;
            private bool canvasActive = true;

            private Button mapButton;
            private Button claimButton;
            private Transform inProgress;
            private Transform complited;

            private TextMeshProUGUI eventName;
            private TextMeshProUGUI title;
            private TextMeshProUGUI progress;
            private TextMeshProUGUI description;
            private TextMeshProUGUI eventMark;
            private TextMeshProUGUI timer;

            private List<Transform> rewards = new List<Transform>();

            void Awake()
            {
                canvas = transform.Find("Canvas");
                var backWithClock = canvas.Find("BackWithClock");

                eventName = canvas.Find("EventName").GetComponent<TextMeshProUGUI>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                progress = canvas.Find("Progress").GetComponent<TextMeshProUGUI>();
                description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();
                eventMark = backWithClock.Find("EventMark").GetComponent<TextMeshProUGUI>();
                timer = backWithClock.Find("Timer").GetComponent<TextMeshProUGUI>();

                mapButton = canvas.Find("MapButton").GetComponent<Button>();
                mapButton.onClick.AddListener(ToMapClick);

                claimButton = canvas.Find("ClaimButton").GetComponent<Button>();
                claimButton.onClick.AddListener(ClaimClick);

                inProgress = canvas.Find("InProgress");
                complited = canvas.Find("Complited");

                for (int i = 1; i <= 5; i++)
                {
                    rewards.Add(backWithClock.Find($"Reward{i}"));
                }
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
                progress.text = $"{_questData?.progressCount}/{_questData?.goalCount ?? 0}";
                description.text = _questData?.description;

                foreach (var reward in rewards)
                {
                    reward.gameObject.SetActive(false);
                }

                var rId = 0;
                foreach (var rewardData in _questData.rewards)
                {
                    
                    var reward = rewards[rId];
                    rId++;
                    var rIcon = reward.GetComponent<Image>();
                    var rCount = reward.Find("Count").GetComponent<TextMeshProUGUI>();

                    rIcon.sprite = ResourceManager.LoadSprite(rewardData.tradableData?.icon);
                    rIcon.preserveAspect = true;
                    rCount.text = rewardData.amount?.ToString();
                    reward.gameObject.SetActive(true);
                }

                if (_eventData.isWeekly)
                {
                    mapButton.gameObject.SetActive(false);
                }
                else
                {
                    mapButton.gameObject.AddComponent<BlendPulseSelector>();
                }

                switch (_questData.status)
                {
                    case AdminBRO.QuestItem.Status_Open:
                        inProgress.gameObject.SetActive(false);
                        complited.gameObject.SetActive(false);
                        claimButton.gameObject.SetActive(false);
                        break;
                    case AdminBRO.QuestItem.Status_In_Progress:
                        inProgress.gameObject.SetActive(true);
                        complited.gameObject.SetActive(false);
                        claimButton.gameObject.SetActive(false);
                        break;
                    case AdminBRO.QuestItem.Status_Complete:
                        inProgress.gameObject.SetActive(false);
                        complited.gameObject.SetActive(false);
                        claimButton.gameObject.SetActive(true);
                        mapButton.gameObject.SetActive(false);
                        break;
                    case AdminBRO.QuestItem.Status_Rewards_Claimed:
                        progress.gameObject.SetActive(false);
                        inProgress.gameObject.SetActive(false);
                        complited.gameObject.SetActive(true);
                        claimButton.gameObject.SetActive(false);
                        mapButton.gameObject.SetActive(false);
                        break;
                }

                StopCoroutine("TimerUpd");
                StartCoroutine(TimerUpd());
            }

            private void ToMapClick()
            {
                Destroy(mapButton.gameObject.GetComponent<Selector>());
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                eventData.SetAsMapEvent();
                UIManager.ShowScreen<EventMapScreen>();
            }

            private async void ClaimClick()
            {
                await GameData.quests.ClaimReward(questId);
                Customize();
            }

            public void SetCanvasActive(bool value)
            {
                if (value != canvasActive)
                {
                    canvasActive = value;
                    canvas.gameObject.SetActive(canvasActive);
                }
            }

            private IEnumerator TimerUpd()
            {
                var dateEnd = eventData.dateEnd;
                var timeToEnd = TimeTools.AvailableTimeToString(dateEnd);
                while (!String.IsNullOrEmpty(timeToEnd))
                {
                    timer.text = timeToEnd;
                    yield return new WaitForSeconds(1.0f);
                    timeToEnd = TimeTools.AvailableTimeToString(dateEnd);
                }
            }

            public static EventQuest GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventQuest>("Prefabs/UI/Overlays/EventOverlay/EventQuest", parent);
            }
        }
    }
}