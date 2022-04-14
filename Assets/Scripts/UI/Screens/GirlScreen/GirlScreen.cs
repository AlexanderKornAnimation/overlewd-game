using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GirlScreen : BaseFullScreen
    {
        protected Image girlImage;
        
        protected Image trustProgress;
        protected TextMeshProUGUI currentProgressLevel;
        protected TextMeshProUGUI nextProgressLevel;
        
        protected Image rewardTier1;
        protected TextMeshProUGUI receivedTier1;
        
        protected Image rewardTier2;
        protected TextMeshProUGUI receivedTier2;

        protected Image rewardTier3;
        protected TextMeshProUGUI receivedTier3;
        
        protected TextMeshProUGUI buffPower;
        protected TextMeshProUGUI buffType;

        protected Button bannerButton;
        protected Image bannerArt;
        protected GameObject bannerNotification; 

        protected Button seduceButton;
        protected Button dialogButton;
        protected Button portalButton;
        protected Button chestButton;
        protected Button backButton;
        
        void Awake()
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

            seduceButton = canvas.Find("SeduceButton").GetComponent<Button>();
            dialogButton = canvas.Find("DialogButton").GetComponent<Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            chestButton = canvas.Find("ChestButton").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            
            bannerButton.onClick.AddListener(BannerButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            chestButton.onClick.AddListener(ChestButtonClick);
            backButton.onClick.AddListener(BackButtonClick);
        }

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            
        }
        
        protected virtual void BannerButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MemoryListScreen>();
        }
        
        protected virtual void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }
        
        protected virtual void ChestButtonClick()
        {
            // SoundManager.PlayOneShot(SoundManager.FMODEventPath.UI.ButtonClick);
            // UIManager.ShowPopup<ChestPopup>();
        }
        
        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }
    }
}
