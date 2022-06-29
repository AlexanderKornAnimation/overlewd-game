using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;

namespace Overlewd
{
    public static class AdminBRO
    {
        public static string GetDeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        // common entities
        [Serializable]
        public class PriceItem
        {
            public int currencyId;
            public int amount;
        }

        [Serializable]
        public class RewardItem
        {
            public string icon;
            public int? amount;
            public int? tradableId;
        }

        // /version
        public static async Task<ApiVersion> versionAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/version"))
            {
                return JsonHelper.DeserializeObject<ApiVersion>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class ApiVersion
        {
            public int version;
        }

        // auth/login; auth/refresh
        public static async Task<Tokens> authLoginAsync()
        {
            var postData = new WWWForm();
            postData.AddField("deviceId", GetDeviceId());
            using (var request = await HttpCore.PostAsync("https://overlewd-api.herokuapp.com/auth/login", postData))
            {
                tokens = JsonHelper.DeserializeObject<Tokens>(request?.downloadHandler.text);
                return tokens;
            }
        }

        public static async Task<Tokens> authRefreshAsync()
        {
            var postData = new WWWForm();
            using (var request = await HttpCore.PostAsync("https://overlewd-api.herokuapp.com/auth/refresh", postData))
            {
                tokens = JsonHelper.DeserializeObject<Tokens>(request?.downloadHandler.text);
                return tokens;
            }
        }

        [Serializable]
        public class Tokens
        {
            public string accessToken;
            public string refreshToken;
        }

        public static Tokens tokens;

        // GET /me; POST /me
        // /me/init
        // /me/reset
        // /me/currency
        public static async Task<PlayerInfo> meAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/me", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<PlayerInfo>(request?.downloadHandler.text);
            }
        }

        public static async Task<PlayerInfo> meAsync(string name)
        {
            var form = new WWWForm();
            form.AddField("name", name);
            form.AddField("currentVersion", HttpCore.ApiVersion);
            using (var request = await HttpCore.PostAsync("https://overlewd-api.herokuapp.com/me", form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<PlayerInfo>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class PlayerInfo
        {
            public int id;
            public string name;
            public string locale;
            public List<WalletItem> wallet;
            public Poison poison;

            public class Poison
            {
                public int hp;
                public int mana;
            }

            public class WalletItem
            {
                public int currencyId;
                public int amount;
            }
        }

        public static async Task initAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/me/init";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }


        public static async Task resetAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/me/reset";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task meCurrencyAsync(int currencyId, int amount)
        {
            var form = new WWWForm();
            form.AddField("currencyId", currencyId);
            form.AddField("amount", amount);
            using (var request = await HttpCore.PostAsync("https://overlewd-api.herokuapp.com/me/currency", form, tokens?.accessToken))
            {
                
            }
        }

        // /markets
        public static async Task<List<EventMarketItem>> eventMarketsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/markets";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventMarketItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class EventMarketItem
        {
            public int id;
            public string name;
            public string description;
            public string bannerImage;
            public string eventMapNodeName;
            public string createdAt;
            public string updatedAt;
            public List<int> tradables;
            public List<int> currencies;

            [JsonProperty(Required = Required.Default)]
            public List<TradableItem> tradablesData =>
                tradables.Select(id => GameData.markets.GetTradableById(id)).
                Where(data => data != null).OrderByDescending(item => item.promo).ToList();
        }

        // /currencies
        public static async Task<List<CurrencyItem>> currenciesAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/currencies", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<CurrencyItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class CurrencyItem
        {
            public int id;
            public string name;
            public string icon356Url;
            public string icon256Url;
            public string icon186Url;
            public string icon153Url;
            public string icon70Url;
            public string iconUrl;
            public string key;
            public bool nutaku;
            public string createdAt;
            public string updatedAt;

            [JsonProperty(Required = Required.Default)]
            public string sprite =>
                key switch
                { 
                    Key_Crystals => "<sprite=\"AssetResources\" name=\"Crystal\">",
                    Key_Wood => "<sprite=\"AssetResources\" name=\"Wood\">",
                    Key_Stone => "<sprite=\"AssetResources\" name=\"Stone\">",
                    Key_Copper => "<sprite=\"AssetResources\" name=\"Copper\">",
                    Key_Gold => "<sprite=\"AssetResources\" name=\"Gold\">",
                    Key_Gems => "<sprite=\"AssetResources\" name=\"Gem\">",
                    Key_Ears => "<sprite=\"EventCurrency\" name=\"CatEras\">",
                    Key_Ngold => "<sprite=\"EventCurrency\" name=\"NutakuGold\">",
                    _ => ""
                };

            public const string Key_Copper = "copper";
            public const string Key_Crystals = "crystal";
            public const string Key_Gems = "gems";
            public const string Key_Gold = "gold";
            public const string Key_Stone = "stone";
            public const string Key_Wood = "wood";
            public const string Key_Ears = "ears";
            public const string Key_Horny = "horny";
            public const string Key_Yen = "yen";
            public const string Key_Ngold = "ngold";
        }

        // /tradable
        public static async Task<List<TradableItem>> tradablesAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/tradable";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<TradableItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class TradableItem
        {
            public int id;
            public string name;
            public string type;
            public bool promo;
            public bool donat;
            public bool hidden;
            public string imageUrl;
            public string description;
            public List<PriceItem> price;
            public string discount;
            public string specialOfferLabel;
            public List<int> itemPack;
            public int? currencyId;
            public int? currencyAmount;
            public int? limit;
            public int? characterId;
            public int? equipmentId;
            public int? potionCount;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
            public int currentCount;
            public bool soldOut;
            public int? matriarchShardId;

            [JsonProperty(Required = Required.Default)]
            public const string Type_Currency = "currency";
            
            [JsonProperty(Required = Required.Default)]
            public bool canBuy => GameData.player.CanBuy(price);
        }

        // /markets/{marketId}/tradable/{tradableId}/buy
        public static async Task<TradableBuyStatus> tradableBuyAsync(int marketId, int tradableId)
        {
            var form = new WWWForm();
            var url = $"https://overlewd-api.herokuapp.com/markets/{marketId}/tradable/{tradableId}/buy";
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<TradableBuyStatus>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class TradableBuyStatus
        {
            public bool status;
        }

        // /resources
        public static async Task<List<NetworkResource>> resourcesAsync()
        {
            var url = Application.platform switch
            {
                RuntimePlatform.Android => "https://overlewd-api.herokuapp.com/resources?platform=android",
                RuntimePlatform.WindowsEditor => "https://overlewd-api.herokuapp.com/resources?platform=windows",
                RuntimePlatform.WindowsPlayer => "https://overlewd-api.herokuapp.com/resources?platform=windows",
                RuntimePlatform.WebGLPlayer => "https://overlewd-api.herokuapp.com/resources?platform=webgl",
                _ => "https://overlewd-api.herokuapp.com/resources"
            };

            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<NetworkResource>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class NetworkResource
        {
            public string id;
            public string type;
            public string buildVersion;
            public string hash;
            public int size;
            public string url;
        }

        [Serializable]
        public class NetworkResourceShort
        {
            public string id;
            public string hash;
        }

        // /event-chapters
        public static async Task<List<EventChapter>> eventChaptersAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/event-chapters", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventChapter>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class EventChapter
        {
            public int id;
            public string name;
            public int? chapterMapId;
            public int eventId;
            public int? nextChapterId;
            public int? durationInDays;
            public List<int> stages;
            public int? order;
            public List<EventChapterReward> rewards;

            public class EventChapterReward
            {
                public string icon;
                public int amount;
                public int currency;
            }

            [JsonProperty(Required = Required.Default)]
            public bool isComplete {
                get {
                    foreach (var stageId in stages)
                    {
                        var stageData = GetStageById(stageId);
                        if (!stageData?.isComplete ?? true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            [JsonProperty(Required = Required.Default)]
            public EventChapter nextChapterData =>
                GameData.events.GetChapterById(nextChapterId);

            public AdminBRO.EventStageItem GetStageById(int? id) =>
                GameData.events.GetStageById(id);

            [JsonProperty(Required = Required.Default)]
            public List<EventStageItem> stagesData =>
                stages.Select(id => GameData.events.GetStageById(id)).Where(data => data != null).ToList();
        }

        // /events
        public static async Task<List<EventItem>> eventsAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/events", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class EventItem
        {
            public int id;
            public string type;
            public string name;
            public string label;
            public string description;
            public string dateStart;
            public string dateEnd;
            public List<int> currencies;
            public string mapBackgroundImage;
            public string mapBannerImage;
            public string eventListBannerImage;
            public string bannerOverlayText;
            public List<int> markets;
            public List<int> quests;
            public string createdAt;
            public string updatedAt;
            public List<int> chapters;
            public List<RewardItem> rewards;

            public const string Type_Quarterly = "quarterly";
            public const string Type_Monthly = "monthly";
            public const string Type_Weekly = "weekly";

            [JsonProperty(Required = Required.Default)]
            public List<EventChapter> chaptersData =>
                chapters.Select(chId => GetChapterById(id)).OrderBy(ch => ch.order).ToList();

            [JsonProperty(Required = Required.Default)]
            public EventChapter firstChapter =>
                chaptersData.First();

            [JsonProperty(Required = Required.Default)]
            public EventChapter activeChapter {
                get {
                    var chapterData = firstChapter;
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

            public EventChapter GetChapterById(int? id) =>
                GameData.events.GetChapterById(id);

            public EventItem SetAsMapEvent() =>
                GameData.events.mapEventData = this;

            [JsonProperty(Required = Required.Default)]
            public List<EventMarketItem> marketsData =>
                markets.Select(id => GameData.markets.GetEventMarketById(id)).Where(data => data != null).ToList();
        }
        

        // /event-stages
        public static async Task<List<EventStageItem>> eventStagesAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/event-stages";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventStageItem>>(request?.downloadHandler.text);
            }
        }

        // /event-stages/{id}/start
        public static async Task<EventStageItem> eventStageStartAsync(int eventStageId)
        {
            var url = $"https://overlewd-api.herokuapp.com/event-stages/{eventStageId}/start";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<EventStageItem>(request?.downloadHandler.text);
            }
        }

        // /event-stages/{id}/end
        public static async Task<EventStageItem> eventStageEndAsync(int eventStageId, EventStageEndData data = null)
        {
            var url = $"https://overlewd-api.herokuapp.com/event-stages/{eventStageId}/end";
            var form = data?.ToWWWForm() ?? new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<EventStageItem>(request?.downloadHandler.text);
            }
        }

        public class EventStageEndData
        {
            public bool win { get; set; } = true;
            public int mana { get; set; } = 0;
            public int hp { get; set; } = 0;

            public WWWForm ToWWWForm()
            {
                var form = new WWWForm();
                form.AddField("result", win ? "win" : "lose");
                form.AddField("mana", -mana);
                form.AddField("hp",-hp);
                return form;
            }
        }


        [Serializable]
        public class EventStageItem
        {
            public int index;
            public int id;
            public string title;
            public int? dialogId;
            public int? battleId;
            public string mapNodeName;
            public List<int> nextStages;
            public string status;
            public int? order;

            public const string Type_Battle = "battle";
            public const string Type_Dialog = "dialog";

            public const string Status_Open = "open";
            public const string Status_Started = "started";
            public const string Status_Complete = "complete";
            public const string Status_Closed = "closed";

            [JsonProperty(Required = Required.Default)]
            public Dialog dialogData =>
                GameData.dialogs.GetById(dialogId);

            [JsonProperty(Required = Required.Default)]
            public Battle battleData =>
                GameData.battles.GetById(battleId);

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;

            [JsonProperty(Required = Required.Default)]
            public bool isStarted => status == Status_Started;

            [JsonProperty(Required = Required.Default)]
            public bool isComplete => status == Status_Complete;

            [JsonProperty(Required = Required.Default)]
            public bool isClosed => status == Status_Closed;
        }

        // /quests
        public static async Task<List<QuestItem>> questsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/quests";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<QuestItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class QuestItem
        {
            public int id;
            public string name;
            public string subtitle;
            public string description;
            public int? goalCount;
            public List<RewardItem> rewards;
            public string status;
            public int progressCount;
            public int? eventId;

            public const string Status_New = "new";
            public const string Status_In_Progress = "in_progress";
            public const string Status_Complete = "complete";
            public const string Status_Rewards_Claimed = "rewards_claimed";
        }

        // //quests/{id}/claim-reward
        public static async Task<QuestClaimReward> questClaimRewardAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/quests/{id}/claim-reward";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<QuestClaimReward>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class QuestClaimReward
        {
            public int userId;
            public int questId;
            public string status;
            public int id;
            public int progressCount;
            public string createdAt;
            public string updatedAt;

            public const string Status_Rewards_Claimed = "rewards_claimed";
        }

        // /i18n
        public static async Task<List<LocalizationItem>> localizationAsync(string locale)
        {
            var url = String.Format("https://overlewd-api.herokuapp.com/i18n?locale={0}", locale);
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<LocalizationItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class LocalizationItem
        {
            public int id;
            public string key;
            public string type;
            public string locale;
            public string text;
            public string descripton;
            public string createdAt;
            public string updatedAt;
        }

        // /dialogs
        public static async Task<List<Dialog>> dialogsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/dialogs";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Dialog>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class DialogReplica
        {
            public int id;
            public int sort;
            public string characterName;
            public string characterSkin;
            public string characterPosition;
            public string message;

            public int? emotionAnimationId;
            public int? cutInAnimationId;
            public int? mainAnimationId;

            public int? backgroundMusicId;
            public int? mainSoundId;
            public int? cutInSoundId;
            public int? replicaSoundId;

            public const string CharacterPosition_Left = "left";
            public const string CharacterPosition_Right = "right";
            public const string CharacterPosition_Middle = "middle";

            public const string CharacterName_Overlord = "Overlord";
            public const string CharacterName_Ulvi = "Ulvi";
            public const string CharacterName_Faye = "Faye";
            public const string CharacterName_Adriel = "Adriel";

            public const string CharacterSkin_Overlord = "Overlord";
            public const string CharacterSkin_Ulvi = "Ulvi";
            public const string CharacterSkin_UlviWolf = "UlviWolf";
            public const string CharacterSkin_Adriel = "Adriel";
            public const string CharacterSkin_Inge = "Inge";
        }

        [Serializable]
        public class Dialog
        {
            public int id;
            public string title;
            public string type;
            public List<DialogReplica> replicas;
            public int? matriarchId;
            public int? matriarchEmpathyPointsReward;

            public const string Type_Dialog = "dialog";
            public const string Type_Sex = "sex";
            public const string Type_Notification = "notification";

            [JsonProperty(Required = Required.Default)]
            public bool isTypeDialog => type == Type_Dialog;

            [JsonProperty(Required = Required.Default)]
            public bool isTypeSex => type == Type_Sex;

            [JsonProperty(Required = Required.Default)]
            public bool isTypeNotification => type == Type_Notification;
        }

        // /battles
        public static async Task<List<Battle>> battlesAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/battles";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Battle>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Battle
        {
            public int id;
            public string title;
            public string type;
            public List<RewardItem> rewards;
            public string rewardSpriteString;
            public List<RewardItem> firstRewards;
            public List<Phase> battlePhases;
            public int? battlePassPointsReward;

            public const string Type_Battle = "battle";
            public const string Type_Boss = "boss";

            public class Phase 
            {
                public List<Character> enemyCharacters;
            }

            [JsonProperty(Required = Required.Default)]
            public bool isTypeBattle => type == Type_Battle;

            [JsonProperty(Required = Required.Default)]
            public bool isTypeBoss => type == Type_Boss;
        }

        // /my/characters
        // /battles/my/characters/{id}/equip/{id} - post
        // /battles/my/characters/{id}/equip/{id} - delete
        // /battles/my/characters/{id}
        // /battles/my/characters/{id}/levelup
        // /battles/my/characters/{tgtId}/merge/{srcId}
        public static async Task<List<Character>> charactersAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/battles/my/characters";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Character>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Character
        {
            public int? id;
            public string teamPosition;
            public string teamEditPersIcon;
            public string teamEditSlotPersIcon;
            public string fullScreenPersIcon;
            public string name;
            public string characterClass;
            public List<Animation> animations;
            public int? level;
            public string rarity;
            public List<int> equipment;
            public List<CharacterSkill> skills;
            public float? speed;
            public float? power;
            public float? constitution;
            public float? agility;
            public float? accuracy;
            public float? dodge;
            public float? critrate;
            public float? health;
            public float? damage;
            public float? mana;
            public float? potency;
            public int? sexSceneId;
            public string sexSceneClosedBanner;
            public string sexSceneOpenedBanner;
            public string key;
            public List<PriceItem> levelUpPrice;
            public List<PriceItem> mergePrice;

            public class Animation
            {
                public string name;
                public int? animation;
            }

            public const string TeamPosition_Slot1 = "slot1";
            public const string TeamPosition_Slot2 = "slot2";
            public const string TeamPosition_None = "none";

            public const string Class_Assassin = "Assassin";
            public const string Class_Bruiser = "Bruiser";
            public const string Class_Caster = "Caster";
            public const string Class_Healer = "Healer";
            public const string Class_Overlord = "Overlord";
            public const string Class_Tank = "Tank";

            public const string Sprite_AllyAssassin = "<sprite=\"ClassesNLevel\" name=\"AllyAssasin\">";
            public const string Sprite_AllyBruiser = "<sprite=\"ClassesNLevel\" name=\"AllyBruiser\">";
            public const string Sprite_AllyCaster = "<sprite=\"ClassesNLevel\" name=\"AllyCaster\">";
            public const string Sprite_AllyHealer = "<sprite=\"ClassesNLevel\" name=\"AllyHealer\">";
            public const string Sprite_Overlord = "<sprite=\"ClassesNLevel\" name=\"Overlord\">";
            public const string Sprite_AllyTank = "<sprite=\"ClassesNLevel\" name=\"AllyTank\">";
            public static string GetMyClassMarker(string classId)
            {
                return classId switch
                {
                    Class_Assassin => Sprite_AllyAssassin,
                    Class_Bruiser => Sprite_AllyBruiser,
                    Class_Caster => Sprite_AllyCaster,
                    Class_Healer => Sprite_AllyHealer,
                    Class_Overlord => Sprite_Overlord,
                    Class_Tank => Sprite_AllyTank,
                    _ => ""
                };
            }
            
            public const string Sprite_EnemyAssassin = "<sprite=\"ClassesNLevel\" name=\"EnemyAssasin\">";
            public const string Sprite_EnemyBruiser = "<sprite=\"ClassesNLevel\" name=\"EnemyBruiser\">";
            public const string Sprite_EnemyCaster = "<sprite=\"ClassesNLevel\" name=\"EnemyCaster\">";
            public const string Sprite_EnemyHealer = "<sprite=\"ClassesNLevel\" name=\"EnemyHealer\">";
            public const string Sprite_EnemyTank = "<sprite=\"ClassesNLevel\" name=\"EnemyTank\">";
            public static string GetEnemyClassMarker(string classId)
            {
                return classId switch
                {
                    Class_Assassin => Sprite_EnemyAssassin,
                    Class_Bruiser => Sprite_EnemyBruiser,
                    Class_Caster => Sprite_EnemyCaster,
                    Class_Healer => Sprite_EnemyHealer,
                    Class_Tank => Sprite_EnemyTank,
                    _ => ""
                };
            }

            public const string Rarity_Basic = "basic";
            public const string Rarity_Advanced = "advanced";
            public const string Rarity_Epic = "epic";
            public const string Rarity_Heroic = "heroic";

            [JsonProperty(Required = Required.Default)]
            public bool isBasic => rarity == Rarity_Basic;
            
            [JsonProperty(Required = Required.Default)]
            public bool isAdvanced => rarity == Rarity_Advanced;
            
            [JsonProperty(Required = Required.Default)]
            public bool isEpic => rarity == Rarity_Epic;
            
            [JsonProperty(Required = Required.Default)]
            public bool isHeroic => rarity == Rarity_Heroic;
            
            [JsonProperty(Required = Required.Default)]
            public bool canLvlUp => !isLvlMax && GameData.player.CanBuy(levelUpPrice);

            [JsonProperty(Required = Required.Default)]
            public bool isLvlMax => maxLvl == level;

            public bool CanSkillLvlUp(CharacterSkill skill)
            {
                var isMax = level == skill.level;
                return !isMax && GameData.player.CanBuy(skill.levelUpPrice);
            } 
            
            [JsonProperty(Required = Required.Default)]
            public int maxLvl => rarity switch
            {
                Rarity_Basic => 10,
                Rarity_Advanced => 20,
                Rarity_Epic => 30,
                Rarity_Heroic => 40,
                _ => 10
            };
        }

        [Serializable]
        public class CharacterSkill
        {
            public string name;
            public string description;
            public string icon;
            public string effect;
            public string type;
            public int effectProb;
            public float effectActingDuration;
            public float effectCooldownDuration;
            public List<PriceItem> levelUpPrice;
            public int level;
            public int manaCost;
            public string actionType;
            public bool AOE;
            public int amount;
            public string trigger;
            public int effectAmount;

            public const string Type_Passive = "passive_skill";
            public const string Type_Attack = "attack";
            public const string Type_Enhanced = "enhanced_attack";
        }

        public static async Task equipAsync(int characterId, int equipmentId)
        {
            var url = $"https://overlewd-api.herokuapp.com/battles/my/characters/{characterId}/equip/{equipmentId}";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                
            }
        }

        public static async Task unequipAsync(int characterId, int equipmentId)
        {
            var url = $"https://overlewd-api.herokuapp.com/battles/my/characters/{characterId}/equip/{equipmentId}";
            using (var request = await HttpCore.DeleteAsync(url, tokens?.accessToken))
            {

            }
        }

        public static async Task characterToSlotAsync(int characterId, string slotId)
        {
            var url = $"https://overlewd-api.herokuapp.com/battles/my/characters/{characterId}";
            var form = new WWWForm();
            form.AddField("teamPosition", slotId);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task characterLvlupAsync(int characterId)
        {
            var url = $"https://overlewd-api.herokuapp.com/battles/my/characters/{characterId}/levelup";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task chracterSkillLvlUp(int characterId, string skillType)
        {
            var url = $"https://overlewd-api.herokuapp.com/battles/my/characters/{characterId}/skills/{skillType}/levelup";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task charactersMrgAsync(int srcCharacterId, int trgtCharacterId)
        {
            var url = $"https://overlewd-api.herokuapp.com/battles/my/characters/{trgtCharacterId}/merge/{srcCharacterId}";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /my/characters/equipment
        public static async Task<List<Equipment>> equipmentAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/battles/my/characters/equipment";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Equipment>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Equipment
        {
            public int id;
            public int? characterId;
            public int? characterClassId;
            public string name;
            public float speed;
            public float power;
            public float constitution;
            public float agility;
            public float accuracy;
            public float dodge;
            public float critrate;
            public float health;
            public float damage;
            public float mana;
        }

        //ftue
        public static async Task<FTUEInfo> ftueAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/ftue";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<FTUEInfo>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class FTUENotificationItem
        {
            public int id;
            public string key;
            public int? ftueChapterId;
            public int? dialogId;

            [JsonProperty(Required = Required.Default)]
            public AdminBRO.Dialog dialogData =>
                GameData.dialogs.GetById(dialogId);
        }

        [Serializable]
        public class FTUEChapter
        {
            public int id;
            public string key;
            public string name;
            public int? chapterMapId;
            public List<FTUENotificationItem> notifications;
            public List<int> stages;
            public int? nextChapterId;
            public int? order;

            public FTUENotificationItem GetNotifByKey(string key)
            {
                return notifications.Find(n => n.key == key);
            }

            public void ShowNotifByKey(string key)
            {
                UIManager.MakeNotification<DialogNotification>().
                    SetData(new DialogNotificationInData
                    {
                        dialogId = GetNotifByKey(key)?.dialogId
                    }).RunShowNotificationProcess();
            }

            public FTUEChapter SetAsMapChapter() =>
                GameData.ftue.mapChapter = this;

            [JsonProperty(Required = Required.Default)]
            public bool isComplete {
                get {
                    foreach (var stageId in stages)
                    {
                        var stageData = GetStageById(stageId);
                        if (!stageData?.isComplete ?? true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            [JsonProperty(Required = Required.Default)]
            public FTUEChapter nextChapterData => 
                GameData.ftue.info.GetChapterById(nextChapterId);

            public FTUEStageItem GetStageById(int? id) =>
                GameData.ftue.stages.Find(s => s.id == id);
            public FTUEStageItem GetStageByKey(string key) =>
                GameData.ftue.stages.Find(s => s.key == key && s.ftueChapterId == id);
        }

        [Serializable]
        public class FTUEInfo
        {
            public List<FTUEChapter> chapters;

            [JsonProperty(Required = Required.Default)]
            public FTUEChapter chapter1 => GetChapterByKey("chapter1");

            [JsonProperty(Required = Required.Default)]
            public FTUEChapter chapter2 => GetChapterByKey("chapter2");

            [JsonProperty(Required = Required.Default)]
            public FTUEChapter chapter3 => GetChapterByKey("chapter3");

            public FTUEChapter GetChapterByKey(string key) =>
                chapters.Find(ch => ch.key == key);
            public FTUEChapter GetChapterById(int? id) =>
                chapters.Find(ch => ch.id == id);
            public FTUEStageItem GetStageById(int? id) =>
                GameData.ftue.stages.Find(s => s.id == id);
            public FTUEStageItem GetStageByKey(string stageKey, int chapterId) =>
                GetChapterById(chapterId).GetStageByKey(stageKey);
            public FTUEStageItem GetStageByKey(string stageKey, string chapterKey) =>
                GetChapterByKey(chapterKey).GetStageByKey(stageKey);
            public bool StageIsComplete(string stageKey, string chapterKey) =>
                GetStageByKey(stageKey, chapterKey).isComplete;

        }

        // ftue/stats
        public static async Task<FTUEStats> ftueStatsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/ftue/stats";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<FTUEStats>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class FTUEStats
        {
            public int? lastStartedStage;
            public int? lastEndedStage;

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastStartedStageData =>
                GameData.ftue.info.GetStageById(lastStartedStage);

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastEndedStageData =>
                GameData.ftue.info.GetStageById(lastEndedStage);

            [JsonProperty(Required = Required.Default)]
            public (string stageKey, string chapterKey)? lastEndedState =>
                GameData.devMode switch {
                    false => (lastEndedStageData?.key, lastEndedStageData?.ftueChapterData?.key),
                    _ => null
                };

            public bool IsLastEnededStage(string stageKey, string chapterKey) =>
                lastEndedState == (stageKey, chapterKey);
        }

        // /ftue-stages
        public static async Task<List<FTUEStageItem>> ftueStagesAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/ftue-stages";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<FTUEStageItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class FTUEStageItem
        {
            public int id;
            public string key;
            public int? ftueChapterId;
            public int? dialogId;
            public int? battleId;
            public string mapNodeName;
            public string status;
            public string type;
            public List<int> nextStages;
            public int? order;

            public const string Status_Open = "open";
            public const string Status_Started = "started";
            public const string Status_Complete = "complete";
            public const string Status_Closed = "closed";

            public const string Type_Dialog = "dialog";
            public const string Type_Battle = "battle";

            [JsonProperty(Required = Required.Default)]
            public FTUEChapter ftueChapterData =>
                GameData.ftue.info.GetChapterById(ftueChapterId);

            [JsonProperty(Required = Required.Default)]
            public Dialog dialogData =>
                GameData.dialogs.GetById(dialogId);

            [JsonProperty(Required = Required.Default)]
            public Battle battleData => 
                GameData.battles.GetById(battleId);

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;

            [JsonProperty(Required = Required.Default)]
            public bool isStarted => status == Status_Started;
         
            [JsonProperty(Required = Required.Default)]
            public bool isComplete => status == Status_Complete;

            [JsonProperty(Required = Required.Default)]
            public bool isClosed => status == Status_Closed;

            [JsonProperty(Required = Required.Default)]
            public (string stageKey, string chapterKey)? ftueState =>
                GameData.devMode switch
                {
                    false => (key, ftueChapterData?.key),
                    _ => null
                };
        }

        // /ftue-stages/{id}/start
        public static async Task ftueStageStartAsync(int stageId)
        {
            var form = new WWWForm();
            var url = $"https://overlewd-api.herokuapp.com/ftue-stages/{stageId}/start";
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /ftue-stages/{id}/end
        public static async Task ftueStageEndAsync(int stageId, FTUEStageEndData data = null)
        {
            var url = $"https://overlewd-api.herokuapp.com/ftue-stages/{stageId}/end";
            var form = data?.ToWWWForm() ?? new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public class FTUEStageEndData
        {
            public bool win { get; set; } = true;
            public int mana { get; set; } = 0;
            public int hp { get; set; } = 0;

            public WWWForm ToWWWForm()
            {
                var form = new WWWForm();
                form.AddField("result", win ? "win" : "lose");
                form.AddField("mana", -mana);
                form.AddField("hp", -hp);
                return form;
            }
        }

        //animations
        public static async Task<List<Animation>> animationsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/animations";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Animation>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Animation
        {
            public int id;
            public string title;
            public List<LayoutData> layouts;

            public class LayoutData
            {
                public string assetBundleId;
                public string animationPath;
                public string animationName;
            }
        }

        //sounds
        public static async Task<List<Sound>> soundsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/sounds";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Sound>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Sound
        {
            public int id;
            public string title;
            public string soundBankId;
            public string eventPath;
        }

        // /buildings
        // /buildings/{id}/build
        // /buildings/{id}/build-crystals
        public static async Task<List<Building>> buildingsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/buildings";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Building>>(request?.downloadHandler.text);
            }
        }

        public static async Task buildingBuildAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/buildings/{id}/build";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task buildingBuildCrystalsAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/buildings/{id}/build-crystals";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class Building
        {
            public int id;
            public string key;
            public string name;
            public string description;
            public List<Level> levels;
            public int? currentLevel;
            public int? nextLevel;
            public int? maxLevel;

            public const string Key_Castle = "castle";
            public const string Key_Catacombs = "catacombs";
            public const string Key_Laboratory = "laboratory";
            public const string Key_Aerostat = "aerostat";
            public const string Key_Forge = "forge";
            public const string Key_Harem = "harem";
            public const string Key_MagicGuild = "magicGuild";
            public const string Key_Market = "market";
            public const string Key_Municipality = "municipality";
            public const string Key_Portal = "portal";

            public class Level
            {
                public List<PriceItem> price;
                public List<PriceItem> crystalPrice;

                [JsonProperty(Required = Required.Default)]
                public bool canBuild => GameData.player.CanBuy(price);

                [JsonProperty(Required = Required.Default)]
                public bool canBuildCrystal => GameData.player.CanBuy(crystalPrice);
            }

            [JsonProperty(Required = Required.Default)]
            public bool isMax => isBuilt ? currentLevel == maxLevel : false;

            [JsonProperty(Required = Required.Default)]
            public bool isBuilt => currentLevel.HasValue;

            [JsonProperty(Required = Required.Default)]
            public Level currentLevelData =>
                currentLevel.HasValue ? levels[currentLevel.Value] : null;

            [JsonProperty(Required = Required.Default)]
            public Level nextLevelData =>
                nextLevel.HasValue ? levels[nextLevel.Value] : null;

            [JsonProperty(Required = Required.Default)]
            public Level maxLevelData =>
                maxLevel.HasValue ? levels[maxLevel.Value] : null;

            [JsonProperty(Required = Required.Default)]
            public bool canUpgradeCrystal =>
                nextLevelData?.canBuildCrystal ?? false;

            [JsonProperty(Required = Required.Default)]
            public bool canUpgrade =>
                nextLevelData?.canBuild ?? false;
        }

        // /municipality/time-left
        // /municipality/collect
        public static async Task<MunicipalityTimeLeft> municipalityTimeLeftAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/municipality/time-left";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<MunicipalityTimeLeft>(request?.downloadHandler.text);
            }
        }

        public static async Task municipalityCollectAsync()
        {
            var url = $"https://overlewd-api.herokuapp.com/municipality/collect";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class MunicipalityTimeLeft
        {
            public int timeLeft;
        }

        // /magicguild/skills
        // /magicguild/{skillType}/levelup
        public static async Task<List<MagicGuildSkill>> magicGuildSkillsAsync()
        {
            var url = $"https://overlewd-api.herokuapp.com/magicguild/skills";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MagicGuildSkill>>(request?.downloadHandler.text);
            }
        }

        public static async Task magicGuildSkillLvlUpAsync(string skillType)
        {
            var url = $"https://overlewd-api.herokuapp.com/magicguild/{skillType}/levelup";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class MagicGuildSkill
        {
            public string type;
            public Current current;
            public int? next;
            public int currentSkillLevel;
            public int maxSkillLevel;

            public class Current
            {
                public int id;
                public string name;
                public string description;
                public string actionType;
                public bool AOE;
                public float amount;
                public float amountBoost;
                public string effect;
                public string type;
                public string trigger;
                public float effectAmount;
                public float effectAmountBoost;
                public float effectProb;
                public float effectProbBoost;
                public string icon;
                public float effectActingDuration;
                public float effectActingDurationBoost;
                public float effectCooldownDuration;
                public float effectCooldownDurationBoost;
                public float manaCost;
                public float manaCostBoost;
                public List<PriceItem> levelUpPrice;
                public List<PriceItem> levelUpPriceBoost;
            }
        }

        //chapter-maps
        public static async Task<List<ChapterMap>> chapterMapsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/chapter-maps";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<ChapterMap>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class ChapterMap
        {
            public int id;
            public string title;
            public string assetBundleId;
            public string chapterMapPath;
        }

        // gacha
        public static async Task<List<GachItem>> gachaAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/gacha";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<GachItem>>(request?.downloadHandler.text);
            }
        }

        public static async Task gachaBuyAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/gacha/{id}/buy";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {

            }
        }

        public static async Task gachaBuyTenAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/gacha/{id}/buy-ten";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class GachItem
        {
            public int id;
            public string tabTitle;
            public string image;
            public string tabImageOn;
            public string tabImageOff;
            public string tabBackgroundImage;
            public string tabType;
            public List<PriceItem> priceForOne;
            public List<PriceItem> priceForTen;
            public int? discount;
            public int? limitOfCycles;
            public List<PoolItem> pool;
            public List<PoolItem> targetPool;
            public string type;
            public List<TierItem> tiers;
            public string dateStart;
            public string dateEnd;
            public int? eventId;

            public class PoolItem
            {
                public int tradableId;
                public int probability;
            }

            public class TierItem
            {
                public string title;
            }

            public const string TabType_Matriachs = "matriachs";
            public const string TabType_CharactersEquipment = "battle_characters_equipment";
            public const string TabType_OverlordEquipment = "overlord_equipment";
            public const string TabType_Shards = "shards";

            public const string Type_Linear = "linear";
            public const string Type_Stepwise = "stepwise";
        }

        // /matriarchs
        // /matriarchs/memories
        // /matriarchs/memories/{id}/buy
        // /matriarchs/{id}/seduce

        public static async Task<List<MatriarchItem>> matriarchsAsync()
        {
            var url = $"https://overlewd-api.herokuapp.com/matriarchs";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MatriarchItem>>(request?.downloadHandler.text);
            }
        }

        public static async Task<List<MemoryItem>> memoriesAsync()
        {
            var url = $"https://overlewd-api.herokuapp.com/matriarchs/memories";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MemoryItem>>(request?.downloadHandler.text);
            }
        }

        public static async Task memoryBuyAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/matriarchs/memories/{id}/buy";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
            
            }
        }

        public static async Task seduceMatriarchAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/matriarchs/{id}/seduce";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class MatriarchItem
        {
            public int id;
            public string name;
            public int? paramAge;
            public string paramZodiac;
            public int? seduceSexSceneId;
            public int seduceCooldown;
            public int? seduceBuffSkillId;
            public string status;
            public int? currentEmpathyPoints;
            public int? empathyLevelTargetPoints;
            public int? currentEmpathyLevel;
            public string rewardsClaimed;
            public string seduceAvailableAt;

            public const string Key_Ulvi = "Ulvi";
            public const string Key_Adriel = "Adriel";
            public const string Key_Ingie = "Ingie";
            public const string Key_Faye = "Faye";
            public const string Key_Lili = "Lili";

            public const string Status_Open = "open";
            public const string Status_Close = "close";

            public const string RewardsClaimed_None = "none";
            public const string RewardsClaimed_TwentFive = "twenty_five";
            public const string RewardsClaimed_Fifty = "fifty";
            public const string RewardsClaimed_All = "all";


            [JsonProperty(Required = Required.Default)]
            public bool isUlvi => name == Key_Ulvi;

            [JsonProperty(Required = Required.Default)]
            public bool isAdriel => name == Key_Adriel;

            [JsonProperty(Required = Required.Default)]
            public bool isIngie => name == Key_Ingie;

            [JsonProperty(Required = Required.Default)]
            public bool isFaye => name == Key_Faye;

            [JsonProperty(Required = Required.Default)]
            public bool isLili => name == Key_Lili;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;

            [JsonProperty(Required = Required.Default)]
            public bool isClose => name == Status_Close;
        }

        [Serializable]
        public class MemoryItem
        {
            public int id;
            public int userId;
            public int? matriarchMemoryId;
            public string status;
            public int? matriarchId;
            public string title;
            public string label;
            public int? sexSceneId;
            public List<OpenShard> shardsToOpen;
            public int? visibleByEmpathyLevel;
            public int? visibleByEventStartId;
            public int? visibleByEventStageCompleteId;

            public class OpenShard
            {
                public int amount;
                public string shardRarity;

                public const string Rariry_White = "white";
                public const string Rariry_Green = "green";
                public const string Rariry_Purple = "purple";
                public const string Rariry_Gold = "gold";
            }

            public const string Status_Visible = "visible";
        }
    }
}
