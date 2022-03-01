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
            public static void Reset()
            {
                currentStageId = 0;

                ulviCaveCanBuilded = false;
                ulviCaveBuilded = false;

                portalCanBuilded = false;
                portalBuilded = false;

                completeFTUE = false;

                ResetStateCastleButtons();
            }

            public static bool newFTUE { get; set; } = false;
            public static bool completeFTUE { get; private set; } = false;
            public static int currentStageId { get; private set; }
            public static void CompleteStageId(int stageId)
            {
                currentStageId = (stageId + 1) > currentStageId ? (stageId + 1) : currentStageId;
                completeFTUE = currentStageId > 16;
            }

            //screens dialogNotifications
            private static int? _castle_DialogNotificationId;
            public static int? castle_DialogNotificationId 
            {
                get
                {
                    var tmp = _castle_DialogNotificationId;
                    _castle_DialogNotificationId = null;
                    return tmp;
                }

                set
                {
                    _castle_DialogNotificationId = value;
                }
            }

            private static int? _map_DialogNotificationId;
            public static int? map_DialogNotificationId
            {
                get
                {
                    var tmp = _map_DialogNotificationId;
                    _map_DialogNotificationId = null;
                    return tmp;
                }

                set
                {
                    _map_DialogNotificationId = value;
                }
            }

            //CastleMapScreen states
            private static string _castle_HintMessage;
            public static string castle_HintMessage
            {
                get
                {
                    var tmp = _castle_HintMessage;
                    _castle_HintMessage = null;
                    return tmp;
                }

                set
                {
                    _castle_HintMessage = value;
                }
            }

            public static bool castle_SideMenuLock = false;
            public static bool castle_CaveLock = false;
            public static bool castle_PortalLock = false;
            public static bool castle_BuildingButtonLock = false;
            public static void ResetStateCastleButtons()
            {
                castle_SideMenuLock = false;
                castle_CaveLock = false;
                castle_PortalLock = false;
                castle_BuildingButtonLock = false;
            }

            public static void UlviCaveCanBuilded()
            {
                if (!ulviCaveBuilded)
                {
                    ulviCaveCanBuilded = true;
                }
            }
            public static void UlviCaveBuild()
            {
                ulviCaveCanBuilded = false;
                ulviCaveBuilded = true;
            }
            public static bool ulviCaveCanBuilded { get; private set; } = false;
            public static bool ulviCaveBuilded { get; private set; } = false;

            public static void PortalCanBuilded()
            {
                if (!portalBuilded)
                {
                    portalCanBuilded = true;
                }
            }
            public static void PortalBuild()
            {
                portalCanBuilded = false;
                portalBuilded = true;
            }
            public static bool portalCanBuilded { get; private set; } = false;
            public static bool portalBuilded { get; private set; } = false;

            //stages state
            public static int ftueChapterId;

            public static int battleScreen_BattleId = 0;
            public static int battleScreen_StageId = 0;
            public static string battleScreen_StageKey;

            public static int sexScreen_StageId;
            public static int sexScreen_DialogId;
            public static string sexScreen_StageKey;
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    if (newFTUE)
                    {
                        var sexDialogId = Overlewd.GameData.ftue.chapters[ftueChapterId].
                            dialogs.Find(item => item.key == sexScreen_StageKey).id;
                        return GameData.GetSexById(sexDialogId);
                    }
                    return GameData.GetSexById(sexScreen_DialogId);
                }
            }

            public static int dialogScreen_StageId;
            public static int dialogScreen_DialogId;
            public static string dialogScreen_StageKey;
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {
                    if (newFTUE)
                    {
                        var dialogId = Overlewd.GameData.ftue.chapters[ftueChapterId].
                            dialogs.Find(item => item.key == dialogScreen_StageKey).id;
                        return GameData.GetDialogById(dialogId);
                    }
                    return GameData.GetDialogById(dialogScreen_DialogId);
                }
            }

            public static int dialogNotification_DialogId;
            public static string dialogNotification_StageKey;
            public static AdminBRO.Dialog dialogNotification_DialogData
            {
                get
                {
                    if (newFTUE)
                    {
                        var notificationDialogId = Overlewd.GameData.ftue.chapters[ftueChapterId].
                            dialogs.Find(item => item.key == dialogNotification_StageKey).id;
                        return GameData.GetNotificationById(notificationDialogId);
                    }
                    return GameData.GetNotificationById(dialogNotification_DialogId);
                }
            }
        }
    }
}
