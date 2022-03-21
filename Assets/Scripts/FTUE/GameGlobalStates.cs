using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                    var stageData = GameData.ftue.chapters[chapterId].
                        stages.Find(s => (s.key == battleScreen_StageKey) && s.battleId.HasValue);
                    return GameData.GetBattleById(stageData.battleId.Value);
                }
            }


            public static string sexScreen_StageKey { get; set; }
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    var stageData = GameData.ftue.chapters[chapterId].
                        stages.Find(s => (s.key == sexScreen_StageKey) && s.dialogId.HasValue);
                    var sexData = GameData.GetDialogById(stageData.dialogId.Value);
                    return sexData?.type == AdminBRO.Dialog.Type_Sex ? sexData : null;
                }
            }

            public static string dialogScreen_StageKey { get; set; }
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {
                    var stageData = GameData.ftue.chapters[chapterId].
                        stages.Find(s => (s.key == dialogScreen_StageKey) && s.dialogId.HasValue);
                    var dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                    return dialogData?.type == AdminBRO.Dialog.Type_Dialog ? dialogData : null;
                }
            }

            public static string dialogNotification_StageKey { get; set; }
            public static AdminBRO.Dialog dialogNotification_DialogData
            {
                get
                {
                    var notificationId = GameData.ftue.chapters[chapterId].
                        notifications.Find(n => n.key == dialogNotification_StageKey).id;
                    return GameData.GetDialogById(notificationId);
                }
            }
        }
    }
}
