using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class MapButton : MonoBehaviour
        {
            private Button button;
            private Text title;
            private Text description;

            void Start()
            {
                button = transform.GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                title = button.transform.Find("Title").GetComponent<Text>();
                description = button.transform.Find("Description").GetComponent<Text>();
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
