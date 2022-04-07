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
        public static bool ftueProgressMode { get; set; } = false;
        public static AdminBRO.Dialog GetFTUENotificationByKey(string key)
        {
            var notifData = ftueChapterData?.notifications.Find(n => n.key == key);
            var dialogId = notifData?.dialogId;
            return dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;
        }
        public static AdminBRO.FTUEStageItem GetFTUEStageByKey(string key)
        {
            return ftueChapterData != null ? GameData.GetFTUEStageByKey(key, ftueChapterData.id) : null;
        }

        //events
        public static AdminBRO.EventItem eventData { get; set; }

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
