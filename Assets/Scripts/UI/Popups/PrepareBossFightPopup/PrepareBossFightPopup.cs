using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBossFightPopup : BasePopup
    {
        protected const int RewardsCount = 3;

        protected Button backButton;
        protected Button battleButton;
        protected Button editTeamButton;
        protected Button buffButton;
        protected RectTransform buffRect;

        protected Image firstTimeReward;
        protected Image[] rewards = new Image[RewardsCount];
        protected TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];

        protected TextMeshProUGUI firstTimeRewardCount;

        protected TextMeshProUGUI markers;

        private int stageId;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/PrepareBossFightPopup/PrepareBossFightPopup",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var levelTitle = canvas.Find("LevelTitle");
            var rewardsTr = canvas.Find("ResourceBack").Find("Rewards");
            var alliesBack = canvas.Find("AlliesBack");
            var buff = canvas.Find("Buff");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            editTeamButton = alliesBack.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);

            buffButton = buff.Find("SwitchBuffButton").GetComponent<Button>();
            buffRect = buff.GetComponent<RectTransform>();

            buffButton.onClick.AddListener(BuffButtonClick);
            buffRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                -buffRect.rect.height, buffRect.rect.height);

            markers = levelTitle.Find("Markers").GetComponent<TextMeshProUGUI>();

            firstTimeReward = rewardsTr.Find("FirstTimeReward").GetComponent<Image>();
            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();

            for (int i = 0; i < rewards.Length; i++)
            {
                var reward = rewardsTr.Find("Reward" + i).GetComponent<Image>();
                rewards[i] = reward;

                var amount = reward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                rewardsAmount[i] = amount;
            }
        }

        protected virtual void Customize()
        {
            var battleData = GameData.GetBattleById(stageId);
            
            if (battleData.rewards.Count < 1 || battleData.firstRewards.Count < 1)
                return;
            
            var firstReward = battleData.firstRewards[0];

            firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
            firstTimeRewardCount.text = firstReward.amount.ToString();

            for (int i = 0; i < rewards.Length; i++)
            {
                var reward = battleData.rewards[i];
                rewards[i].sprite = ResourceManager.LoadSprite(reward.icon);
                rewardsAmount[i].text = reward.amount.ToString();
            }
        }
        
        public PrepareBossFightPopup SetData(int stageId)
        {
            this.stageId = stageId;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        protected virtual void EditTeamButtonClick()
        {
        }

        protected virtual void BuffButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual void BattleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
            UIManager.MakeScreen<BossFightScreen>().SetData(stageId).RunShowScreenProcess();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }

        public override async Task AfterShowAsync()
        {
            await UITools.TopShowAsync(buffRect, 0.2f);
        }

        public override async Task BeforeHideAsync()
        {
            await UITools.TopHideAsync(buffRect, 0.2f);
        }
    }
}