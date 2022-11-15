using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class SideQuestInfo : BaseQuestInfo
        {
            private TextMeshProUGUI title;
            private TextMeshProUGUI progress;

            private Image[] rewards = new Image[6];
            private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[6];

            private Button claimButton;
            private GameObject inProgress;
            private GameObject completed;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                var rewardWindow = canvas.Find("RewardWindow");

                title = rewardWindow.Find("Title").GetComponent<TextMeshProUGUI>();
                progress = rewardWindow.Find("Progress").GetComponent<TextMeshProUGUI>();

                var rewardGrid = rewardWindow.Find("RewardGrid");

                for (int i = 0; i < 6; i++)
                {
                    rewards[i] = rewardGrid.Find($"Reward{i + 1}").GetComponent<Image>();
                    rewardsAmount[i] = rewards[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                    rewards[i].gameObject.SetActive(false);
                }

                claimButton = rewardWindow.Find("ClaimButton").GetComponent<Button>();
                claimButton.onClick.AddListener(ClaimButtonClick);

                inProgress = rewardWindow.Find("InProgress").gameObject;
                completed = rewardWindow.Find("Completed").gameObject;
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (questData != null)
                {
                    title.text = questData.name;
                    
                    progress.text = questData.goalCount.HasValue
                        ? $"{questData?.progressCount}/{questData?.goalCount}" : "";
                    
                    for (int i = 0; i < questData.rewards?.Count; i++)
                    {
                        var reward = rewards[i];
                        var rewardAmount = rewardsAmount[i];
                        var questItem = questData.rewards[i];

                        reward.gameObject.SetActive(true);
                        reward.sprite = ResourceManager.LoadSprite(questItem.icon);
                        rewardAmount.text = questItem.amount.ToString();
                    }
                    
                    inProgress.SetActive(questData.inProgress);
                    claimButton.gameObject.SetActive(questData.isCompleted);
                    completed.SetActive(questData.isClaimed);
                }
            }
            
            private async void ClaimButtonClick()
            {
                await GameData.quests.ClaimReward(questId);
                UITools.ClaimRewards(questData?.rewards);
                Customize();
            }

            public static SideQuestInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SideQuestInfo>
                    ("Prefabs/UI/Overlays/QuestOverlay/SideQuestInfo", parent);
            }
        }
    }
}
