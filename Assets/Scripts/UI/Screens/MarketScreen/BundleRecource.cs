using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleRecource : MonoBehaviour
        {
            public static BundleRecource GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BundleRecource>
                    ("Prefabs/UI/Screens/MarketScreen/BundleRecource", parent);
            }
        }
    }
}
