using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public static class GameData
    {
        public static AdminBRO.PlayerInfo playerInfo { get; set; }
        public static bool CanEventMarketItemBuy(AdminBRO.MarketProductItem marketItem)
        {
            foreach (var priceItem in marketItem.price)
            {
                var walletCurrency = playerInfo.wallet.Find(item => item.currency.id == priceItem.currencyId);

                if (walletCurrency == null)
                {
                    return false;
                }

                if (walletCurrency.amount < priceItem.amount)
                {
                    return false;
                }
            }
            return true;
        }

        public static async Task BuyEventMarketItem(AdminBRO.MarketProductItem marketItem)
        {
            await AdminBRO.tradableBuyAsync(marketItem.id);
            GameData.playerInfo = await AdminBRO.meAsync();
        }

        public static List<AdminBRO.EventItem> events { get; set; }
        public static AdminBRO.EventItem GetEventById(int id)
        {
            return events.Find(e => e.id == id);
        }
        public static List<AdminBRO.QuestItem> quests { get; set; } = new List<AdminBRO.QuestItem>();
        public static AdminBRO.QuestItem GetQuestById(int id)
        {
            return quests.Find(q => q.id == id);
        }
        public static List<AdminBRO.MarketItem> markets { get; set; } = new List<AdminBRO.MarketItem>();
        public static AdminBRO.MarketItem GetMarketById(int id)
        {
            return markets.Find(m => m.id == id);
        }

        public static List<AdminBRO.CurrencyItem> currenies { get; set; }
        public static AdminBRO.CurrencyItem GetCurrencyById(int id)
        {
            return currenies.Find(c => c.id == id);
        }
        public static List<AdminBRO.Dialog> dialogs { get; set; } = new List<AdminBRO.Dialog>();
        public static AdminBRO.Dialog GetDialogById(int id)
        {
            return dialogs.Find(d => d.id == id);
        }
        public static List<AdminBRO.Battle> battles { get; set; } = new List<AdminBRO.Battle>();
        public static AdminBRO.Battle GetBattleById(int id)
        {
            return battles.Find(d => d.id == id);
        }
    }

}
