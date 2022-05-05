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

                if (battleData.type == AdminBRO.Battle.Type_Battle)
                {
                    UIManager.MakePopup<PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            eventStageId = stageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.type == AdminBRO.Battle.Type_Boss)
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