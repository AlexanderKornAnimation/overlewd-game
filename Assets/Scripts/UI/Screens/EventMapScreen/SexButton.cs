using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class SexButton : MonoBehaviour
        {
            public int stageId { get; set; }

            private Button button;
            private GameObject sceneDone;
            private TextMeshProUGUI title;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                sceneDone = button.transform.Find("SceneDone").gameObject;
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
                sceneDone.SetActive(eventStageData.status == AdminBRO.EventStageItem.Status_Complete);
            } 

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        eventStageId = stageId
                    }).RunShowScreenProcess();
            }

            public static SexButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SexButton>
                    ("Prefabs/UI/Screens/EventMapScreen/SexSceneButton", parent);
            }
        }
    }
}
