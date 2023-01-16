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
        public class EventShortQuest : BaseQuest
        {
            private Button mapButton;
            private Button claimButton;
            private TextMeshProUGUI title;
            private TextMeshProUGUI quest;
            private TextMeshProUGUI progress;
            private TextMeshProUGUI marker;
            private GameObject inProgress;
            private GameObject completed;
            private Transform rewardGrid;

            void Awake()
            {
                canvas = transform.Find("Canvas");
                rewardGrid = canvas.Find("Grid");

                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                quest = canvas.Find("Quest").GetComponent<TextMeshProUGUI>();
                progress = canvas.Find("Progress").GetComponent<TextMeshProUGUI>();
                marker = canvas.Find("Marker").GetComponent<TextMeshProUGUI>();
                inProgress = canvas.Find("InProgress").gameObject;
                completed = canvas.Find("Completed").gameObject;

                mapButton = canvas.Find("MapButton").GetComponent<Button>();
                mapButton.onClick.AddListener(ToMapClick);

                claimButton = canvas.Find("ClaimButton").GetComponent<Button>();
                claimButton.onClick.AddListener(ClaimClick);
            }

            private void Start()
            {
                Customize();
            }

            protected override void Customize()
            {
                var _eventData = eventData;
                var _questData = questData;

                if (_eventData.isWeekly)
                {
                    mapButton.gameObject.SetActive(false);
                }
                else
                {
                    mapButton.gameObject.AddComponent<BlendPulseSelector>();
                }

                if (_questData != null)
                {
                    title.text = _eventData.name;
                    quest.text = _questData.name;
                    progress.text = $"{_questData.progressCount}/{_questData.goalCount}";
                    
                    completed.SetActive(_questData.isClaimed);
                    inProgress.SetActive(_questData.inProgress);
                    claimButton.gameObject.SetActive(_questData.isCompleted);
                    progress.gameObject.SetActive(!_questData.isClaimed);

                    var rewardIcons = rewardGrid.GetComponentsInChildren<Image>();
                    var rewardAmounts = rewardGrid.GetComponentsInChildren<TextMeshProUGUI>();

                    foreach (var reward in rewardIcons)
                    {
                        reward.gameObject.SetActive(false);
                    }
                    
                    for (int i = 0; i < _questData.rewards?.Count; i++)
                    {
                        if (i >= rewardIcons.Length)
                            break;
                        rewardIcons[i].gameObject.SetActive(true);
                        rewardIcons[i].sprite = ResourceManager.LoadSprite(_questData.rewards[i].icon);
                        rewardAmounts[i].text = _questData.rewards[i].amount.ToString();
                    }
                }
            }

            private void ToMapClick()
            {
                Destroy(mapButton.gameObject.GetComponent<Selector>());
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                eventData.SetAsMapEvent();
                UIManager.ShowScreen<EventMapScreen>();
            }

            public static EventShortQuest GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventShortQuest>(
                    "Prefabs/UI/Overlays/EventOverlay/EventShortQuest", parent);
            }
        }
    }
}