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
            private NSMapScreen.FightButton stage_1_fight_1;
            private NSMapScreen.FightButton stage_2_fight_2;
            private NSMapScreen.SexSceneButton stage_3_sex_2;
            private NSMapScreen.FightButton stage_4_fight_3;
            private NSMapScreen.DialogButton stage_5_dialogue_1;
            private NSMapScreen.FightButton stage_6_fight_4;
            private NSMapScreen.FightButton stage_7_fight_5;
            private NSMapScreen.FightButton stage_8_fight_6;
            private NSMapScreen.FightButton stage_9_fight_7;
            private NSMapScreen.DialogButton stage_10_dialogue_2;
            private NSMapScreen.FightButton stage_11_fight_8;
            private NSMapScreen.FightButton stage_12_fight_9;
            private NSMapScreen.FightButton stage_13_fight_10;
            private NSMapScreen.DialogButton stage_14_dialogue_3;
            private NSMapScreen.SexSceneButton stage_15_sex_3;
            private NSMapScreen.FightButton stage_16_fight_11;

            protected override void Awake()
            {
                var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MapScreen/MapScreenFTUE", transform);

                var canvas = screenInst.transform.Find("Canvas");
                chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
                backbutton = canvas.Find("BackButton").GetComponent<Button>();

                chapterButton.onClick.AddListener(ChapterButtonClick);
                backbutton.onClick.AddListener(BackButtonClick);

                map = canvas.Find("Map");
            }

            protected override void Customize()
            {
                backbutton.gameObject.SetActive(false);
                chapterButton.gameObject.SetActive(GameGlobalStates.completeFTUE);

                //EventsWidget.GetInstance(transform);
                QuestsWidget.GetInstance(transform);
                BuffWidget.GetInstance(transform);

                stage_1_fight_1 = NSMapScreen.FightButton.GetInstance(map.Find("stage_1_fight_1"));
                stage_1_fight_1.stageId = 1;
                stage_1_fight_1.battleId = 1;

                stage_2_fight_2 = NSMapScreen.FightButton.GetInstance(map.Find("stage_2_fight_2"));
                stage_2_fight_2.stageId = 2;
                stage_2_fight_2.battleId = 2;

                stage_3_sex_2 = NSMapScreen.SexSceneButton.GetInstance(map.Find("stage_3_sex_2"));
                stage_3_sex_2.stageId = 3;
                stage_3_sex_2.sexId = 2;

                stage_4_fight_3 = NSMapScreen.FightButton.GetInstance(map.Find("stage_4_fight_3"));
                stage_4_fight_3.stageId = 4;
                stage_4_fight_3.battleId = 3;

                stage_5_dialogue_1 = NSMapScreen.DialogButton.GetInstance(map.Find("stage_5_dialogue_1"));
                stage_5_dialogue_1.stageId = 5;
                stage_5_dialogue_1.dialogId = 1;

                stage_6_fight_4 = NSMapScreen.FightButton.GetInstance(map.Find("stage_6_fight_4"));
                stage_6_fight_4.stageId = 6;
                stage_6_fight_4.battleId = 4;

                stage_7_fight_5 = NSMapScreen.FightButton.GetInstance(map.Find("stage_7_fight_5"));
                stage_7_fight_5.stageId = 7;
                stage_7_fight_5.battleId = 5;

                stage_8_fight_6 = NSMapScreen.FightButton.GetInstance(map.Find("stage_8_fight_6"));
                stage_8_fight_6.stageId = 8;
                stage_8_fight_6.battleId = 6;

                stage_9_fight_7 = NSMapScreen.FightButton.GetInstance(map.Find("stage_9_fight_7"));
                stage_9_fight_7.stageId = 9;
                stage_9_fight_7.battleId = 7;

                stage_10_dialogue_2 = NSMapScreen.DialogButton.GetInstance(map.Find("stage_10_dialogue_2"));
                stage_10_dialogue_2.stageId = 10;
                stage_10_dialogue_2.dialogId = 2;

                stage_11_fight_8 = NSMapScreen.FightButton.GetInstance(map.Find("stage_11_fight_8"));
                stage_11_fight_8.stageId = 11;
                stage_11_fight_8.battleId = 8;

                stage_12_fight_9 = NSMapScreen.FightButton.GetInstance(map.Find("stage_12_fight_9"));
                stage_12_fight_9.stageId = 12;
                stage_12_fight_9.battleId = 9;

                stage_13_fight_10 = NSMapScreen.FightButton.GetInstance(map.Find("stage_13_fight_10"));
                stage_13_fight_10.stageId = 13;
                stage_13_fight_10.battleId = 10;

                stage_14_dialogue_3 = NSMapScreen.DialogButton.GetInstance(map.Find("stage_14_dialogue_3"));
                stage_14_dialogue_3.stageId = 14;
                stage_14_dialogue_3.dialogId = 3;

                stage_15_sex_3 = NSMapScreen.SexSceneButton.GetInstance(map.Find("stage_15_sex_3"));
                stage_15_sex_3.stageId = 15;
                stage_15_sex_3.sexId = 3;

                stage_16_fight_11 = NSMapScreen.FightButton.GetInstance(map.Find("stage_16_fight_11"));
                stage_16_fight_11.stageId = 16;
                stage_16_fight_11.battleId = 11;
            }

            protected override void BackButtonClick()
            {

            }

            protected override void ChapterButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<Overlewd.CastleScreen>();
            }

            public override void AfterShow()
            {
                var notification = GameGlobalStates.map_DialogNotificationId;
                if (notification.HasValue)
                {
                    GameGlobalStates.dialogNotification_DialogId = notification.Value;
                    UIManager.ShowNotification<DialogNotification>();
                }
            }
        }
    }
}