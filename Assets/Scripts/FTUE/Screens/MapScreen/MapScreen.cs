using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                chapterButton.gameObject.SetActive(true);

                //EventsWidget.GetInstance(transform);
                QuestsWidget.GetInstance(transform);
                BuffWidget.GetInstance(transform);


                /*var fight = NSMapScreen.FightButton.GetInstance(map.Find("fightKeyName"));
                fight.battleKey = "fightKeyName";

                var sex = NSMapScreen.SexSceneButton.GetInstance(map.Find("sexKeyName"));
                sex.sexKey = "sexKeyName";

                var dialog = NSMapScreen.DialogButton.GetInstance(map.Find("dialogKeyName"));
                dialog.dialogKey = "dialogKeyName";*/
            }

            protected override void BackButtonClick()
            {

            }

            protected override void ChapterButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<Overlewd.CastleScreen>();
            }
        }
    }
}