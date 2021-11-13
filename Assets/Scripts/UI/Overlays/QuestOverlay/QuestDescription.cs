using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestDescription : MonoBehaviour
        {
            private Text topText;
            private Image girlEmotion1;
            private Text bottomText;
            private Image girlEmotion2;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                topText = canvas.Find("TopText").GetComponent<Text>();
                girlEmotion1 = canvas.Find("GirlEmotion1").GetComponent<Image>();
                bottomText = canvas.Find("BottomText").GetComponent<Text>();
                girlEmotion2 = canvas.Find("GirlEmotion2").GetComponent<Image>();
            }

            public static QuestDescription GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/QuestDescription"), parent);
                newItem.name = nameof(QuestDescription);
                return newItem.AddComponent<QuestDescription>();
            }
        }
    }
}
