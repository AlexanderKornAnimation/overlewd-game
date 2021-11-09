using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class SexSceneButton : MonoBehaviour
        {
            private Button button;
            private Transform sceneDone;
            private Text title;

            private void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                sceneDone = button.transform.Find("SceneDone");
                title = button.transform.Find("Title").GetComponent<Text>();

                button.onClick.AddListener(ButtonClick);
            }

            private void ButtonClick()
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