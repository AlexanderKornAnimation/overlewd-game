using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GirlScreen : BaseFullScreenParent<GirlScreenInData>
    {
        private Image girlImage;
        
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

        private Button bannerButton;
        private Image bannerArt;
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

            girlImage = canvas.Find("Girl").GetComponent<Image>();
            
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

            bannerButton = canvas.Find("Banner").Find("Button").GetComponent<Button>();
            bannerArt = bannerButton.GetComponent<Image>();
            bannerNotification = banner.Find("Notification").GetComponent<GameObject>();

            sexButton = canvas.Find("SexButton").GetComponent<Button>();
            dialogButton = canvas.Find("DialogButton").GetComponent<Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            chestButton = canvas.Find("ChestButton").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            
            bannerButton.onClick.AddListener(BannerButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            chestButton.onClick.AddListener(ChestButtonClick);
            backButton.onClick.AddListener(BackButtonClick);
            sexButton.onClick.AddListener(SexButtonClick);
            dialogButton.onClick.AddListener(DialogButtonClick);
        }

        private void Start()
        {
            Customize();
        }

        private void Customize()
        {
            
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
                        girlName = inputData.girlName,
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
        public string girlName;
    }
}
