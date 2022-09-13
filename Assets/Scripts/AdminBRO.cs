using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using TMPro;

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

            [JsonProperty(Required = Required.Default)]
            public AdminBRO.CurrencyItem currencyData =>
                GameData.currencies.GetById(currencyId);

            [JsonProperty(Required = Required.Default)]
            public string icon => currencyData?.iconUrl;

            [JsonProperty(Required = Required.Default)]
            public string tmpSprite => currencyData?.tmpSprite;

            public static PriceItem operator *(int mul, PriceItem a) =>
                new PriceItem { currencyId = a.currencyId, amount = a.amount * mul };
            public static PriceItem operator *(PriceItem a, int mul) => mul * a;
        }

        [Serializable]
        public class RewardItem
        {
            public int? amount;
            public int? tradableId;

            [JsonProperty(Required = Required.Default)]
            public AdminBRO.TradableItem tradableData =>
                GameData.markets.GetTradableById(tradableId);

            [JsonProperty(Required = Required.Default)]
            public string icon => tradableData?.icon;

            [JsonProperty(Required = Required.Default)]
            public string tmpSprite => tradableData?.tmpCurrencySprite;
        }

        [Serializable]
        public class MapPosition
        {
            public float mapCX;
            public float mapCY;

            [JsonProperty(Required = Required.Default)]
            public Vector2 pos => new Vector2(mapCX, -mapCY);
        }

        public class Rarity
        {
            public const string Basic = "basic";
            public const string Advanced = "advanced";
            public const string Epic = "epic";
            public const string Heroic = "heroic";
        }

        // /version
        public static async Task<ApiVersion> versionAsync()
        {
            using (var request = await HttpCore.GetAsync("http://api.overlewd.com/version"))
            {
                return JsonHelper.DeserializeObject<ApiVersion>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class ApiVersion
        {
            public int version;
        }

        //log
        public static async void logAsync(LogData data)
        {
            var url = "http://api.overlewd.com/log";
            var postData = new WWWForm();
            postData.AddField("platform", data.platform);
            postData.AddField("condition", data.condition);
            postData.AddField("stackTrace", data.stackTrace);
            postData.AddField("type", data.type);
            using (var request = await HttpCore.PostAsync(url, postData, tokens?.accessToken, false))
            {

            }
        }

        public class LogData
        {
            public string platform;
            public string condition;
            public string stackTrace;
            public string type;
        }

        // auth/login; auth/refresh
        public static async Task<Tokens> authLoginAsync()
        {
            var postData = new WWWForm();
            postData.AddField("deviceId", GetDeviceId());
            using (var request = await HttpCore.PostAsync("http://api.overlewd.com/auth/login", postData))
            {
                tokens = JsonHelper.DeserializeObject<Tokens>(request?.downloadHandler.text);
                return tokens;
            }
        }

        public static async Task<Tokens> authRefreshAsync()
        {
            var postData = new WWWForm();
            using (var request = await HttpCore.PostAsync("http://api.overlewd.com/auth/refresh", postData))
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
        // /me/special-for-vova/{characterId}
        public static async Task<PlayerInfo> meAsync()
        {
            using (var request = await HttpCore.GetAsync("http://api.overlewd.com/me", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<PlayerInfo>(request?.downloadHandler.text);
            }
        }

        public static async Task<PlayerInfo> meAsync(string name)
        {
            var form = new WWWForm();
            form.AddField("name", name);
            form.AddField("currentVersion", HttpCore.ApiVersion);
            using (var request = await HttpCore.PostAsync("http://api.overlewd.com/me", form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<PlayerInfo>(request?.downloadHandler.text);
            }
        }

        public static async Task addCharacter(int chId, int level)
        {
            var url = $"http://api.overlewd.com/me/special-for-vova/{chId}";
            var form = new WWWForm();
            form.AddField("level", level);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class PlayerInfo
        {
            public int id;
            public string name;
            public string locale;
            public List<WalletItem> wallet;
            public Potion potion;
            public int energyPoints;

            public class Potion
            {
                public int hp;
                public int mana;
                public int energy;
                public int replay;
            }

            public class WalletItem
            {
                public int currencyId;
                public int amount;
            }
        }

        public static async Task initAsync()
        {
            var url = "http://api.overlewd.com/me/init";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }


        public static async Task resetAsync()
        {
            var url = "http://api.overlewd.com/me/reset";
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
            using (var request = await HttpCore.PostAsync("http://api.overlewd.com/me/currency", form, tokens?.accessToken))
            {

            }
        }

        // /markets
        public static async Task<List<EventMarketItem>> eventMarketsAsync()
        {
            var url = "http://api.overlewd.com/markets";
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
            public MapPosition mapPos;
            public string createdAt;
            public string updatedAt;
            public List<int> tradables;
            public List<int> currencies;
            public List<Tab> tabs;

            public class Tab
            {
                public string title;
                public bool isDefault;
                public string icon;
                public string banner;
                public string viewType;
                public List<int> goods;

                public const string ViewTab_GoodsList = "goods_list";
                public const string ViewTab_Bundle = "bundle";
                public const string ViewTab_Pack = "pack";
            }

            [JsonProperty(Required = Required.Default)]
            public List<TradableItem> tradablesData =>
                tradables.Select(id => GameData.markets.GetTradableById(id)).
                Where(data => data != null).OrderByDescending(item => item.promo).ToList();
        }

        // /currencies
        public static async Task<List<CurrencyItem>> currenciesAsync()
        {
            using (var request = await HttpCore.GetAsync("http://api.overlewd.com/currencies", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<CurrencyItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class CurrencyItem
        {
            public int id;
            public string name;
            public string iconUrl;
            public string key;
            public bool nutaku;
            public string createdAt;
            public string updatedAt;

            [JsonProperty(Required = Required.Default)]
            public string tmpSprite =>
                key switch
                {
                    Key_Crystals => TMPSprite.Crystal,
                    Key_Wood => TMPSprite.Wood,
                    Key_Stone => TMPSprite.Stone,
                    Key_Copper => TMPSprite.Copper,
                    Key_Gold => TMPSprite.Gold,
                    Key_Gems => TMPSprite.Gem,
                    Key_Ears => TMPSprite.EventCurrencyEar,
                    Key_Ngold => TMPSprite.EventCurrencyNutakuGold,
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
            var url = "http://api.overlewd.com/tradable";
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
            public string rarity;

            public const string Type_Default = "default";
            public const string Type_Currency = "currency";
            public const string Type_Pack = "pack";
            public const string Type_BattleCharacter = "battle-character";
            public const string Type_BattleCharacterEquipment = "battle-character-equipment";
            public const string Type_MatriarchShard = "matriarch_shard";
            public const string Type_ManaPotion = "mana_potion";
            public const string Type_HpPotion = "hp_potion";

            [JsonProperty(Required = Required.Default)]
            public bool canBuy => GameData.player.CanBuy(price);

            public string GetIconByRarity(string rarity, int? entityId = null) => type switch
            {
                Type_Currency => GameData.currencies.GetById(entityId.HasValue ? entityId : currencyId)?.iconUrl,
                Type_BattleCharacter => GameData.characters.GetById(entityId.HasValue ? entityId : characterId)?.GetIconByRarity(rarity),
                Type_BattleCharacterEquipment => GameData.equipment.GetById(entityId.HasValue ? entityId : equipmentId)?.icon,
                Type_MatriarchShard => GameData.matriarchs.GetShardById(entityId.HasValue ? entityId : matriarchShardId, rarity).icon,
                _ => imageUrl
            };

            [JsonProperty(Required = Required.Default)]
            public string icon => GetIconByRarity(rarity);

            [JsonProperty(Required = Required.Default)]
            public string tmpCurrencySprite => GameData.currencies.GetById(currencyId)?.tmpSprite;
        }

        // /markets/{marketId}/tradable/{tradableId}/buy
        // /tradable/{tradableId}/buy
        public static async Task<TradableBuyStatus> tradableBuyAsync(int marketId, int tradableId)
        {
            var form = new WWWForm();
            var url = $"http://api.overlewd.com/markets/{marketId}/tradable/{tradableId}/buy";
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<TradableBuyStatus>(request?.downloadHandler.text);
            }
        }

        public static async Task<TradableBuyStatus> tradableBuyAsync(int tradableId)
        {
            var form = new WWWForm();
            var url = $"http://api.overlewd.com/tradable/{tradableId}/buy";
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
                RuntimePlatform.Android => "http://api.overlewd.com/resources?platform=android",
                RuntimePlatform.WindowsEditor => "http://api.overlewd.com/resources?platform=windows",
                RuntimePlatform.WindowsPlayer => "http://api.overlewd.com/resources?platform=windows",
                RuntimePlatform.WebGLPlayer => "http://api.overlewd.com/resources?platform=webgl",
                _ => "http://api.overlewd.com/resources"
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
            using (var request = await HttpCore.GetAsync("http://api.overlewd.com/event-chapters", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventChapter>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class EventChapter
        {
            public int id;
            public string name;
            public string mapImgUrl;
            public MapPosition nextChapterMapPos;
            public int eventId;
            public int? nextChapterId;
            public int? durationInDays;
            public List<int> stages;
            public int? order;
            public List<RewardItem> rewards;
            public int? battleEnergyPointsCost;

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
            public AdminBRO.EventItem eventData => GameData.events.GetEventById(eventId);

            [JsonProperty(Required = Required.Default)]
            public bool isActive => GameData.events.activeChapter == this;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => GameData.devMode ? true : (isActive || isComplete);
            
            [JsonProperty(Required = Required.Default)]
            public EventChapter nextChapterData =>
                GameData.events.GetChapterById(nextChapterId);

            public void SetAsMapChapter() =>
                GameData.events.mapChapter = this;
            
            public AdminBRO.EventStageItem GetStageById(int? id) =>
                GameData.events.GetStageById(id);

            [JsonProperty(Required = Required.Default)]
            public List<EventStageItem> stagesData =>
                stages.Select(id => GameData.events.GetStageById(id)).Where(data => data != null).ToList();

        }

        // /events
        public static async Task<List<EventItem>> eventsAsync()
        {
            using (var request = await HttpCore.GetAsync("http://api.overlewd.com/events", tokens?.accessToken))
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
            public int? narratorMatriarchId;

            public const string Type_Quarterly = "quarterly";
            public const string Type_Monthly = "monthly";
            public const string Type_Weekly = "weekly";

            [JsonProperty(Required = Required.Default)]
            public List<EventChapter> chaptersData =>
                chapters.Select(chId => GetChapterById(chId)).
                    OrderBy(ch => ch.order).ToList();

            [JsonProperty(Required = Required.Default)]
            public EventChapter firstChapter =>
                chaptersData.FirstOrDefault();
            
            public EventChapter GetChapterById(int? id) =>
                GameData.events.GetChapterById(id);

            public EventItem SetAsMapEvent() =>
                GameData.events.mapEventData = this;
            
            [JsonProperty(Required = Required.Default)]
            public List<EventMarketItem> marketsData =>
                markets.Select(id => GameData.markets.GetEventMarketById(id)).Where(data => data != null).ToList();

            [JsonProperty(Required = Required.Default)]
            public bool isWeekly => type == Type_Weekly;

            [JsonProperty(Required = Required.Default)]
            public bool isMonthly => type == Type_Monthly;

            [JsonProperty(Required = Required.Default)]
            public bool isQuarterly => type == Type_Quarterly;

            [JsonProperty(Required = Required.Default)]
            public AdminBRO.MatriarchItem narratorMatriarch =>
                GameData.matriarchs.GetMatriarchById(narratorMatriarchId);

            [JsonProperty(Required = Required.Default)]
            public bool timePeriodIsActive => TimeTools.PeriodIsActive(dateStart, dateEnd);

            [JsonProperty(Required = Required.Default)]
            public string timePeriodLeft => TimeTools.AvailableTimeToString(dateEnd);
        }


        // /event-stages
        public static async Task<List<EventStageItem>> eventStagesAsync()
        {
            var url = "http://api.overlewd.com/event-stages";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventStageItem>>(request?.downloadHandler.text);
            }
        }

        // /event-stages/{id}/start
        public static async Task<EventStageItem> eventStageStartAsync(int eventStageId)
        {
            var url = $"http://api.overlewd.com/event-stages/{eventStageId}/start";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<EventStageItem>(request?.downloadHandler.text);
            }
        }

        // /event-stages/{id}/end
        public static async Task<EventStageItem> eventStageEndAsync(int eventStageId, EventStageEndData data = null)
        {
            var url = $"http://api.overlewd.com/event-stages/{eventStageId}/end";
            var form = data?.ToWWWForm() ?? new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<EventStageItem>(request?.downloadHandler.text);
            }
        }

        // /event-stages/{id}/replay
        public static async Task eventStageReplayAsync(int eventStageId, int count)
        {
            var url = $"http://api.overlewd.com/event-stages/{eventStageId}/replay";
            var form = new WWWForm();
            form.AddField("count", count);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

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
                form.AddField("hp", -hp);
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
            public MapPosition mapPos;
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

            [JsonProperty(Required = Required.Default)]
            public EventChapter eventChapterData =>
                GameData.events.GetChapterByStageId(id);
        }

        // /quests
        public static async Task<List<QuestItem>> questsAsync()
        {
            var url = "http://api.overlewd.com/quests";
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
            public int? ftueChapterId;

            public const string Status_Open = "open";
            public const string Status_In_Progress = "in_progress";
            public const string Status_Complete = "complete";
            public const string Status_Rewards_Claimed = "rewards_claimed";

            [JsonProperty(Required = Required.Default)]
            public bool hasDescription => !String.IsNullOrEmpty(description);

            [JsonProperty(Required = Required.Default)]
            public bool isCompleted => status == Status_Complete;

            [JsonProperty(Required = Required.Default)]
            public bool inProgress => status == Status_In_Progress;

            [JsonProperty(Required = Required.Default)]
            public bool isClaimed => status == Status_Rewards_Claimed;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;
        }

        // //quests/{id}/claim-reward
        public static async Task questClaimRewardAsync(int id)
        {
            var url = $"http://api.overlewd.com/quests/{id}/claim-reward";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /i18n
        public static async Task<List<LocalizationItem>> localizationAsync(string locale)
        {
            var url = String.Format("http://api.overlewd.com/i18n?locale={0}", locale);
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
            var url = "http://api.overlewd.com/dialogs";
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

            public int? characterDialogAnimationId;
            public int? emotionAnimationId;
            public int? cutInAnimationId;
            public int? mainAnimationId;

            public int? backgroundMusicId;
            public int? mainSoundId;
            public int? cutInSoundId;
            public int? replicaSoundId;

            public float mainAnimationTimeScale;

            public const string CharacterPosition_Left = "left";
            public const string CharacterPosition_Right = "right";
            public const string CharacterPosition_Middle = "middle";

            public const string CharacterName_Overlord = "Overlord";
            public const string CharacterName_Ulvi = "Ulvi";
            public const string CharacterName_Faye = "Faye";
            public const string CharacterName_Adriel = "Adriel";
            public const string CharacterName_Dragon= "Dragon";
            public const string CharacterName_Inge = "Inge";
            public const string CharacterName_Lili = "Lili";
            public const string CharacterName_Pisha = "Pisha";
            public const string CharacterName_Valkyrie = "Valkyrie";

            public const string CharacterSkin_Overlord = "Overlord";
            public const string CharacterSkin_Ulvi = "Ulvi";
            public const string CharacterSkin_UlviWolf = "UlviWolf";
            public const string CharacterSkin_Adriel = "Adriel";
            public const string CharacterSkin_Inge = "Inge";
            public const string CharacterSkin_Dragon = "Dragon";
            public const string CharacterSkin_Faye = "Faye";
            public const string CharacterSkin_Lili = "Lili";
            public const string CharacterSkin_Pisha = "Pisha";
            public const string CharacterSkin_Valkyrie = "Valkyrie";
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
            var url = "http://api.overlewd.com/battles";
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
        // /battles/my/characters/{id}
        // /battles/my/characters/{id}/levelup
        // /battles/my/characters/{characterId}/skills/{skillId}/levelup
        // /battles/my/characters/{tgtId}/merge/{srcId}
        // /battles/skills/effects
        // /battles/pass
        public static async Task<List<Character>> charactersAsync()
        {
            var url = "http://api.overlewd.com/battles/my/characters";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Character>>(request?.downloadHandler.text);
            }
        }

        public static async Task characterToSlotAsync(int characterId, string slotId)
        {
            var url = $"http://api.overlewd.com/battles/my/characters/{characterId}";
            var form = new WWWForm();
            form.AddField("teamPosition", slotId);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task characterLvlupAsync(int characterId)
        {
            var url = $"http://api.overlewd.com/battles/my/characters/{characterId}/levelup";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task chracterSkillLvlUp(int characterId, int skillId)
        {
            var url = $"http://api.overlewd.com/battles/my/characters/{characterId}/skills/{skillId}/levelup";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task charactersMrgAsync(int srcCharacterId, int trgtCharacterId)
        {
            var url = $"http://api.overlewd.com/battles/my/characters/{trgtCharacterId}/merge/{srcCharacterId}";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task<List<SkillEffect>> skillEffectsAsync()
        {
            var url = "http://api.overlewd.com/battles/skills/effects";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<SkillEffect>>(request?.downloadHandler.text);
            }
        }

        public static async Task<List<BattlePass>> battlePassesAsync()
        {
            var url = "http://api.overlewd.com/battles/pass";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<BattlePass>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Character
        {
            public int? id;
            public string teamPosition;
            public string basicIcon;
            public string advancedIcon;
            public string epicIcon;
            public string heroicIcon;
            public string teamEditSlotPersIcon;
            public string fullScreenPersIcon;
            public string name;
            public string characterClass;
            public int? animationId;
            public int? level;
            public string rarity;
            public List<int> equipment;
            public List<CharacterSkill> skills;
            public float speed;
            public float power;
            public float constitution;
            public float agility;
            public float accuracy;
            public float dodge;
            public float critrate;
            public float health;
            public float damage;
            public int mana;
            public float? potency;
            public int? sexSceneId;
            public string sexSceneVisibleByRarity;
            public string sexSceneClosedBanner;
            public string sexSceneOpenedBanner;
            public string key;
            public List<PriceItem> levelUpPrice;
            public List<PriceItem> mergePrice;

            public const string TeamPosition_Slot1 = "slot1";
            public const string TeamPosition_Slot2 = "slot2";
            public const string TeamPosition_None = "none";

            public const string Class_Assassin = "Assassin";
            public const string Class_Bruiser = "Bruiser";
            public const string Class_Caster = "Caster";
            public const string Class_Healer = "Healer";
            public const string Class_Overlord = "Overlord";
            public const string Class_Tank = "Tank";

            public string GetIconByRarity(string rarity) => rarity switch
            {
                Rarity.Basic => basicIcon,
                Rarity.Advanced => advancedIcon,
                Rarity.Epic => epicIcon,
                Rarity.Heroic => heroicIcon,
                _ => null
            };

            [JsonProperty(Required = Required.Default)]
            public string iconUrl => GetIconByRarity(rarity);

            [JsonProperty(Required = Required.Default)]
            public string classMarker => characterClass switch
            {
                Class_Assassin => TMPSprite.ClassAssassin,
                Class_Bruiser => TMPSprite.ClassBruiser,
                Class_Caster => TMPSprite.ClassCaster,
                Class_Healer => TMPSprite.ClassHealer,
                Class_Overlord => TMPSprite.ClassOverlord,
                Class_Tank => TMPSprite.ClassTank,
                _ => ""
            };

            [JsonProperty(Required = Required.Default)]
            public bool isBasic => rarity == Rarity.Basic;

            [JsonProperty(Required = Required.Default)]
            public bool isAdvanced => rarity == Rarity.Advanced;

            [JsonProperty(Required = Required.Default)]
            public bool isEpic => rarity == Rarity.Epic;

            [JsonProperty(Required = Required.Default)]
            public bool isHeroic => rarity == Rarity.Heroic;

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
                Rarity.Basic => 10,
                Rarity.Advanced => 20,
                Rarity.Epic => 30,
                Rarity.Heroic => 40,
                _ => 10
            };

            [JsonProperty(Required = Required.Default)]
            public bool hasEquipment => equipment.Count > 0;

            [JsonProperty(Required = Required.Default)]
            public Equipment characterEquipmentData => GameData.equipment.GetById(equipment.FirstOrDefault());

            [JsonProperty(Required = Required.Default)]
            public Animation animationData => GameData.animations.GetById(animationId);
        }

        [Serializable]
        public class CharacterSkill
        {
            public int id;
            public string name;
            public string description;
            public string icon;
            public string effect;
            public string type;
            public float effectProb;
            public float effectActingDuration;
            public float effectCooldownDuration;
            public List<PriceItem> levelUpPrice;
            public int level;
            public int manaCost;
            public string actionType;
            public bool AOE;
            public float amount;
            public string trigger;
            public float effectAmount;

            public const string Type_Passive = "passive_skill";
            public const string Type_Attack = "attack";
            public const string Type_Enhanced = "enhanced_attack";
        }

        [Serializable]
        public class SkillEffect
        {
            public string name;
            public string description;
        }

        [Serializable]
        public class BattlePass
        {
            public int id;
            public int? eventId;
            public List<Level> levels;
            public List<PriceItem> premiumPrice;
            public int currentPointsCount;

            public class Level
            {
                public int pointsThreshold;
                public List<RewardItem> defaultReward;
                public List<RewardItem> premiumReward;
            }
        }

        // /my/characters/equipment
        // /battles/my/characters/{id}/equip/{id} - post
        // /battles/my/characters/{id}/equip/{id} - delete
        public static async Task<List<Equipment>> equipmentAsync()
        {
            var url = "http://api.overlewd.com/battles/my/characters/equipment";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Equipment>>(request?.downloadHandler.text);
            }
        }

        public static async Task equipAsync(int characterId, int equipmentId)
        {
            var url = $"http://api.overlewd.com/battles/my/characters/{characterId}/equip/{equipmentId}";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task unequipAsync(int characterId, int equipmentId)
        {
            var url = $"http://api.overlewd.com/battles/my/characters/{characterId}/equip/{equipmentId}";
            using (var request = await HttpCore.DeleteAsync(url, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class Equipment
        {
            public int id;
            public int? equipmentId;
            public int? characterId;
            public string characterClass;
            public string name;
            public string icon;
            public string equipmentType;
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
            public float potency;

            [JsonProperty(Required = Required.Default)]
            public bool isEquipped => characterId.HasValue;

            public bool IsMy(int? myId) => isEquipped && myId == characterId;

            public const string Class_Assassin = "Assassin";
            public const string Class_Bruiser = "Bruiser";
            public const string Class_Tank = "Tank";
            public const string Class_Caster = "Caster";
            public const string Class_Healer = "Healer";
            public const string Class_Overlord = "Overlord";

            public const string Type_CharacterWeapon = "battle_character_weapon";
            public const string Type_OverlordThighs = "overlord_thighs";
            public const string Type_OverlordHelmet = "overlord_helmet";
            public const string Type_OverlordBoots = "overlord_boots";
            public const string Type_OverlordHarness = "overlord_harness";
            public const string Type_OverlordWeapon = "overlord_weapon";
            public const string Type_OverlordGloves = "overlord_gloves";
        }

        //ftue
        public static async Task<FTUEInfo> ftueAsync()
        {
            var url = "http://api.overlewd.com/ftue";
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

            [JsonProperty(Required = Required.Default)]
            public bool isShown { get; set; } = false;
        }

        [Serializable]
        public class FTUEChapter
        {
            public int id;
            public string key;
            public string name;
            public string mapImgUrl;
            public MapPosition monthlyEventMapPos;
            public MapPosition quarterlyEventMapPos;
            public MapPosition nextChapterMapPos;
            public List<FTUENotificationItem> notifications;
            public List<int> stages;
            public int? nextChapterId;
            public int? order;
            public int? battleEnergyPointsCost;

            public FTUENotificationItem GetNotifByKey(string key)
            {
                return notifications.Find(n => n.key == key);
            }

            public void ShowNotifByKey(string key, bool checkShowRestriction = true)
            {
                var notifData = GetNotifByKey(key);
                if (checkShowRestriction && (notifData?.isShown ?? false))
                {
                    return;
                }

                UIManager.MakeNotification<DialogNotification>().
                    SetData(new DialogNotificationInData
                    {
                        dialogId = GetNotifByKey(key)?.dialogId
                    }).RunShowNotificationProcess();

                if (notifData != null)
                {
                    notifData.isShown = true;
                }
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
            public bool isActive => GameData.ftue.activeChapter == this;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => GameData.devMode ? true : (isComplete || isActive);

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
            var url = "http://api.overlewd.com/ftue/stats";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<FTUEStats>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class FTUEStats
        {
            public int? lastStartedStage;
            public int? lastUpdatedStage;
            public int? lastEndedStage;

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastStartedStageData =>
                GameData.ftue.info.GetStageById(lastStartedStage);

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastUpdatedStageData =>
                GameData.ftue.info.GetStageById(lastUpdatedStage);

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
            var url = "http://api.overlewd.com/ftue-stages";
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
            public MapPosition mapPos;
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
            var url = $"http://api.overlewd.com/ftue-stages/{stageId}/start";
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /ftue-stages/{id}/end
        public static async Task ftueStageEndAsync(int stageId, FTUEStageEndData data = null)
        {
            var url = $"http://api.overlewd.com/ftue-stages/{stageId}/end";
            var form = data?.ToWWWForm() ?? new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /ftue-stages/{id}/replay
        public static async Task ftueStageReplayAsync(int stageId, int count)
        {
            var url = $"http://api.overlewd.com/ftue-stages/{stageId}/replay";
            var form = new WWWForm();
            form.AddField("count", count);
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
            var url = "http://api.overlewd.com/animations";
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
            var url = "http://api.overlewd.com/sounds";
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
            var url = "http://api.overlewd.com/buildings";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Building>>(request?.downloadHandler.text);
            }
        }

        public static async Task buildingBuildAsync(int id)
        {
            var url = $"http://api.overlewd.com/buildings/{id}/build";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task buildingBuildCrystalsAsync(int id)
        {
            var url = $"http://api.overlewd.com/buildings/{id}/build-crystals";
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
            var url = "http://api.overlewd.com/municipality/time-left";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<MunicipalityTimeLeft>(request?.downloadHandler.text);
            }
        }

        public static async Task municipalityCollectAsync()
        {
            var url = $"http://api.overlewd.com/municipality/collect";
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
            var url = $"http://api.overlewd.com/magicguild/skills";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MagicGuildSkill>>(request?.downloadHandler.text);
            }
        }

        public static async Task magicGuildSkillLvlUpAsync(string skillType)
        {
            var url = $"http://api.overlewd.com/magicguild/{skillType}/levelup";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task magicGuildSkillLvlUpCrystalAsync(string skillType)
        {
            var url = $"http://api.overlewd.com/magicguild/{skillType}/levelup-crystals";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class MagicGuildSkill
        {
            public string type;
            public SkillData current;
            public SkillData next;
            public int currentSkillLevel;
            public int requiredBuildingLevel;
            public int maxSkillLevel;
            public int skillId;
            public List<PriceItem> price;
            public List<PriceItem> priceCrystal;

            [JsonProperty(Required = Required.Default)]
            public bool isLvlMax => currentSkillLevel == maxSkillLevel;

            [JsonProperty(Required = Required.Default)]
            public bool canlvlUp => GameData.player.CanBuy(current.levelUpPrice);

            [JsonProperty(Required = Required.Default)]
            public bool canCrystallvlUp => GameData.player.CanBuy(priceCrystal);

            [JsonProperty(Required = Required.Default)]
            public bool canUpgrade => GameData.buildings.magicGuild.currentLevel >= requiredBuildingLevel && next != null;

            public const string Type_ActiveSkill = "overlord_enhanced_attack";
            public const string Type_UltimateSkill = "overlord_ultimate_attack";
            public const string Type_PassiveSkill1 = "overlord_first_passive_skill";
            public const string Type_PassiveSkill2 = "overlord_second_passive_skill";

            public class SkillData
            {
                public int id;
                public string name;
                public string description;
                public string actionType;
                public bool AOE;
                public float amount;
                public string effect;
                public string type;
                public string trigger;
                public float effectAmount;
                public float effectProb;
                public string icon;
                public float effectActingDuration;
                public float effectCooldownDuration;
                public float manaCost;
                public List<PriceItem> levelUpPrice;
                public int level;
            }
        }

        // /forge/price
        // /forge/merge
        public static async Task<ForgePrice> forgePrices()
        {
            var url = "http://api.overlewd.com/forge/price";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<ForgePrice>(request?.downloadHandler.text);
            }
        }

        public static async Task forgeMergeEquipment(string mergeType, int[] mergeIds)
        {
            var url = "http://api.overlewd.com/forge/merge/equipment";
            var form = new WWWForm();
            form.AddField("mergeType", mergeType);
            foreach (var id in mergeIds)
            {
                form.AddField("ids[]", id);
            }
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task forgeMergeShard(int matriarchId, string rarity, int amount)
        {
            var url = "http://api.overlewd.com/forge/merge/shard";
            var form = new WWWForm();
            form.AddField("matriarchId", matriarchId);
            form.AddField("rarity", rarity);
            form.AddField("amount", amount);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task forgeExchangeShard(int matriarchSourceId, int matriarchTargetId, string rarity, int amount)
        {
            var url = "http://api.overlewd.com/forge/exchange/shard";
            var form = new WWWForm();
            form.AddField("matriarchSourceId", matriarchSourceId);
            form.AddField("matriarchTargetId", matriarchTargetId);
            form.AddField("rarity", rarity);
            form.AddField("amount", amount);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        [Serializable]
        public class ForgePrice
        {
            public List<MergeEquipmentSettings> mergeEquipmentSettings;
            public MergeShardSettings mergeShardSettings;
            public ExchangeShardSettings exchangeShardSettings;

            public class MergeEquipmentSettings
            {
                public string mergeType;
                public int mergeCount;
                public List<MergePrice> pricesOfMergeType;
            }

            public class MergeShardSettings
            {
                public int mergeAmount;
                public int maxPossibleResultAmount;
                public List<MergePrice> pricesOfMergeType;
            }

            public class ExchangeShardSettings
            {
                public int exchangeAmount;
                public int maxPossibleResultAmount;
                public List<MergePrice> pricesOfExchangeType;
            }

            public class MergePrice
            {
                public string rarity;
                public List<PriceItem> price;
            }
        }

        // gacha
        public static async Task<List<GachaItem>> gachaAsync()
        {
            var url = "http://api.overlewd.com/gacha";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<GachaItem>>(request?.downloadHandler.text);
            }
        }

        public static async Task<List<GachaBuyResult>> gachaBuyAsync(int id)
        {
            var url = $"http://api.overlewd.com/gacha/{id}/buy";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<GachaBuyResult>>(request?.downloadHandler.text);
            }
        }

        public static async Task<List<GachaBuyResult>> gachaBuyManyAsync(int id)
        {
            var url = $"http://api.overlewd.com/gacha/{id}/buy-many";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<GachaBuyResult>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class GachaBuyResult
        {
            public int? tradableId;
            public int? entityUserProgressId;
            public string rarity;

            [JsonProperty(Required = Required.Default)]
            public TradableItem tradableData =>
                GameData.markets.GetTradableById(tradableId);

            [JsonProperty(Required = Required.Default)]
            public string icon =>
                tradableData?.GetIconByRarity(rarity, entityUserProgressId);
        }

        [Serializable]
        public class GachaItem
        {
            public int id;
            public string tabTitle;
            public string tabImageOn;
            public string tabImageOff;
            public string backgroundImage;
            public string backgroundImageText;
            public string tabType;
            public List<PriceItem> priceForOne;
            public List<PriceItem> priceForMany;
            public int? discount;
            public string type;
            public List<TierItem> tiers;
            public int? eventId;
            public int count;
            public int currentCount;
            public int manyAmount;
            public bool available;
            public string dateStart;
            public string dateEnd;

            [JsonProperty(Required = Required.Default)]
            public bool isTempOffer => eventId.HasValue || TimeTools.PeriodHasEnd(dateStart, dateEnd);

            [JsonProperty(Required = Required.Default)]
            public bool timePeriodIsActive => eventData?.timePeriodIsActive ?? TimeTools.PeriodIsActive(dateStart, dateEnd);

            [JsonProperty(Required = Required.Default)]
            public string timePeriodLeft => eventData?.timePeriodLeft ?? TimeTools.AvailableTimeToString(dateEnd);

            [JsonProperty(Required = Required.Default)]
            public EventItem eventData => GameData.events.GetEventById(eventId);

            public class TierItem
            {
                public string title;
                public string rarity;
                public int? targetTradableId;
                public int probability;
            }

            public const string TabType_Characters = "battle_characters";
            public const string TabType_CharactersEquipment = "battle_characters_equipment";
            public const string TabType_OverlordEquipment = "overlord_equipment";
            public const string TabType_MatriachsShards = "matriarch_shards";

            public const string Type_TargetByCount = "target_by_count";
            public const string Type_TargetByTier = "target_by_tier";
        }

        // /matriarchs
        // /matriarchs/memories
        // /matriarchs/memories/{id}/buy
        // /matriarchs/{id}/seduce
        // /matriarchs/shards

        public static async Task<List<MatriarchItem>> matriarchsAsync()
        {
            var url = $"http://api.overlewd.com/matriarchs";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MatriarchItem>>(request?.downloadHandler.text);
            }
        }

        public static async Task<List<MemoryItem>> memoriesAsync()
        {
            var url = $"http://api.overlewd.com/matriarchs/memories";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MemoryItem>>(request?.downloadHandler.text);
            }
        }

        public static async Task memoryBuyAsync(int id)
        {
            var url = $"http://api.overlewd.com/matriarchs/memories/{id}/buy";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {

            }
        }

        public static async Task seduceMatriarchAsync(int id)
        {
            var url = $"http://api.overlewd.com/matriarchs/{id}/seduce";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task<List<MemoryShardItem>> memoryShardsAsync()
        {
            var url = $"https://overlewd-api.herokuapp.com/matriarchs/shards";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<MemoryShardItem>>(request?.downloadHandler.text);
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
            public int? nextEmpathyLevel;
            public string rewardsClaimed;
            public string seduceAvailableAt;

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem basicShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Basic);

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem advancedShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Advanced);

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem epicShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Epic);

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem heroicShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Heroic);

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
            public string key => name;

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
            public bool isClose => status == Status_Close;
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
                public int shardId;
                public int amount;
                public string shardRarity;
            }

            [JsonProperty(Required = Required.Default)]
            public OpenShard basicShard =>
                shardsToOpen?.Find(r => r.shardRarity == Rarity.Basic);

            [JsonProperty(Required = Required.Default)]
            public OpenShard advancedShard =>
                shardsToOpen.Find(r => r.shardRarity == Rarity.Advanced);

            [JsonProperty(Required = Required.Default)]
            public OpenShard epicShard =>
                shardsToOpen?.Find(r => r.shardRarity == Rarity.Epic);

            [JsonProperty(Required = Required.Default)]
            public OpenShard heroicShard =>
                shardsToOpen?.Find(r => r.shardRarity == Rarity.Heroic);


            public const string Status_Visible = "visible";
            public const string Status_Open = "open";

            [JsonProperty(Required = Required.Default)]
            public bool isVisible => status == Status_Visible;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;

        }

        [Serializable]
        public class MemoryShardItem
        {
            public int id;
            public int matriarchId;
            public string rarity;
            public string icon;
            public int amount;
        }

        // /potions
        // /potions/{type}/buy
        public static async Task<PotionsInfo> potionsAsync()
        {
            var url = $"http://api.overlewd.com/potions";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<PotionsInfo>(request?.downloadHandler.text);
            }
        }

        public static async Task potionBuyAsync(string type, int count)
        {
            var url = $"http://api.overlewd.com/potions/{type}/buy";
            var form = new WWWForm();
            form.AddField("count", count);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public static async Task potionEnergyUseAsync(int count)
        {
            var url = $"http://api.overlewd.com/potions/energy/use";
            var form = new WWWForm();
            form.AddField("count", count);
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }


        [Serializable]
        public class PotionsInfo
        {
            public List<PotionPrice> prices;
            public int maxEnergyVolume;
            public int energyPerCan;
            public float energyRecoverySpeedPerMinute;

            public class PotionPrice
            {
                public string type;
                public List<PriceItem> price;
            }

            public const string Type_hp = "hp";
            public const string Type_mana = "mana";
            public const string Type_energy = "energy";
            public const string Type_replay = "replay";
        }
        
    }
}