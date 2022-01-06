using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeB : MonoBehaviour
        {
            public static BundleTypeB GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleTypeB>
                    ("Prefabs/UI/Screens/MarketScreen/Bundle2", parent);
            }
        }
    }
}
