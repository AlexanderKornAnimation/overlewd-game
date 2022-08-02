using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class CurrencyPacksOffer : BaseOffer
        {
            private List<CurrencyPack> packs = new List<CurrencyPack>();
            
            private Transform content;

            protected override void Awake()
            {
                base.Awake();

                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
            }

            protected override void Customize()
            {
                base.Customize();

                for (int i = 0; i <= 6; i++)
                {
                    packs.Add(CurrencyPack.GetInstance(content));
                }
            }

            public static CurrencyPacksOffer GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CurrencyPacksOffer>(
                    "Prefabs/UI/Overlays/MarketOverlay/CurrencyPacksOffer", parent);
            }
        }
    }
}
