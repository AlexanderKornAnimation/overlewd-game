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
                var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/EventsWidget/EventsWidget"), parent);
                prefab.name = nameof(EventsWidget);
                var rectTransform = prefab.GetComponent<RectTransform>();
                UIManager.SetStretch(rectTransform);
                return prefab.AddComponent<EventsWidget>();
            }
        }
    }
}