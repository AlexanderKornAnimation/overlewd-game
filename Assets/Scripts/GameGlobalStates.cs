using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class GameGlobalStates
    {
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

        public static int bannerNotifcation_TradableId;
        public static AdminBRO.TradableItem bannerNotifcation_TradableData
        {
            get
            {
                return GameData.GetTradableById(bannerNotifcation_TradableId);
            }
        }
    }

}
