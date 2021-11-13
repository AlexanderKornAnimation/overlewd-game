using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public abstract class QuestButton : MonoBehaviour
        {
            public QuestOverlay questOverlay { get; set; }
            public QuestContentScrollView contentScrollView { get; set; }

            protected Button blueButton;
            protected Text blueButtonText;
            protected Image blueButtonImage;
            protected Button darkButton;
            protected Text darkButtonText;
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
                blueButtonText = blueButton.transform.Find("Text").GetComponent<Text>();
                blueButtonImage = blueButton.GetComponent<Image>();

                darkButton = canvas.Find("DarkButton").GetComponent<Button>();
                darkButton.onClick.AddListener(ButtonClick);
                darkButtonText = darkButton.transform.Find("Text").GetComponent<Text>();
                darkButtonImage = darkButton.GetComponent<Image>();

                newQuestMark = canvas.Find("NewQuestMark").gameObject;
                questDoneMark = canvas.Find("QuestDoneMark").gameObject;

                blueButtonDefaultSprite = blueButtonImage.sprite;
                darkButtonDefaultSprite = darkButtonImage.sprite;
            }

            protected virtual void ButtonClick()
            {
                questOverlay.SelectQuest(this);
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
