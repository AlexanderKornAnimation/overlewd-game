using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        public class MapScreen : Overlewd.MapScreen
        {
            private NSMapScreen.DialogButton dialog_1;
            private NSMapScreen.DialogButton dialog_2;
            private NSMapScreen.DialogButton dialog_3;

            protected override void Awake()
            {
                var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/MapScreenFTUE"));
                var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
                screenRectTransform.SetParent(transform, false);
                UIManager.SetStretch(screenRectTransform);

                var canvas = screenRectTransform.Find("Canvas");
                chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
                backbutton = canvas.Find("BackButton").GetComponent<Button>();

                chapterButton.onClick.AddListener(ChapterButtonClick);
                backbutton.onClick.AddListener(BackButtonClick);

                map = canvas.Find("Map");
            }

            protected override void Customize()
            {
                backbutton.gameObject.SetActive(false);
                chapterButton.gameObject.SetActive(false);

                EventsWidget.GetInstance(transform);
                QuestsWidget.GetInstance(transform);
                BuffWidget.GetInstance(transform);

                dialog_1 = NSMapScreen.DialogButton.GetInstance(map.Find("dialogue_1"));
                dialog_1.dialogId = 1;
                dialog_2 = NSMapScreen.DialogButton.GetInstance(map.Find("dialogue_2"));
                dialog_2.dialogId = 2;
                dialog_3 = NSMapScreen.DialogButton.GetInstance(map.Find("dialogue_3"));
                dialog_3.dialogId = 3;
                NSMapScreen.FightButton.GetInstance(map.Find("fight_1"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_2"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_3"));
            }

            protected override void BackButtonClick()
            {

            }

            protected override void ChapterButtonClick()
            {

            }
        }
    }
}