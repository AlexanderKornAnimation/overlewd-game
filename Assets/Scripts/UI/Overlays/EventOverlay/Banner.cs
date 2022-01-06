using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class Banner : MonoBehaviour
        {
            public static Banner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Banner>
                    ("Prefabs/UI/Overlays/EventOverlay/Banner", parent);
            }
        }
    }
}
