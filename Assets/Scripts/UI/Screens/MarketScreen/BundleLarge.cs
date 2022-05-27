using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleLarge : MonoBehaviour
        {
            public static BundleLarge GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleLarge>
                    ("Prefabs/UI/Screens/MarketScreen/BundleLarge", parent);
            }
        }
    }
}
