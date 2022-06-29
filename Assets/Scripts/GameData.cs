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
            BuildingBuild,
            BuildingBuildCrystal,
            CharacterLvlUp
        }
    }

    public static class GameData
    {
        public static bool devMode { get; set; } = false;
        public static Quests quests { get; } = new Quests();
        public static FTUE ftue { get; } = new FTUE();
        public static Gacha gacha { get; } = new Gacha();
        public static Buildings buildings { get; } = new Buildings();
        public static Characters characters { get; } = new Characters();
        public static Equipment equipment { get; } = new Equipment();
        public static Events events { get; } = new Events();
        public static Markets markets { get; } = new Markets();
        public static Currencies currencies { get; } = new Currencies();
        public static Player player { get; } = new Player();
        public static Dialogs dialogs { get; } = new Dialogs();
        public static Battles battles { get; } = new Battles();
        public static Animations animations { get; } = new Animations();
        public static Sounds sounds { get; } = new Sounds();
        public static ChapterMaps chapterMaps { get; } = new ChapterMaps();
        public static Matriarchs matriarchs { get; } = new Matriarchs();
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
    }

    //buildings
    public class Buildings
    {
        public List<AdminBRO.Building> buildings { get; private set; }
        public List<AdminBRO.MagicGuildSkill> magicGuildSkills { get;private set; }

        public async Task Get()
        {
            buildings = await AdminBRO.buildingsAsync();
            magicGuildSkills = await AdminBRO.magicGuildSkillsAsync();
        }
        public AdminBRO.Building GetBuildingById(int? id) =>
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

        public async Task Build(int buildingId)
        {
            await AdminBRO.buildingBuildAsync(buildingId);
            await Get();
            await GameData.player.Get();
            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuild
                });
        }

        public async Task BuildCrystals(int buildingId)
        {
            await AdminBRO.buildingBuildCrystalsAsync(buildingId);
            await Get();
            await GameData.player.Get();
            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuildCrystal
                });
        }

        public async Task<int> MunicipalityTimeLeft()
        {
            var timeLeft = await AdminBRO.municipalityTimeLeftAsync();
            return timeLeft.timeLeft;
        }

        public async Task MunicipalityCollect()
        {
            await AdminBRO.municipalityCollectAsync();
        }

        public async Task MagicGuildSkillLvlUp(string skillType)
        {
            await AdminBRO.magicGuildSkillLvlUpAsync(skillType);
            magicGuildSkills = await AdminBRO.magicGuildSkillsAsync();
        }
    }

    //gacha
    public class Gacha
    {
        public List<AdminBRO.GachItem> items { get; private set; }

        public async Task Get() =>
            items = await AdminBRO.gachaAsync();
        public AdminBRO.GachItem GetGachaById(int? id) =>
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

        public async Task Get()
        {
            characters = await AdminBRO.charactersAsync();
        }

        public AdminBRO.Character GetById(int? id) =>
            characters.Find(ch => ch.id == id);
        public AdminBRO.Character GetByClass(string chClass) => 
            characters.Find(ch => ch.characterClass == chClass);

        public async Task LvlUp(int chId)
        {
            await AdminBRO.characterLvlupAsync(chId);
            await Get();
            await GameData.player.Get();
            
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.CharacterLvlUp
            });
        }

        public async Task SkillLvlUp(int chId, string skillType)
        {
            await AdminBRO.chracterSkillLvlUp(chId, skillType);
            await Get(); 
            await GameData.player.Get();
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

    //equipment
    public class Equipment
    {
        public List<AdminBRO.Equipment> equipment { get; private set; } = new List<AdminBRO.Equipment>();

        public async Task Get()
        {
            equipment = await AdminBRO.equipmentAsync();
        }

        public AdminBRO.Equipment GetById(int? id) =>
            equipment.Find(eq => eq.id == id);
    }

    //events
    public class Events
    {
        public List<AdminBRO.EventItem> events { get; private set; } = new List<AdminBRO.EventItem>();
        public List<AdminBRO.EventChapter> chapters { get; private set; } = new List<AdminBRO.EventChapter>();
        public List<AdminBRO.EventStageItem> stages { get; private set; } = new List<AdminBRO.EventStageItem>();

        public async Task Get()
        {
            events = await AdminBRO.eventsAsync();
            chapters = await AdminBRO.eventChaptersAsync();
            stages = await AdminBRO.eventStagesAsync();
        }

        public AdminBRO.EventItem GetEventById(int? id) =>
            events.Find(e => e.id == id);
        public AdminBRO.EventChapter GetChapterById(int? id) =>
            chapters.Find(c => c.id == id);
        public AdminBRO.EventStageItem GetStageById(int? id) =>
            stages.Find(s => s.id == id);

        public async Task StageStart(int stageId)
        {
            var newEventStageData = await AdminBRO.eventStageStartAsync(stageId);
            stages = await AdminBRO.eventStagesAsync();
        }
        public async Task StageEnd(int stageId, AdminBRO.EventStageEndData data = null)
        {
            var newEventStageData = await AdminBRO.eventStageEndAsync(stageId, data);
            stages = await AdminBRO.eventStagesAsync();
        }

        public AdminBRO.EventItem mapEventData { get; set; }
    }

    //markets
    public class Markets
    {
        public List<AdminBRO.EventMarketItem> eventMarkets { get; private set; } = new List<AdminBRO.EventMarketItem>();

        public List<AdminBRO.TradableItem> tradables { get; private set; } = new List<AdminBRO.TradableItem>();

        public async Task Get()
        {
            eventMarkets = await AdminBRO.eventMarketsAsync();
            tradables = await AdminBRO.tradablesAsync();
        }

        public AdminBRO.EventMarketItem GetEventMarketById(int? id) =>
            eventMarkets.Find(m => m.id == id);
        public AdminBRO.TradableItem GetTradableById(int? id) =>
            tradables.Find(t => t.id == id);

        public async Task BuyTradable(int? marketId, int? tradableId)
        {
            if (!marketId.HasValue || !tradableId.HasValue)
                return;

            await AdminBRO.tradableBuyAsync(marketId.Value, tradableId.Value);
            await GameData.player.Get();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuyTradable
                });
        }
    }

    //quests
    public class Quests
    {
        public List<AdminBRO.QuestItem> quests { get; private set; } = new List<AdminBRO.QuestItem>();

        public async Task Get()
        {
            quests = await AdminBRO.questsAsync();
        }

        public AdminBRO.QuestItem GetById(int? id) =>
            quests.Find(q => q.id == id);

        public async void ClaimReward(int? id)
        {
            var cr_struct = await AdminBRO.questClaimRewardAsync(id.Value);
        }
    }
    
    //currencies
    public class Currencies
    {
        public List<AdminBRO.CurrencyItem> currencies { get; private set; } = new List<AdminBRO.CurrencyItem>();

        public async Task Get()
        {
            currencies = await AdminBRO.currenciesAsync();
        }

        public AdminBRO.CurrencyItem GetById(int? id) =>
            currencies.Find(c => c.id == id);
        public AdminBRO.CurrencyItem GetByKey(string key) =>
            currencies.Find(c => c.key == key);

        public AdminBRO.CurrencyItem Copper =>
            GetByKey(AdminBRO.CurrencyItem.Key_Copper);
        public AdminBRO.CurrencyItem Crystals =>
            GetByKey(AdminBRO.CurrencyItem.Key_Crystals);
        public AdminBRO.CurrencyItem Gems =>
            GetByKey(AdminBRO.CurrencyItem.Key_Gems);
        public AdminBRO.CurrencyItem Gold =>
            GetByKey(AdminBRO.CurrencyItem.Key_Gold);
        public AdminBRO.CurrencyItem Stone =>
            GetByKey(AdminBRO.CurrencyItem.Key_Stone);
        public AdminBRO.CurrencyItem Wood =>
            GetByKey(AdminBRO.CurrencyItem.Key_Wood);
        public AdminBRO.CurrencyItem CatEars =>
            GetByKey(AdminBRO.CurrencyItem.Key_Ears);
        public AdminBRO.CurrencyItem HornyCoins =>
            GetByKey(AdminBRO.CurrencyItem.Key_Horny);
        public AdminBRO.CurrencyItem JapaneseYen =>
            GetByKey(AdminBRO.CurrencyItem.Key_Yen);
        public AdminBRO.CurrencyItem NutakuGold =>
            GetByKey(AdminBRO.CurrencyItem.Key_Ngold);

    }

    //player
    public class Player
    {
        public AdminBRO.PlayerInfo info { get; private set; }

        public async Task Get()
        {
            info = await AdminBRO.meAsync();
            //var locale = await AdminBRO.localizationAsync("en");
        }

        public async Task AddCrystals(int amount = 1000)
        {
            var crystalCurrencyId = GameData.currencies.Crystals.id;
            await AdminBRO.meCurrencyAsync(crystalCurrencyId, amount);
            await Get();
        }

        public AdminBRO.PlayerInfo.WalletItem Crystal =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.Crystals.id);
        
        public AdminBRO.PlayerInfo.WalletItem Wood =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.Wood.id);
        
        public AdminBRO.PlayerInfo.WalletItem Stone =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.Stone.id);
        
        public AdminBRO.PlayerInfo.WalletItem Copper =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.Copper.id);
        
        public AdminBRO.PlayerInfo.WalletItem Gold =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.Gold.id);
        
        public AdminBRO.PlayerInfo.WalletItem Gems =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.Gems.id);
        
        public AdminBRO.PlayerInfo.WalletItem CatEars =>
            info.wallet.Find(item => item.currencyId == GameData.currencies.CatEars.id);

        public bool CanBuy(List<AdminBRO.PriceItem> price)
        {
            foreach (var priceItem in price)
            {
                var walletCurrency = info.wallet.Find(item => item.currencyId == priceItem.currencyId);
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
    }

    //dialogs
    public class Dialogs
    {
        public static List<AdminBRO.Dialog> dialogs { get; private set; } = new List<AdminBRO.Dialog>();

        public async Task Get()
        {
            dialogs = await AdminBRO.dialogsAsync();
        }

        public AdminBRO.Dialog GetById(int? id) =>
            dialogs.Find(d => d.id == id);
    }

    //battles
    public class Battles
    {
        public List<AdminBRO.Battle> battles { get; private set; } = new List<AdminBRO.Battle>();

        public async Task Get()
        {
            battles = await AdminBRO.battlesAsync();
        }

        public AdminBRO.Battle GetById(int? id) =>
            battles.Find(d => d.id == id);
    }

    //animations
    public class Animations
    {
        public List<AdminBRO.Animation> animations { get; private set; } = new List<AdminBRO.Animation>();

        public async Task Get()
        {
            animations = await AdminBRO.animationsAsync();
        }

        public AdminBRO.Animation GetById(int? id) =>
            animations.Find(a => a.id == id);
    }

    //sounds
    public class Sounds
    {
        public List<AdminBRO.Sound> sounds { get; private set; } = new List<AdminBRO.Sound>();

        public async Task Get()
        {
            sounds = await AdminBRO.soundsAsync();
        }

        public AdminBRO.Sound GetById(int? id) =>
            sounds.Find(s => s.id == id);
    }

    //chapterMaps
    public class ChapterMaps
    {
        public List<AdminBRO.ChapterMap> chapterMaps { get; private set; } = new List<AdminBRO.ChapterMap>();

        public async Task Get()
        {
            chapterMaps = await AdminBRO.chapterMapsAsync();
        }

        public AdminBRO.ChapterMap GetById(int? id) =>
            chapterMaps.Find(cm => cm.id == id);
    }

    //matriarchs
    public class Matriarchs
    {
        public List<AdminBRO.MatriarchItem> matriarchs { get; private set; } = new List<AdminBRO.MatriarchItem>();
        public List<AdminBRO.MemoryItem> memories { get; private set; } = new List<AdminBRO.MemoryItem>();

        public async Task Get()
        {
            matriarchs = await AdminBRO.matriarchsAsync();
            memories = await AdminBRO.memoriesAsync();
        }

        public AdminBRO.MatriarchItem GetMatriarchById(int? id) =>
            matriarchs.Find(m => m.id == id);
        public AdminBRO.MatriarchItem GetMatriarchByKey(string key) =>
            matriarchs.Find(m => m.name == key);

        public AdminBRO.MatriarchItem Ulvi =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Ulvi);
        public bool UlviIsOpen => Ulvi?.isOpen ?? false;
        public AdminBRO.MatriarchItem Adriel =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Adriel);
        public bool AdrielIsOpen => Adriel?.isOpen ?? false;
        public AdminBRO.MatriarchItem Ingie =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Ingie);
        public bool IngieIsOpen => Ingie?.isOpen ?? false;
        public AdminBRO.MatriarchItem Faye =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Faye);
        public bool FayeIsOpen => Faye?.isOpen ?? false;
        public AdminBRO.MatriarchItem Lili =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Lili);
        public bool LiliIsOpen => Lili?.isOpen ?? false;



        public AdminBRO.MemoryItem GetMemoryById(int? id) =>
            memories.Find(m => m.id == id);

        public async Task memoryBuy(int? id)
        {
            if (id.HasValue)
            {
                await AdminBRO.memoryBuyAsync(id.Value);
                memories = await AdminBRO.memoriesAsync();
            }
        }

        public async Task matriarchSeduce(int? id)
        {
            if (id.HasValue)
            {
                await AdminBRO.seduceMatriarchAsync(id.Value);
                matriarchs = await AdminBRO.matriarchsAsync();
            }
        }
    }
}
