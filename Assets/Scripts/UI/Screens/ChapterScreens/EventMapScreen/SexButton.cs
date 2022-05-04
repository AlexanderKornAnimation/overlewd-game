using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class SexButton : BaseStageButton
        {
            protected override void Customize()
            {
                base.Customize();
                var eventStageData = stageData;
                title.text = eventStageData.title;
                
                if (anim != null)
                {
                    anim.Initialize("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/sex_scene/Idle_SkeletonData", false,
                        "Prefabs/UI/Screens/ChapterScreens/FX/StageNew/sex_scene/Idle_Material");
                    anim.PlayAnimation("action", false);
                }
            } 

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        eventStageId = stageId
                    }).RunShowScreenProcess();
            }

            public static SexButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SexButton>
                    ("Prefabs/UI/Screens/ChapterScreens/SexSceneButton", parent);
            }
        }
    }
}
