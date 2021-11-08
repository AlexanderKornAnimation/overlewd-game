using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestButton : MonoBehaviour
        {
            private Button blueButton;
            private Text blueButtonText;
            private Button darkButton;
            private Text darkButtonText;
            private GameObject newQuestMark;
            private GameObject questDoneMark;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                blueButton = canvas.Find("BlueButton").GetComponent<Button>();
                blueButton.onClick.AddListener(ButtonClick);
                blueButtonText = blueButton.transform.Find("Text").GetComponent<Text>();

                darkButton = canvas.Find("DarkButton").GetComponent<Button>();
                darkButton.onClick.AddListener(ButtonClick);
                darkButtonText = darkButton.transform.Find("Text").GetComponent<Text>();

                newQuestMark = canvas.Find("NewQuestMark").gameObject;
                questDoneMark = canvas.Find("QuestDoneMark").gameObject;
            }

            void Update()
            {

            }

            private void ButtonClick()
            {

            }

            public static QuestButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/QuestButton"), parent);
                newItem.name = nameof(QuestButton);
                return newItem.AddComponent<QuestButton>();
            }
        }
    }
}
