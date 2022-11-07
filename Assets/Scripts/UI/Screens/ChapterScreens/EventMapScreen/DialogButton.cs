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
                
                SetAnimation("Prefabs/UI/Screens/ChapterScreens/FX/StageNew/dialog/DialogButtonAnim");

            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<DialogScreen>().
                    SetData(new DialogScreenInData
                    {
                        eventStageId = stageId
                    }).DoShow();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                    ("Prefabs/UI/Screens/ChapterScreens/DialogButton", parent);
            }
        }
    }
}
