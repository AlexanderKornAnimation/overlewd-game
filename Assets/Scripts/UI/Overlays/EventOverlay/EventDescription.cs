using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class EventDescription : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static EventDescription GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/EventDescription"), parent);
                newItem.name = nameof(EventDescription);
                return newItem.AddComponent<EventDescription>();
            }
        }
    }
}
