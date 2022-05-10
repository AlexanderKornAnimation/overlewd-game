using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class GameDataEvent
    {
        public Type type = Type.None;

        public enum Type
        {
            None,
            BuyTradable,
            BuildingBuildNow,
            BuildingBuildStarted,
        }
    }

    public static class GameData
    {
        public static AdminBRO.PlayerInfo playerInfo { get; set; }

        public static int GetCurencyCatEarsCount()
        {
            var currency = GetCurencyCatEars();
            var walletCurrency = playerInfo.wallet.Find(item => item.currency.id == currency.id);
            return walletCurrency?.amount ?? 0;
        }

        public static async Task BuyTradableAsync(int marketId, int tradableId)
        {
            await AdminBRO.tradableBuyAsync(marketId, tradableId);
            playerInfo = await AdminBRO.meAsync();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                { 
                    type = GameDataEvent.Type.BuyTradable 
                });
        }

        public static List<AdminBRO.EventItem> events { get; set; }
        public static AdminBRO.EventItem GetEventById(int id)
        {
            return events.Find(e => e.id == id);
        }

        public static List<AdminBRO.EventChapter> eventChapters { get; set; }
        public static AdminBRO.EventChapter GetEventChapterById(int id)
        {
            return eventChapters.Find(c => c.id == id);
        }

        public static List<AdminBRO.QuestItem> quests { get; set; } = new List<AdminBRO.QuestItem>();
        public static AdminBRO.QuestItem GetQuestById(int id)
        {
            return quests.Find(q => q.id == id);
        }

        public static List<AdminBRO.EventMarketItem> eventMarkets { get; set; } = new List<AdminBRO.EventMarketItem>();
        public static AdminBRO.EventMarketItem GetEventMarketById(int id)
        {
            return eventMarkets.Find(m => m.id == id);
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

        public static List<AdminBRO.TradableItem> tradables { get; set; }
        public static AdminBRO.TradableItem GetTradableById(int id)
        {
            return tradables.Find(t => t.id == id);
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
        public static async Task EventStageStartAsync(int stageId)
        {
            var newEventStageData = await AdminBRO.eventStageStartAsync(stageId);
            eventStages = await AdminBRO.eventStagesAsync();
        }
        public static async Task EventStageEndAsync(int stageId)
        {
            var newEventStageData = await AdminBRO.eventStageEndAsync(stageId);
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

        public static AdminBRO.FTUEInfo ftue { get; set; }
        public static List<AdminBRO.FTUEStageItem> ftueStages { get; set; } = new List<AdminBRO.FTUEStageItem>();
        public static AdminBRO.FTUEStats ftueStats { get; set; } = new AdminBRO.FTUEStats();

        public static AdminBRO.FTUEChapter GetFTUEChapterByKey(string key)
        {
            return ftue.chapters.Find(ch => ch.key == key);
        }
        public static AdminBRO.FTUEChapter GetFTUEChapterById(int id)
        {
            return ftue.chapters.Find(ch => ch.id == id);
        }
        public static AdminBRO.FTUEStageItem GetFTUEStageById(int id)
        {
            return ftueStages.Find(s => s.id == id);
        }
        public static AdminBRO.FTUEStageItem GetFTUEStageByKey(string key, int chapterId)
        {
            return ftueStages.Find(s => s.key == key && s.ftueChapterId == chapterId);
        }
        public static AdminBRO.FTUEStageItem GetFTUEStageByKey(string key, string chapterKey)
        {
            var chpaterData = GetFTUEChapterByKey(chapterKey);
            return (chpaterData != null) ? GetFTUEStageByKey(key, chpaterData.id) : null;
        }
        public static async Task FTUEStartStage(int stageId)
        {
            await AdminBRO.ftueStageStartAsync(stageId);
            ftueStages = await AdminBRO.ftueStagesAsync();
            ftueStats = await AdminBRO.ftueStatsAsync();
        }
        public static async Task FTUEEndStage(int stageId)
        {
            await AdminBRO.ftueStageEndAsync(stageId);
            ftueStages = await AdminBRO.ftueStagesAsync();
            ftueStats = await AdminBRO.ftueStatsAsync();
        }
        public static async Task FTUEReset()
        {
            await AdminBRO.resetAsync(new List<string> { AdminBRO.ResetEntityName.FTUE });
            ftueStages = await AdminBRO.ftueStagesAsync();
            ftueStats = await AdminBRO.ftueStatsAsync();
        }

        public static List<AdminBRO.Animation> animations { get; set; } = new List<AdminBRO.Animation>();
        public static AdminBRO.Animation GetAnimationById(int id)
        {
            return animations.Find(a => a.id == id);
        }

        public static List<AdminBRO.Sound> sounds { get; set; } = new List<AdminBRO.Sound>();
        public static AdminBRO.Sound GetSoundById(int id)
        {
            return sounds.Find(s => s.id == id);
        }

        public static List<AdminBRO.ChapterMap> chapterMaps { get; set; } = new List<AdminBRO.ChapterMap>();
        public static AdminBRO.ChapterMap GetChapterMapById(int id)
        {
            return chapterMaps.Find(cm => cm.id == id);
        }

        public static List<AdminBRO.Building> buildings { get; set; } = new List<AdminBRO.Building>();
        public static AdminBRO.Building GetBuildingById(int id)
        {
            return buildings.Find(b => b.id == id);
        }
        public static AdminBRO.Building GetBuildingByKey(string key)
        {
            return buildings.Find(b => b.key == key);
        }
        public static async Task BuildingBuildNow(int buildingId)
        {
            await AdminBRO.buildingBuildNowAsync(buildingId);
            buildings = await AdminBRO.buildingsAsync();
            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuildNow
                });
        }

        public static async Task BuildingBuild(int buildingId)
        {
            await AdminBRO.buildingBuildAsync(buildingId);
            buildings = await AdminBRO.buildingsAsync();
            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuildStarted
                });
        }

        public static async Task BuildingsReset()
        {
            await AdminBRO.resetAsync(new List<string> { AdminBRO.ResetEntityName.Building });
            await AdminBRO.initAsync();
            buildings = await AdminBRO.buildingsAsync();
        }

        public static List<AdminBRO.Character> characters { get; set; } = new List<AdminBRO.Character>();
        public static AdminBRO.Character GetCharacterById(int id)
        {
            return characters.Find(ch => ch.id == id);
        }
        public static AdminBRO.Character GetCharacterByClass(string chClass)
        {
            return characters.Find(ch => ch.characterClass == chClass);
        }

        public static List<AdminBRO.Equipment> equipment { get; set; } = new List<AdminBRO.Equipment>();
        public static AdminBRO.Equipment GetEquipmentById(int id)
        {
            return equipment.Find(eq => eq.id == id);
        }
    }

}
