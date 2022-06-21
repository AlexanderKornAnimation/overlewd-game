using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketPopup
    {
        public class CrystalPackLarge : BaseCurrencyPack
        {
            public static CrystalPackLarge GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CrystalPackLarge>
                    ("Prefabs/UI/Overlays/MarketOverlay/CrystalPackLarge", parent);
            }
        }
    }
}
