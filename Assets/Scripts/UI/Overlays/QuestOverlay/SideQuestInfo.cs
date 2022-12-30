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
            private GameObject trustRewardBack;
            private TextMeshProUGUI trustPoints;

            private Button claimButton;
            private GameObject inProgress;
            private GameObject completed;
            private Button advanceButton;
            private Transform rewardsGrid;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                var rewardWindow = canvas.Find("RewardWindow");

                title = rewardWindow.Find("Title").GetComponent<TextMeshProUGUI>();
                progress = rewardWindow.Find("Progress").GetComponent<TextMeshProUGUI>();
                trustRewardBack = rewardWindow.Find("TrustRewardBack").gameObject;
                trustPoints = trustRewardBack.transform.Find("Trust").GetComponent<TextMeshProUGUI>();

                rewardsGrid = rewardWindow.Find("RewardGrid");

                claimButton = rewardWindow.Find("ClaimButton").GetComponent<Button>();
                claimButton.onClick.AddListener(ClaimButtonClick);

                inProgress = rewardWindow.Find("InProgress").gameObject;
                completed = rewardWindow.Find("Completed").gameObject;

                advanceButton = rewardWindow.Find("AdvanceButton").GetComponent<Button>();
                advanceButton.onClick.AddListener(AdvanceButtonClick);
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

                    var rewardIcons = rewardsGrid.GetComponentsInChildren<Image>();
                    var rewardAmounts = rewardsGrid.GetComponentsInChildren<TextMeshProUGUI>();

                    foreach (var reward in rewardIcons)
                    {
                        reward.gameObject.SetActive(false);
                    }
                    
                    for (int i = 0; i < questData.rewards?.Count; i++)
                    {
                        if (i >= rewardIcons.Length)
                            break;
                        
                        var questItem = questData.rewards[i];

                        rewardIcons[i].gameObject.SetActive(true);
                        rewardIcons[i].sprite = ResourceManager.LoadSprite(questItem.icon);
                        rewardAmounts[i].text = questItem.amount.ToString();
                    }
                    
                    trustRewardBack.SetActive(questData.matriarchEmpathyPointsReward.HasValue);
                    trustPoints.text = UITools.IncNumberSize("Trust +" + questData.matriarchEmpathyPointsReward, 
                        trustPoints.fontSize);
                    inProgress.SetActive(false);
                    advanceButton.gameObject.SetActive(questData.isOpen || questData.inProgress);
                    claimButton.gameObject.SetActive(questData.isCompleted);
                    completed.SetActive(questData.isClaimed);
                }
            }
            
            private async void ClaimButtonClick()
            {
                await GameData.quests.ClaimReward(questData?.id);
                Customize();
                UITools.ClaimRewardsAsync(questData?.rewards);
                questContentScrollView.questButton.Remove();
            }

            private void AdvanceToScreen<T, TData>() where T : BaseFullScreenParent<TData>
                                                     where TData : BaseFullScreenInData, new()
            {
                if (UIManager.HasScreen<T>())
                {
                    UIManager.HideOverlay();
                    return;
                }

                UIManager.MakeScreen<T>().
                    SetData(new TData
                    {
                        questId = questId
                    }).DoShow();
            }

            private void AdvanceButtonClick()
            {
                switch (questData.screenTarget)
                {
                    case AdminBRO.QuestItem.ScreenTarget_BattleGirls:
                        AdvanceToScreen<BattleGirlScreen, BattleGirlScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_Forge:
                        AdvanceToScreen<ForgeScreen, ForgeScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_GuestRoom:
                       
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_Harem:
                        AdvanceToScreen<HaremScreen, HaremScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_Laboratory:
                        AdvanceToScreen<LaboratoryScreen, LaboratoryScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_MagicGuild:
                        AdvanceToScreen<MagicGuildScreen, MagicGuildScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_Map:
                        AdvanceToScreen<MapScreen, MapScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_Municipality:
                        AdvanceToScreen<MunicipalityScreen, MunicipalityScreenInData>();
                        break;
                    case AdminBRO.QuestItem.ScreenTarget_Portal:
                        AdvanceToScreen<PortalScreen, PortalScreenInData>();
                        break;
                }
            }

            public static SideQuestInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SideQuestInfo>
                    ("Prefabs/UI/Overlays/QuestOverlay/SideQuestInfo", parent);
            }
        }
    }
}
