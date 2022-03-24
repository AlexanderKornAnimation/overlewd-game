using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class GameGlobalStates
    {
        //ftue
        public static AdminBRO.FTUEChapter ftueChapterData { get; set; }
        public static AdminBRO.Dialog GetFTUENotificationByKey(string key)
        {
            var notifData = ftueChapterData?.notifications.Find(n => n.key == key);
            var dialogId = notifData?.dialogId;
            return dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;
        }
        public static string ftue_StageKey { get; set; }
        private static AdminBRO.FTUEStageItem ftue_StageData
        {
            get
            {
                return ftueChapterData != null ? GameData.GetFTUEStageByKey(ftue_StageKey, ftueChapterData.id) : null;
            }
        }
        public static int? ftue_StageId {
            get 
            {
                return ftue_StageData?.id;
            }
        }
        public static AdminBRO.Dialog ftue_StageDialogData
        {
            get
            {
                var dialogId = ftue_StageData?.dialogId;
                return dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;
            }
        }
        public static AdminBRO.Battle ftue_StageBattleData
        {
            get
            {
                var battleId = ftue_StageData?.battleId;
                return battleId.HasValue ? GameData.GetBattleById(battleId.Value) : null;
            }
        }

        //
        public static AdminBRO.Dialog dialogNotificationData { get; set; }
        //

        public static int eventMapScreen_EventId;
        public static AdminBRO.EventItem eventMapScreen_EventData 
        { 
            get
            {
                return GameData.GetEventById(eventMapScreen_EventId);
            }
        }

        public static int bossFight_EventStageId;
        public static AdminBRO.EventStageItem bossFight_EventStageData 
        { 
            get
            {
                return GameData.GetEventStageById(bossFight_EventStageId);
            }
                
        }

        public static int battle_EventStageId;
        public static AdminBRO.EventStageItem battle_EventStageData 
        { 
            get 
            {
                return GameData.GetEventStageById(battle_EventStageId);
            } 
        }

        public static int dialog_EventStageId;
        public static AdminBRO.EventStageItem dialog_EventStageData 
        { 
            get 
            {
                return GameData.GetEventStageById(dialog_EventStageId);
            }
        }

        public static int sex_EventStageId;
        public static AdminBRO.EventStageItem sex_EventStageData 
        { 
            get
            {
                return GameData.GetEventStageById(sex_EventStageId);
            }
        }

        public static int eventShop_MarketId;
        public static AdminBRO.EventMarketItem eventShop_MarketData
        { 
            get 
            {
                return GameData.GetEventMarketById(eventShop_MarketId);
            } 
        }

        public static int bannerNotification_EventMarketId;
        public static int bannerNotification_TradableId;
        public static AdminBRO.TradableItem bannerNotifcation_TradableData
        {
            get
            {
                return GameData.GetTradableById(bannerNotification_EventMarketId, bannerNotification_TradableId);
            }
        }

        public static string haremGirlNameSelected { get; set; }
    }
}
