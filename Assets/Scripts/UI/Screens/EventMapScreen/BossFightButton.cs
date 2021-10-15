using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class BossFightButton : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static BossFightButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/BossFightButton"), parent);
                newItem.name = nameof(BossFightButton);
                return newItem.AddComponent<BossFightButton>();
            }
        }
    }
}
