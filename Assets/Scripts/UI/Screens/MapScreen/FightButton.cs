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
            protected GameObject markers;
            protected GameObject eventMark1;
            protected GameObject eventMark2;
            protected GameObject eventMark3;
            protected GameObject mainQuestMark;
            protected GameObject sideQuestMark;

            protected override void Awake()
            {
                base.Awake();

                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
                icon = button.transform.Find("Icon").gameObject;
                bossIcon = button.transform.Find("BossIcon").gameObject;
                markers = button.transform.Find("Markers").gameObject;
                eventMark1 = markers.transform.Find("EventMark1").gameObject;
                eventMark2 = markers.transform.Find("EventMark2").gameObject;
                eventMark3 = markers.transform.Find("EventMark3").gameObject;
                mainQuestMark = markers.transform.Find("MainQuestMark").gameObject;
                sideQuestMark = markers.transform.Find("SideQuestMark").gameObject;
            }

            protected override void Start()
            {
                base.Start();

                var battleId = stageData?.battleId;
                if (battleId.HasValue)
                {
                    var battleData = GameData.GetBattleById(battleId.Value);
                    title.text = battleData.title;
                    icon.SetActive(battleData.type == AdminBRO.Battle.Type_Battle);
                    bossIcon.SetActive(battleData.type == AdminBRO.Battle.Type_Boss);
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                GameGlobalStates.ftue_StageKey = stageData?.key;
                UIManager.ShowPopup<FTUE.PrepareBattlePopup>();
            }

            public static FightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FightButton>
                    ("Prefabs/UI/Screens/MapScreen/FightButton", parent);
            }
        }
    }
}

