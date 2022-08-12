using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public class UlviContent : BaseContent
        {

            public static UlviContent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<UlviContent>("Prefabs/UI/Screens/MemoryScreen/Ulvi",
                    parent);
            }
        }
    }
}