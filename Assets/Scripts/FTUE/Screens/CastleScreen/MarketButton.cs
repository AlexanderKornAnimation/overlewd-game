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
                
                public new static MarketButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/MarketButton"), parent);
                    newItem.name = nameof(MarketButton);

                    return newItem.AddComponent<MarketButton>();
                }
            }
        }
    }
}