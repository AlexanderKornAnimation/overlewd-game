using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class QuestsWidget : Overlewd.BaseWidget
        {
            public static QuestsWidget GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                    ("Prefabs/UI/Widgets/QuestsWidget/QuestWidgetFTUE", parent);
            }
        }
    }
}
