using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Overlewd
{
    public static class GameData
    {
        public static bool devMode { get; set; } = false;
        public static ProgressFlags progressFlags { get; } = new ProgressFlags();
        public static ResourcesMeta resources { get; } = new ResourcesMeta();
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
        public static NutakuMy nutaku { get; } = new NutakuMy();
        public static Alchemy alchemy { get; } = new Alchemy();
        public static BossMiniGame bossMiniGame { get; } = new BossMiniGame();
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
            GameData.devMode ? false : !GameData.buildings.harem.meta.isBuilt;
        public bool questWidgetEnabled =>
            GameData.devMode ? true :
            GameData.ftue.chapter1_battle1.isComplete && !GameData.ftue.chapter1_battle1.isLastEnded;
        public bool eventsWidgetEnabled =>
            GameData.devMode ? true : GameData.buildings.aerostat.meta.isBuilt;
    }

    //resources
    public class ResourcesMeta : BaseGameMeta
    {
        public List<AdminBRO.NetworkResource> meta { get; private set; } = new List<AdminBRO.NetworkResource>();

        public override async Task Get()
        {
            meta = await AdminBRO.resourcesAsync();
        }

        public AdminBRO.NetworkResource GetById(string id) =>
            meta.Find(r => r.id == id);
    }

    //ftue
    public class FTUE : BaseGameMeta
    {
        public const string CHAPTER_1 = "chapter1";
        public const string CHAPTER_2 = "chapter2";
        public const string CHAPTER_3 = "chapter3";
        public const string BATTLE_1 = "battle1";
        public const string BATTLE_2 = "battle2";
        public const string BATTLE_3 = "battle3";
        public const string BATTLE_4 = "battle4";
        public const string BATTLE_5 = "battle5";
        public const string DIALOGUE_1 = "dialogue1";
        public const string DIALOGUE_2 = "dialogue2";
        public const string DIALOGUE_3 = "dialogue3";
        public const string DIALOGUE_4 = "dialogue4";
        public const string DIALOGUE_5 = "dialogue5";
        public const string SEX_1 = "sex1";
        public const string SEX_2 = "sex2";
        public const string SEX_3 = "sex3";

        public AdminBRO.FTUEInfo info { get; private set; }
        public List<AdminBRO.FTUEStageItem> stages { get; private set; }
        public AdminBRO.FTUEStats stats { get; private set; }

        //chapter_1
        public AdminBRO.FTUEChapter chapter1 => GetChapterByKey(CHAPTER_1);
        public AdminBRO.FTUEStageItem chapter1_battle1 => chapter1.GetStageByKey(BATTLE_1);
        public AdminBRO.FTUEStageItem chapter1_battle2 => chapter1.GetStageByKey(BATTLE_2);
        public AdminBRO.FTUEStageItem chapter1_battle3 => chapter1.GetStageByKey(BATTLE_3);
        public AdminBRO.FTUEStageItem chapter1_battle4 => chapter1.GetStageByKey(BATTLE_4);
        public AdminBRO.FTUEStageItem chapter1_battle5 => chapter1.GetStageByKey(BATTLE_5);
        public AdminBRO.FTUEStageItem chapter1_dialogue1 => chapter1.GetStageByKey(DIALOGUE_1);
        public AdminBRO.FTUEStageItem chapter1_dialogue2 => chapter1.GetStageByKey(DIALOGUE_2);
        public AdminBRO.FTUEStageItem chapter1_dialogue3 => chapter1.GetStageByKey(DIALOGUE_3);
        public AdminBRO.FTUEStageItem chapter1_dialogue4 => chapter1.GetStageByKey(DIALOGUE_4);
        public AdminBRO.FTUEStageItem chapter1_sex1 => chapter1.GetStageByKey(SEX_1);
        public AdminBRO.FTUEStageItem chapter1_sex2 => chapter1.GetStageByKey(SEX_2);
        public AdminBRO.FTUEStageItem chapter1_sex3 => chapter1.GetStageByKey(SEX_3);

        //chapter_2
        public AdminBRO.FTUEChapter chapter2 => GetChapterByKey(CHAPTER_2);
        public AdminBRO.FTUEStageItem chapter2_battle1 => chapter2.GetStageByKey(BATTLE_1);
        public AdminBRO.FTUEStageItem chapter2_battle2 => chapter2.GetStageByKey(BATTLE_2);
        public AdminBRO.FTUEStageItem chapter2_battle3 => chapter2.GetStageByKey(BATTLE_3);
        public AdminBRO.FTUEStageItem chapter2_battle4 => chapter2.GetStageByKey(BATTLE_4);
        public AdminBRO.FTUEStageItem chapter2_battle5 => chapter2.GetStageByKey(BATTLE_5);
        public AdminBRO.FTUEStageItem chapter2_dialogue1 => chapter2.GetStageByKey(DIALOGUE_1);
        public AdminBRO.FTUEStageItem chapter2_dialogue2 => chapter2.GetStageByKey(DIALOGUE_2);
        public AdminBRO.FTUEStageItem chapter2_dialogue3 => chapter2.GetStageByKey(DIALOGUE_3);
        public AdminBRO.FTUEStageItem chapter2_dialogue4 => chapter2.GetStageByKey(DIALOGUE_4);
        public AdminBRO.FTUEStageItem chapter2_dialogue5 => chapter2.GetStageByKey(DIALOGUE_5);
        public AdminBRO.FTUEStageItem chapter2_sex2 => chapter2.GetStageByKey(SEX_2);

        //chapter_3
        public AdminBRO.FTUEChapter chapter3 => GetChapterByKey(CHAPTER_3);
        public AdminBRO.FTUEStageItem chapter3_battle1 => chapter3.GetStageByKey(BATTLE_1);
        public AdminBRO.FTUEStageItem chapter3_battle2 => chapter3.GetStageByKey(BATTLE_2);
        public AdminBRO.FTUEStageItem chapter3_battle3 => chapter3.GetStageByKey(BATTLE_3);
        public AdminBRO.FTUEStageItem chapter3_battle4 => chapter3.GetStageByKey(BATTLE_4);
        public AdminBRO.FTUEStageItem chapter3_dialogue1 => chapter3.GetStageByKey(DIALOGUE_1);
        public AdminBRO.FTUEStageItem chapter3_dialogue2 => chapter3.GetStageByKey(DIALOGUE_2);
        public AdminBRO.FTUEStageItem chapter3_dialogue3 => chapter3.GetStageByKey(DIALOGUE_3);
        public AdminBRO.FTUEStageItem chapter3_dialogue4 => chapter3.GetStageByKey(DIALOGUE_4);
        public AdminBRO.FTUEStageItem chapter3_dialogue5 => chapter3.GetStageByKey(DIALOGUE_5);
        public AdminBRO.FTUEStageItem chapter3_sex1 => chapter3.GetStageByKey(SEX_1);
        public AdminBRO.FTUEStageItem chapter3_sex2 => chapter3.GetStageByKey(SEX_2);
        public AdminBRO.FTUEStageItem chapter3_sex3 => chapter3.GetStageByKey(SEX_3);

        public AdminBRO.FTUEChapter GetChapterByKey(string key) => info.chapters.Find(ch => ch.key == key);
        public AdminBRO.FTUEChapter GetChapterById(int? id) => info.chapters.Find(ch => ch.id == id);
        public AdminBRO.FTUEStageItem GetStageById(int? id) => stages.Find(s => s.id == id);
        public AdminBRO.FTUEChapter activeChapter
        {
            get
            {
                var chapterData = chapter1;
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
        public async Task<List<AdminBRO.GenRewardItem>> EndStage(int stageId, AdminBRO.BattleEndData battleEndData = null)
        {
            var genRewards = await AdminBRO.ftueStageEndAsync(stageId, battleEndData);
            stages = await AdminBRO.ftueStagesAsync();
            stats = await AdminBRO.ftueStatsAsync();

            await GameData.characters.Get();
            await GameData.equipment.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();

            return genRewards;
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
        public List<AdminBRO.Building> buildingsMeta { get; private set; }
        public List<Building> buildings => new List<Building>
        {
            municipality,
            magicGuild,
            forge,
            castle,
            catacombs,
            laboratory,
            aerostat,
            harem,
            market,
            portal
        };
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
            buildingsMeta = await AdminBRO.buildingsAsync();
            magicGuild.skills = await AdminBRO.magicGuildSkillsAsync();
            forge.prices = await AdminBRO.forgePrices();
            municipality.settings = await AdminBRO.municipalitySettingsAsync();
        }

        public AdminBRO.Building GetBuildingMetaById(int? id) =>
            buildingsMeta.Find(b => b.id == id);
        public AdminBRO.Building GetBuildingMetaByKey(string key) =>
            buildingsMeta.Find(b => b.key == key);
        public Building GetBuildingById(int? id) =>
            buildings.Find(b => b.meta?.id == id);
        public Building GetBuildingByKey(string key) =>
            buildings.Find(b => b.meta?.key == key);

        public async Task Build(int buildingId)
        {
            await AdminBRO.buildingBuildAsync(buildingId);
            await Get();
            await GameData.player.Get();
            await GameData.characters.Get();
            municipality.settings = await AdminBRO.municipalitySettingsAsync();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    id = GameDataEventId.BuildingBuild
                });

            await GameData.quests.Get();
        }

        public async Task BuildCrystals(int buildingId)
        {
            await AdminBRO.buildingBuildCrystalsAsync(buildingId);
            await Get();
            await GameData.player.Get();
            await GameData.characters.Get();
            municipality.settings = await AdminBRO.municipalitySettingsAsync();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    id = GameDataEventId.BuildingBuildCrystal
                });

            await GameData.quests.Get();
        }

        public abstract class Building
        {
            public virtual AdminBRO.Building meta => null;
            public T As<T>() where T : Building =>
                this as T;
            public bool Is<T>() where T : Building =>
                this.GetType() == typeof(T);
        }

        public class Municipality : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Municipality);
            public AdminBRO.MunicipalitySettings settings { get; set; }

            private DateTime lastTimeLeftGoldAccUpd;
            public float goldAccTimeLeftMs { get; set; }

            public async Task GetTimeLeft()
            {
                var timeLeft = await AdminBRO.municipalityTimeLeftAsync();
                goldAccTimeLeftMs = timeLeft.dData?.timeLeft ?? 0.0f;
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

        public class MagicGuild : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_MagicGuild);
            public List<AdminBRO.MagicGuildSkill> skills { get; set; }

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
                await GameData.characters.Get();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new MagicGuildDataEvent
                {
                    id = GameDataEventId.MagicGuildSpellLvlUp,
                    skillType = skillType
                });
            }

            public async Task SkillLvlUpCrystal(string skillType)
            {
                await AdminBRO.magicGuildSkillLvlUpCrystalAsync(skillType);
                skills = await AdminBRO.magicGuildSkillsAsync();
                await GameData.characters.Get();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new MagicGuildDataEvent
                {
                    id = GameDataEventId.MagicGuildSpellLvlUp,
                    skillType = skillType
                });
            }
        }

        public class Forge : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Forge);
            public AdminBRO.ForgePrice prices { get; set; }

            public async Task MergeEquipment(string mergeType, int[] mergeIds)
            {
                await AdminBRO.forgeMergeEquipment(mergeType, mergeIds);
                await GameData.equipment.Get();
                await GameData.characters.Get();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    id = GameDataEventId.ForgeMergeEquip
                });
            }

            public async Task MergeShard(int matriarchId, string rarity, int amount)
            {
                await AdminBRO.forgeMergeShard(matriarchId, rarity, amount);
                await GameData.matriarchs.Get();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    id = GameDataEventId.ForgeMergeShards
                });
            }

            public async Task ExchangeShard(int matriarchSourceId, int matriarchTargetId, string rarity, int amount)
            {
                await AdminBRO.forgeExchangeShard(matriarchSourceId, matriarchTargetId, rarity, amount);
                await GameData.matriarchs.Get();
                await GameData.player.Get();

                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    id = GameDataEventId.ForgeExchangeShards
                });
            }
        }

        public class Castle : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Castle);
        }

        public class Catacombs : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Catacombs);
        }

        public class Laboratory : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Laboratory);
        }

        public class Aerostat : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Aerostat);
        }

        public class Harem : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Harem);
        }

        public class Market : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Market);
        }

        public class Portal : Building
        {
            public override AdminBRO.Building meta =>
                GameData.buildings.GetBuildingMetaByKey(AdminBRO.Building.Key_Portal);
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
            await GameData.equipment.Get();
            await GameData.matriarchs.Get();

            UIManager.ThrowGameDataEvent(new GachaDataEvent
            {
                id = GameDataEventId.GachaBuy,
                buyResult = result
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
            await GameData.equipment.Get();
            await GameData.matriarchs.Get();

            UIManager.ThrowGameDataEvent(new GachaDataEvent
            {
                id = GameDataEventId.GachaBuy,
                buyResult = result
            });

            return result;
        }
    }

    //characters
    public class Characters : BaseGameMeta
    {
        public List<AdminBRO.CharacterBase> charactersBase { get; private set; } = new List<AdminBRO.CharacterBase>();
        public List<AdminBRO.Character> characters { get; private set; } = new List<AdminBRO.Character>();
        public List<AdminBRO.SkillEffect> effects { get; private set; } = new List<AdminBRO.SkillEffect>();

        public override async Task Get()
        {
            charactersBase = await AdminBRO.charactersBaseAsync();
            characters = await AdminBRO.charactersAsync();
            effects = await AdminBRO.skillEffectsAsync();
        }

        public AdminBRO.CharacterBase GetBaseById(int? id) =>
            charactersBase.Find(ch => ch.id == id);
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
                id = GameDataEventId.CharacterLvlUp
            });
        }

        public async Task SkillLvlUp(int chId, int skillId)
        {
            await AdminBRO.chracterSkillLvlUp(chId, skillId);
            await Get();
            await GameData.player.Get();
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                id = GameDataEventId.CharacterSkillLvlUp
            });
        }

        public async Task<AdminBRO.Character> Mrg(int? srcID, int? tgtId)
        {
            if (!srcID.HasValue || !tgtId.HasValue)
                return null;

            var result = await AdminBRO.charactersMrgAsync(srcID.Value, tgtId.Value);
            
            await GameData.player.Get();
            await Get();
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                id = GameDataEventId.CharacterMerge
            });

            return result.dData;
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
            GetByClass(AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Character> orderByLevel =>
            characters.OrderBy(ch => ch.level).ToList();
        public AdminBRO.Character slot1Ch =>
            characters.Find(ch => ch.teamPosition == AdminBRO.Character.TeamPosition_Slot1);
        public AdminBRO.Character slot2Ch =>
            characters.Find(ch => ch.teamPosition == AdminBRO.Character.TeamPosition_Slot2);

        public int myTeamPotency
        {
            get
            {
                int potency = 0;

                potency += overlord.potency;

                foreach (var character in myTeamCharacters)
                {
                    potency += character.potency;
                }

                return potency;
            }
        }
    }

    //equipment
    public class Equipment : BaseGameMeta
    {
        public List<AdminBRO.EquipmentBase> equipmentBase { get; private set; } = new List<AdminBRO.EquipmentBase>();
        public List<AdminBRO.Equipment> equipment { get; private set; } = new List<AdminBRO.Equipment>();

        public override async Task Get()
        {
            equipmentBase = await AdminBRO.equipmentBaseAsync();
            equipment = await AdminBRO.equipmentAsync();
        }

        public AdminBRO.EquipmentBase GetBaseById(int? id) =>
            equipmentBase.Find(e => e.id == id);
        public AdminBRO.Equipment GetById(int? id) =>
            equipment.Find(eq => eq.id == id);

        public List<AdminBRO.Equipment> chAll =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.CharacterWeapon &&
                !String.IsNullOrEmpty(e.characterClass) && e.characterClass != AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Equipment> chAssassins =>
            equipment.FindAll(e => e.characterClass == AdminBRO.CharacterClass.Assassin &&
                e.equipmentType == AdminBRO.EquipmentType.CharacterWeapon);
        public List<AdminBRO.Equipment> chBruisers =>
            equipment.FindAll(e => e.characterClass == AdminBRO.CharacterClass.Bruiser &&
                e.equipmentType == AdminBRO.EquipmentType.CharacterWeapon);
        public List<AdminBRO.Equipment> chTanks =>
            equipment.FindAll(e => e.characterClass == AdminBRO.CharacterClass.Tank &&
                e.equipmentType == AdminBRO.EquipmentType.CharacterWeapon);
        public List<AdminBRO.Equipment> chCasters =>
            equipment.FindAll(e => e.characterClass == AdminBRO.CharacterClass.Caster &&
                e.equipmentType == AdminBRO.EquipmentType.CharacterWeapon);
        public List<AdminBRO.Equipment> chHealers =>
            equipment.FindAll(e => e.characterClass == AdminBRO.CharacterClass.Healer &&
                e.equipmentType == AdminBRO.EquipmentType.CharacterWeapon);

        public List<AdminBRO.Equipment> ovAll =>
            equipment.FindAll(e => e.characterClass == AdminBRO.CharacterClass.Overlord &&
                !String.IsNullOrEmpty(e.equipmentType) && e.equipmentType != AdminBRO.EquipmentType.CharacterWeapon);
        public List<AdminBRO.Equipment> ovThighs =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.OverlordThighs &&
                e.characterClass == AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Equipment> ovHelmets =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.OverlordHelmet &&
                e.characterClass == AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Equipment> ovBoots =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.OverlordHarness &&
                e.characterClass == AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Equipment> ovWeapons =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.OverlordWeapon &&
                e.characterClass == AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Equipment> ovGloves =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.OverlordGloves &&
                e.characterClass == AdminBRO.CharacterClass.Overlord);
        public List<AdminBRO.Equipment> ovHarness =>
            equipment.FindAll(e => e.equipmentType == AdminBRO.EquipmentType.OverlordHarness &&
                e.characterClass == AdminBRO.CharacterClass.Overlord);

        public async Task Equip(int chId, int eqId)
        {
            await AdminBRO.equipAsync(chId, eqId);
            await GameData.characters.Get();
            await Get();
            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                id = GameDataEventId.EquipmentEquipped
            });
        }

        public async Task Unequip(int chId, int eqId)
        {
            await AdminBRO.unequipAsync(chId, eqId);
            await GameData.characters.Get();
            await Get();

            UIManager.ThrowGameDataEvent(new GameDataEvent
            {
                id = GameDataEventId.EquipmentUnequipped
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
        public async Task StageEnd(int stageId, AdminBRO.BattleEndData battleEndData = null)
        {
            var newEventStageData = await AdminBRO.eventStageEndAsync(stageId, battleEndData);
            stages = await AdminBRO.eventStagesAsync();

            await GameData.characters.Get();
            await GameData.equipment.Get();
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
        public List<AdminBRO.MarketItem> markets { get; private set; } = new List<AdminBRO.MarketItem>();

        public List<AdminBRO.TradableItem> tradables { get; private set; } = new List<AdminBRO.TradableItem>();

        public override async Task Get()
        {
            markets = await AdminBRO.marketsAsync();
            tradables = await AdminBRO.tradablesAsync();
        }

        public AdminBRO.MarketItem GetMarketById(int? id) =>
            markets.Find(m => m.id == id);
        public AdminBRO.TradableItem GetTradableById(int? id) =>
            tradables.Find(t => t.id == id);

        public AdminBRO.MarketItem mainMarket =>
            markets.Find(m => m.isMain);
        public List<AdminBRO.MarketItem> eventMarkets =>
            markets.FindAll(m => m.isEvent);

        public async Task<AdminBRO.TradableBuyStatus> BuyTradable(int? marketId, int? tradableId)
        {
            if (!marketId.HasValue || !tradableId.HasValue)
                return new AdminBRO.TradableBuyStatus { status = false };

            var result = await AdminBRO.tradableBuyAsync(marketId.Value, tradableId.Value);
            await GameData.player.Get();
            await GameData.characters.Get();
            await GameData.equipment.Get();
            await GameData.matriarchs.Get();

            if (result.dData.status == true)
            {
                UIManager.ThrowGameDataEvent(
                    new GameDataEvent
                    {
                        id = GameDataEventId.BuyTradable
                    });
            }

            return result;
        }
    }

    //quests
    public class Quests : BaseGameMeta
    {
        public List<AdminBRO.QuestItem> quests { get; private set; } = new List<AdminBRO.QuestItem>();

        //local marks (runtime only)
        public List<int> newIds { get; private set; } = new List<int>();
        public List<int> lastAddedIds { get; private set; } = new List<int>();
        public List<int> markCompletedIds { get; private set; } = new List<int>();

        public override async Task Get()
        {
            if (quests.Count > 0)
            {
                var prevQuestsIds = quests.Select(q => q.id).ToList();
                quests = await AdminBRO.questsAsync();
                lastAddedIds = quests.Select(q => q.id).
                    Where(qId => !prevQuestsIds.Exists(pqId => pqId == qId)).ToList();
                newIds.AddRange(lastAddedIds);

                //clear trash from marks
                newIds.RemoveAll(qId => !quests.Exists(q => qId == q.id));
                markCompletedIds.RemoveAll(qId => !quests.Exists(q => qId == q.id));
            }
            else
            {
                quests = await AdminBRO.questsAsync();
            }
        }

        public AdminBRO.QuestItem GetById(int? id) =>
            quests.Find(q => q.id == id);

        public List<AdminBRO.QuestItem> newQuests =>
            newIds.Select(qId => GetById(qId)).ToList();
        public List<AdminBRO.QuestItem> lastAddedQuests =>
            lastAddedIds.Select(qId => GetById(qId)).ToList();

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
        public AdminBRO.PlayerInfo prevInfo { get; private set; }

        public override async Task Get()
        {
            prevInfo = info;
            info = await AdminBRO.meAsync();
            //var locale = await AdminBRO.localizationAsync("en");

            lastTimeUpd = DateTime.Now;
            accEnergyPoints = 0.0f;

            CalcWalletChanges();
        }

        private void CalcWalletChanges()
        {
            var hasWalletChange = false;
            if (prevInfo != null)
            {
                foreach (var cw in info.wallet)
                {
                    var pw = prevInfo.wallet.Find(pw => pw.currencyId == cw.currencyId);
                    if (pw == null)
                    {
                        hasWalletChange = true;
                        break;
                    }
                    else
                    {
                        if (pw.amount != cw.amount)
                        {
                            hasWalletChange = true;
                            break;
                        }
                    }
                }
            }

            if (hasWalletChange)
            {
                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    id = GameDataEventId.WalletStateChange
                });
            }
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

                if (info.energyPointsAmount < GameData.potions.baseEnergyVolume)
                {
                    accEnergyPoints += (float)dt.TotalMinutes * GameData.potions.energyRecoverySpeed;
                    int accPointsIntPart = (int)accEnergyPoints;
                    accEnergyPoints -= accPointsIntPart;
                    info.energyPoints = Math.Min(info.energyPointsAmount + accPointsIntPart, GameData.potions.baseEnergyVolume);
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

        public bool CanBuy(List<AdminBRO.PriceItem> price)
        {
            if (price == null)
                return false;
        
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

        public async Task Start(int id)
        {
            await AdminBRO.dialogStartAsync(id);
        }

        public async Task End(int id)
        {
            await AdminBRO.dialogEndAsync(id);

            await GameData.characters.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();
        }
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

        public async Task Start(int id)
        {
            await AdminBRO.battleStartAsync(id);
        }

        public async Task<List<AdminBRO.GenRewardItem>> End(int id, AdminBRO.BattleEndData battleEndData = null)
        {
            var genRewards = await AdminBRO.battleEndAsync(id, battleEndData);

            await GameData.characters.Get();
            await GameData.quests.Get();
            await GameData.battlePass.Get();
            await GameData.player.Get();

            return genRewards;
        }
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

        public async Task MemoryPieceOfGlassBuy(int? memoryId, string shardKey)
        {
            if (memoryId.HasValue)
            {
                await AdminBRO.memoryPieceOfGlassBuyAsync(memoryId.Value, shardKey);
                memories = await AdminBRO.memoriesAsync();
                memoryShards = await AdminBRO.memoryShardsAsync();
                UIManager.ThrowGameDataEvent(new GameDataEvent
                {
                    id = GameDataEventId.PieceOfMemoryBuy,
                });
            }
        }

        public async Task MatriarchSeduce(int? id)
        {
            if (id.HasValue)
            {
                await AdminBRO.seduceMatriarchAsync(id.Value);
                matriarchs = await AdminBRO.matriarchsAsync();
                buffs = await AdminBRO.buffsAsync();
                await GameData.characters.Get();
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

        public async Task BuyPremium(int battlePassId)
        {
            await AdminBRO.battlePassBuyPremiumAsync(battlePassId);
            await Get();
        }

        public async Task ClaimRewards(int battlePassId)
        {
            await AdminBRO.battlePassClaimAsync(battlePassId);
            await Get();
        }
    }

    //potions
    public class Potions : BaseGameMeta
    {
        public AdminBRO.PotionsInfo potions { get; private set; } = new AdminBRO.PotionsInfo();

        public override async Task Get()
        {
            potions = await AdminBRO.potionsAsync();
        }

        public AdminBRO.PotionsInfo.PotionInfo hpInfo =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_hp);
        public AdminBRO.PotionsInfo.PotionInfo manaInfo =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_mana);
        public AdminBRO.PotionsInfo.PotionInfo energyInfo =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_energy);
        public AdminBRO.PotionsInfo.PotionInfo replayInfo =>
            potions.prices.Find(p => p.type == AdminBRO.PotionsInfo.Type_replay);

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
                    id = GameDataEventId.BuyPotions
                });
        }

        public async Task UseEnergy(int count)
        {
            await AdminBRO.potionEnergyUseAsync(count);
            await GameData.player.Get();

            UIManager.ThrowGameDataEvent(
                new GameDataEvent
                {
                    id = GameDataEventId.UsePotions
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

    //nutaku
    public class NutakuMy : BaseGameMeta
    {
        public AdminBRO.NutakuSettings settings { get; private set; } = new AdminBRO.NutakuSettings();

        public override async Task Get()
        {
            settings = await AdminBRO.nutakuSettingsAsync();
        }
    }
}
