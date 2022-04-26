using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ChestPopup : BasePopup
    {
        private TextMeshProUGUI timer;
        private GameObject timerBackground;
        
        private Button buyButton;
        private TextMeshProUGUI buyButtonText;
        private Button backButton;
        private Image banner1;
        private Image banner2;
        private Image girlIcon;
        
        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/ChestPopup/ChestPopup", transform);
            var canvas = screenInst.transform.Find("Canvas");
            var banners = canvas.Find("Banners");

            timerBackground = canvas.Find("Headline").Find("TimerBackground").gameObject;
            timer = timerBackground.transform.Find("Timer").GetComponent<TextMeshProUGUI>();

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButtonText = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            buyButton.onClick.AddListener(BuyButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            banner1 = banners.Find("Banner1").GetComponent<Image>();
            banner2 = banners.Find("Banner2").GetComponent<Image>();

            girlIcon = canvas.Find("Girl").GetComponent<Image>();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        private void BuyButtonClick()
        {
            
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
