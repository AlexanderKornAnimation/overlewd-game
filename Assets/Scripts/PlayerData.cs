using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class PlayerData
    {
        public static AdminBRO.PlayerInfo playerInfo { get; set; }
        public static List<AdminBRO.EventItem> events { get; set; }
        public static List<AdminBRO.QuestItem> quests { get; set; }
        public static List<AdminBRO.MarketItem> markets { get; set; }

        public class MarketProducts
        {
            public int marketId;
            public List<AdminBRO.MarketProductItem> products;
        }
        public static List<MarketProducts> marketProducts { get; set; } = new List<MarketProducts>();

        public static List<AdminBRO.CurrencyItem> currenies { get; set; }
    }

}
