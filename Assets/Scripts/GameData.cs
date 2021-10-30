using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class GameData
    {
        public static AdminBRO.PlayerInfo playerInfo { get; set; }
        public static List<AdminBRO.EventItem> events { get; set; }
        public static AdminBRO.EventItem GetEventById(int id)
        {
            return events.Find(e => e.id == id);
        }
        public static List<AdminBRO.QuestItem> quests { get; set; }
        public static AdminBRO.QuestItem GetQuestById(int id)
        {
            return quests.Find(q => q.id == id);
        }
        public static List<AdminBRO.MarketItem> markets { get; set; }

        public class MarketProducts
        {
            public int marketId;
            public List<AdminBRO.MarketProductItem> marketProducts;
        }
        public static List<MarketProducts> marketProducts { get; set; } = new List<MarketProducts>();

        public static List<AdminBRO.CurrencyItem> currenies { get; set; }
        public static List<AdminBRO.Dialog> dialogs { get; set; } = new List<AdminBRO.Dialog>();
    }

}
