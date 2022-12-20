using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class CurrencyPacksOffer : BaseOffer
        {            
            private Transform content;

            protected override void Awake()
            {
                base.Awake();

                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
            }

            protected override void Customize()
            {
                base.Customize();

                var tData = offerButton.tabData;
                foreach (var goodId in tData.goods)
                {
                    var pack = CurrencyPack.GetInstance(content);
                    pack.packOffer = this;
                    pack.tradableId = goodId;
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
