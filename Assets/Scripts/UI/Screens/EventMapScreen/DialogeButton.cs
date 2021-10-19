using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class DialogeButton : MonoBehaviour
        {
            void Start()
            {
                transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.ShowScreen<DialogScreen>();
                });

                var rectTransform = GetComponent<RectTransform>();
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 400.0f, rectTransform.rect.width);
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 400.0f, rectTransform.rect.height);
            }

            void Update()
            {

            }

            public static DialogeButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/DialogueButton"), parent);
                newItem.name = nameof(DialogeButton);
                return newItem.AddComponent<DialogeButton>();
            }
        }
    }
}