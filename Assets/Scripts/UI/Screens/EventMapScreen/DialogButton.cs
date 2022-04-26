using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class DialogButton : MonoBehaviour
        {
            public int stageId { get; set; }

            private Button button;
            private GameObject dialogDone;
            private TextMeshProUGUI title;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                dialogDone = button.transform.Find("Done").gameObject;
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var eventStageData = GameData.GetEventStageById(stageId);

                title.text = eventStageData.title;
                dialogDone.SetActive(eventStageData.status == AdminBRO.EventStageItem.Status_Complete);
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<DialogScreen>().
                    SetData(stageId).RunShowScreenProcess();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                    ("Prefabs/UI/Screens/EventMapScreen/DialogButton", parent);
            }
        }
    }
}
