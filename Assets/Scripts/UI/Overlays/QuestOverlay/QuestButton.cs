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
            protected Image darkButtonImage;
            protected GameObject newQuestMark;
            protected GameObject questDoneMark;

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

                newQuestMark = canvas.Find("NewQuestMark").gameObject;
                questDoneMark = canvas.Find("QuestDoneMark").gameObject;

                blueButtonDefaultSprite = blueButtonImage.sprite;
                darkButtonDefaultSprite = darkButtonImage.sprite;
            }

            protected virtual void ButtonClick()
            {
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

            protected static GameObject LoadPrefab(Transform parent)
            {
                return (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/QuestButton"), parent);
            }
        }
    }
}
