using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class FightButton : BaseStageButton
        {
            protected TextMeshProUGUI loot;
            protected GameObject icon;
            protected GameObject bossIcon;
            protected TextMeshProUGUI markers;


            private AdminBRO.Battle battleData;

            protected override void Awake()
            {
                base.Awake();

                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
                icon = button.transform.Find("Icon").gameObject;
                bossIcon = button.transform.Find("BossIcon").gameObject;
                markers = button.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            }

            protected override void Start()
            {
                base.Start();

                var battleId = stageData?.battleId;
                if (battleId.HasValue)
                {
                    battleData = GameData.GetBattleById(battleId.Value);
                    title.text = battleData.title;
                    icon.SetActive(battleData.type == AdminBRO.Battle.Type_Battle);
                    bossIcon.SetActive(battleData.type == AdminBRO.Battle.Type_Boss);
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                if (battleData.type == AdminBRO.Battle.Type_Battle)
                {
                    UIManager.MakePopup<FTUE.PrepareBattlePopup>().
                        SetStageData(stageData).RunShowPopupProcess();
                }
                else if (battleData.type == AdminBRO.Battle.Type_Boss)
                {
                    UIManager.MakePopup<FTUE.PrepareBossFightPopup>().
                        SetStageData(stageData).RunShowPopupProcess();
                }
            }

            public static FightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FightButton>
                    ("Prefabs/UI/Screens/MapScreen/FightButton", parent);
            }
        }
    }
}

