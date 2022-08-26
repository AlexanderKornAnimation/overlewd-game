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
    public class PrepareBossFightPopup : BasePopupParent<PrepareBossFightPopupInData>
    {
        private const int RewardsCount = 3;

        private Button backButton;
        private Button battleButton;
        private TextMeshProUGUI battleButtonText;
        private Button editTeamButton;
        private Button buffButton;
        private Button staminaBuyButton;
        private Button manaBuyButton;
        private Button healthBuyButton;
        private Button scrollBuyButton;

        private RectTransform buffRect;
        private Transform bossPos;
        private TextMeshProUGUI enemyTeamPotency;
        private Transform allyContent;
        private TextMeshProUGUI userHpAmount;
        private TextMeshProUGUI userManaAmount;
        private TextMeshProUGUI userStaminaAmount;
        private TextMeshProUGUI userScrollAmount;
        private TextMeshProUGUI allyTeamPotency;
        
        private GameObject fastBattleAvailable;
        private Button fastBattleButton;
        private TextMeshProUGUI fastBattleText;
        private Button buttonPlus;
        private Button buttonMinus;
        private TextMeshProUGUI uiScrollCount;
        private int scrollAmount = 1;


        private Image[] rewards = new Image[RewardsCount];
        private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];

        private Image firstTimeReward;
        private TextMeshProUGUI firstTimeRewardCount;
        private GameObject firstTimeRewardStatus;

        private TextMeshProUGUI markers;
        private AdminBRO.Battle battleData;

        private int energyCost => inputData?.energyCost ?? 0;
        private int replayCost => inputData?.replayCost ?? 0;

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
            var charactersBack = canvas.Find("CharactersBackground");
            var bottlePanel = canvas.Find("BottlePanel");

            staminaBuyButton = bottlePanel.Find("Stamina").Find("BuyButton").GetComponent<Button>();
            staminaBuyButton.onClick.AddListener(PotionBuyButtonClick);

            manaBuyButton = bottlePanel.Find("Mana").Find("BuyButton").GetComponent<Button>();
            manaBuyButton.onClick.AddListener(PotionBuyButtonClick);

            healthBuyButton = bottlePanel.Find("Health").Find("BuyButton").GetComponent<Button>();
            healthBuyButton.onClick.AddListener(PotionBuyButtonClick);
            
            scrollBuyButton = bottlePanel.Find("Scroll").Find("BuyButton").GetComponent<Button>();
            scrollBuyButton.onClick.AddListener(ScrollBuyButtonClick);
            
            bossPos = charactersBack.Find("BossPos");
            allyContent = charactersBack.Find("AllyCharacters");
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);
            battleButtonText = battleButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            editTeamButton = canvas.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);

            buffButton = buff.Find("SwitchBuffButton").GetComponent<Button>();
            buffRect = buff.GetComponent<RectTransform>();

            var fastBattle = canvas.Find("FastBattle");
            fastBattleAvailable = fastBattle.Find("Available").gameObject;
            var substrateCounter = fastBattleAvailable.transform.Find("SubstrateCounter");
            uiScrollCount = substrateCounter.Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
            buttonPlus = substrateCounter.Find("ButtonPlus").GetComponent<Button>();
            buttonPlus.onClick.AddListener(PlusButtonClick);
            buttonMinus = substrateCounter.Find("ButtonMinus").GetComponent<Button>();
            buttonMinus.onClick.AddListener(MinusButtonClick);
            fastBattleButton = fastBattleAvailable.transform.Find("Button").GetComponent<Button>();
            fastBattleButton.onClick.AddListener(FastBattleButtonClick);
            fastBattleText = fastBattleButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            userHpAmount = bottlePanel.Find("Health").Find("Value").GetComponent<TextMeshProUGUI>();
            userManaAmount = bottlePanel.Find("Mana").Find("Value").GetComponent<TextMeshProUGUI>();
            userStaminaAmount = bottlePanel.Find("Stamina").Find("Value").GetComponent<TextMeshProUGUI>();
            userScrollAmount = bottlePanel.Find("Scroll").Find("Value").GetComponent<TextMeshProUGUI>();

            buffButton.onClick.AddListener(BuffButtonClick);
            UITools.TopHide(buffRect);

            markers = levelTitle.Find("Markers").GetComponent<TextMeshProUGUI>();

            firstTimeReward = rewardsTr.Find("FirstTimeReward").GetComponent<Image>();
            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            firstTimeRewardStatus = firstTimeReward.transform.Find("ClaimStatus").gameObject;
            firstTimeReward.gameObject.SetActive(false);

            enemyTeamPotency = canvas.Find("CharactersBackground").Find("EnemiesHeadline").Find("PotencyBack")
                .Find("Potency").GetComponent<TextMeshProUGUI>();
            allyTeamPotency = canvas.Find("CharactersBackground").Find("AlliesHeadline").Find("PotencyBack")
                .Find("Potency").GetComponent<TextMeshProUGUI>();
            
            for (int i = 0; i < rewards.Length; i++)
            {
                var reward = rewardsTr.Find("Reward" + i).GetComponent<Image>();
                rewards[i] = reward;
                rewards[i].gameObject.SetActive(false);

                var amount = reward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                rewardsAmount[i] = amount;
            }
        }

        private void Customize()
        {
            float enemyPotency = 0;
            foreach (var phase in battleData.battlePhases)
            {
                foreach (var enemy in phase.enemyCharacters)
                {
                    var enemyChar = NSPrepareBossFightPopup.Boss.GetInstance(bossPos);
                    enemyChar.characterData = enemy;
                    
                    if (enemy != null)
                    {
                        if (enemy.potency.HasValue)
                        {
                            enemyPotency += enemy.potency.Value;
                        }
                    }
                }
            }
            
            enemyTeamPotency.text = enemyPotency.ToString();

            var characters = GameData.characters.myTeamCharacters;
            var overlordData = GameData.characters.overlord;

            var overlordInst = NSPrepareBattlePopup.AllyCharacter.GetInstance(allyContent);
            overlordInst.characterData = overlordData;
            
            foreach (var ally in characters)
            {
                var allyChar = NSPrepareBattlePopup.AllyCharacter.GetInstance(allyContent);
                allyChar.characterData = ally;
            }
            
            var isStageComplete = inputData?.ftueStageData?.isComplete ?? inputData?.eventStageData.isComplete;
            if (battleData.firstRewards.Count > 0)
            {
                var firstReward = battleData.firstRewards[0];

                firstTimeReward.gameObject.SetActive(true);
                firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
                firstTimeRewardCount.text = firstReward.amount.ToString();
                
                if (isStageComplete.HasValue)
                {
                    firstTimeRewardStatus.SetActive(isStageComplete.Value);
                    firstTimeReward.color = isStageComplete.Value ? Color.gray : Color.white;
                }
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

            userHpAmount.text = GameData.player.hpAmount.ToString();
            userManaAmount.text = GameData.player.manaAmount.ToString();
            userStaminaAmount.text = GameData.player.energyPoints + "/120";
            allyTeamPotency.text = GameData.characters.myTeamPotency.ToString();
            
            battleButtonText.text =
                $"Make them suffer\nwith <size=40>{AdminBRO.PlayerInfo.Sprite_Energy}</size> {energyCost} energy!";
        }

        public override async Task BeforeShowMakeAsync()
        {
            battleData = inputData.eventStageData?.battleData ?? inputData.ftueStageData?.battleData;
            Customize();

            switch (inputData.ftueStageData?.ftueState)
            {
                case (_, "chapter1"):
                    UITools.DisableButton(editTeamButton);
                    break;
            }

            await Task.CompletedTask;
        }

        private async void FastBattleButtonClick()
        {            
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (GameData.player.energyPoints >= energyCost)
            {
                if (inputData.ftueStageId.HasValue)
                {
                    await GameData.ftue.ReplayStage(inputData.ftueStageId.Value, scrollAmount);
                }
                else if (inputData.eventStageId.HasValue)
                {
                    await GameData.events.StageReplay(inputData.eventStageId.Value, scrollAmount);
                }
                    
                UIManager.HidePopup();
            }
            else
            {
                UIManager.ShowPopup<BottlesPopup>();
            }
        }
        
        private void PlusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            scrollAmount++;
            fastBattleText.text =
                $"Give an order to hunt\nfor {AdminBRO.PlayerInfo.Sprite_Energy} {energyCost * scrollAmount} and {AdminBRO.PlayerInfo.Sprite_Scroll} {scrollAmount}";
            uiScrollCount.text = scrollAmount.ToString();
        }
        
        private void MinusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            scrollAmount--;
            
            if (scrollAmount < 1)
                scrollAmount = 1;

            fastBattleText.text =
                $"Give an order to hunt\nfor {AdminBRO.PlayerInfo.Sprite_Energy} {energyCost * scrollAmount} and {AdminBRO.PlayerInfo.Sprite_Scroll} {scrollAmount}";
            
            uiScrollCount.text = scrollAmount.ToString();
        }

        private void ScrollBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BottlesPopup>().
                SetData(new BottlesPopupInData
                {
                    prevPopupInData = inputData
                }).RunShowPopupProcess();
        }

        
        private void PotionBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakePopup<BottlesPopup>().
                SetData(new BottlesPopupInData
                {
                    prevPopupInData = inputData
                }).RunShowPopupProcess();
        }
        
        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<TeamEditScreen>().
                SetData(new TeamEditScreenInData 
                {
                    prevScreenInData = UIManager.prevScreenInData,
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
        }

        private void BuffButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<HaremScreen>().
                SetData(new HaremScreenInData
                {
                    prevScreenInData = UIManager.prevScreenInData,
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        private void BattleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
            if (GameData.player.energyPoints >= energyCost)
            {
                UIManager.MakeScreen<BossFightScreen>().
                    SetData(new BaseBattleScreenInData
                    {
                        prevScreenInData = UIManager.prevScreenInData,
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).RunShowScreenProcess();
            }
            else
            {
                UIManager.ShowPopup<BottlesPopup>();
            }
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

            //ftue part
            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    GameData.ftue.info.chapter1.ShowNotifByKey("potionstutor1");
                    break;
            }
        }

        public override async Task BeforeHideAsync()
        {
            await UITools.TopHideAsync(buffRect, 0.2f);
        }
    }

    public class PrepareBossFightPopupInData : BasePrepareBattlePopupInData
    {
       
    }
}