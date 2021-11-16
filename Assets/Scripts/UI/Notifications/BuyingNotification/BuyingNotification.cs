using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuyingNotification : BaseNotification
    {
        protected Button button;

        protected virtual void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BuyingNotification/BuyingNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            UIManager.HideNotification();
        }
    }
}
