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
    public class PrepareBattlePopup : BasePopupParent<PrepareBattlePopupInData>
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

        private GameObject fastBattleAvailable;
        private Button fastBattleButton;
        private TextMeshProUGUI fastBattleText;
        private Button buttonPlus;
        private Button buttonMinus;
        private TextMeshProUGUI uiBattlesCount;
        
        private RectTransform buffRect;
        private Transform enemyContent;
        private TextMeshProUGUI enemyTeamPotency;
        private Transform allyContent;
        private TextMeshProUGUI allyTeamPotency;
        private TextMeshProUGUI userHpAmount;
        private TextMeshProUGUI userManaAmount;
        private TextMeshProUGUI userStaminaAmount;
        private TextMeshProUGUI userScrollAmount;

        private Image firstTimeReward;

        private Image[] rewards = new Image[RewardsCount];
        private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];
        private TextMeshProUGUI firstTimeRewardCount;
        private GameObject firstTimeRewardStatus;

        private TextMeshProUGUI markers;
        private TextMeshProUGUI stageTitle;
        private AdminBRO.Battle battleData;

        private int battlesCount => int.Parse(uiBattlesCount.text);
        private int energyCost => inputData?.energyCost ?? 0;
        private int replayCost => inputData?.replayCost ?? 0;
        private (int scrollCost, int energyCost) fastBattleCost => (battlesCount * replayCost, battlesCount * energyCost);

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab(
                    "Prefabs/UI/Popups/PrepareBattlePopups/PrepareBattlePopup/PrepareBattlePopup",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");
            var rewardsTr = canvas.Find("ResourceBack").Find("Rewards");
            var levelTitle = canvas.Find("LevelTitle");
            var buff = canvas.Find("Buff");
            var enemiesBack = canvas.Find("CharactersBackground").Find("Enemies");
            var bottlePanel = canvas.Find("BottlePanel");

            staminaBuyButton = bottlePanel.Find("Stamina").Find("BuyButton").GetComponent<Button>();
            staminaBuyButton.onClick.AddListener(PotionBuyButtonClick);

            manaBuyButton = bottlePanel.Find("Mana").Find("BuyButton").GetComponent<Button>();
            manaBuyButton.onClick.AddListener(PotionBuyButtonClick);

            healthBuyButton = bottlePanel.Find("Health").Find("BuyButton").GetComponent<Button>();
            healthBuyButton.onClick.AddListener(PotionBuyButtonClick);

            scrollBuyButton = bottlePanel.Find("Scroll").Find("BuyButton").GetComponent<Button>();
            scrollBuyButton.onClick.AddListener(PotionBuyButtonClick);
            
            enemyContent = enemiesBack.Find("Viewport").Find("Content");
            allyContent = canvas.Find("CharactersBackground").Find("Allies");

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
            uiBattlesCount = substrateCounter.Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
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

            enemyTeamPotency = canvas.Find("CharactersBackground").Find("EnemiesHeadline").Find("PotencyBack")
                .Find("Potency").GetComponent<TextMeshProUGUI>();
            allyTeamPotency = canvas.Find("CharactersBackground").Find("AlliesHeadline").Find("PotencyBack")
                .Find("Potency").GetComponent<TextMeshProUGUI>();
            
            buffButton.onClick.AddListener(BuffButtonClick);
            UITools.RightHide(buffRect);

            stageTitle = levelTitle.Find("Title").GetComponent<TextMeshProUGUI>();
            markers = stageTitle.transform.Find("Markers").GetComponent<TextMeshProUGUI>();

            firstTimeReward = rewardsTr.Find("FirstTimeReward").GetComponent<Image>();
            firstTimeRewardCount = firstTimeReward.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            firstTimeRewardStatus = firstTimeReward.transform.Find("ClaimStatus").gameObject;
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

        private void Customize()
        {
            var enemyPotency = 0;
            foreach (var phase in battleData.battlePhases)
            {
                foreach (var enemy in phase.enemyCharacters)
                {
                    var enemyChar = NSPrepareBattlePopup.EnemyCharacter.GetInstance(enemyContent);
                    enemyChar.characterData = enemy;
                    enemyChar.widgetPos = transform;
                    
                    if (enemy != null)
                    {
                        enemyPotency += enemy.potency;
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
                allyChar.widgetPos = transform;
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
                rewards[i].sprite = reward.icon == null ? null : ResourceManager.LoadSprite(reward.icon);
                rewardsAmount[i].text = reward.amount.ToString();
            }

            if (isStageComplete.HasValue)
                fastBattleAvailable.SetActive(isStageComplete.Value);
            
            userHpAmount.text = GameData.player.hpPotionAmount.ToString();
            userManaAmount.text = GameData.player.manaPotionAmount.ToString();
            userStaminaAmount.text = GameData.player.energyPoints + "/" + GameData.potions.baseEnergyVolume;
            userScrollAmount.text = GameData.player.replayAmount.ToString();
            allyTeamPotency.text = GameData.characters.myTeamPotency.ToString();
            stageTitle.text = battleData.title;
            
            battleButtonText.text =
                $"Make them suffer\nwith <size=40>{TMPSprite.Energy}</size> {energyCost} energy!";
            fastBattleText.text =
                $"Give an order to hunt\nfor <size=40>{TMPSprite.Energy}</size> {fastBattleCost.energyCost} and {TMPSprite.Scroll} {fastBattleCost.scrollCost}";
            CheckButtonState();

            CustomizeBuff();

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                    if (!GameData.ftue.chapter2_battle1.isComplete)
                    {
                        UITools.DisableButton(battleButton, GameData.characters.myTeamCharacters.Count < 2);
                    }
                    break;
            }
        }

        public override async Task BeforeShowDataAsync()
        {
            await GameData.player.Get();

            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            battleData = inputData.eventStageData?.battleData ?? inputData.ftueStageData?.battleData;
            Customize();

            switch (inputData?.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, _):
                    UITools.DisableButton(editTeamButton);
                    break;
            }

            StartCoroutine(GameData.player.UpdLocalEnergyPoints(RefreshEnergy));

            await Task.CompletedTask;
        }

        private async void FastBattleButtonClick()
        {            
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            bool canPlayFastBattle = GameData.player.energyPoints >= fastBattleCost.energyCost &&
                GameData.player.replayAmount >= fastBattleCost.scrollCost;
            
            if (canPlayFastBattle)
            {
                if (inputData.ftueStageId.HasValue)
                {
                    await GameData.ftue.ReplayStage(inputData.ftueStageId.Value, battlesCount);
                }
                else if (inputData.eventStageId.HasValue)
                {
                    await GameData.events.StageReplay(inputData.eventStageId.Value, battlesCount);
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
            Inc();
        }
        
        private void MinusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Dec();
        }

        private void Inc()
        {
            uiBattlesCount.text = (battlesCount + 1).ToString();
            CheckButtonState();
        }

        private void Dec()
        {
            uiBattlesCount.text = (battlesCount - 1).ToString();
            CheckButtonState();
        }

        private void CheckButtonState()
        {
            fastBattleText.text =
                $"Give an order to hunt\nfor {TMPSprite.Energy} {fastBattleCost.energyCost} and {TMPSprite.Scroll} {fastBattleCost.scrollCost}";
            buttonPlus.gameObject.SetActive(battlesCount < 5);
            buttonMinus.gameObject.SetActive(battlesCount > 1);
        }
        

        
        private void PotionBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowPopup<BottlesPopup>();
        }
        
        private void BuffButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<TeamEditScreen>();
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
                UIManager.MakeScreen<BattleScreen>().
                    SetData(new BaseBattleScreenInData
                    {
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).DoShow();
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
            await UITools.RightShowAsync(buffRect, 0.2f);
        }

        public override async void OnUIEvent(UIEvent eventData)
        {
            switch (eventData.type)
            {
                case UIEvent.Type.ChangeScreenComplete:
                    switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
                    {
                        case (FTUE.CHAPTER_3, FTUE.SEX_1):
                            if (!UIManager.currentState.prevState.ScreenTypeIs<OverlordScreen>())
                            {   
                                GameData.ftue.chapter3.ShowNotifByKey("ch3overequiptutor1");
                                await UIManager.WaitHideNotifications();
                                GameData.ftue.chapter3.ShowNotifByKey("ch3overequiptutor1");
                                await UIManager.WaitHideNotifications();
                                UIManager.ShowScreen<OverlordScreen>();
                            }
                            break;
                    }
                    break;
            }
        }

        public override async Task BeforeHideAsync()
        {
            await UITools.RightHideAsync(buffRect, 0.2f);
        }

        private void RefreshEnergy()
        {
            userStaminaAmount.text = GameData.player.energyPoints + "/" + GameData.potions.baseEnergyVolume;
        }

        private void CustomizeBuff()
        {
            var iconUlvi = buffRect.Find("IconUlvi");
            var iconAdriel = buffRect.Find("IconAdriel");
            var iconIngie = buffRect.Find("IconIngie");
            var iconLili = buffRect.Find("IconLili");
            var iconFaye = buffRect.Find("IconFaye");
            var descr = buffRect.Find("Description").GetComponent<TextMeshProUGUI>();
            var title = buffRect.Find("Title").GetComponent<TextMeshProUGUI>();
            var icon = buffRect.Find("Icon").GetComponent<Image>();

            iconUlvi.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isUlvi ?? false);
            iconAdriel.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isAdriel ?? false);
            iconIngie.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isIngie ?? false);
            iconLili.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isLili ?? false);
            iconFaye.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isFaye ?? false);
            icon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.activeBuff?.icon);
            title.text = GameData.matriarchs.activeBuff?.name;
            descr.text = UITools.ChangeTextSize(GameData.matriarchs.activeBuff?.description, descr.fontSize);
        }
    }

    public class PrepareBattlePopupInData : BasePrepareBattlePopupInData
    {
        
    }
}