using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeD : MonoBehaviour
        {
            public static BundleTypeD GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleTypeD>
                    ("Prefabs/UI/Screens/MarketScreen/Bundle4", parent);
            }
        }
    }
}
