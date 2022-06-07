using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class BattleGirls : Item
        {
            public static BattleGirls GetInsance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattleGirls>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/BattleGirls/BattleGirlsAnim", parent);
            }
        }
    }
}
