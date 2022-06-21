using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CatacombsBuilding : BaseBuilding
        {
            public static CatacombsBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CatacombsBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/CatacombsBuilding", parent);
            }
        }
    }
}
