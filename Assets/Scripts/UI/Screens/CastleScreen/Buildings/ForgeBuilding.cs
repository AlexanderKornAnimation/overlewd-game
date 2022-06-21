using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class ForgeBuilding : BaseBuilding
        {
            public static ForgeBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ForgeBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/ForgeBuilding", parent);
            }
        }
    }
}
