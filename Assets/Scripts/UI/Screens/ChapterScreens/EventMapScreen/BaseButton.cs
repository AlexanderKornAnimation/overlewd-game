using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class BaseButton : MonoBehaviour
        {
            public int? eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);

            protected Transform canvas;
            protected Button button;
            protected TextMeshProUGUI title;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }
        }
    }
}
