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
            public class BattleGirlsBannerTypeA : Overlewd.NSPortalScreen.BattleGirlsBannerTypeA
            {
                public new static BattleGirlsBannerTypeA GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/BattleGirlsBannerTypeA"), parent);
                    newItem.name = nameof(BattleGirlsBannerTypeA);
                    return newItem.AddComponent<BattleGirlsBannerTypeA>();
                }
            }
        }
    }
}
