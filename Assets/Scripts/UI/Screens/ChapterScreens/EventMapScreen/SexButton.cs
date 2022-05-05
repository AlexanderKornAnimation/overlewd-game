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
                
                SetAnimation("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/sex_scene/SexButtonAnim");
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
