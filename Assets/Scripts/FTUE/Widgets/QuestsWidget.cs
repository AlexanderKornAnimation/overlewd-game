using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class QuestsWidget : Overlewd.QuestsWidget
        {
            protected override void Customize()
            {

            }

            protected override void MainQuestButtonClick()
            {

            }

            public static new QuestsWidget GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                    ("Prefabs/UI/Widgets/QuestsWidget/QuestWidget", parent);
            }
        }
    }
}
