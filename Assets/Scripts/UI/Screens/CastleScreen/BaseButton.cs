using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public abstract class BaseButton : MonoBehaviour
        {
            protected Button button;
            protected TextMeshProUGUI title;

            protected Image quarterlyEventMark;
            protected Image monthlyEventMark;
            protected Image weeklyEventMark;
            protected Image mainQuestMark;
            protected Image sideQuestMark;

            protected virtual void Awake()
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

            protected virtual void ButtonClick()
            {

            }
        }
    }
}