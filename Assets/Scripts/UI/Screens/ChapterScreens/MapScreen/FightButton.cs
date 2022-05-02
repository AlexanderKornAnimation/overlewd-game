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
            
            protected override void Awake()
            {
                base.Awake();
                
                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
                icon = button.transform.Find("Icon").gameObject;
                bossIcon = button.transform.Find("BossIcon").gameObject;
                markers = button.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                base.Customize();
                var battleData = stageData.battleData;
                title.text = battleData.title;
                loot.text = battleData.rewardSpriteString;
                icon.SetActive(battleData.type == AdminBRO.Battle.Type_Battle);
                bossIcon.SetActive(battleData.type == AdminBRO.Battle.Type_Boss);
                
                if (anim != null)
                {
                    anim.Initialize("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/battle/Idle_SkeletonData");
                    anim.PlayAnimation("action", false);
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                var battleData = stageData.battleData;
                if (battleData.type == AdminBRO.Battle.Type_Battle)
                {
                    UIManager.MakePopup<FTUE.PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            ftueStageId = stageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.type == AdminBRO.Battle.Type_Boss)
                {
                    UIManager.MakePopup<FTUE.PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            ftueStageId = stageId
                        }).RunShowPopupProcess();
                }
            }

            public static FightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FightButton>
                    ("Prefabs/UI/Screens/ChapterScreens/FightButton", parent);
            }
        }
    }
}

