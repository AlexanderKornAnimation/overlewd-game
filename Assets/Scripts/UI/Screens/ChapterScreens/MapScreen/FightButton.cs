using System;
using System.Collections;
using System.Collections.Generic;
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

                switch (battleData.type)
                {
                    case AdminBRO.Battle.Type_Battle:
                        SetAnimation("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/battle/Idle_SkeletonData");
                        break;
                    case AdminBRO.Battle.Type_Boss:
                        SetAnimation("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/Boss/Idle_SkeletonData");
                        break;
                }
            }

            protected virtual void SetAnimation(string animPath)
            {
                if (anim != null)
                {
                    anim.Initialize(animPath);
                    anim.PlayAnimation("action", false);
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                var battleData = stageData.battleData;

                var directToBattleScreen = GameGlobalStates.ftueChapterData.key switch
                {
                    "chapter1" => stageData.key switch
                    {
                        "battle1" => true,
                        "battle2" => true,
                        "battle3" => true,
                        "battle4" => true,
                        _ => false
                    },
                    _ => false
                };

                if (battleData.type == AdminBRO.Battle.Type_Battle)
                {
                    if (directToBattleScreen)
                    {
                        UIManager.MakeScreen<FTUE.BattleScreen>().
                            SetData(new BattleScreenInData
                        {
                            ftueStageId = stageId
                        }).RunShowScreenProcess();
                    }
                    else
                    {
                        UIManager.MakePopup<FTUE.PrepareBattlePopup>().
                            SetData(new PrepareBattlePopupInData
                        {
                            ftueStageId = stageId
                        }).RunShowPopupProcess();
                    }
                }
                else if (battleData.type == AdminBRO.Battle.Type_Boss)
                {
                    if (directToBattleScreen)
                    {
                        UIManager.MakeScreen<FTUE.BossFightScreen>().SetData(new BossFightScreenInData
                        {
                            ftueStageId = stageId
                        }).RunShowScreenProcess();
                    }
                    else
                    {
                        UIManager.MakePopup<FTUE.PrepareBossFightPopup>().SetData(new PrepareBossFightPopupInData
                        {
                            ftueStageId = stageId
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