using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class NutakuBuyingStatusNotification : BaseNotification
    {
        private Button backButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Notifications/NutakuNotification/BuyingStatusNotification", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
        }

        private void BackButtonClick()
        {
            UIManager.ShowNotification<BuyingNotification>();
        }
    }
}
