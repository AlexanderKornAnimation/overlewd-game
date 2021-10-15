using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class EventItem : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static EventItem GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventOverlay/EventItem"), parent);
                newItem.name = nameof(EventItem);
                return newItem.AddComponent<EventItem>();
            }
        }
    }
}
