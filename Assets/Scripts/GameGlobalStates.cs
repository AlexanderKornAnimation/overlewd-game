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
                return GameData.GetTradableById(bannerNotification_TradableId);
            }
        }

        public static string haremGirlNameSelected { get; set; }
    }
}
