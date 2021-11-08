using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class Description : MonoBehaviour
        {
            private Text topText;
            private Image girlEmotion1;
            private Text bottomText;
            private Image girlEmotion2;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                topText = canvas.Find("TopText").GetComponent<Text>();
                girlEmotion1 = canvas.Find("GirlEmotion1").GetComponent<Image>();
                bottomText = canvas.Find("BottomText").GetComponent<Text>();
                girlEmotion2 = canvas.Find("GirlEmotion2").GetComponent<Image>();
            }

            void Update()
            {

            }

            public static Description GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/Description"), parent);
                newItem.name = nameof(Description);
                return newItem.AddComponent<Description>();
            }
        }
    }
}
