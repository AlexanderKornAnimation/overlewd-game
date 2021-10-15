using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class ComingEvent : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static ComingEvent GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventOverlay/ComingEvent"), parent);
                newItem.name = nameof(ComingEvent);
                return newItem.AddComponent<ComingEvent>();
            }
        }
    }
}
