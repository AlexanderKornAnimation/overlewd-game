using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class MarketButton : Overlewd.NSCastleScreen.MarketButton
            {
                protected override void ButtonClick()
                {
                    
                }

                protected override void Customize()
                {
                    base.Customize();

                }

                public new static MarketButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<MarketButton>
                        ("Prefabs/UI/Screens/CastleScreen/MarketButton", parent);
                }
            }
        }
    }
}