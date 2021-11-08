using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class MainQuest : MonoBehaviour
        {
            private Text title;
            private Text progress;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                title = canvas.Find("QuestHead").Find("Title").GetComponent<Text>();
                progress = canvas.Find("QuestHead").Find("Progress").GetComponent<Text>();
            }

            void Update()
            {

            }

            public static MainQuest GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/MainQuest"), parent);
                newItem.name = nameof(MainQuest);
                return newItem.AddComponent<MainQuest>();
            }
        }
    }
}
