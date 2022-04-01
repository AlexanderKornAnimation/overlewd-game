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

        protected Image eventMark;
        protected Image questMark;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/PrepareBossFightPopup/PrepareBossFightPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var levelTitle = canvas.Find("LevelTitle");
            var rewards = canvas.Find("ResourceBack").Find("Rewards");
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

            eventMark = levelTitle.Find("EventMark").GetComponent<Image>();
            questMark = levelTitle.Find("QuestMark").GetComponent<Image>();
            
            firstTimeReward = rewards.Find("FirstTimeReward").GetComponent<Image>();
            reward1 = rewards.Find("Reward1").GetComponent<Image>();
            reward2 = rewards.Find("Reward2").GetComponent<Image>();
            reward3 = rewards.Find("Reward3").GetComponent<Image>();
            
            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            reward1Count = reward1.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            reward2Count = reward2.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            reward3Count = reward3.transform.Find("Count").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            Customize();
        }
        
        protected virtual void Customize()
        {
            // if (!GameGlobalStates.bossFight_EventStageData.battleId.HasValue)
            //     return;
            //
            // var battleData = GameData.GetBattleById(GameGlobalStates.bossFight_EventStageData.battleId.Value);
            // if (battleData.firstRewards == null || battleData.rewards == null)
            //     return;
            // if (battleData.firstRewards.Count < 1 || battleData.rewards.Count < 3)
            //     return;
            //
            // var firstIconURL = GameData.GetCurrencyById(battleData.firstRewards[0].currencyId).iconUrl;
            // firstTimeReward.sprite = ResourceManager.LoadSprite(firstIconURL);
            // firstTimeRewardCount.text = $"{battleData.firstRewards[0].amount}";
            //
            //
            // var icon1URL = GameData.GetCurrencyById(battleData.rewards[0].currencyId).iconUrl;
            // var icon2URL = GameData.GetCurrencyById(battleData.rewards[1].currencyId).iconUrl;
            // var icon3URL = GameData.GetCurrencyById(battleData.rewards[2].currencyId).iconUrl;
            //
            // reward1.sprite = ResourceManager.LoadSprite(icon1URL);
            // reward2.sprite = ResourceManager.LoadSprite(icon2URL);
            // reward3.sprite = ResourceManager.LoadSprite(icon3URL);
            // reward1Count.text = $"{battleData.rewards[0].amount}";
            // reward2Count.text = $"{battleData.rewards[1].amount}";
            // reward3Count.text = $"{battleData.rewards[2].amount}";
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
            UIManager.ShowScreen<BossFightScreen>();
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
