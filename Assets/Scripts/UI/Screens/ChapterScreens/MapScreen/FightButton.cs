using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

                var directToBattleScreen = stageData.ftueState switch
                {
                    ("battle1", "chapter1") => true,
                    ("battle2", "chapter1") => true,
                    ("battle3", "chapter1") => true,
                    ("battle4", "chapter1") => true,
                    _ => false
                };

                if (battleData.isTypeBattle)
                {
                    if (directToBattleScreen)
                    {
                        UIManager.MakeScreen<BattleScreen>().
                            SetData(new BattleScreenInData
                            {
                                ftueStageId = stageId,
                            }).RunShowScreenProcess();
                    }
                    else
                    {
                        UIManager.MakePopup<PrepareBattlePopup>().
                            SetData(new PrepareBattlePopupInData
                            {
                                prevScreenInData = screenInData,
                                ftueStageId = stageId,
                            }).RunShowPopupProcess();
                    }
                }
                else if (battleData.isTypeBoss)
                {
                    if (directToBattleScreen)
                    {
                        UIManager.MakeScreen<BossFightScreen>().
                            SetData(new BossFightScreenInData
                            {
                                ftueStageId = stageId,
                            }).RunShowScreenProcess();
                    }
                    else
                    {
                        UIManager.MakePopup<PrepareBossFightPopup>().
                            SetData(new PrepareBossFightPopupInData
                            {
                                prevScreenInData = screenInData,
                                ftueStageId = stageId,
                            }).RunShowPopupProcess();
                    }
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