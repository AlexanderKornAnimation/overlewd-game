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
            protected override void Customize()
            {
                backbutton.gameObject.SetActive(false);
                chapterButton.gameObject.SetActive(false);

                EventsWidget.GetInstance(transform);
                QuestsWidget.GetInstance(transform);
                BuffWidget.GetInstance(transform);

                NSMapScreen.DialogButton.GetInstance(map.Find("dialogue_1"));
                NSMapScreen.DialogButton.GetInstance(map.Find("dialogue_2"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_1"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_2"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_3"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_4"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_5"));
                NSMapScreen.FightButton.GetInstance(map.Find("fight_6"));
                NSMapScreen.EventButton.GetInstance(map.Find("event_1"));
                NSMapScreen.EventButton.GetInstance(map.Find("event_2"));
                NSMapScreen.EventButton.GetInstance(map.Find("event_3"));
                NSMapScreen.SexSceneButton.GetInstance(map.Find("sexDialogue"));
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