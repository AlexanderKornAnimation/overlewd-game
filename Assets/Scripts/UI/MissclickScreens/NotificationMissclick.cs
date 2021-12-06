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
        protected override void Awake()
        {
            base.Awake();

            image.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);
        }

        public override void Show()
        {
            gameObject.AddComponent<FadeShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<FadeHide>();
        }
    }
}
