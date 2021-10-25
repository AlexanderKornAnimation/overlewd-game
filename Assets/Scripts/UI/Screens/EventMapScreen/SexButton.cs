using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class SexButton : MonoBehaviour
        {
            void Start()
            {
                transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.ShowScreen<SexScreen>();
                });
            }

            void Update()
            {

            }

            public static SexButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/SexSceneButton"), parent);
                newItem.name = nameof(SexButton);
                return newItem.AddComponent<SexButton>();
            }
        }
    }
}
