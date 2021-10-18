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

                var rectTransform = GetComponent<RectTransform>();
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 40.0f, rectTransform.rect.width);
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 400.0f, rectTransform.rect.height);
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
