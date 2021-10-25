using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class EventItem : MonoBehaviour
        {
            void Start()
            {
                transform.Find("Canvas").Find("MapButton").GetComponent<Button>().onClick.AddListener(() => 
                {
                    UIManager.ShowScreen<EventMapScreen>();
                });
            }

            void Update()
            {

            }

            public static EventItem GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/EventItem"), parent);
                newItem.name = nameof(EventItem);
                return newItem.AddComponent<EventItem>();
            }
        }
    }
}
