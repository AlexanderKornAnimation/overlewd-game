using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleMedium : MonoBehaviour
        {
            public static BundleMedium GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleMedium>
                    ("Prefabs/UI/Screens/MarketScreen/BundleMedium", parent);
            }
        }
    }
}
