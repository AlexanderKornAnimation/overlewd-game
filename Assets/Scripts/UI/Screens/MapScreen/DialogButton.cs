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
            protected GameObject markers;
            protected GameObject eventMark1;
            protected GameObject eventMark2;
            protected GameObject eventMark3;
            protected GameObject mainQuestMark;
            protected GameObject sideQuestMark;


            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                dialogueDone = button.transform.Find("DialogueDone");
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                markers = button.transform.Find("Markers").gameObject;
                eventMark1 = markers.transform.Find("EventMark1").gameObject;
                eventMark2 = markers.transform.Find("EventMark2").gameObject;
                eventMark3 = markers.transform.Find("EventMark3").gameObject;
                mainQuestMark = markers.transform.Find("MainQuestMark").gameObject;
                sideQuestMark = markers.transform.Find("SideQuestMark").gameObject;

                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<SexScreen>();
            }

            public static DialogButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                    ("Prefabs/UI/Screens/MapScreen/DialogButton", parent);
            }
        }
    }
    
}
