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
                if (questData != null)
                {
                    title.text = eventData.name;
                    quest.text = questData.name;
                    progress.text = $"{questData.progressCount}/{questData.goalCount}";
                    
                    completed.SetActive(questData.isClaimed);
                    inProgress.SetActive(questData.inProgress);
                    claimButton.gameObject.SetActive(questData.isCompleted);
                    progress.gameObject.SetActive(!questData.isClaimed);
                    
                    for (int i = 0; i < questData.rewards?.Count; i++)
                    {
                        rewards[i].gameObject.SetActive(true);
                        rewards[i].sprite = ResourceManager.LoadSprite(questData.rewards[i].icon);
                        rewardsAmount[i].text = questData.rewards[i].amount.ToString();
                    }
                }
            }

            protected override void ClaimClick()
            {
                base.ClaimClick();
                Customize();
            }
            
            public static EventShortQuest GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventShortQuest>(
                    "Prefabs/UI/Overlays/EventOverlay/EventShortQuest", parent);
            }
        }
    }
}