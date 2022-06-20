using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketPopup
    {
        public class CrystalPackSmall : BaseCurrencyPack
        {
            public static CrystalPackSmall GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CrystalPackSmall>
                    ("Prefabs/UI/Popups/MarketPopup/CrystalPackSmall", parent);
            }
        }
    }
}
