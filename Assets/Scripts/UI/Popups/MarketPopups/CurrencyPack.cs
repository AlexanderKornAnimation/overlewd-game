using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketPopup
    {
        public class CurrencyPack : BaseCurrencyPack
        {
            public static CurrencyPack GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CurrencyPack>
                    ("Prefabs/UI/Popups/MarketPopup/CurrencyPack", parent);
            }
        }
    }
}
