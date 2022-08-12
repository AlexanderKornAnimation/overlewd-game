using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public class AdrielContent : BaseContent
        {
            public static AdrielContent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AdrielContent>("Prefabs/UI/Screens/MemoryScreen/Adriel",
                    parent);
            }
        }
    }
}
