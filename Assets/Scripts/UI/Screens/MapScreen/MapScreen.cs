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

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MapScreen/MapScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
            backbutton = canvas.Find("BackButton").GetComponent<Button>();

            chapterButton.onClick.AddListener(ChapterButtonClick);
            backbutton.onClick.AddListener(BackButtonClick);

            map = canvas.Find("Map");            
        }

        void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
        
        protected virtual void ChapterButtonClick()
        {

        }
    }
}