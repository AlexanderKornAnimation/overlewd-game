using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public class FayeContent : BaseContent
        {
            public static FayeContent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FayeContent>("Prefabs/UI/Screens/MemoryScreen/Faye",
                    parent);
            }
        }
    }
}