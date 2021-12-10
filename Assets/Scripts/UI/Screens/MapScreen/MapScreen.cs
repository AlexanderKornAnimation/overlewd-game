using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class MapScreen : BaseScreen
    {
        protected Transform map;
        protected Button chapterButton;
        protected Button backbutton;
        
        private void Awake()
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

            map = canvas.Find("Map");            
        }

        private void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);

            NSMapScreen.DialogButton.GetInstance(map.Find("dialogue_1"));
            NSMapScreen.FightButton.GetInstance(map.Find("fight_1"));
            NSMapScreen.EventButton.GetInstance(map.Find("event_1"));
        }

        protected virtual void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
        
        protected virtual void ChapterButtonClick()
        {

        }
    }
}