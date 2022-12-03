using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryPopup : BasePopupParent<VictoryPopupInData>
    {
        private Transform grid;

        private Button nextButton;
        private Button repeatButton;
        private Image firstTimeReward;
        private TextMeshProUGUI firstTimeRewardAmount;
        private GameObject firstTimeRewardStatus;
        private Image[] rewards = new Image[RewardsCount];
        private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];
        private const int RewardsCount = 3;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/VictoryPopup/VictoryPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            grid = canvas.Find("Grid");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            firstTimeReward = grid.Find("FirstTimeReward").GetComponent<Image>();
            firstTimeRewardAmount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            firstTimeRewardStatus = firstTimeReward.transform.Find("ClaimStatus").gameObject;
            firstTimeRewardStatus.SetActive(false);

            for (int i = 0; i < RewardsCount; i++)
            {
                rewards[i] = grid.Find($"Reward{i + 1}").GetComponent<Image>();
                rewardsAmount[i] = rewards[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                rewards[i].gameObject.SetActive(false);
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            var battleData = inputData?.ftueStageData?.battleData ?? inputData?.eventStageData?.battleData;

            var firstReward = battleData?.firstRewards?.FirstOrDefault();

            firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward?.icon);
            firstTimeRewardAmount.text = firstReward?.amount.ToString();
            var isStageComplete = inputData?.ftueStageData?.isComplete ?? inputData?.eventStageData?.isComplete;
            
            if (isStageComplete.HasValue)
            {
                firstTimeRewardStatus.SetActive(isStageComplete.Value);
                firstTimeReward.color = isStageComplete.Value ? Color.gray : Color.white;

            }
            
            for (int i = 0; i < battleData?.rewards?.Count; i++)
            {
                var reward = battleData.rewards[i];
                rewards[i].sprite = ResourceManager.LoadSprite(reward.icon);
                rewardsAmount[i].text = reward.amount.ToString();
                rewards[i].gameObject.SetActive(true);
            }

            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_1):
                    UITools.DisableButton(repeatButton);
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Winning_a_battle);
                    break;
                
                case (FTUE.CHAPTER_2, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Winning_a_battle);
                    break;
                case (FTUE.CHAPTER_3, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Inge_Winning_a_battle);
                    break;
            }

            await Task.CompletedTask;
        }

        public override BaseMissclick MakeMissclick()
        {
            var missClick = UIManager.MakePopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
            return missClick;
        }

        private void NextButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_4):
                    UIManager.ShowScreen<CastleScreen>();
                    break;
                default:
                    if (inputData.ftueStageId.HasValue)
                    {
                        UIManager.ShowScreen<MapScreen>();
                    }
                    else if (inputData.eventStageId.HasValue)
                    {
                        UIManager.ShowScreen<EventMapScreen>();
                    }
                    break;
            }
        }

        private void RepeatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<BattleScreen>().
                SetData(new BaseBattleScreenInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).DoShow();
        }
    }

    public class VictoryPopupInData : BasePopupInData
    {
        
    }
}