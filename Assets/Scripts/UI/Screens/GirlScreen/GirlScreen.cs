using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GirlScreen : BaseScreen
    {
        private Image girlImage;
        
        private Image trustProgress;
        private TextMeshProUGUI currentProgressLevel;
        private TextMeshProUGUI nextProgressLevel;
        
        private Image rewardTier1;
        private Image receivedTier1;
        
        private Image rewardTier2;
        private Image receivedTier2;

        private Image rewardTier3;
        private Image receivedTier3;
        
        private TextMeshProUGUI buffPower;
        private TextMeshProUGUI buffType;

        private Button bannerButton;
        private Image bannerArt;
        private GameObject bannerNotification; 

        private Button seduceButton;
        private Button dialogButton;
        private Button portalButton;
        private Button chestButton;
        private Button backButton;
        
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
            receivedTier1 = rewardTier1.transform.Find("Received").GetComponent<Image>();
            
            rewardTier2 = progressBar.Find("RewardTier2").GetComponent<Image>();
            receivedTier2 = rewardTier2.transform.Find("Received").GetComponent<Image>();
            
            rewardTier3 = progressBar.Find("RewardTier3").GetComponent<Image>();
            receivedTier3 = rewardTier3.transform.Find("Received").GetComponent<Image>();

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

        private void Start()
        {
            Customize();
        }

        private void Customize()
        {
            
        }
        
        private void BannerButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<MemoryListScreen>();
        }
        
        private void PortalButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }
        
        private void ChestButtonClick()
        {
            // SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
            // UIManager.ShowPopup<ChestPopup>();
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }
    }
}
