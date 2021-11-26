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
        public static bool CanTradableBuy(AdminBRO.TradableItem tradable)
        {
            foreach (var priceItem in tradable.price)
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
        public static int GetCurencyCatEarsCount()
        {
            var currency = GetCurencyCatEars();
            var walletCurrency = playerInfo.wallet.Find(item => item.currency.id == currency.id);
            return walletCurrency?.amount ?? 0;
        }

        public static async Task BuyTradableAsync(AdminBRO.TradableItem tradable)
        {
            await AdminBRO.tradableBuyAsync(tradable.id);
            GameData.playerInfo = await AdminBRO.meAsync();
        }

        public static List<AdminBRO.EventItem> events { get; set; }
        public static AdminBRO.EventItem GetEventById(int id)
        {
            return events.Find(e => e.id == id);
        }

        public static List<AdminBRO.EventQuestItem> eventQuests { get; set; } = new List<AdminBRO.EventQuestItem>();
        public static AdminBRO.EventQuestItem GetEventQuestById(int id)
        {
            return eventQuests.Find(q => q.id == id);
        }

        public static List<AdminBRO.EventMarketItem> eventMarkets { get; set; } = new List<AdminBRO.EventMarketItem>();
        public static AdminBRO.EventMarketItem GetEventMarketById(int id)
        {
            return eventMarkets.Find(m => m.id == id);
        }

        public static List<AdminBRO.TradableItem> tradables { get; set; } = new List<AdminBRO.TradableItem>();
        public static AdminBRO.TradableItem GetTradableById(int id)
        {
            return tradables.Find(t => t.id == id);
        }

        public static List<AdminBRO.CurrencyItem> currenies { get; set; }
        public static AdminBRO.CurrencyItem GetCurrencyById(int id)
        {
            return currenies.Find(c => c.id == id);
        }
        public static AdminBRO.CurrencyItem GetCurencyCatEars()
        {
            return currenies.Find(c => c.name == "Cat Ears");
        }

        public static List<AdminBRO.EventStageItem> eventStages { get; set; } = new List<AdminBRO.EventStageItem>();
        public static AdminBRO.EventStageItem GetEventStageById(int id)
        {
            return eventStages.Find(s => s.id == id);
        }
        public static void SetEventStage(AdminBRO.EventStageItem stageData)
        {
            var stageIndex = eventStages.FindIndex(s => s.id == stageData.id);
            if (stageIndex != -1)
            {
                eventStages[stageIndex] = stageData;
            }
        }
        public static async Task EventStageStartAsync(AdminBRO.EventStageItem stage)
        {
            var newEventStageData = await AdminBRO.eventStageStartAsync(stage.id);
            eventStages = await AdminBRO.eventStagesAsync();
        }
        public static async Task EventStageEndAsync(AdminBRO.EventStageItem stage)
        {
            var newEventStageData = await AdminBRO.eventStageEndAsync(stage.id);
            eventStages = await AdminBRO.eventStagesAsync();
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
