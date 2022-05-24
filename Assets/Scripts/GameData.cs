using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

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
        public static bool progressMode { get; set; } = false;

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
        public static async Task EventStageEndAsync(int stageId, AdminBRO.EventStageEndData data = null)
        {
            var newEventStageData = await AdminBRO.eventStageEndAsync(stageId, data);
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

        public static List<AdminBRO.Equipment> equipment { get; set; } = new List<AdminBRO.Equipment>();
        public static AdminBRO.Equipment GetEquipmentById(int id)
        {
            return equipment.Find(eq => eq.id == id);
        }

        public static FTUE ftue { get; } = new FTUE();
        public static Gacha gacha { get; } = new Gacha();
        public static Buildings buildings { get; } = new Buildings();
        public static Characters characters { get; } = new Characters();
    }

    //ftue
    public class FTUE
    {
        public AdminBRO.FTUEInfo info { get; private set; }
        public List<AdminBRO.FTUEStageItem> stages { get; private set; }
        public AdminBRO.FTUEStats stats { get; private set; }
        public AdminBRO.FTUEChapter activeChapter
        {
            get
            {
                var chapterData = info.chapter1;
                while (chapterData.isComplete)
                {
                    if (chapterData.nextChapterId.HasValue)
                    {
                        chapterData = chapterData.nextChapterData;
                        continue;
                    }
                    break;
                }
                return chapterData;
            }
        }
        public AdminBRO.FTUEChapter mapChapter { get; set; }

        public async Task Get()
        {
            info = await AdminBRO.ftueAsync();
            stages = await AdminBRO.ftueStagesAsync();
            stats = await AdminBRO.ftueStatsAsync();
        }

        public async Task StartStage(int stageId)
        {
            await AdminBRO.ftueStageStartAsync(stageId);
            stages = await AdminBRO.ftueStagesAsync();
            stats = await AdminBRO.ftueStatsAsync();
        }
        public async Task EndStage(int stageId, AdminBRO.FTUEStageEndData data = null)
        {
            await AdminBRO.ftueStageEndAsync(stageId, data);
            stages = await AdminBRO.ftueStagesAsync();
            stats = await AdminBRO.ftueStatsAsync();
        }
        public async Task Reset()
        {
            await AdminBRO.resetAsync(new List<string> { AdminBRO.ResetEntityName.FTUE });
            stages = await AdminBRO.ftueStagesAsync();
            stats = await AdminBRO.ftueStatsAsync();
        }
    }

    //buildings
    public class Buildings
    {
        public List<AdminBRO.Building> buildings { get; private set; }

        public async Task Get() =>
            buildings = await AdminBRO.buildingsAsync();
        public AdminBRO.Building GetBuildingById(int id) =>
            buildings.Find(b => b.id == id);
        public AdminBRO.Building GetBuildingByKey(string key) =>
            buildings.Find(b => b.key == key);
        public AdminBRO.Building castle =>
            GetBuildingByKey(AdminBRO.Building.Key_Castle);
        public AdminBRO.Building catacombs =>
            GetBuildingByKey(AdminBRO.Building.Key_Catacombs);
        public AdminBRO.Building laboratory =>
            GetBuildingByKey(AdminBRO.Building.Key_Laboratory);
        public AdminBRO.Building aerostat =>
            GetBuildingByKey(AdminBRO.Building.Key_Aerostat);
        public AdminBRO.Building forge =>
            GetBuildingByKey(AdminBRO.Building.Key_Forge);
        public AdminBRO.Building harem =>
            GetBuildingByKey(AdminBRO.Building.Key_Harem);
        public AdminBRO.Building magicGuild =>
            GetBuildingByKey(AdminBRO.Building.Key_MagicGuild);
        public AdminBRO.Building market =>
            GetBuildingByKey(AdminBRO.Building.Key_Market);
        public AdminBRO.Building municipality =>
            GetBuildingByKey(AdminBRO.Building.Key_Municipality);
        public AdminBRO.Building portal =>
            GetBuildingByKey(AdminBRO.Building.Key_Portal);

        public async Task BuildNow(int buildingId)
        {
            await AdminBRO.buildingBuildNowAsync(buildingId);
            buildings = await AdminBRO.buildingsAsync();
            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuildNow
                });
        }

        public async Task Build(int buildingId)
        {
            await AdminBRO.buildingBuildAsync(buildingId);
            buildings = await AdminBRO.buildingsAsync();
            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuildStarted
                });
        }

        public async Task Reset()
        {
            await AdminBRO.resetAsync(new List<string> { AdminBRO.ResetEntityName.Building });
            await AdminBRO.initAsync();
            buildings = await AdminBRO.buildingsAsync();
        }
    }

    //gacha
    public class Gacha
    {
        public List<AdminBRO.GachItem> items { get; private set; }

        public async Task Get() =>
            items = await AdminBRO.gachaAsync();
        public AdminBRO.GachItem GetGachaById(int id) =>
            items.Find(g => g.id == id);

        public async Task Buy(int id)
        {
            await AdminBRO.gachaBuyAsync(id);
            await Get();
        }

        public async Task BuyTen(int id)
        {
            await AdminBRO.gachaBuyTenAsync(id);
            await Get();
        }
    }

    //characters
    public class Characters
    {
        public List<AdminBRO.Character> characters { get; private set; } = new List<AdminBRO.Character>();

        public async Task Get() =>
            characters = await AdminBRO.charactersAsync();
        public AdminBRO.Character GetById(int id) =>
            characters.Find(ch => ch.id == id);
        public AdminBRO.Character GetByClass(string chClass) => 
            characters.Find(ch => ch.characterClass == chClass);

        public async Task LvlUp(int chId)
        {
            await AdminBRO.characterLvlupAsync(chId);
            await Get();
        }

        public async Task Mrg(int srcID, int tgtId)
        {
            await AdminBRO.charactersMrgAsync(srcID, tgtId);
            await Get();
        }

        public async Task ToSlot1(int chId)
        {
            await AdminBRO.characterToSlotAsync(chId, AdminBRO.Character.TeamPosition_Slot1);
            await Get();
        }

        public async Task ToSlot2(int chId)
        {
            await AdminBRO.characterToSlotAsync(chId, AdminBRO.Character.TeamPosition_Slot2);
            await Get();
        }

        public async Task ToSlotNone(int chId)
        {
            await AdminBRO.characterToSlotAsync(chId, AdminBRO.Character.TeamPosition_None);
            await Get();
        }

        public async Task Equip(int chId, int eqId)
        {
            await AdminBRO.equipAsync(chId, eqId);
            await Get();
        }

        public async Task Unequip(int chId, int eqId)
        {
            await AdminBRO.unequipAsync(chId, eqId);
            await Get();
        }

        public List<AdminBRO.Character> myTeamCharacters =>
            characters.FindAll(ch => ch.teamPosition != AdminBRO.Character.TeamPosition_None);
        public AdminBRO.Character overlord =>
            GetByClass(AdminBRO.Character.Class_Overlord);
        public List<AdminBRO.Character> orderById =>
            characters.OrderBy(ch => ch.id).ToList();
        public AdminBRO.Character slot1Ch =>
            characters.Find(ch => ch.teamPosition == AdminBRO.Character.TeamPosition_Slot1);
        public AdminBRO.Character slot2Ch =>
            characters.Find(ch => ch.teamPosition == AdminBRO.Character.TeamPosition_Slot2);
    }
}
