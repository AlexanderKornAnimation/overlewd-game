using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class PrepareBattlePopup : BasePopup
    {
        protected const int RewardsCount = 3;

        protected Button backButton;
        protected Button battleButton;
        protected Button editTeamButton;
        protected Button buffButton;
        protected RectTransform buffRect;
        protected Transform enemyContent;
        protected Transform allyContent;

        protected Image firstTimeReward;

        protected Image[] rewards = new Image[RewardsCount];
        protected TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];
        protected TextMeshProUGUI firstTimeRewardCount;

        protected TextMeshProUGUI markers;
        protected AdminBRO.Battle battleData;

        private int stageId;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab(
                    "Prefabs/UI/Popups/PrepareBattlePopups/PrepareBattlePopup/PrepareBattlePopup",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var rewardsTr = canvas.Find("ResourceBack").Find("Rewards");
            var levelTitle = canvas.Find("LevelTitle");
            var buff = canvas.Find("Buff");
            var enemiesBack = canvas.Find("EnemiesBack");
            var alliesBack = canvas.Find("AlliesBack");

            enemyContent = enemiesBack.Find("Characters").Find("ScrollView").Find("Viewport").Find("Content");
            allyContent = alliesBack.Find("Characters");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            editTeamButton = canvas.Find("AlliesBack").Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);

            buffButton = buff.Find("SwitchBuffButton").GetComponent<Button>();
            buffRect = buff.GetComponent<RectTransform>();

            buffButton.onClick.AddListener(BuffButtonClick);
            buffRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                -buffRect.rect.height, buffRect.rect.height);

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
                    var enemyChar = NSPrepareBattlePopup.EnemyCharacter.GetInstance(enemyContent);
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
                rewards[i].sprite = reward.icon == null ? null : ResourceManager.LoadSprite(reward.icon);
                rewardsAmount[i].text = reward.amount.ToString();
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            battleData = GameData.GetBattleById(stageId);
            Customize();

            await Task.CompletedTask;
        }

        public PrepareBattlePopup SetData(int stageId)
        {
            this.stageId = stageId;
            return this;
        }

        protected virtual void BuffButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        protected virtual void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<TeamEditScreen>().
                SetData(new TeamEditScreenInData { eventMapStageId = stageId }).RunShowScreenProcess();
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual void BattleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
            UIManager.MakeScreen<BattleScreen>().SetData(stageId).RunShowScreenProcess();
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