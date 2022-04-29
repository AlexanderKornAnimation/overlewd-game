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
            protected override void Start()
            {
                base.Start();

                var dialogData = stageData.dialogData;
                title.text = dialogData.title;
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<FTUE.DialogScreen>().
                    SetData(new DialogScreenInData
                    {
                        ftueStageId = stageId
                    }).RunShowScreenProcess();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                    ("Prefabs/UI/Screens/ChapterScreens/MapScreen/DialogButton", parent);
            }
        }
    }
}
