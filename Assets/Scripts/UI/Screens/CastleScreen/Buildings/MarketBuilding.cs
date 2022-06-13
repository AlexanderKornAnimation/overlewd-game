using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MarketBuilding : BaseBuilding
        {
            public static MarketBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MarketBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/MarketBuilding", parent);
            }
        }
    }
}