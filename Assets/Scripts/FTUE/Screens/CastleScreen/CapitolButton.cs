using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class CapitolButton : Overlewd.NSCastleScreen.CapitolButton
            {
                
                public new static CapitolButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/CapitolButton"), parent);
                    newItem.name = nameof(CapitolButton);

                    return newItem.AddComponent<CapitolButton>();
                }
            }
        }
    }
}
