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
            void Start()
            {
                transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.ShowScreen<MapScreen>();
                });
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
