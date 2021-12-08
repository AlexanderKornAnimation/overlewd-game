using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSPortalScreen
        {
            public class BattleGirlsBannerTypeB : Overlewd.NSPortalScreen.BattleGirlsBannerTypeB
            {
                public new static BattleGirlsBannerTypeB GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/BattleGirlsBannerTypeB"), parent);
                    newItem.name = nameof(BattleGirlsBannerTypeB);
                    return newItem.AddComponent<BattleGirlsBannerTypeB>();
                }
            }
        }
    }
}
