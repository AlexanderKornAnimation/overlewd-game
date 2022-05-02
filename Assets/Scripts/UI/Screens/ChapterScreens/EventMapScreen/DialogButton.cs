using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class DialogButton : BaseStageButton
        {
            protected override void Customize()
            {
                base.Customize();
                var dialogData = stageData.dialogData;
                title.text = dialogData.title;
                if (anim != null)
                {
                    anim.Initialize("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/dialog/Idle_SkeletonData");
                    anim.PlayAnimation("action", false);
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<DialogScreen>().
                    SetData(new DialogScreenInData
                    {
                        eventStageId = stageId
                    }).RunShowScreenProcess();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                    ("Prefabs/UI/Screens/ChapterScreens/DialogButton", parent);
            }
        }
    }
}
