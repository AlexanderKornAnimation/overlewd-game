using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class PortalButton : Overlewd.NSCastleScreen.PortalButton
            {
               
                public new static PortalButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/PortalButton"), parent);
                    newItem.name = nameof(PortalButton);

                    return newItem.AddComponent<PortalButton>();
                }
            }
        }
    }
}
