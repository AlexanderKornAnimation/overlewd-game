using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketPopup
    {
        public class CrystalPackMedium : BaseCurrencyPack
        {
            public static CrystalPackMedium GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CrystalPackMedium>
                    ("Prefabs/UI/Overlays/MarketOverlay/CrystalPackMedium", parent);
            }
        }
    }
}
