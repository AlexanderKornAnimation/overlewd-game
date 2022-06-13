using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CastleBuilding : BaseBuilding
        {
            public static CastleBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CastleBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/CastleBuilding", parent);
            }
        }
    }
}
