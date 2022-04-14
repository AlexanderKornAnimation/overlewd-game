using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class EventButton : BaseButton
        {
            protected TextMeshProUGUI description;
            protected Image icon;
            protected Image arrowTop;
            protected Image arrowLeft;
            protected Image arrowBot;
            protected TextMeshProUGUI questMark;

            protected override void Awake()
            {
                base.Awake();

                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                icon = button.transform.Find("Icon").GetComponent<Image>();
                arrowTop = button.transform.Find("ArrowTop").GetComponent<Image>();
                arrowLeft = button.transform.Find("ArrowLeft").GetComponent<Image>();
                arrowBot = button.transform.Find("ArrowBot").GetComponent<Image>();
                button.transform.Find("QuestMark").GetComponent<TextMeshProUGUI>();
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<EventMapScreen>();
            }

            public static EventButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventButton>
                    ("Prefabs/UI/Screens/MapScreen/EventButton", parent);
            }
        }
    }
}