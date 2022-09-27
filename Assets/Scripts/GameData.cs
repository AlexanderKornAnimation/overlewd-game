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
        public Data data;

        public enum Type
        {
            None,
            BuyTradable,
            BuildingBuild,
            BuildingBuildCrystal,
            CharacterLvlUp,
            CharacterSkillLvlUp,
            CharacterMerge,
            MagicGuildSpellLvlUp,
            EquipmentEquipped,
            EquipmentUnequipped,
            BuyPotions,
            UsePotions,
            GachaBuy
        }

        public abstract class Data
        {
            public T As<T>() where T : Data =>
                this as T;
        }
    }

    public static class GameData
    {
        public static bool devMode { get; set; } = false;
        public static ProgressFlags progressFlags { get; } = new ProgressFlags();
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
        public static Matriarchs matriarchs { get; } = new Matriarchs();
        public static BattlePass battlePass { get; } = new BattlePass();
        public static Potions potions { get; } = new Potions();
    }

    public abstract class BaseGameMeta
    {
        public virtual async Task Get()
        {
            await Task.CompletedTask;
        }
    }

    //progress
    public class ProgressFlags
    {
        public bool showSidebarButton =>
            GameData.devMode ? true : GameData.buildings.castle.meta.isBuilt;
        public bool lockBuff =>
            GameData.devMode ? false : !GameData.buildings.castle.meta.isBuilt;
    }

    //ftue
    public class FTUE : BaseGameMeta
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

        public override async Task Get()
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

            await GameData.characters.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();
        }

        public async Task ReplayStage(int stageId, int count)
        {
            await AdminBRO.ftueStageReplayAsync(stageId, count);
            stages = await AdminBRO.ftueStagesAsync();
            stats = await AdminBRO.ftueStatsAsync();

            await GameData.characters.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();
        }
    }

    //buildings
    public class Buildings : BaseGameMeta
    {
        public List<AdminBRO.Building> buildings { get; private set; }
        public Municipality municipality { get; private set; } = new Municipality();
        public MagicGuild magicGuild { get; private set; } = new MagicGuild();
        public Forge forge { get; private set; } = new Forge();
        public Castle castle { get; private set; } = new Castle();
        public Catacombs catacombs { get; private set; } = new Catacombs();
        public Laboratory laboratory { get; private set; } = new Laboratory();
        public Aerostat aerostat { get; private set; } = new Aerostat();
        public Harem harem { get; private set; } = new Harem();
        public Market market { get; private set; } = new Market();
        public Portal portal { get; private set; } = new Portal();

        public override async Task Get()
        {
            buildings = await AdminBRO.buildingsAsync();
            magicGuild.skills = await AdminBRO.magicGuildSkillsAsync();
            forge.prices = await AdminBRO.forgePrices();
            municipality.settings = await AdminBRO.municipalitySettingsAsync();
        }
        public AdminBRO.Building GetBuildingById(int? id) =>
            buildings.Find(b => b.id == id);
        public AdminBRO.Building GetBuildingByKey(string key) =>
            buildings.Find(b => b.key == key);

        public async Task Build(int buildingId)
        {
            await AdminBRO.buildingBuildAsync(buildingId);
            await Get();
            await GameData.player.Get();
            municipality.settings = await AdminBRO.municipalitySettingsAsync();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuild
                });

            await GameData.quests.Get();
        }

        public async Task BuildCrystals(int buildingId)
        {
            await AdminBRO.buildingBuildCrystalsAsync(buildingId);
            await Get();
            await GameData.player.Get();
            municipality.settings = await AdminBRO.municipalitySettingsAsync();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuildingBuildCrystal
                });

            await GameData.quests.Get();
        }

        public class Municipality
        {
            public AdminBRO.MunicipalitySettings settings { get; set; }

            private DateTime lastTimeLeftGoldAccUpd;
            public float goldAccTimeLeftMs { get; set; }

            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Municipality);

            public async Task GetTimeLeft()
            {
                var timeLeft = await AdminBRO.municipalityTimeLeftAsync();
                goldAccTimeLeftMs = timeLeft?.timeLeft ?? 0.0f;
                lastTimeLeftGoldAccUpd = DateTime.Now;
                await Task.CompletedTask;
            }

            public async Task GoldCollect()
            {
                await AdminBRO.municipalityCollectAsync();
                await GameData.player.Get();
                await GetTimeLeft();
            }

            public IEnumerator TimeLeftLocalUpd(Action action)
            {
                while (true)
                {
                    var time = DateTime.Now;
                    var dt = time - lastTimeLeftGoldAccUpd;
                    lastTimeLeftGoldAccUpd = time;

                    goldAccTimeLeftMs -= (float)dt.TotalMilliseconds;
                    goldAccTimeLeftMs = goldAccTimeLeftMs > 0.0f ? goldAccTimeLeftMs : 0.0f;

                    action?.Invoke();
                    yield return new WaitForSeconds(1.0f);
                }
            }
        }

        public class MagicGuild
        {
            public List<AdminBRO.MagicGuildSkill> skills { get; set; }
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_MagicGuild);
            public AdminBRO.MagicGuildSkill GetSkillByType(string type) =>
                skills.Find(s => s.type == type);
            public AdminBRO.MagicGuildSkill GetSkillById(int id) =>
                skills.Find(s => s.current.id == id);
            public AdminBRO.MagicGuildSkill activeSkill =>
                GetSkillByType(AdminBRO.MagicGuildSkill.Type_ActiveSkill);
            public AdminBRO.MagicGuildSkill ultimateSkill =>
                GetSkillByType(AdminBRO.MagicGuildSkill.Type_UltimateSkill);
            public AdminBRO.MagicGuildSkill passiveSkill1 =>
                GetSkillByType(AdminBRO.MagicGuildSkill.Type_PassiveSkill1);
            public AdminBRO.MagicGuildSkill PassiveSkill2 =>
                GetSkillByType(AdminBRO.MagicGuildSkill.Type_PassiveSkill2);

            public async Task SkillLvlUp(string skillType)
            {
                await AdminBRO.magicGuildSkillLvlUpAsync(skillType);
                skills = await AdminBRO.magicGuildSkillsAsync();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    type = GameDataEvent.Type.MagicGuildSpellLvlUp
                });
            }

            public async Task SkillLvlUpCrystal(string skillType)
            {
                await AdminBRO.magicGuildSkillLvlUpCrystalAsync(skillType);
                skills = await AdminBRO.magicGuildSkillsAsync();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    type = GameDataEvent.Type.MagicGuildSpellLvlUp
                });
            }
        }

        public class Forge
        {
            public AdminBRO.ForgePrice prices { get; set; }
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Forge);

            public async Task MergeEquipment(string mergeType, int[] mergeIds)
            {
                await AdminBRO.forgeMergeEquipment(mergeType, mergeIds);
            }

            public async Task MergeShard(int matriarchId, string rarity, int amount)
            {
                await AdminBRO.forgeMergeShard(matriarchId, rarity, amount);
            }

            public async Task ExchangeShard(int matriarchSourceId, int matriarchTargetId, string rarity, int amount)
            {
                await AdminBRO.forgeExchangeShard(matriarchSourceId, matriarchTargetId, rarity, amount);
            }
        }

        public class Castle
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Castle);
        }

        public class Catacombs
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Catacombs);
        }

        public class Laboratory
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Laboratory);
        }

        public class Aerostat
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Aerostat);
        }

        public class Harem
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Harem);
        }

        public class Market
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Market);
        }

        public class Portal
        {
            public AdminBRO.Building meta =>
                GameData.buildings.GetBuildingByKey(AdminBRO.Building.Key_Portal);
        }
    }

    //gacha
    public class Gacha : BaseGameMeta
    {
        public List<AdminBRO.GachaItem> items { get; private set; }

        public override async Task Get()
        {
            items = await AdminBRO.gachaAsync();
        }

        public AdminBRO.GachaItem GetGachaById(int? id) =>
            items.Find(g => g.id == id);

        public async Task<List<AdminBRO.GachaBuyResult>> Buy(int? id)
        {
            if (!id.HasValue)
            {
                return new List<AdminBRO.GachaBuyResult>();
            }

            var result = await AdminBRO.gachaBuyAsync(id.Value);
            await Get();
            await GameData.player.Get();
            await GameData.characters.Get();

            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.GachaBuy,
                data = new EventData
                {
                    buyResult = result
                }
            });

            return result;
        }

        public async Task<List<AdminBRO.GachaBuyResult>> BuyMany(int? id)
        {
            if (!id.HasValue)
            {
                return new List<AdminBRO.GachaBuyResult>();
            }

            var result = await AdminBRO.gachaBuyManyAsync(id.Value);
            await Get();
            await GameData.player.Get();
            await GameData.characters.Get();

            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.GachaBuy,
                data = new EventData
                {
                    buyResult = result
                }
            });

            return result;
        }

        public class EventData : GameDataEvent.Data
        {
            public List<AdminBRO.GachaBuyResult> buyResult;
        }
    }

    //characters
    public class Characters : BaseGameMeta
    {
        public List<AdminBRO.Character> characters { get; private set; } = new List<AdminBRO.Character>();
        public List<AdminBRO.SkillEffect> effects { get; private set; } = new List<AdminBRO.SkillEffect>();

        public override async Task Get()
        {
            characters = await AdminBRO.charactersAsync();
            effects = await AdminBRO.skillEffectsAsync();
        }

        public async Task TestInit()
        {
            await AdminBRO.addCharacter(19, 10);
            await AdminBRO.addCharacter(19, 10);
            await AdminBRO.addCharacter(16, 20);
            await AdminBRO.addCharacter(16, 20);

            await AdminBRO.addCharacter(15, 20);
            await AdminBRO.addCharacter(15, 20);
            await AdminBRO.addCharacter(20, 30);
            await AdminBRO.addCharacter(20, 30);

            await AdminBRO.addCharacter(13, 40);
            await AdminBRO.addCharacter(13, 40);
            await Get();
        }

        public AdminBRO.Character GetById(int? id) =>
            characters.Find(ch => ch.id == id);
        public AdminBRO.Character GetByClass(string chClass) =>
            characters.Find(ch => ch.characterClass == chClass);
        public AdminBRO.SkillEffect EffectByName(string name) =>
            effects.Find(e => e.name == name);

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

        public async Task SkillLvlUp(int chId, int skillId)
        {
            await AdminBRO.chracterSkillLvlUp(chId, skillId);
            await Get();
            await GameData.player.Get();
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.CharacterSkillLvlUp
            });
        }

        public async Task Mrg(int? srcID, int? tgtId)
        {
            if (!srcID.HasValue || !tgtId.HasValue)
                return;

            await AdminBRO.charactersMrgAsync(srcID.Value, tgtId.Value);
            await GameData.player.Get();
            await Get();
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.CharacterMerge
            });
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

        public List<AdminBRO.Character> myTeamCharacters =>
            characters.FindAll(ch => ch.teamPosition != AdminBRO.Character.TeamPosition_None);
        public AdminBRO.Character overlord =>
            GetByClass(AdminBRO.Character.Class_Overlord);
        public List<AdminBRO.Character> orderByLevel =>
            characters.OrderBy(ch => ch.level).ToList();
        public AdminBRO.Character slot1Ch =>
            characters.Find(ch => ch.teamPosition == AdminBRO.Character.TeamPosition_Slot1);
        public AdminBRO.Character slot2Ch =>
            characters.Find(ch => ch.teamPosition == AdminBRO.Character.TeamPosition_Slot2);

        public float myTeamPotency
        {
            get
            {
                float potency = 0;

                if (overlord.potency.HasValue)
                    potency += overlord.potency.Value;
                
                foreach (var character in myTeamCharacters)
                {
                    if (character.potency.HasValue)
                    {
                        potency += character.potency.Value;
                    }
                }

                return potency;
            }
        }
    }

    //equipment
    public class Equipment : BaseGameMeta
    {
        public List<AdminBRO.Equipment> equipment { get; private set; } = new List<AdminBRO.Equipment>();

        public override async Task Get()
        {
            equipment = await AdminBRO.equipmentAsync();
        }

        public AdminBRO.Equipment GetById(int? id) =>
            equipment.Find(eq => eq.id == id);

        public AdminBRO.Equipment GetByType(string type) =>
            equipment.Find(eq => eq.equipmentType == type);

        public async Task Equip(int chId, int eqId)
        {
            await AdminBRO.equipAsync(chId, eqId);
            await GameData.characters.Get();
            await Get();
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.EquipmentEquipped
            });
        }

        public async Task Unequip(int chId, int eqId)
        {
            await AdminBRO.unequipAsync(chId, eqId);
            await GameData.characters.Get();
            await Get();

            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                type = GameDataEvent.Type.EquipmentUnequipped
            });
        }
    }

    //events
    public class Events : BaseGameMeta
    {
        public List<AdminBRO.EventItem> events { get; private set; } = new List<AdminBRO.EventItem>();
        public List<AdminBRO.EventChapter> chapters { get; private set; } = new List<AdminBRO.EventChapter>();
        public List<AdminBRO.EventStageItem> stages { get; private set; } = new List<AdminBRO.EventStageItem>();

        public override async Task Get()
        {
            events = await AdminBRO.eventsAsync();
            chapters = await AdminBRO.eventChaptersAsync();
            stages = await AdminBRO.eventStagesAsync();
        }

        public AdminBRO.EventItem GetEventById(int? id) =>
            events.Find(e => e.id == id);
        public AdminBRO.EventChapter GetChapterById(int? id) =>
            chapters.Find(c => c.id == id);
        public AdminBRO.EventChapter GetChapterByStageId(int? id) =>
            chapters.Find(c => c.stages.Exists(sId => sId == id));
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

            await GameData.characters.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();
        }

        public async Task StageReplay(int stageId, int count)
        {
            await AdminBRO.eventStageReplayAsync(stageId, count);
            stages = await AdminBRO.eventStagesAsync();

            await GameData.characters.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();
        }

        public AdminBRO.EventChapter activeChapter
        {
            get
            {
                var chapterData = mapEventData?.firstChapter;
                while (chapterData?.isComplete ?? false)
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

        public AdminBRO.EventChapter mapChapter { get; set; }

        public AdminBRO.EventItem mapEventData { get; set; }

        public AdminBRO.EventItem activeWeekly =>
            events.Where(e => e.isWeekly && e.timePeriodIsActive).FirstOrDefault();
        public AdminBRO.EventItem activeMonthly =>
            events.Where(e => e.isMonthly && e.timePeriodIsActive).FirstOrDefault();
        public AdminBRO.EventItem activeQuarterly =>
            events.Where(e => e.isQuarterly && e.timePeriodIsActive).FirstOrDefault();
        public AdminBRO.EventItem comingSoonMonthly =>
            events.Where(e => e.isMonthly && TimeTools.LessTimeDiff(e.dateStart, TimeSpan.FromDays(30))).FirstOrDefault();
        public AdminBRO.EventItem comingSoonQuarterly =>
            events.Where(e => e.isQuarterly && TimeTools.LessTimeDiff(e.dateStart, TimeSpan.FromDays(30))).FirstOrDefault();
    }

    //markets
    public class Markets : BaseGameMeta
    {
        public List<AdminBRO.EventMarketItem> eventMarkets { get; private set; } = new List<AdminBRO.EventMarketItem>();

        public List<AdminBRO.TradableItem> tradables { get; private set; } = new List<AdminBRO.TradableItem>();

        public override async Task Get()
        {
            eventMarkets = await AdminBRO.eventMarketsAsync();
            tradables = await AdminBRO.tradablesAsync();
        }

        public AdminBRO.EventMarketItem GetEventMarketById(int? id) =>
            eventMarkets.Find(m => m.id == id);
        public AdminBRO.TradableItem GetTradableById(int? id) =>
            tradables.Find(t => t.id == id);

        public async Task<AdminBRO.TradableBuyStatus> BuyTradable(int? marketId, int? tradableId)
        {
            if (!marketId.HasValue || !tradableId.HasValue)
                return new AdminBRO.TradableBuyStatus { status = false };

            var result = await AdminBRO.tradableBuyAsync(marketId.Value, tradableId.Value);
            await GameData.player.Get();

            if (result.status == true)
            {
                UIManager.ThrowGameDataEvent(
                    new GameDataEvent
                    {
                        type = GameDataEvent.Type.BuyTradable
                    });
            }

            return result;
        }

        public async Task<AdminBRO.TradableBuyStatus> BuyTradable(int? tradableId)
        {
            if (!tradableId.HasValue)
                return new AdminBRO.TradableBuyStatus { status = false }; ;

            var result  = await AdminBRO.tradableBuyAsync(tradableId.Value);
            await GameData.player.Get();

            if (result.status == true)
            {
                UIManager.ThrowGameDataEvent(
                    new GameDataEvent
                    {
                        type = GameDataEvent.Type.BuyTradable
                    });
            }

            return result;
        }
    }

    //quests
    public class Quests : BaseGameMeta
    {
        public List<AdminBRO.QuestItem> quests { get; private set; } = new List<AdminBRO.QuestItem>();

        public override async Task Get()
        {
            quests = await AdminBRO.questsAsync();
        }

        public AdminBRO.QuestItem GetById(int? id) =>
            quests.Find(q => q.id == id);

        public List<AdminBRO.QuestItem> ftueQuests =>
            quests.FindAll(q => q.isFTUE);
        public AdminBRO.QuestItem ftueMainQuest =>
            quests.Find(q => q.isFTUEMain);
        public List<AdminBRO.QuestItem> ftueMatriarchQuests =>
            quests.FindAll(q => q.isFTUEMatriarch);
        public List<AdminBRO.QuestItem> ftueSideQuests =>
            quests.FindAll(q => q.isFTUESide);


        public async Task ClaimReward(int? id)
        {
            if (id.HasValue)
            {
                await AdminBRO.questClaimRewardAsync(id.Value);
                await Get();
                await GameData.player.Get();
            }
        }
    }

    //currencies
    public class Currencies : BaseGameMeta
    {
        public List<AdminBRO.CurrencyItem> currencies { get; private set; } = new List<AdminBRO.CurrencyItem>();

        public override async Task Get()
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
    public class Player : BaseGameMeta
    {
        public AdminBRO.PlayerInfo info { get; private set; }

        public override async Task Get()
        {
            info = await AdminBRO.meAsync();
            //var locale = await AdminBRO.localizationAsync("en");

            lastTimeUpd = DateTime.Now;
            accEnergyPoints = 0.0f;
        }

        private DateTime lastTimeUpd;
        private float accEnergyPoints = 0.0f;
        public IEnumerator UpdLocalEnergyPoints(Action action)
        {
            while (true)
            {
                var time = DateTime.Now;
                var dt = time - lastTimeUpd;
                lastTimeUpd = time;

                if (info.energyPoints < GameData.potions.baseEnergyVolume)
                {
                    accEnergyPoints += (float)dt.TotalMinutes * GameData.potions.energyRecoverySpeed;
                    int accPointsIntPart = (int)accEnergyPoints;
                    accEnergyPoints -= accPointsIntPart;
                    info.energyPoints = Math.Min(info.energyPoints + accPointsIntPart, GameData.potions.baseEnergyVolume);
                }
                else
                {
                    accEnergyPoints = 0.0f;
                }

                action?.Invoke();
                yield return new WaitForSeconds(1.0f);
            }
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

        public int hpPotionAmount => info.potion.hp;
        public int manaPotionAmount => info.potion.mana;
        public int energyPotionAmount => info.potion.energy;
        public int replayAmount => info.potion.replay;
        public int energyPoints => info.energyPoints;

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
    public class Dialogs : BaseGameMeta
    {
        public static List<AdminBRO.Dialog> dialogs { get; private set; } = new List<AdminBRO.Dialog>();

        public override async Task Get()
        {
            dialogs = await AdminBRO.dialogsAsync();
        }

        public AdminBRO.Dialog GetById(int? id) =>
            dialogs.Find(d => d.id == id);
    }

    //battles
    public class Battles : BaseGameMeta
    {
        public List<AdminBRO.Battle> battles { get; private set; } = new List<AdminBRO.Battle>();

        public override async Task Get()
        {
            battles = await AdminBRO.battlesAsync();
        }

        public AdminBRO.Battle GetById(int? id) =>
            battles.Find(d => d.id == id);
    }

    //animations
    public class Animations : BaseGameMeta
    {
        public List<AdminBRO.Animation> animations { get; private set; } = new List<AdminBRO.Animation>();

        public override async Task Get()
        {
            animations = await AdminBRO.animationsAsync();
        }

        public AdminBRO.Animation GetById(int? id) =>
            animations.Find(a => a.id == id);
        public AdminBRO.Animation GetByTitle(string title) =>
            animations.Find(a => a.title == title);

        public AdminBRO.Animation this[int id] => GetById(id);
        public AdminBRO.Animation this[string title] => GetByTitle(title);
        public SpineWidget this[string title, Transform parent] =>
            SpineWidget.GetInstance(GetByTitle(title), parent);
    }

    //sounds
    public class Sounds : BaseGameMeta
    {
        public List<AdminBRO.Sound> sounds { get; private set; } = new List<AdminBRO.Sound>();

        public override async Task Get()
        {
            sounds = await AdminBRO.soundsAsync();
        }

        public AdminBRO.Sound GetById(int? id) =>
            sounds.Find(s => s.id == id);
        public AdminBRO.Sound GetByTitle(string title) =>
            sounds.Find(s => s.title == title);
    }

    //matriarchs
    public class Matriarchs : BaseGameMeta
    {
        public List<AdminBRO.MatriarchItem> matriarchs { get; private set; } = new List<AdminBRO.MatriarchItem>();
        public List<AdminBRO.MemoryItem> memories { get; private set; } = new List<AdminBRO.MemoryItem>();
        public List<AdminBRO.MemoryShardItem> memoryShards { get; private set; } = new List<AdminBRO.MemoryShardItem>();
        public List<AdminBRO.BuffItem> buffs { get; private set; } = new List<AdminBRO.BuffItem>();

        public override async Task Get()
        {
            matriarchs = await AdminBRO.matriarchsAsync();
            memories = await AdminBRO.memoriesAsync();
            memoryShards = await AdminBRO.memoryShardsAsync();
            buffs = await AdminBRO.buffsAsync();
        }

        public AdminBRO.MatriarchItem GetMatriarchById(int? id) =>
            matriarchs.Find(m => m.id == id);
        public AdminBRO.MatriarchItem GetMatriarchByKey(string key) =>
            matriarchs.Find(m => m.name == key);
        public AdminBRO.MemoryShardItem GetShardByMatriarchId(int? matriarchId, string rarity) =>
            memoryShards.Find(ms => ms.matriarchId == matriarchId && ms.rarity == rarity);
        public AdminBRO.MemoryShardItem GetShardById(int? id, string rarity) =>
            memoryShards.Find(ms => ms.id == id && ms.rarity == rarity);
        public AdminBRO.BuffItem GetBuffById(int? id) =>
            buffs.Find(b => b.id == id);
        public AdminBRO.BuffItem GetBuffByMatriarchId(int? id) =>
            buffs.Find(b => b.matriarchId == id);

        public AdminBRO.MatriarchItem Ulvi =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Ulvi);
        public AdminBRO.MatriarchItem Adriel =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Adriel);
        public AdminBRO.MatriarchItem Ingie =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Ingie);
        public AdminBRO.MatriarchItem Faye =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Faye);
        public AdminBRO.MatriarchItem Lili =>
            GetMatriarchByKey(AdminBRO.MatriarchItem.Key_Lili);


        public AdminBRO.MemoryItem GetMemoryById(int? id) =>
            memories.Find(m => m.id == id);

        public AdminBRO.BuffItem activeBuff =>
            buffs.Find(b => b.active);

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
                buffs = await AdminBRO.buffsAsync();
            }
        }
    }
    
    //battlePass
    public class BattlePass : BaseGameMeta
    {
        public List<AdminBRO.BattlePass> passes { get; private set; } = new List<AdminBRO.BattlePass>();

        public override async Task Get()
        {
            passes = await AdminBRO.battlePassesAsync();
        }

        public AdminBRO.BattlePass GetByEventId(int? eventId) =>
            passes.Find(p => p.eventId == eventId);
        public AdminBRO.BattlePass GetById(int id) =>
            passes.Find(p => p.id == id);
    }

    //potions
    public class Potions : BaseGameMeta
    {
        public AdminBRO.PotionsInfo potions { get; private set; } = new AdminBRO.PotionsInfo();

        public override async Task Get()
        {
            potions = await AdminBRO.potionsAsync();
        }

        public List<AdminBRO.PriceItem> hpPrice =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_hp)?.price ?? new List<AdminBRO.PriceItem>();
        public List<AdminBRO.PriceItem> manaPrice =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_mana)?.price ?? new List<AdminBRO.PriceItem>();
        public List<AdminBRO.PriceItem> energyPrice =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_energy)?.price ?? new List<AdminBRO.PriceItem>();
        public List<AdminBRO.PriceItem> replayPrice =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_replay)?.price ?? new List<AdminBRO.PriceItem>();

        public int baseEnergyVolume => potions.maxEnergyVolume;
        public int energyPerPotion => potions.energyPerCan;
        public float energyRecoverySpeed => potions.energyRecoverySpeedPerMinute;

        private async Task Buy(string type, int count)
        {
            await AdminBRO.potionBuyAsync(type, count);
            await GameData.player.Get();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.BuyPotions
                });
        }

        public async Task UseEnergy(int count)
        {
            await AdminBRO.potionEnergyUseAsync(count);
            await GameData.player.Get();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    type = GameDataEvent.Type.UsePotions
                });
        }

        public async Task BuyHp(int count)
        {
            await Buy(AdminBRO.PotionsInfo.Type_hp, count);
        }
        public async Task BuyMana(int count)
        {
            await Buy(AdminBRO.PotionsInfo.Type_mana, count);
        }
        public async Task BuyEnergy(int count)
        {
            await Buy(AdminBRO.PotionsInfo.Type_energy, count);
        }
        public async Task BuyReplay(int count)
        {
            await Buy(AdminBRO.PotionsInfo.Type_replay, count);
        }
    }
}
