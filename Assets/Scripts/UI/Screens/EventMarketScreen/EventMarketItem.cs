using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventMarketScreen
    {
        public class EventMarketItem : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static EventMarketItem GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMarketScreen/Item"), parent);
                newItem.name = nameof(EventMarketItem);
                return newItem.AddComponent<EventMarketItem>();
            }
        }
    }
}
