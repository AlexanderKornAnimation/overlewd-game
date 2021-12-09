using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuyingNotification : BaseNotification
    {
        protected Button button;
        protected Text text;

        protected virtual void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BuyingNotification/BuyingNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);

            text = canvas.Find("Banner").Find("Text").GetComponent<Text>();
        }

        protected virtual void ButtonClick()
        {
            UIManager.HideNotification();
        }
    }
}
