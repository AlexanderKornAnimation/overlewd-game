using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd.NSMapScreen;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class MapScreen : BaseScreen
    {
        private Transform map;
        private Button chapterButton;
        private Button backbutton;
        
        private void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/MapScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
            backbutton = canvas.Find("BackButton").GetComponent<Button>();

            chapterButton.onClick.AddListener(ChapterButtonClick);
            backbutton.onClick.AddListener(BackButtonClick);

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            BuffWidget.CreateInstance(transform);

            map = canvas.Find("Map");

            DialogButton.GetInstance(map.Find("dialogue_1"));
            FightButton.GetInstance(map.Find("fight_1"));
            EventButton.GetInstance(map.Find("event_1"));
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
        
        private void ChapterButtonClick()
        {
        }
    }
}