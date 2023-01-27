using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public class LiliContent : BaseContent
        {
            public static LiliContent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<LiliContent>("Prefabs/UI/Screens/MemoryScreen/Lili",
                    parent);
            }
        }
    }
}