using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class EventsWidget : Overlewd.EventsWidget
        {
            protected override void OnEventButtonClick()
            {

            }

            public new static EventsWidget GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateScreenPrefab<EventsWidget>
                    ("Prefabs/UI/Widgets/EventsWidget/EventsWidget", parent);
            }
        }
    }
}