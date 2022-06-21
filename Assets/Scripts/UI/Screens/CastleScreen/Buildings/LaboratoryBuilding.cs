using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class LaboratoryBuilding : BaseBuilding
        {
            public static LaboratoryBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<LaboratoryBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/LaboratoryBuilding", parent);
            }
        }
    }
}
