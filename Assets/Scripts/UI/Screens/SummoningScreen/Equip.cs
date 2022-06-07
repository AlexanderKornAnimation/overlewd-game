using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class Equip : Item
        {
            public static Equip GetInsance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Equip>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/Equip/EquipAnim", parent);
            }
        }
    }
}
