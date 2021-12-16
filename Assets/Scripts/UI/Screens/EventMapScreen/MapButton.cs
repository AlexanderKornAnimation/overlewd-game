using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class MapButton : MonoBehaviour
        {
            private Button button;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            }

            private void ButtonClick()
            {
                UIManager.ShowScreen<MapScreen>();
            }

            void Update()
            {
                
            }

            public static MapButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/MapButton"), parent);
                newItem.name = nameof(MapButton);
                return newItem.AddComponent<MapButton>();
            }
        }
    }
}
