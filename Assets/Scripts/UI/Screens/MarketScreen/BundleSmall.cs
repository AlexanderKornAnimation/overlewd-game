using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleSmall : MonoBehaviour
        {
            public static BundleSmall GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleSmall>
                    ("Prefabs/UI/Screens/MarketScreen/BundleSmall", parent);
            }
        }
    }
}
