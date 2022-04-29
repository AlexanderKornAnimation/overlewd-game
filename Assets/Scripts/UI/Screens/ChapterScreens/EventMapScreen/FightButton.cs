using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class FightButton : BaseStageButton
        {
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

                fightDone = button.transform.Find("FightDone").gameObject;
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
                var eventStageData = stageData;
                var battleData = stageData.battleData;
                title.text = eventStageData.title;
                loot.text = battleData.rewardSpriteString;
                fightDone.SetActive(eventStageData.status == AdminBRO.EventStageItem.Status_Complete);
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakePopup<PrepareBattlePopup>().
                    SetData(new PrepareBattlePopupInData
                    {
                        eventStageId = stageId
                    }).RunShowPopupProcess();
            }

            public static FightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FightButton>
                    ("Prefabs/UI/Screens/ChapterScreens/EventMapScreen/FightButton", parent);
            }
        }
    }
}