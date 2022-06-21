using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MunicipalityBuilding : BaseBuilding
        {
            public static MunicipalityBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MunicipalityBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/MunicipalityBuilding", parent);
            }
        }
    }
}
