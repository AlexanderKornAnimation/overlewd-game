using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class PrepareBattlePopup : BasePopup
    {
        protected Button backButton;
        protected Button battleButton;
        protected Button editTeamButton;
        protected Button buffButton;
        protected RectTransform buffRect;

        protected Image firstTimeReward;
        protected Image reward1;
        protected Image reward2;
        protected Image reward3;
        protected TextMeshProUGUI firstTimeRewardCount;
        protected TextMeshProUGUI reward1Count;
        protected TextMeshProUGUI reward2Count;
        protected TextMeshProUGUI reward3Count;

        protected TextMeshProUGUI markers;

        private int stageId;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/PrepareBattlePopup/PrepareBattlePopup",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var rewards = canvas.Find("ResourceBack").Find("Rewards");
            var levelTitle = canvas.Find("LevelTitle");
            var buff = canvas.Find("Buff");

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
            
            firstTimeReward = rewards.Find("FirstTimeReward").GetComponent<Image>();
            reward1 = rewards.Find("Reward1").GetComponent<Image>();
            reward2 = rewards.Find("Reward2").GetComponent<Image>();
            reward3 = rewards.Find("Reward3").GetComponent<Image>();

            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            reward1Count = reward1.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            reward2Count = reward2.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            reward3Count = reward3.transform.Find("Count").GetComponent<TextMeshProUGUI>();
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
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual void BattleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
            UIManager.MakeScreen<BattleScreen>().
                SetData(stageId).RunShowScreenProcess();
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