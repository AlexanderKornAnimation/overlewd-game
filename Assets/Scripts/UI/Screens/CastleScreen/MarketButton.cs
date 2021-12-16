using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MarketButton : BaseButton
        {
            protected Transform saleNotification;
            protected Transform timeLimitNotification;
            protected Transform newOfferNotification;

            protected override void Awake()
            {
                base.Awake();

                var notificationGrid = transform.Find("NotificationGrid");

                saleNotification = notificationGrid.Find("SaleNotification");
                timeLimitNotification = notificationGrid.Find("TimeLimitNotification");
                newOfferNotification = notificationGrid.Find("NewOfferNotification");
            }

            protected override void ButtonClick()
            {
                // UIManager.ShowScreen<MarketScreen>();
            }

            public static MarketButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/MarketButton"), parent);
                newItem.name = nameof(MarketButton);

                return newItem.AddComponent<MarketButton>();
            }
        }
    }
}