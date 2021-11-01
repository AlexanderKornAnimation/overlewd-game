using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BannerNotification : BaseNotification
    {
        private Button closeButton;
        private Button buyButton;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/BannerNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            closeButton = canvas.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(BuyButtonClick);
        }

        void Update()
        {

        }

        private void CloseButtonClick()
        {
            UIManager.HideNotification();
        }

        private void BuyButtonClick()
        {
            UIManager.ShowNotification<NutakuBuyingNotification>();
        }
    }
}
