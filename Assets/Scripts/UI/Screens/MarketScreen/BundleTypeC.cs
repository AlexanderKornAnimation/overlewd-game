using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeC : MonoBehaviour
        {
            public static BundleTypeC GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleTypeC>
                    ("Prefabs/UI/Screens/MarketScreen/Bundle3", parent);
            }
        }
    }
}
