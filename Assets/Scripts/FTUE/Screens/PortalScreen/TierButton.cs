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
            public class TierButton : Overlewd.NSPortalScreen.TierButton
            {
                
                public new static TierButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/TierButton"), parent);
                    newItem.name = nameof(TierButton);
                    return newItem.AddComponent<TierButton>();
                }
            }
        }
    }
}
