using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GirlScreen : BaseFullScreenParent<GirlScreenInData>
    {
        private Transform girlUlviImage;
        private Transform girlAdrielImage;
        private Transform girlIngieImage;
        private Transform girlFayeImage;
        private Transform girlLiliImage;

        private Transform ulviZodiac;
        private Transform adrielZodiac;
        private Transform ingieZodiac;
        private Transform fayeZodiac;
        private Transform liliZodiac;
        private TextMeshProUGUI zodiacName;
        private TextMeshProUGUI birthday;
        private TextMeshProUGUI girlName;

        private Transform lvlProgressStep1;
        private Transform lvlProgressStep2;
        private Transform lvlProgressStep3;
        private TextMeshProUGUI currentProgressLevel;
        private TextMeshProUGUI nextProgressLevel;        
        private Image rewardTier1;
        private GameObject receivedTier1;
        private Image rewardTier2;
        private GameObject receivedTier2;
        private Image rewardTier3;
        private GameObject receivedTier3;

        
        private Transform buffInfo;
        private TextMeshProUGUI buffTitle;
        private TextMeshProUGUI buffDescription;
        private TextMeshProUGUI buffHint;
        private GameObject buffActive;
        private Image buffIcon;
        
        private Button bannerUlviButton;
        private Button bannerAdrielButton;
        private Button bannerIngieButton;
        private Button bannerFayeButton;
        private Button bannerLiliButton;
        private GameObject bannerNotification;

        private Button sexButton;
        private TextMeshProUGUI sexButtonTitle;
        private Button dialogButton;
        private TextMeshProUGUI dialogButtonTitle;
        private Button backButton;

        private Transform sexCooldown;
        private TextMeshProUGUI sexCooldownTimer;

        private bool seduceSex = false;


        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/GirlScreen/Girl", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var progressBar = canvas.Find("TrustProgressBar");
            var banner = canvas.Find("Banner");

            var girlInfo = canvas.Find("GirlInfo");
            ulviZodiac = girlInfo.Find("ZodiacInfo").Find("UlviZodiacIcon");
            adrielZodiac = girlInfo.Find("ZodiacInfo").Find("AdrielZodiacIcon");
            ingieZodiac = girlInfo.Find("ZodiacInfo").Find("IngieZodiacIcon");
            fayeZodiac = girlInfo.Find("ZodiacInfo").Find("FayeZodiacIcon");
            liliZodiac = girlInfo.Find("ZodiacInfo").Find("LiliZodiacIcon");
            zodiacName = girlInfo.Find("ZodiacInfo").Find("ZodiacName").GetComponent<TextMeshProUGUI>();
            birthday = girlInfo.Find("BirthdayInfo").Find("BirthdayDate").GetComponent<TextMeshProUGUI>();
            girlName = girlInfo.Find("Name").GetComponent<TextMeshProUGUI>();

            girlUlviImage = canvas.Find("GirlUlvi");
            girlAdrielImage = canvas.Find("GirlAdriel");
            girlIngieImage = canvas.Find("GirlIngie");
            girlFayeImage = canvas.Find("GirlFaye");
            girlLiliImage = canvas.Find("GirlLili");

            lvlProgressStep1 = progressBar.Find("Step1");
            lvlProgressStep2 = progressBar.Find("Step2");
            lvlProgressStep3 = progressBar.Find("Step3");
            currentProgressLevel = progressBar.Find("CurrentLvl").GetComponent<TextMeshProUGUI>();
            nextProgressLevel = progressBar.Find("NextLvl").GetComponent<TextMeshProUGUI>();
            rewardTier1 = progressBar.Find("RewardTier1").GetComponent<Image>();
            receivedTier1 = rewardTier1.transform.Find("Received").gameObject;
            rewardTier2 = progressBar.Find("RewardTier2").GetComponent<Image>();
            receivedTier2 = rewardTier2.transform.Find("Received").gameObject;
            rewardTier3 = progressBar.Find("RewardTier3").GetComponent<Image>();
            receivedTier3 = rewardTier3.transform.Find("Received").gameObject;

           
            buffInfo = progressBar.Find("BuffInfo");
            buffTitle = buffInfo.Find("Title").GetComponent<TextMeshProUGUI>();
            buffDescription = buffInfo.Find("Description").GetComponent<TextMeshProUGUI>();
            buffHint = buffInfo.Find("Hint").Find("Text").GetComponent<TextMeshProUGUI>();
            buffActive = buffInfo.Find("BuffActive").gameObject;
            buffIcon = buffInfo.Find("BuffBack/Icon").GetComponent<Image>();
            

            bannerUlviButton = canvas.Find("Banner").Find("BannerButtonUlvi").GetComponent<Button>();
            bannerAdrielButton = canvas.Find("Banner").Find("BannerButtonAdriel").GetComponent<Button>();
            bannerIngieButton = canvas.Find("Banner").Find("BannerButtonIngie").GetComponent<Button>();
            bannerFayeButton = canvas.Find("Banner").Find("BannerButtonFaye").GetComponent<Button>();
            bannerLiliButton = canvas.Find("Banner").Find("BannerButtonLili").GetComponent<Button>();
            bannerUlviButton.onClick.AddListener(BannerButtonClick);
            bannerAdrielButton.onClick.AddListener(BannerButtonClick);
            bannerIngieButton.onClick.AddListener(BannerButtonClick);
            bannerFayeButton.onClick.AddListener(BannerButtonClick);
            bannerLiliButton.onClick.AddListener(BannerButtonClick);
            bannerNotification = banner.Find("Notification").GetComponent<GameObject>();

            sexButton = canvas.Find("SexButton").GetComponent<Button>();
            sexButton.onClick.AddListener(SexButtonClick);
            sexButtonTitle = sexButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            dialogButton = canvas.Find("DialogButton").GetComponent<Button>();
            dialogButton.onClick.AddListener(DialogButtonClick);
            dialogButtonTitle = dialogButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            sexCooldown = sexButton.transform.Find("Cooldown");
            sexCooldownTimer = sexCooldown.Find("Timer").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowDataAsync()
        {
            var girlData = inputData?.girlData;
            var availableTime = girlData?.seduceAvailableAt;
            if (!String.IsNullOrEmpty(availableTime))
            {
                var availableTimeStr = TimeTools.AvailableTimeToString(availableTime);
                if (String.IsNullOrEmpty(availableTimeStr))
                {
                    await GameData.matriarchs.Get();
                }
            }
            
            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            var girlData = inputData?.girlData;

            girlUlviImage.gameObject.SetActive(girlData.isUlvi);
            bannerUlviButton.gameObject.SetActive(girlData.isUlvi);
            girlAdrielImage.gameObject.SetActive(girlData.isAdriel);
            bannerAdrielButton.gameObject.SetActive(girlData.isAdriel);
            girlIngieImage.gameObject.SetActive(girlData.isIngie);
            bannerIngieButton.gameObject.SetActive(girlData.isIngie);
            girlFayeImage.gameObject.SetActive(girlData.isFaye);
            bannerFayeButton.gameObject.SetActive(girlData.isFaye);
            girlLiliImage.gameObject.SetActive(girlData.isLili);
            bannerLiliButton.gameObject.SetActive(girlData.isLili);

            ulviZodiac.gameObject.SetActive(girlData.isUlvi);
            adrielZodiac.gameObject.SetActive(girlData.isAdriel);
            ingieZodiac.gameObject.SetActive(girlData.isIngie);
            fayeZodiac.gameObject.SetActive(girlData.isFaye);
            liliZodiac.gameObject.SetActive(girlData.isLili);
            
            zodiacName.text = girlData.paramZodiac;
            birthday.text = girlData.paramAge.ToString();
            girlName.text = girlData.name;

            currentProgressLevel.text = girlData.currentEmpathyLevel?.ToString();
            nextProgressLevel.text = girlData.nextEmpathyLevel?.ToString();
            switch (girlData.rewardsClaimed)
            {
                case AdminBRO.MatriarchItem.RewardsClaimed_None:
                    lvlProgressStep1.gameObject.SetActive(false);
                    lvlProgressStep2.gameObject.SetActive(false);
                    lvlProgressStep3.gameObject.SetActive(false);
                    break;
                case AdminBRO.MatriarchItem.RewardsClaimed_TwentFive:
                    lvlProgressStep1.gameObject.SetActive(true);
                    lvlProgressStep2.gameObject.SetActive(false);
                    lvlProgressStep3.gameObject.SetActive(false);
                    break;
                case AdminBRO.MatriarchItem.RewardsClaimed_Fifty:
                    lvlProgressStep1.gameObject.SetActive(true);
                    lvlProgressStep2.gameObject.SetActive(true);
                    lvlProgressStep3.gameObject.SetActive(false);
                    break;
                case AdminBRO.MatriarchItem.RewardsClaimed_All:
                    lvlProgressStep1.gameObject.SetActive(true);
                    lvlProgressStep2.gameObject.SetActive(true);
                    lvlProgressStep3.gameObject.SetActive(true);
                    break;
            }

            receivedTier1.SetActive(lvlProgressStep1.gameObject.activeSelf);
            receivedTier2.SetActive(lvlProgressStep2.gameObject.activeSelf);
            receivedTier3.SetActive(lvlProgressStep3.gameObject.activeSelf);
            sexButtonTitle.text = $"Get {inputData?.girlKey}'s\nbuff";
            dialogButtonTitle.text = $"Get {inputData?.girlKey}'s\ndaily quest";
            
            if (!String.IsNullOrEmpty(girlData.seduceAvailableAt))
            {
                UITools.DisableButton(sexButton);
                StartCoroutine(SexCooldownUpd());
            }
            else
            {
                sexCooldown.gameObject.SetActive(false);
            }

            CustomizeBuffInfo();

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_2):
                    UITools.DisableButton(bannerUlviButton);
                    UITools.DisableButton(bannerAdrielButton);
                    UITools.DisableButton(bannerIngieButton);
                    UITools.DisableButton(bannerFayeButton);
                    UITools.DisableButton(bannerLiliButton);
                    
                    UITools.DisableButton(sexButton);
                    break;
            }
            
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_2):
                    GameData.ftue.chapter2.ShowNotifByKey("ch2empathytutor1");
                    break;
            }
            
            switch (inputData?.girlKey)
            {
                case AdminBRO.MatriarchItem.Key_Ulvi:
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_matriarch_screen);
                    break;
                case AdminBRO.MatriarchItem.Key_Adriel:
                    SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_matriarch_screen);
                    break;
                case AdminBRO.MatriarchItem.Key_Inge:
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_matriarch_screen);
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task BeforeHideDataAsync()
        {
            if (seduceSex)
            {
                await GameData.matriarchs.MatriarchSeduce(inputData.girlData.id);
            }
            await Task.CompletedTask;
        }

        private void SexButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            seduceSex = true;
            UIManager.MakeScreen<SexScreen>().
                SetData(new SexScreenInData
            {
                dialogId = inputData.girlData.seduceSexSceneId
            }).DoShow();
        }

        private void DialogButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<DialogScreen>().
                SetData(new DialogScreenInData
                {
                    dialogId = inputData?.girlData?.dailyQuestGiverDialogId,
                }).DoShow();

        }
        
        private void BannerButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (inputData == null)
            {
                UIManager.ShowScreen<MatriarchMemoryListScreen>();
            }
            else
            {
                UIManager.MakeScreen<MatriarchMemoryListScreen>().
                    SetData(new MatriarchMemoryListScreenInData
                    {
                        girlKey = inputData.girlKey,
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId,
                    }).DoShow();
            }
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ToPrevScreen();
        }

        private IEnumerator SexCooldownUpd()
        {
            var time = TimeTools.AvailableTimeToString(inputData.girlData.seduceAvailableAt);
            while (!String.IsNullOrEmpty(time))
            {
                sexCooldownTimer.text = UITools.IncNumberSize(time, sexCooldownTimer.fontSize);
                yield return new WaitForSeconds(1.0f);
                time = TimeTools.AvailableTimeToString(inputData.girlData.seduceAvailableAt);
            }
            UnlockSexButton();
            CustomizeBuffInfo();
        }

        private async void UnlockSexButton()
        {
            await GameData.matriarchs.Get();
            sexCooldown.gameObject.SetActive(false);
            UITools.DisableButton(sexButton, false);
        }

        private void CustomizeBuffInfo()
        {
            var girlData = inputData?.girlData;

            var buffIsActive = girlData?.buff?.active ?? false;
            buffTitle.gameObject.SetActive(buffIsActive);
            buffDescription.text = UITools.IncNumberSize(girlData?.buff?.description, buffDescription.fontSize);
            buffHint.text = girlData?.buff?.postDescription;
            buffActive.SetActive(buffIsActive);
            buffIcon.sprite = ResourceManager.LoadSprite(girlData?.buff?.icon);
        }
    }

    public class GirlScreenInData : BaseFullScreenInData
    {
        public string girlKey;

        public AdminBRO.MatriarchItem girlData =>
            GameData.matriarchs.GetMatriarchByKey(girlKey);
    }
}
