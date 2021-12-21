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
            public static int currentStageId { get; private set; }
            public static void CompleteStageId(int stageId)
            {
                currentStageId = (stageId + 1) > currentStageId ? (stageId + 1) : currentStageId;
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

            public static int battleScreen_BattleId = 0;
            public static int battleScreen_StageId = 0;

            public static int sexScreen_StageId;
            public static int sexScreen_DialogId;
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    return GameData.GetSexById(sexScreen_DialogId);
                }
            }

            public static int dialogScreen_StageId;
            public static int dialogScreen_DialogId;
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {
                    return GameData.GetDialogById(dialogScreen_DialogId);
                }
            }

            public static int dialogNotification_DialogId;
            public static AdminBRO.Dialog dialogNotification_DialogData
            {
                get
                {
                    return GameData.GetNotificationById(dialogNotification_DialogId);
                }
            }
        }
    }
}
