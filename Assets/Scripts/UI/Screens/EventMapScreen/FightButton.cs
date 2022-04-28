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
                var stageData = GameData.GetEventStageById(stageId);
                var battleId = stageData.battleId;
                
                if (battleId.HasValue)
                {
                    var battleData = GameData.GetBattleById(battleId.Value);
                    loot.text = battleData.rewardSpriteString;
                }
                
                title.text = stageData.title;
                fightDone.SetActive(stageData.status == AdminBRO.EventStageItem.Status_Complete);
                
                if (stageId == 1)
                {
                    button.gameObject.AddComponent<BlendPulseSelector>();
                    
                    if (GameData.GetEventStageById(stageId).status == AdminBRO.EventStageItem.Status_Complete)
                    {
                        Destroy(button.gameObject.GetComponent<Selector>());
                    }
                }
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
                    ("Prefabs/UI/Screens/EventMapScreen/FightButton", parent);
            }
        }
    }
}