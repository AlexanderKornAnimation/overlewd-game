using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class DialogButton : MonoBehaviour
        {
            public AdminBRO.EventStageItem eventStageData { get; set; }

            private Button button;
            private Transform dialogDone;
            private Text title;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                dialogDone = button.transform.Find("DialogueDone");
                title = button.transform.Find("Title").GetComponent<Text>();
            }

            void Update()
            {

            }

            private void ButtonClick()
            {
                GameGlobalStates.dialog_EventStageData = eventStageData;
                UIManager.ShowScreen<DialogScreen>();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/DialogButton"), parent);
                newItem.name = nameof(DialogButton);
                return newItem.AddComponent<DialogButton>();
            }
        }
    }
}
