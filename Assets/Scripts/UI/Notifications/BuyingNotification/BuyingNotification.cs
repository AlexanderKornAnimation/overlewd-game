using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuyingNotification : BaseNotification
    {
        protected Button button;
        protected Image girlEmotion;
        protected TextMeshProUGUI text;

        protected virtual void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BuyingNotification/BuyingNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            
            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);

            text = canvas.Find("Banner").Find("Text").GetComponent<TextMeshProUGUI>();
            girlEmotion = canvas.Find("Banner").Find("GirlEmotion").GetComponent<Image>();
        }

        protected virtual void ButtonClick()
        {
            UIManager.HideNotification();
        }
    }
}
