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

        private Image trustProgress;
        private TextMeshProUGUI currentProgressLevel;
        private TextMeshProUGUI nextProgressLevel;
        
        private Image rewardTier1;
        private TextMeshProUGUI receivedTier1;
        
        private Image rewardTier2;
        private TextMeshProUGUI receivedTier2;

        private Image rewardTier3;
        private TextMeshProUGUI receivedTier3;
        
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
        
        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/GirlScreen/Girl", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var progressBar = canvas.Find("TrustProgressBar");
            var buff = progressBar.Find("Buff");
            var banner = canvas.Find("Banner");

            girlUlviImage = canvas.Find("GirlUlvi");
            girlAdrielImage = canvas.Find("GirlAdriel");
            girlIngieImage = canvas.Find("GirlIngie");
            girlFayeImage = canvas.Find("GirlFaye");
            girlLiliImage = canvas.Find("GirlLili");

            trustProgress = progressBar.Find("Progress").GetComponent<Image>();
            currentProgressLevel = progressBar.Find("CurrentLvl").GetComponent<TextMeshProUGUI>();
            nextProgressLevel = progressBar.Find("NextLvl").GetComponent<TextMeshProUGUI>();

            rewardTier1 = progressBar.Find("RewardTier1").GetComponent<Image>();
            receivedTier1 = rewardTier1.transform.Find("Received").GetComponent<TextMeshProUGUI>();
            
            rewardTier2 = progressBar.Find("RewardTier2").GetComponent<Image>();
            receivedTier2 = rewardTier2.transform.Find("Received").GetComponent<TextMeshProUGUI>();
            
            rewardTier3 = progressBar.Find("RewardTier3").GetComponent<Image>();
            receivedTier3 = rewardTier3.transform.Find("Received").GetComponent<TextMeshProUGUI>();

            buffPower = buff.Find("Power").GetComponent<TextMeshProUGUI>();
            buffType = buff.Find("Type").GetComponent<TextMeshProUGUI>();

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
        }

        public override async Task BeforeShowMakeAsync()
        {
            var girlData = inputData?.girlData;
            girlUlviImage.gameObject.SetActive(girlData?.isUlvi ?? false);
            bannerUlviButton.gameObject.SetActive(girlData?.isUlvi ?? false);
            girlAdrielImage.gameObject.SetActive(girlData?.isAdriel ?? false);
            bannerAdrielButton.gameObject.SetActive(girlData?.isAdriel ?? false);
            girlIngieImage.gameObject.SetActive(girlData?.isIngie ?? false);
            bannerIngieButton.gameObject.SetActive(girlData?.isIngie ?? false);
            girlFayeImage.gameObject.SetActive(girlData?.isFaye ?? false);
            bannerFayeButton.gameObject.SetActive(girlData?.isFaye ?? false);
            girlLiliImage.gameObject.SetActive(girlData?.isLili ?? false);
            bannerLiliButton.gameObject.SetActive(girlData?.isLili ?? false);

            await Task.CompletedTask;
        }

        private void SexButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<SexScreen>().
                SetData(new SexScreenInData
            {
                prevScreenInData = inputData
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
    }

    public class GirlScreenInData : BaseFullScreenInData
    {
        public string girlKey;

        public AdminBRO.MatriarchItem girlData =>
            GameData.matriarchs.GetMatriarchByKey(girlKey);
    }
}
