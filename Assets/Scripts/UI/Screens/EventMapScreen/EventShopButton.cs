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
            private Button button;
            private Text title;
            private Text description;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                title = button.transform.Find("Title").GetComponent<Text>();
                description = button.transform.Find("Description").GetComponent<Text>();
            }

            void Update()
            {

            }

            private void ButtonClick()
            {
                UIManager.ShowScreen<EventMarketScreen>();
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
