using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class BuffWidget : Overlewd.BuffWidget
        {
            public new static BuffWidget GetInstance(Transform parent)
            {
                var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/BuffWidget/BuffWidget"), parent);
                prefab.name = nameof(BuffWidget);
                var rectTransform = prefab.GetComponent<RectTransform>();
                UIManager.SetStretch(rectTransform);
                return prefab.AddComponent<BuffWidget>();
            }
        }
    }
}

