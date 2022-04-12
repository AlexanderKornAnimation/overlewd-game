using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class SexSceneButton : BaseStageButton
        {
            protected override void Start()
            {
                base.Start();

                var dialogId = stageData?.dialogId;
                if (dialogId.HasValue)
                {
                    var dialogData = GameData.GetDialogById(dialogId.Value);
                    title.text = dialogData.title;
                }
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<FTUE.SexScreen>().
                    SetStageData(stageData).RunShowScreenProcess();
            }

            public static SexSceneButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SexSceneButton>
                    ("Prefabs/UI/Screens/MapScreen/SexSceneButton", parent);
            }
        }
    }
}