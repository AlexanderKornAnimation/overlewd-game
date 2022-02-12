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
            protected GameObject markers;
            protected GameObject mainQuestMark;
            protected GameObject sideQuestMark;
            protected GameObject firstEventMark;
            protected GameObject secondEventMark;
            protected GameObject thirdEventMark;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                sceneDone = button.transform.Find("SceneDone");
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                markers = button.transform.Find("Markers").gameObject;
                mainQuestMark = markers.transform.Find("MainQuestMark").gameObject;
                sideQuestMark = markers.transform.Find("SideQuestMark").gameObject;
                firstEventMark = markers.transform.Find("FirstEventMark").gameObject;
                secondEventMark = markers.transform.Find("SecondEventMark").gameObject;
                thirdEventMark = markers.transform.Find("ThirdEventMark").gameObject;

                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<SexScreen>();
            }

            public static SexSceneButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SexSceneButton>
                    ("Prefabs/UI/Screens/MapScreen/SexSceneButton", parent);
            }
        }
    }
}