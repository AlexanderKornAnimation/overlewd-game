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
            protected override void OnQuestButtonClick()
            {

            }

            public new static QuestsWidget GetInstance(Transform parent)
            {
                var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/QuestsWidget/QuestWidget"), parent);
                prefab.name = nameof(QuestsWidget);
                var rectTransform = prefab.GetComponent<RectTransform>();
                UIManager.SetStretch(rectTransform);
                return prefab.AddComponent<QuestsWidget>();
            }
        }
    }
}
