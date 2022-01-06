using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class NutakuBuyingNotification : BaseNotification
    {
        private Button buyButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Notifications/NutakuNotification/BuyingNotification", transform);

            var canvas = screenInst.transform.Find("Canvas");

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(BuyButtonClick);
        }

        private void BuyButtonClick()
        {
            UIManager.ShowNotification<NutakuBuyingStatusNotification>();
        }
    }
}
