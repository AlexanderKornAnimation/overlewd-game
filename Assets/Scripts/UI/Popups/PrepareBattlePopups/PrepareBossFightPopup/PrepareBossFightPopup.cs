using System;
using System.Linq;
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
        protected Transform bossPos;
        protected Transform allyContent;

        protected Image firstTimeReward;
        protected Image[] rewards = new Image[RewardsCount];
        protected TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];

        protected TextMeshProUGUI firstTimeRewardCount;

        protected TextMeshProUGUI markers;
        protected AdminBRO.Battle battleData;

        protected PrepareBossFightPopupInData inputData;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/PrepareBattlePopups/PrepareBossFightPopup/PrepareBossFightPopup",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var levelTitle = canvas.Find("LevelTitle");
            var rewardsTr = canvas.Find("ResourceBack").Find("Rewards");
            var alliesBack = canvas.Find("AlliesBack");
            var buff = canvas.Find("Buff");
            var enemyBack = canvas.Find("EnemyBack");
            
            bossPos = enemyBack.Find("BossPos");
            allyContent = alliesBack.Find("Characters");
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            editTeamButton = alliesBack.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);

            buffButton = buff.Find("SwitchBuffButton").GetComponent<Button>();
            buffRect = buff.GetComponent<RectTransform>();

            buffButton.onClick.AddListener(BuffButtonClick);
            UITools.TopHide(buffRect);

            markers = levelTitle.Find("Markers").GetComponent<TextMeshProUGUI>();

            firstTimeReward = rewardsTr.Find("FirstTimeReward").GetComponent<Image>();
            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            firstTimeReward.gameObject.SetActive(false);

            for (int i = 0; i < rewards.Length; i++)
            {
                var reward = rewardsTr.Find("Reward" + i).GetComponent<Image>();
                rewards[i] = reward;
                rewards[i].gameObject.SetActive(false);

                var amount = reward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                rewardsAmount[i] = amount;
            }
        }

        protected void Customize()
        {
            foreach (var phase in battleData.battlePhases)
            {
                foreach (var enemy in phase.enemyCharacters)
                {
                    var enemyChar = NSPrepareBossFightPopup.Boss.GetInstance(bossPos);
                    enemyChar.characterData = enemy;
                }
            }
            
            var characters = GameData.characters.Where(ch => ch.teamPosition != AdminBRO.Character.TeamPosition_None);
            
            var overlordData = GameData.characters.Find(ch => ch.characterClass == AdminBRO.Character.Class_Overlord);

            var overlordInst = NSPrepareBattlePopup.AllyCharacter.GetInstance(allyContent);
            overlordInst.characterData = overlordData;
            
            foreach (var ally in characters)
            {
                var allyChar = NSPrepareBattlePopup.AllyCharacter.GetInstance(allyContent);
                allyChar.characterData = ally;
            }
            
            if (battleData.firstRewards.Count > 0)
            {
                var firstReward = battleData.firstRewards[0];

                firstTimeReward.gameObject.SetActive(true);
                firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
                firstTimeRewardCount.text = firstReward.amount.ToString();
            }

            if (battleData.rewards.Count < 1)
                return;

            for (int i = 0; i < battleData.rewards.Count; i++)
            {
                var reward = battleData.rewards[i];
                rewards[i].gameObject.SetActive(true);
                rewards[i].sprite = ResourceManager.LoadSprite(reward.icon);
                rewardsAmount[i].text = reward.amount.ToString();
            }
        }

        public PrepareBossFightPopup SetData(PrepareBossFightPopupInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            battleData = inputData.eventStageData.battleData;
            Customize();

            await Task.CompletedTask;
        }

        protected virtual void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<TeamEditScreen>().
                SetData(new TeamEditScreenInData 
                { 
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
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
            UIManager.MakeScreen<BossFightScreen>().
                SetData(new BossFightScreenInData
                {
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
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

    public class PrepareBossFightPopupInData : BaseScreenInData
    {
       
    }
}