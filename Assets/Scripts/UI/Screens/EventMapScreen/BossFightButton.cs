using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class BossFightButton : MonoBehaviour
        {
            void Start()
            {
                transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.ShowScreen<PrepareBossFightScreen>();
                });
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
