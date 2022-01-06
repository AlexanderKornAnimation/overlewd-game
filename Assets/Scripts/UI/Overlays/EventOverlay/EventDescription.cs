using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class EventDescription : MonoBehaviour
        {
            public static EventDescription GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventDescription>
                    ("Prefabs/UI/Overlays/EventOverlay/EventDescription", parent);
            }
        }
    }
}
