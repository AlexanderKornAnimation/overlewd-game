using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class EventButton : MonoBehaviour
        {
            protected Button button;
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI description;
            protected Image icon;
            protected Image arrowTop;
            protected Image arrowLeft;
            protected Image arrowBot;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                
                icon = button.transform.Find("Icon").GetComponent<Image>();
                arrowTop = button.transform.Find("ArrowTop").GetComponent<Image>();
                arrowLeft = button.transform.Find("ArrowLeft").GetComponent<Image>();
                arrowBot = button.transform.Find("ArrowBot").GetComponent<Image>();

                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
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