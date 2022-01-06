using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class ComingEvent : MonoBehaviour
        {
            public static ComingEvent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ComingEvent>
                    ("Prefabs/UI/Overlays/EventOverlay/ComingEvent", parent);
            }
        }
    }
}
