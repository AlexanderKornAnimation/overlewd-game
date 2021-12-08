using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MarketButton : BaseButton
    {
        private Transform saleNotification;
        private Transform timeLimitNotification;
        private Transform newOfferNotification;

        private void Awake()
        {
            base.Awake();

            var notificationGrid = transform.Find("NotificationGrid");

            saleNotification = notificationGrid.Find("SaleNotification");
            timeLimitNotification = notificationGrid.Find("TimeLimitNotification");
            newOfferNotification = notificationGrid.Find("NewOfferNotification");
        }

        protected override void ButtonClick()
        {
            UIManager.ShowScreen<MarketScreen>();
        }

        public static MarketButton GetInstance(Transform parent)
        {
            var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/MarketButton"), parent);
            newItem.name = nameof(MarketButton);

            return newItem.AddComponent<MarketButton>();
        }
    }

}