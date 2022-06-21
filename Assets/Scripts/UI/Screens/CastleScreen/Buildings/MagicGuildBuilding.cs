using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MagicGuildBuilding : BaseBuilding
        {
            public static MagicGuildBuilding GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MagicGuildBuilding>(
                    "Prefabs/UI/Screens/CastleScreen/Buildings/MagicGuildBuilding", parent);
            }
        }
    }
}
