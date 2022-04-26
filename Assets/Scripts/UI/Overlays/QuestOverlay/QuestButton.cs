using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public abstract class QuestButton : MonoBehaviour
        {
            public QuestContentScrollView contentScrollView { get; set; }

            public event Action<QuestButton> buttonPressed;

            protected Button blueButton;
            protected Image blueButtonImage;
            protected Button darkButton;
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI notification;
            protected Image darkButtonImage;

            private Sprite blueButtonDefaultSprite;
            private Sprite darkButtonDefaultSprite;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");

                blueButton = canvas.Find("BlueButton").GetComponent<Button>();
                blueButton.onClick.AddListener(ButtonClick);
                blueButtonImage = blueButton.GetComponent<Image>();

                darkButton = canvas.Find("DarkButton").GetComponent<Button>();
                darkButton.onClick.AddListener(ButtonClick);
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                darkButtonImage = darkButton.GetComponent<Image>();

                notification = canvas.Find("Notification").GetComponent<TextMeshProUGUI>();

                blueButtonDefaultSprite = blueButtonImage.sprite;
                darkButtonDefaultSprite = darkButtonImage.sprite;
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                buttonPressed?.Invoke(this);
            }

            public void Select()
            {
                blueButtonImage.sprite = blueButton.spriteState.selectedSprite;
                darkButtonImage.sprite = darkButton.spriteState.selectedSprite;
                contentScrollView?.Show();
            }

            public void Deselect()
            {
                blueButtonImage.sprite = blueButtonDefaultSprite;
                darkButtonImage.sprite = darkButtonDefaultSprite;
                contentScrollView?.Hide();
            }
        }
    }
}
