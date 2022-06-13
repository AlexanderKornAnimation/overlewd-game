using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class PortalBuilding : BaseBuilding
        {

            public static PortalBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<PortalBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/PortalBuilding", parent);
            }
        }
    }
}
