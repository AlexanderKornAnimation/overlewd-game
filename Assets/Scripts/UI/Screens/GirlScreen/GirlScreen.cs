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

        private Transform lvlProgressStep1;
        private Transform lvlProgressStep2;
        private Transform lvlProgressStep3;
        private TextMeshProUGUI currentProgressLevel;
        private TextMeshProUGUI nextProgressLevel;        
        private Image rewardTier1;
        private TextMeshProUGUI receivedTier1;
        private Image rewardTier2;
        private TextMeshProUGUI receivedTier2;
        private Image rewardTier3;
        private TextMeshProUGUI receivedTier3;

        private Transform buffInfo;
        private TextMeshProUGUI buffPower;
        private TextMeshProUGUI buffType;

        private Button bannerUlviButton;
        private Button bannerAdrielButton;
        private Button bannerIngieButton;
        private Button bannerFayeButton;
        private Button bannerLiliButton;
        private GameObject bannerNotification; 

        private Button sexButton;
        private Button dialogButton;
        private Button portalButton;
        private Button chestButton;
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
            receivedTier1 = rewardTier1.transform.Find("Received").GetComponent<TextMeshProUGUI>();
            rewardTier2 = progressBar.Find("RewardTier2").GetComponent<Image>();
            receivedTier2 = rewardTier2.transform.Find("Received").GetComponent<TextMeshProUGUI>();
            rewardTier3 = progressBar.Find("RewardTier3").GetComponent<Image>();
            receivedTier3 = rewardTier3.transform.Find("Received").GetComponent<TextMeshProUGUI>();

            buffInfo = progressBar.Find("BuffInfo");
            buffPower = buffInfo.Find("Power").GetComponent<TextMeshProUGUI>();
            buffType = buffInfo.Find("Type").GetComponent<TextMeshProUGUI>();

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
            dialogButton = canvas.Find("DialogButton").GetComponent<Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            chestButton = canvas.Find("ChestButton").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            
            portalButton.onClick.AddListener(PortalButtonClick);
            chestButton.onClick.AddListener(ChestButtonClick);
            backButton.onClick.AddListener(BackButtonClick);
            sexButton.onClick.AddListener(SexButtonClick);
            dialogButton.onClick.AddListener(DialogButtonClick);

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

            if (!String.IsNullOrEmpty(girlData.seduceAvailableAt))
            {
                UITools.DisableButton(sexButton);
                StartCoroutine(SexCooldownUpd());
            }
            else
            {
                sexCooldown.gameObject.SetActive(false);
            }

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (inputData?.girlKey)
            {
                case AdminBRO.MatriarchItem.Key_Ulvi:
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_matriarch_screen);
                    break;
                case AdminBRO.MatriarchItem.Key_Adriel:
                    SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_matriarch_screen);
                    break;
                case AdminBRO.MatriarchItem.Key_Ingie:
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_matriarch_screen);
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task BeforeHideDataAsync()
        {
            if (seduceSex)
            {
                await GameData.matriarchs.matriarchSeduce(inputData.girlData.id);
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
                prevScreenInData = inputData,
                dialogId = inputData.girlData.seduceSexSceneId
            }).RunShowScreenProcess();
        }

        private void DialogButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<DialogScreen>().SetData(new DialogScreenInData
            {
                prevScreenInData = inputData
            }).RunShowScreenProcess();
        }
        
        private void BannerButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (inputData == null)
            {
                UIManager.ShowScreen<MemoryListScreen>();
            }
            else
            {
                UIManager.MakeScreen<MemoryListScreen>().
                    SetData(new MemoryListScreenInData
                    {
                        girlKey = inputData.girlKey,
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId,
                        prevScreenInData = inputData
                    }).RunShowScreenProcess();
            }
        }
        
        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
            {
                activeButtonId = PortalScreenInData.shardsButtonId
            }).RunShowScreenProcess();
        }
        
        private void ChestButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowPopup<ChestPopup>();
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (inputData == null)
            {
                UIManager.ShowScreen<HaremScreen>();
            }
            else
            {
                UIManager.MakeScreen<HaremScreen>().
                    SetData(inputData.prevScreenInData as HaremScreenInData).
                    RunShowScreenProcess();
            }
                
        }

        private IEnumerator SexCooldownUpd()
        {
            var time = TimeTools.AvailableTimeToString(inputData.girlData.seduceAvailableAt);
            while (!String.IsNullOrEmpty(time))
            {
                sexCooldownTimer.text = time;
                yield return new WaitForSeconds(1.0f);
                time = TimeTools.AvailableTimeToString(inputData.girlData.seduceAvailableAt);
            }
            UnlockSexButton();
        }

        private async void UnlockSexButton()
        {
            await GameData.matriarchs.Get();
            sexCooldown.gameObject.SetActive(false);
            UITools.DisableButton(sexButton, false);
        }
    }

    public class GirlScreenInData : BaseFullScreenInData
    {
        public string girlKey;

        public AdminBRO.MatriarchItem girlData =>
            GameData.matriarchs.GetMatriarchByKey(girlKey);
    }
}
