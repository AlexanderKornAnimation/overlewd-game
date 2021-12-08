using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class BaseButton : MonoBehaviour
    {
        protected Button button;
        protected TextMeshProUGUI title;

        private Image quarterlyEventMark;
        private Image monthlyEventMark;
        private Image weeklyEventMark;
        private Image mainQuestMark;
        private Image sideQuestMark;

        protected void Awake()
        {
            button = transform.Find("Button").GetComponent<Button>();
            title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            button.onClick.AddListener(ButtonClick);
        }

        private void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            title.text = transform.parent.name;
        }

        protected abstract void ButtonClick();
    }
}