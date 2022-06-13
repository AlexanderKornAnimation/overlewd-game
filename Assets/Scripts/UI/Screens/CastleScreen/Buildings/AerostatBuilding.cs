using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class AerostatBuilding : BaseBuilding
        {
            public static AerostatBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AerostatBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/AerostatBuilding", parent);
            }
        }
    }
}
