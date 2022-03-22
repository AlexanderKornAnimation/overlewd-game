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
            public static AdminBRO.FTUEChapter chapterData { get; set; }

            public static List<AdminBRO.FTUENotificationItem> chapterNotifications 
            {
                get 
                {
                    return chapterData?.notifications;
                }
            }

            public static AdminBRO.FTUEStageItem GetStageByKey(string key)
            {
                return GameData.ftueStages.Find(s => s.ftueChapterId == chapterData?.id && s.key == key);
            }

            public static string battleScreen_StageKey { get; set; }
            public static AdminBRO.Battle battleScreen_BattleData
            {
                get
                {
                    var stageData = GetStageByKey(battleScreen_StageKey);
                    var battleId = stageData?.battleId;
                    return battleId.HasValue ? GameData.GetBattleById(battleId.Value) : null;
                }
            }


            public static string sexScreen_StageKey { get; set; }
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    var stageData = GetStageByKey(sexScreen_StageKey);
                    var sexId = stageData?.dialogId;
                    var sexData = sexId.HasValue ? GameData.GetDialogById(sexId.Value) : null;
                    return sexData?.type == AdminBRO.Dialog.Type_Sex ? sexData : null;
                }
            }

            public static string dialogScreen_StageKey { get; set; }
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {

                    var stageData = GetStageByKey(dialogScreen_StageKey);
                    var dialogId = stageData?.dialogId;
                    var dialogData = dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;
                    return dialogData?.type == AdminBRO.Dialog.Type_Dialog ? dialogData : null;
                }
            }

            public static string dialogNotification_StageKey { get; set; }
            public static AdminBRO.Dialog dialogNotification_DialogData
            {
                get
                {
                    var notificationData = chapterNotifications.Find(n => n.key == dialogNotification_StageKey);
                    var dialogId = notificationData?.dialogId;
                    return dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;
                }
            }
        }
    }
}
