using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class EventShopButton : MonoBehaviour
        {
            void Start()
            {
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.ShowScreen<EventMarketScreen>();
                });
            }

            void Update()
            {

            }

            public static EventShopButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/EventShopButton"), parent);
                newItem.name = nameof(EventShopButton);
                return newItem.AddComponent<EventShopButton>();
            }
        }
    }
}
