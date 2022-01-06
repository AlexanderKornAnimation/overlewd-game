using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeA : MonoBehaviour
        {
            public static BundleTypeA GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleTypeA>
                    ("Prefabs/UI/Screens/MarketScreen/Bundle1", parent);
            }
        }
    }
}
