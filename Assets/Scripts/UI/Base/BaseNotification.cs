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

        protected override void ShowMissclick()
        {
            UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
        }

        public override void Show()
        {
            ShowMissclick();
            gameObject.AddComponent<ImmediateShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<ImmediateHide>();
        }
    }
}
