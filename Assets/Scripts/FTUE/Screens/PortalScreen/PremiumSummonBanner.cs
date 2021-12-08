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
            public class PremiumSummonBanner : Overlewd.NSPortalScreen.PremiumSummonBanner
            {

                public new static PremiumSummonBanner GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/PremiumSummonBanner"), parent);
                    newItem.name = nameof(PremiumSummonBanner);
                    return newItem.AddComponent<PremiumSummonBanner>();
                }
            }
        }
    }
}
