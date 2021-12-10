using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class SexSceneButton : MonoBehaviour
        {
            protected Button button;
            protected Transform sceneDone;
            protected TextMeshProUGUI title;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                sceneDone = button.transform.Find("SceneDone");
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                UIManager.ShowScreen<SexScreen>();
            }

            public static SexSceneButton GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/SexSceneButton"), parent);
                newItem.name = nameof(SexSceneButton);

                return newItem.AddComponent<SexSceneButton>();
            }
        }
    }
}