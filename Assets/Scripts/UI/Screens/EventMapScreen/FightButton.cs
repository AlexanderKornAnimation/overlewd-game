using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class FightButton : MonoBehaviour
        {
            public int eventStageId;

            private Button button;
            private GameObject fightDone;
            private TextMeshProUGUI title;
            private TextMeshProUGUI loot;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                fightDone = button.transform.Find("FightDone").gameObject;
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {

                var eventStageData = GameData.GetEventStageById(eventStageId);

                title.text = eventStageData.title;
                fightDone.SetActive(eventStageData.status == AdminBRO.EventStageStatus.Complete);
                
                if (eventStageId == 1)
                {
                    button.gameObject.AddComponent<Bling>();
                    
                    if (GameData.GetEventStageById(eventStageId).status == AdminBRO.EventStageStatus.Complete)
                    {
                        Destroy(button.gameObject.GetComponent<Bling>());
                    }
                }
            }

            private void ButtonClick()
            {
                GameGlobalStates.battle_EventStageId = eventStageId;
                UIManager.ShowPopup<PrepareBattlePopup>();
            }

            public static FightButton GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/FightButton"),
                    parent);
                newItem.name = nameof(FightButton);
                return newItem.AddComponent<FightButton>();
            }
        }
    }
}