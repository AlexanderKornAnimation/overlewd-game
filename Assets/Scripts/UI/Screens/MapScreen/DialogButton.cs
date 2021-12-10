using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class DialogButton : MonoBehaviour
        {
            protected Button button;
            protected Transform dialogueDone;
            protected TextMeshProUGUI title;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                dialogueDone = button.transform.Find("DialogueDone");
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                UIManager.ShowScreen<SexScreen>();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/DialogButton"), parent);
                newItem.name = nameof(DialogButton);
                
                return newItem.AddComponent<DialogButton>();
            }
        }
    }
    
}
