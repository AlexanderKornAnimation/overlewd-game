using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class SexButton : MonoBehaviour
        {
            public AdminBRO.EventStageItem eventStageData { get; set; }

            private Button button;
            private Transform sceneDone;
            private Text title;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                sceneDone = button.transform.Find("SceneDone");
                title = button.transform.Find("Title").GetComponent<Text>();
            }

            void Update()
            {

            }

            private void ButtonClick()
            {
                GameGlobalStates.sex_EventStageData = eventStageData;
                UIManager.ShowScreen<SexScreen>();
            }

            public static SexButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/SexSceneButton"), parent);
                newItem.name = nameof(SexButton);
                return newItem.AddComponent<SexButton>();
            }
        }
    }
}
