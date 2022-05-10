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
            protected GameObject icon;
            protected GameObject bossIcon;
            
            private TextMeshProUGUI loot;
            private TextMeshProUGUI markers;

            protected override void Awake()
            {
                base.Awake();
                icon = button.transform.Find("Icon").gameObject;
                bossIcon = button.transform.Find("BossIcon").gameObject;
                markers = button.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                base.Customize();
                var battleData = stageData.battleData;
                title.text = stageData.title;
                loot.text = battleData.rewardSpriteString;
                
                icon.SetActive(battleData.isTypeBattle);
                bossIcon.SetActive(battleData.isTypeBoss);
                
                switch (battleData.type)
                {
                    case AdminBRO.Battle.Type_Battle:
                        SetAnimation("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/battle/BattleButtonAnim");
                        break;
                    case AdminBRO.Battle.Type_Boss:
                        SetAnimation("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/Boss/BossButtonAnim");
                        break;
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                var battleData = stageData.battleData;

                if (battleData.isTypeBattle)
                {
                    UIManager.MakePopup<PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            eventStageId = stageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            eventStageId = stageId
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