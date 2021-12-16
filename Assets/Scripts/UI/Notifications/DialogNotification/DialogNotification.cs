using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogNotification : BaseNotification
    {
        protected Button button;
        protected TextMeshProUGUI text;
        protected Transform emotionBack;
        protected Transform emotionPos;

        protected virtual void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/DialogNotification/DialogNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
 
            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);

            var banner = canvas.Find("Banner");
            text = banner.Find("Text").GetComponent<TextMeshProUGUI>();
            emotionBack = banner.Find("EmotionBack");
            emotionPos = emotionBack.Find("EmotionPos");
        }

        protected virtual void ButtonClick()
        {
            UIManager.HideNotification();
        }
    }
}
