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
        private Button buffButton;
        private Button chapterButton;

        private void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/MapScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            buffButton = canvas.Find("BuffButton").GetComponent<Button>();
            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();

            buffButton.onClick.AddListener(BuffButtonClick);
            chapterButton.onClick.AddListener(ChapterButtonClick);

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);

            map = canvas.Find("Map");

            EventButton.GetInstance(map.Find("event_1"));
            FightButton.GetInstance(map.Find("fight_1"));
            DialogButton.GetInstance(map.Find("dialogue_1"));

        }

        private void ChapterButtonClick()
        {
        }

        private void BuffButtonClick()
        {
            UIManager.ShowScreen<GirlScreen>();
        }
    }
}