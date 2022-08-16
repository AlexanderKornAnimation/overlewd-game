using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public class IngieContent : BaseContent
        {
            public static IngieContent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<IngieContent>("Prefabs/UI/Screens/MemoryScreen/Ingie",
                    parent);
            }
        }
    }
}