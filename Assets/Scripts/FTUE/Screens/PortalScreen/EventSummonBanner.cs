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
            public class EventSummonBanner : Overlewd.NSPortalScreen.EventSummonBanner
            {
               
                public new static EventSummonBanner GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/EventSummonBanner"), parent);
                    newItem.name = nameof(EventSummonBanner);
                    return newItem.AddComponent<EventSummonBanner>();
                }
            }
        }
    }
}
