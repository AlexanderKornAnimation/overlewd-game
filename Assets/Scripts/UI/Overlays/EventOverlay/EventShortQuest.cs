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
            private const int rewardsCount = 6;
            private Image[] rewards = new Image[rewardsCount];
            private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[rewardsCount];

            private Button mapButton;
            private Button claimButton;
            private TextMeshProUGUI title;
            private TextMeshProUGUI quest;
            private TextMeshProUGUI progress;
            private TextMeshProUGUI marker;
            private GameObject inProgress;
            private GameObject completed;

            private void Awake()
            {
                canvas = transform.Find("Canvas");
                var grid = canvas.Find("Grid");

                claimButton = canvas.Find("ClaimButton").GetComponent<Button>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                quest = canvas.Find("Quest").GetComponent<TextMeshProUGUI>();
                progress = canvas.Find("Progress").GetComponent<TextMeshProUGUI>();
                marker = canvas.Find("Marker").GetComponent<TextMeshProUGUI>();
                inProgress = canvas.Find("InProgress").gameObject;
                completed = canvas.Find("Completed").gameObject;

                mapButton = canvas.Find("MapButton").GetComponent<Button>();
                mapButton.onClick.AddListener(ToMapClick);

                for (int i = 0; i < rewardsCount; i++)
                {
                    rewards[i] = grid.Find($"Reward{i + 1}").GetComponent<Image>();
                    rewardsAmount[i] = rewards[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                    rewards[i].gameObject.SetActive(false);
                }
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
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
                    
                    for (int i = 0; i < _questData.rewards?.Count; i++)
                    {
                        rewards[i].gameObject.SetActive(true);
                        rewards[i].sprite = ResourceManager.LoadSprite(_questData.rewards[i].icon);
                        rewardsAmount[i].text = _questData.rewards[i].amount.ToString();
                    }
                }
            }

            protected override void ClaimClick()
            {
                base.ClaimClick();
                Customize();
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