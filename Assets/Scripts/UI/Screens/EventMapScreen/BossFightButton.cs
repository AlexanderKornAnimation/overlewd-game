using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class BossFightButton : MonoBehaviour
        {
            public int stageId { get; set; }

            private Button button;
            private GameObject fightDone;
            private TextMeshProUGUI title;
            private TextMeshProUGUI loot;
            private TextMeshProUGUI markers;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                fightDone = button.transform.Find("Done").gameObject;
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
                markers = button.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var eventStageData = GameData.GetEventStageById(stageId);

                title.text = eventStageData.title;
                fightDone.SetActive(eventStageData.status == AdminBRO.EventStageItem.Status_Complete);
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakePopup<PrepareBossFightPopup>().
                    SetData(stageId).RunShowPopupProcess();
            }

            public static BossFightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BossFightButton>
                    ("Prefabs/UI/Screens/EventMapScreen/BossFightButton", parent);
            }
        }
    }
}
