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
            protected override void ButtonClick()
            {

            }

            public new static BuffWidget GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateScreenPrefab<BuffWidget>
                    ("Prefabs/UI/Widgets/BuffWidget/BuffWidget", parent);
            }
        }
    }
}

