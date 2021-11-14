using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class NotificationMissclick : BaseMissclick
    {
        protected override void OnClick()
        {
            UIManager.HideNotification();
        }
    }

    public class NotificationMissclickClear : NotificationMissclick
    {

    }

    public class NotificationMissclickColored : NotificationMissclick
    {
        public override void Show()
        {
            gameObject.AddComponent<MissclickColoredShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<MissclickColoredHide>();
        }
    }
}
