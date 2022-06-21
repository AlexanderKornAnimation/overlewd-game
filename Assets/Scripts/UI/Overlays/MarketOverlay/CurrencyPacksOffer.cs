using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketPopup
    {
        public class CurrencyPacksOffer : BaseOffer
        {
            private List<BaseCurrencyPack> packs = new List<BaseCurrencyPack>();
            
            private Transform content;

            protected override void Awake()
            {
                base.Awake();

                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
            }

            protected override void Customize()
            {
                base.Customize();

                for (int i = 0; i <= 3; i++)
                {
                    packs.Add(CurrencyPack.GetInstance(content));
                    packs.Add(CrystalPackSmall.GetInstance(content));
                    packs.Add(CrystalPackMedium.GetInstance(content));
                    packs.Add(CrystalPackLarge.GetInstance(content));
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
