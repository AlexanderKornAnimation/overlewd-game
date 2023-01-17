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
        private TextMeshProUGUI uiBattlesCount;

        private Image[] rewards = new Image[RewardsCount];
        private TextMeshProUGUI[] rewardsAmount = new TextMeshProUGUI[RewardsCount];

        private Image firstTimeReward;
        private TextMeshProUGUI firstTimeRewardCount;
        private GameObject firstTimeRewardStatus;

        private TextMeshProUGUI markers;
        private TextMeshProUGUI stageTitle;
        private AdminBRO.Battle battleData;

        private int battlesCount => int.Parse(uiBattlesCount.text);
        private int energyCost => inputData?.energyCost ?? 0;
        private int replayCost => inputData?.replayCost ?? 0;
        private (int scrollCost, int energyCost) fastBattleCost => (battlesCount * replayCost, battlesCount * energyCost);
        private bool stageIsComplete =>
            inputData?.ftueStageData?.isComplete ?? inputData?.eventStageData?.isComplete ?? false;

        private BaseBattleScreen.BossMiniGameInfo _bossMiniGameInfo { get; set; }

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab(
                "Prefabs/UI/Popups/PrepareBattlePopups/PrepareBossFightPopup/PrepareBossFightPopup", transform);

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
            scrollBuyButton.onClick.AddListener(PotionBuyButtonClick);
            
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

            buffButton.onClick.AddListener(BuffButtonClick);
            UITools.RightHide(buffRect);

            stageTitle = levelTitle.Find("Title").GetComponent<TextMeshProUGUI>();
            markers = stageTitle.transform.Find("Markers").GetComponent<TextMeshProUGUI>();

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
            var enemyPotency = 0;
            foreach (var phase in battleData.battlePhases)
            {
                foreach (var enemy in phase.enemyCharacters)
                {
                    var enemyChar = NSPrepareBossFightPopup.Boss.GetInstance(bossPos);
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
            
            if (battleData.firstRewards.Count > 0)
            {
                var firstReward = battleData.firstRewards[0];

                firstTimeReward.gameObject.SetActive(true);
                firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
                firstTimeRewardCount.text = firstReward.amount.ToString();
                
                firstTimeRewardStatus.SetActive(stageIsComplete);
                firstTimeReward.color = stageIsComplete ? Color.gray : Color.white;
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

            fastBattleAvailable.SetActive(stageIsComplete);

            allyTeamPotency.text = GameData.characters.myTeamPotency.ToString();
            stageTitle.text = battleData?.title;

            battleButtonText.text =
                $"Make them suffer\nwith <size=40>{TMPSprite.Energy}</size> {energyCost} energy!";

            CustomizeBuff();

            SetSkipButtonState();
            SetPotionValues();
        }

        public override async Task BeforeShowDataAsync()
        {
            await GameData.player.Get();
            _bossMiniGameInfo = await BaseBattleScreen.GetBossMiniGameInfoFromServer(inputData);

            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            battleData = inputData.eventStageData?.battleData ?? inputData.ftueStageData?.battleData;
            Customize();

            switch (inputData.ftueStageData?.lerningKey)
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
            
            bool canPlayFastBattle = GameData.player.info.energyPointsAmount >= fastBattleCost.energyCost &&
                GameData.player.info.replayAmount >= fastBattleCost.scrollCost;
            
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

                SetPotionValues();
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
            SetSkipButtonState();
        }

        private void Dec()
        {
            uiBattlesCount.text = (battlesCount - 1).ToString();
            SetSkipButtonState();
        }

        private void SetSkipButtonState()
        {
            fastBattleText.text =
                $"Give an order to hunt\nfor {TMPSprite.Energy} {fastBattleCost.energyCost} and {TMPSprite.Scroll} {fastBattleCost.scrollCost}";
            buttonPlus.gameObject.SetActive(battlesCount < 5);
            buttonMinus.gameObject.SetActive(battlesCount > 1);
        }

        private void SetPotionValues()
        {
            userHpAmount.text = GameData.player.info.hpPotionAmount.ToString();
            userManaAmount.text = GameData.player.info.manaPotionAmount.ToString();
            userStaminaAmount.text = GameData.player.info.energyPointsAmount + "/" + GameData.potions.baseEnergyVolume;
            userScrollAmount.text = GameData.player.info.replayAmount.ToString();
        }

        private void PotionBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowPopup<BottlesPopup>();
        }
        
        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<TeamEditScreen>();
        }

        private void BuffButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        private void BattleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
            if (GameData.player.info.energyPointsAmount >= energyCost)
            {
                UIManager.MakeScreen<BossFightScreen>().
                    SetData(new BaseBattleScreenInData
                    {
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId,

                        bossMiniGameInfo = _bossMiniGameInfo,
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

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_4):
                    GameData.ftue.chapter1.ShowNotifByKey("potionstutor1");
                    break;
            }
        }

        public override async Task BeforeHideAsync()
        {
            await UITools.RightHideAsync(buffRect, 0.2f);
        }

        private void RefreshEnergy()
        {
            userStaminaAmount.text = GameData.player.info.energyPointsAmount + "/" + GameData.potions.baseEnergyVolume;
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
            descr.text = UITools.IncNumberSize(GameData.matriarchs.activeBuff?.description, descr.fontSize);
        }
    }

    public class PrepareBossFightPopupInData : BasePrepareBattlePopupInData
    {
       
    }
}