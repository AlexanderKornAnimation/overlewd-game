using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class BattleGirl : BaseShard
        {
            public static BattleGirl GetInsance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattleGirl>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/BattleGirl/BattleGirl", parent);
            }
        }
    }
}
