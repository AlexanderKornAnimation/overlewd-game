using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseNotification : BaseScreen
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public override void Show()
        {
            UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
            gameObject.AddComponent<ImmediateShow>();
        }

        public override void Hide()
        {
            UIManager.HideNotificationMissclick();
            gameObject.AddComponent<ImmediateHide>();
        }
    }
}
