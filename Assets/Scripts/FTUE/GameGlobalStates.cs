using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        public static class GameGlobalStates
        {
            public static int chapterId { get; set; }

            public static string battleScreen_StageKey { get; set; }
            public static AdminBRO.Battle battleScreen_BattleData
            {
                get
                {
                    var battleId = GameData.ftue.chapters[chapterId].
                        battles.Find(item => item.key == battleScreen_StageKey).id;
                    return GameData.GetBattleById(battleId);
                }
            }


            public static string sexScreen_StageKey { get; set; }
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    var sexId = GameData.ftue.chapters[chapterId].
                        dialogs.Find(item => item.key == sexScreen_StageKey).id;
                    return GameData.GetDialogById(sexId);
                }
            }

            public static string dialogScreen_StageKey { get; set; }
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {
                    var dialogId = GameData.ftue.chapters[chapterId].
                        dialogs.Find(item => item.key == dialogScreen_StageKey).id;
                    return GameData.GetDialogById(dialogId);
                }
            }

            public static string dialogNotification_StageKey { get; set; }
            public static AdminBRO.Dialog dialogNotification_DialogData
            {
                get
                {
                    var notificationId = GameData.ftue.chapters[chapterId].
                        dialogs.Find(item => item.key == dialogNotification_StageKey).id;
                    return GameData.GetDialogById(notificationId);
                }
            }
        }
    }
}
