using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
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
                    ftueStageId = stageId
                }).RunShowScreenProcess();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>(
                    "Prefabs/UI/Screens/ChapterScreens/DialogButton", parent);
            }
        }
    }
}