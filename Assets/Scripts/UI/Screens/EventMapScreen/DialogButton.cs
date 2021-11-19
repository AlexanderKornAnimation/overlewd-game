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
            public int eventStageId;

            private Button button;
            private GameObject dialogDone;
            private Text title;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                dialogDone = button.transform.Find("DialogueDone").gameObject;
                title = button.transform.Find("Title").GetComponent<Text>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var eventStageData = GameData.GetEventStageById(eventStageId);

                title.text = eventStageData.title;
                dialogDone.SetActive(eventStageData.status == AdminBRO.EventStageStatus.Complete);
            }

            private void ButtonClick()
            {
                GameGlobalStates.dialog_EventStageId = eventStageId;
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
