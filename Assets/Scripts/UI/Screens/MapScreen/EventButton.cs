using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class EventButton : MonoBehaviour
        {
            private Button button;
            private Text title;
            private Text description;
            private Image icon;
            private Image arrowTop;
            private Image arrowLeft;
            private Image arrowBot;
            
            private void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                
                title = button.transform.Find("Title").GetComponent<Text>();
                description = button.transform.Find("Description").GetComponent<Text>();
                
                icon = button.transform.Find("Icon").GetComponent<Image>();
                arrowTop = button.transform.Find("ArrowTop").GetComponent<Image>();
                arrowLeft = button.transform.Find("ArrowLeft").GetComponent<Image>();
                arrowBot = button.transform.Find("ArrowBot").GetComponent<Image>();

                button.onClick.AddListener(ButtonClick);
            }

            private void ButtonClick()
            {
                UIManager.ShowScreen<EventMapScreen>();
            }

            public static EventButton GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/Event"), parent);
                newItem.name = nameof(EventButton);

                return newItem.AddComponent<EventButton>();
            }
        }
    }
}