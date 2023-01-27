using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMatriarchMemoryListScreen
    {
        public class MemoryBanner : BaseMemoryBanner
        {
            public static MemoryBanner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MemoryBanner>(
                    "Prefabs/UI/Screens/MatriarchMemoryListScreen/MemoryBanner", parent);
            }
        }
    }
}
