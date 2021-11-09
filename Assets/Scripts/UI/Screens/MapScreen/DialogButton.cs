using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class DialogButton : MonoBehaviour
        {
            private Button button;
            private Transform dialogueDone;
            private Text title;

            private void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                dialogueDone = button.transform.Find("DialogueDone");
                title = button.transform.Find("Title").GetComponent<Text>();

                button.onClick.AddListener(ButtonClick);
            }

            public void ButtonClick()
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
