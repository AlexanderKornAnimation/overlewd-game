using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    
    public class PrepareBattlePopup : BasePopup
    {
        
        protected Button backButton;
        protected Button battleButton;
        protected Button prepareButton;

        protected Image firstTimeReward;
        protected Image reward1;
        protected Image reward2;
        protected Image reward3;
        protected TextMeshProUGUI firstTimeRewardCount;
        protected TextMeshProUGUI reward1Count;
        protected TextMeshProUGUI reward2Count;
        protected TextMeshProUGUI reward3Count;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/PrepareBattlePopup/PrepareBattlePopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            prepareButton = canvas.Find("PrepareBattleButton").GetComponent<Button>();
            prepareButton.onClick.AddListener(PrepareButtonClick);
            
            firstTimeReward = canvas.Find("FirstTimeReward").Find("Resource").GetComponent<Image>();
            reward1 = canvas.Find("Reward1").Find("Resource").GetComponent<Image>();
            reward2 = canvas.Find("Reward2").Find("Resource").GetComponent<Image>();
            reward3 = canvas.Find("Reward3").Find("Resource").GetComponent<Image>();
            firstTimeRewardCount = canvas.Find("FirstTimeReward").Find("Count").GetComponent<TextMeshProUGUI>();
            reward1Count = canvas.Find("Reward1").Find("Count").GetComponent<TextMeshProUGUI>();
            reward2Count = canvas.Find("Reward2").Find("Count").GetComponent<TextMeshProUGUI>();
            reward3Count = canvas.Find("Reward3").Find("Count").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            if (!GameGlobalStates.battle_EventStageData.battleId.HasValue)
                return;

            var battleData = GameData.GetBattleById(GameGlobalStates.battle_EventStageData.battleId.Value);
            if (battleData.firstRewards == null || battleData.rewards == null)
                return;
            if (battleData.firstRewards.Count < 1 || battleData.rewards.Count < 3)
                return;

            var firstIconURL = GameData.GetCurrencyById(battleData.firstRewards[0].currencyId).iconUrl;
            firstTimeReward.sprite = ResourceManager.LoadSprite(firstIconURL);
            firstTimeRewardCount.text = $"{battleData.firstRewards[0].amount}";


            var icon1URL = GameData.GetCurrencyById(battleData.rewards[0].currencyId).iconUrl;
            var icon2URL = GameData.GetCurrencyById(battleData.rewards[1].currencyId).iconUrl;
            var icon3URL = GameData.GetCurrencyById(battleData.rewards[2].currencyId).iconUrl;

            reward1.sprite = ResourceManager.LoadSprite(icon1URL);
            reward2.sprite = ResourceManager.LoadSprite(icon2URL);
            reward3.sprite = ResourceManager.LoadSprite(icon3URL);
            reward1Count.text = $"{battleData.rewards[0].amount}";
            reward2Count.text = $"{battleData.rewards[1].amount}";
            reward3Count.text = $"{battleData.rewards[2].amount}";
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual void BattleButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowScreen<BattleScreen>();
        }

        protected virtual void PrepareButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowSubPopup<BottlesSubPopup>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }
    }

}
