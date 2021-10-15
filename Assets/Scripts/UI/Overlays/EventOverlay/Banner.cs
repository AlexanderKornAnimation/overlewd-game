using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class Banner : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static Banner GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/Banner"), parent);
                newItem.name = nameof(Banner);
                return newItem.AddComponent<Banner>();
            }
        }
    }
}
