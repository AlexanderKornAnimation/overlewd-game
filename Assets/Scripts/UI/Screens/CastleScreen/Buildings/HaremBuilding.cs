using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class HaremBuilding : BaseBuilding
        {
            public static HaremBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<HaremBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/HaremBuilding", parent);
            }
        }
    }
}
