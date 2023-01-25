using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using System.Text;

namespace Overlewd
{
    public static partial class AdminBRO
    {
        private static string make_url(string url_part) => $"{BuildParameters.ServerDomainURL}{url_part}";

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
            public CurrencyItem currencyData =>
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
            public float? probability;

            [JsonProperty(Required = Required.Default)]
            public TradableItem tradableData =>
                GameData.markets.GetTradableById(tradableId);

            [JsonProperty(Required = Required.Default)]
            public string icon => tradableData?.icon;

            [JsonProperty(Required = Required.Default)]
            public string tmpSprite => tradableData?.tmpCurrencySprite;

            [JsonProperty(Required = Required.Default)]
            public bool isCurrency => tradableData?.isCurrency ?? false;
        }

        [Serializable]
        public class GenRewardItem
        {
            public int? amount;
            public int? tradableId;
            public int? entityUserProgressId;
            public string rarity;
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

            public static int SortValue(string rarity) => rarity switch
            {
                Basic => 1,
                Advanced => 2,
                Epic => 3,
                Heroic => 4,
                _ => 0
            };
            public static int DescSortValue(string rarity) => rarity switch
            {
                Basic => 4,
                Advanced => 3,
                Epic => 2,
                Heroic => 1,
                _ => 0
            };
            public static int MaxLvl(string rarity) => rarity switch
            {
                Basic => 10,
                Advanced => 20,
                Epic => 30,
                Heroic => 40,
                _ => 0
            };
        }

        // /version
        public static async Task<HttpCoreResponse<ApiVersionResponse>> versionAsync() =>
            await HttpCore.GetAsync<ApiVersionResponse>(make_url("version"));

        [Serializable]
        public class ApiVersionResponse
        {
            public int version;
            public List<string> availableVersions;

            public bool VersionAvailable(string ver) =>
                availableVersions.Exists(v => v == ver);
        }

        //log
        public static async void logAsync(LogData data)
        {
            var url = make_url("log");
            var postData = new WWWForm();
            postData.AddField("platform", data.platform);
            postData.AddField("condition", data.condition);
            postData.AddField("stackTrace", data.stackTrace);
            postData.AddField("type", data.type);
            await HttpCore.PostAsync(url, postData, false);
        }

        public class LogData
        {
            public string platform;
            public string condition;
            public string stackTrace;
            public string type;
        }

        // auth/login; auth/refresh
        public static async Task<HttpCoreResponse> authLoginAsync()
        {
            var postData = new WWWForm();
            postData.AddField("deviceId", GetDeviceId());
            if (NutakuApiHelper.loggedIn)
            {
                postData.AddField("nutakuId", NutakuApiHelper.loginInfo.userId);
            }
            var result = await HttpCore.PostAsync<Tokens>(make_url("auth/login"), postData);
            tokens = result.dData;
            return result;
        }

        public static async Task<HttpCoreResponse<Tokens>> authRefreshAsync() =>
            await HttpCore.PostAsync<Tokens>(make_url("auth/refresh"));

        [Serializable]
        public class Tokens
        {
            public string accessToken;
            public string refreshToken;
            public string userId;
        }

        public static Tokens tokens { get; private set; }

        // GET /me; POST /me
        // /me/init
        // /me/reset
        // /me/currency
        public static async Task<HttpCoreResponse<PlayerInfo>> meAsync() =>
            await HttpCore.GetAsync<PlayerInfo>(make_url("me"));

        public static async Task<HttpCoreResponse<PlayerInfo>> mePostAsync()
        {
            var form = new WWWForm();
            form.AddField("currentVersion", BuildParameters.ApiVersion);
            if (NutakuApiHelper.loggedIn)
            {
                form.AddField("name", NutakuApiHelper.userInfo.nickname);
                form.AddField("nutaku", JsonHelper.SerializeObject(NutakuApiHelper.userInfo), Encoding.UTF8);
            }
            else
            {
                form.AddField("name", SystemInfo.deviceModel);
            }
            return await HttpCore.PostAsync<PlayerInfo>(make_url("me"), form);
        }

        [Serializable]
        public class PlayerInfo
        {
            public int id;
            public string name;
            public string locale;
            public List<WalletItem> wallet;
            public List<WalletItem> walletEvent;
            public Potion potion;
            public int energyPoints;
            public List<Device> devices;

            public class Potion
            {
                public int hp;
                public int mana;
                public int energy;
                public int replay;
            }

            public class WalletItem
            {
                public int? currencyId;
                public int amount;

                [JsonProperty(Required = Required.Default)]
                public CurrencyItem currencyData => GameData.currencies.GetById(currencyId);
            }

            public class Device
            {
                public string id;
                public int userId;
            }

            [JsonProperty(Required = Required.Default)]
            public WalletItem Crystal => wallet.Find(item => item.currencyId == GameData.currencies.Crystals.id);

            [JsonProperty(Required = Required.Default)]
            public WalletItem Wood => wallet.Find(item => item.currencyId == GameData.currencies.Wood.id);

            [JsonProperty(Required = Required.Default)]
            public WalletItem Stone => wallet.Find(item => item.currencyId == GameData.currencies.Stone.id);

            [JsonProperty(Required = Required.Default)]
            public WalletItem Copper => wallet.Find(item => item.currencyId == GameData.currencies.Copper.id);

            [JsonProperty(Required = Required.Default)]
            public WalletItem Gold => wallet.Find(item => item.currencyId == GameData.currencies.Gold.id);

            [JsonProperty(Required = Required.Default)]
            public WalletItem Gems => wallet.Find(item => item.currencyId == GameData.currencies.Gems.id);

            [JsonProperty(Required = Required.Default)]
            public int hpPotionAmount => potion.hp;

            [JsonProperty(Required = Required.Default)]
            public int manaPotionAmount => potion.mana;

            [JsonProperty(Required = Required.Default)]
            public int energyPotionAmount => potion.energy;

            [JsonProperty(Required = Required.Default)]
            public int replayAmount => potion.replay;

            [JsonProperty(Required = Required.Default)]
            public int energyPointsAmount => energyPoints;

            [JsonProperty(Required = Required.Default)]
            public List<WalletItem> fullWallet =>
                new[] { wallet, walletEvent }.SelectMany(w => w).ToList();
            public WalletItem GetWalletItemById(int? currencyId) =>
                fullWallet.Find(w => w.currencyId == currencyId);
        }

        public static async Task<HttpCoreResponse> resetAsync() =>
            await HttpCore.PostAsync(make_url("me/reset"));

        public static async Task<HttpCoreResponse> meCurrencyAsync(int currencyId, int amount)
        {
            var form = new WWWForm();
            form.AddField("currencyId", currencyId);
            form.AddField("amount", amount);
            return await HttpCore.PostAsync(make_url("me/currency"), form);
        }

        // /markets
        public static async Task<HttpCoreResponse<List<MarketItem>>> marketsAsync() =>
            await HttpCore.GetAsync<List<MarketItem>>(make_url("markets"));

        [Serializable]
        public class MarketItem
        {
            public int id;
            public string name;
            public string description;
            public string bannerImage;
            public List<int> tradables;
            public List<Tab> tabs;
            public string type;

            public const string Type_Main = "main";
            public const string Type_Event = "event";

            public class Tab
            {
                public int tabId;
                public string title;
                public bool isVisible;
                public string promoGirlIcon;
                public string viewType;
                public List<int> goods;
                public int order;
                public string profit;
                public bool isCrystalsOffer;
                public string bannerArt;
                public string bannerDescription;

                public const string ViewType_GoodsList = "goods_list";
                public const string ViewType_Bundle = "bundle";
                public const string ViewType_Pack = "pack";

                [JsonProperty(Required = Required.Default)]
                public List<TradableItem> tradablesData =>
                    goods.Select(goodId => GameData.markets.GetTradableById(goodId)).
                    Where(trData => trData != null).ToList();
            }

            public Tab GetTabById(int? tabId) =>
                tabs.Find(t => t.tabId == tabId);

            [JsonProperty(Required = Required.Default)]
            public List<TradableItem> tradablesData =>
                tradables.Select(id => GameData.markets.GetTradableById(id)).
                Where(data => data != null).OrderByDescending(item => item.promo).ToList();

            [JsonProperty(Required = Required.Default)]
            public bool isMain => type == Type_Main;

            [JsonProperty(Required = Required.Default)]
            public bool isEvent => type == Type_Event;
        }

        // /currencies
        public static async Task<HttpCoreResponse<List<CurrencyItem>>> currenciesAsync() =>
            await HttpCore.GetAsync<List<CurrencyItem>>(make_url("currencies"));

        [Serializable]
        public class CurrencyItem
        {
            public int id;
            public string name;
            public string iconUrl;
            public string key;
            public string createdAt;
            public string updatedAt;
            public string currencyType;

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

            public const string CurrencyType_Main = "main";
            public const string CurrencyType_Event = "event";
            public const string CurrencyType_Fiat = "fiat";
            public const string CurrencyType_Nutaku = "nutaku";

            [JsonProperty(Required = Required.Default)]
            public bool isTypeNutaku => currencyType == CurrencyType_Nutaku;

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

            [JsonProperty(Required = Required.Default)]
            public PlayerInfo.WalletItem walletInfo =>
                GameData.player.info.GetWalletItemById(id);
        }

        // /tradable
        public static async Task<HttpCoreResponse<List<TradableItem>>> tradablesAsync() =>
            await HttpCore.GetAsync<List<TradableItem>>(make_url("tradable"));

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
            public List<PriceItem> priceWithoutDiscount;
            public int? discount;
            public string specialOfferLabel;
            public List<TradablePack> itemPack;
            public int? currencyId;
            public int? currencyAmount;
            public int? limit;
            public int? baseCharacterId;
            public int? baseEquipmentId;
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
            public int? ingredientId;

            public const string Type_Default = "default";
            public const string Type_Currency = "currency";
            public const string Type_Pack = "pack";
            public const string Type_BattleCharacter = "battle-character";
            public const string Type_BattleCharacterBoss = "battle-character-boss";
            public const string Type_BattleCharacterEquipment = "battle-character-equipment";
            public const string Type_MatriarchShard = "matriarch_shard";
            public const string Type_ManaPotion = "mana_potion";
            public const string Type_HpPotion = "hp_potion";

            public class TradablePack
            {
                public int tradableId;
                public int count;

                [JsonProperty(Required = Required.Default)]
                public TradableItem tradableData =>
                    GameData.markets.GetTradableById(tradableId);
            }

            [JsonProperty(Required = Required.Default)]
            public bool canBuy => GameData.player.CanBuy(price);

            [JsonProperty(Required = Required.Default)]
            public bool nutakuPriceValid =>
                !price.Exists(p => !p.currencyData?.isTypeNutaku ?? true);

            public string GetIconByRarity(string rarity, int? entityId = null) => type switch
            {
                Type_Currency => GameData.currencies.GetById(entityId.HasValue ? entityId : currencyId)?.iconUrl,
                Type_BattleCharacter => entityId.HasValue ?
                    GameData.characters.GetById(entityId)?.GetIconByRarity(rarity) :
                    GameData.characters.GetBaseById(baseCharacterId)?.GetIconByRarity(rarity),
                Type_BattleCharacterEquipment => entityId.HasValue ?
                    GameData.equipment.GetById(entityId)?.GetIconByRarity(rarity) :
                    GameData.equipment.GetBaseById(baseEquipmentId)?.GetIconByRarity(rarity),
                Type_MatriarchShard => GameData.matriarchs.GetShardById(entityId.HasValue ? entityId : matriarchShardId, rarity).icon,
                _ => imageUrl
            };

            [JsonProperty(Required = Required.Default)]
            public string icon => GetIconByRarity(rarity);

            [JsonProperty(Required = Required.Default)]
            public CurrencyItem currencyData =>
                GameData.currencies.GetById(currencyId);

            [JsonProperty(Required = Required.Default)]
            public string tmpCurrencySprite => currencyData?.tmpSprite;

            [JsonProperty(Required = Required.Default)]
            public bool isCurrency => currencyId.HasValue;
        }

        // /markets/{marketId}/tradable/{tradableId}/buy
        // /tradable/{tradableId}/buy
        public static async Task<HttpCoreResponse<TradableBuyStatus>> tradableBuyAsync(int marketId, int tradableId) =>
            await HttpCore.PostAsync<TradableBuyStatus>(make_url($"markets/{marketId}/tradable/{tradableId}/buy"));

        [Serializable]
        public class TradableBuyStatus
        {
            public bool status;
        }

        // /resources
        public static async Task<HttpCoreResponse<List<NetworkResource>>> resourcesAsync()
        {
            var url = Application.platform switch
            {
                RuntimePlatform.Android => make_url("resources?platform=android"),
                RuntimePlatform.WindowsEditor => make_url("resources?platform=windows"),
                RuntimePlatform.WindowsPlayer => make_url("resources?platform=windows"),
                RuntimePlatform.WebGLPlayer => make_url("resources?platform=webgl"),
                _ => make_url("resources")
            };
            return await HttpCore.GetAsync<List<NetworkResource>>(url);
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

            public const string Type_Texture = "texture";
            public const string Type_AssetBundle = "assetbundle";
            public const string Type_Sound = "sound";
        }

        [Serializable]
        public class NetworkResourceShort
        {
            public string id;
            public string hash;
        }

        // /event-chapters
        public static async Task<HttpCoreResponse<List<EventChapter>>> eventChaptersAsync() =>
            await HttpCore.GetAsync<List<EventChapter>>(make_url("event-chapters"));

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
            public List<Market> markets;

            public class EventChapterReward
            {
                public string icon;
                public int amount;
                public int currency;
            }
            public class Market
            {
                public int? marketId;
                public MapPosition mapPos;

                [JsonProperty(Required = Required.Default)]
                public MarketItem marketData =>
                GameData.markets.GetMarketById(marketId);
            }

            [JsonProperty(Required = Required.Default)]
            public bool isComplete
            {
                get
                {
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
            public EventItem eventData => GameData.events.GetEventById(eventId);

            [JsonProperty(Required = Required.Default)]
            public bool isActive => eventData?.activeChapter?.id == id;

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
        public static async Task<HttpCoreResponse<List<EventItem>>> eventsAsync() =>
            await HttpCore.GetAsync<List<EventItem>>(make_url("events"));

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
            public string marketBannerImage;
            public string overlayBannerImage;
            public string eventListBannerImage;
            public string bannerOverlayText;
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

            [JsonProperty(Required = Required.Default)]
            public EventChapter activeChapter
            {
                get
                {
                    var chapterData = firstChapter;
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

            public EventChapter GetChapterById(int? id) =>
                GameData.events.GetChapterById(id);

            public EventItem SetAsMapEvent() =>
                GameData.events.mapEventData = this;

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

            [JsonProperty(Required = Required.Default)]
            public List<CurrencyItem> currenciesData =>
               currencies.Select(currencyId => GameData.currencies.GetById(currencyId)).
                Where(cd => cd != null).ToList();
        }


        // /event-stages
        // /event-stages/{id}/start
        // /event-stages/{id}/end
        // /event-stages/{id}/replay
        public static async Task<HttpCoreResponse<List<EventStageItem>>> eventStagesAsync() =>
            await HttpCore.GetAsync<List<EventStageItem>>(make_url("event-stages"));

        public static async Task<HttpCoreResponse<EventStageItem>> eventStageStartAsync(int eventStageId) =>
            await HttpCore.PostAsync<EventStageItem>(make_url($"event-stages/{eventStageId}/start"));

        public static async Task<HttpCoreResponse<EventStageItem>> eventStageEndAsync(int eventStageId, BattleEndData battleEndData = null) =>
            await HttpCore.PostAsync<EventStageItem>(make_url($"event-stages/{eventStageId}/end"), battleEndData?.ToWWWForm());

        public static async Task<HttpCoreResponse> eventStageReplayAsync(int eventStageId, int count)
        {
            var form = new WWWForm();
            form.AddField("count", count);
            return await HttpCore.PostAsync(make_url($"event-stages/{eventStageId}/replay"), form);
        }

        [Serializable]
        public class EventStageItem
        {
            public int index;
            public int id;
            public string title;
            public int? dialogId;
            public int? battleId;
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
        public static async Task<HttpCoreResponse<List<QuestItem>>> questsAsync() =>
            await HttpCore.GetAsync<List<QuestItem>>(make_url("quests"));

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
            public string type;
            public int? eventId;
            public int? ftueChapterId;
            public string ftueQuestType;
            public string screenTarget;
            public int? matriarchEmpathyPointsReward;
            public int? matriarchId;
            public string matriarchEmotionIcon;

            public const string Status_Open = "open";
            public const string Status_In_Progress = "in_progress";
            public const string Status_Complete = "complete";
            public const string Status_Rewards_Claimed = "rewards_claimed";

            public const string Type_Ftue = "ftue";
            public const string Type_Event = "event";

            public const string QuestType_Main = "main";
            public const string QuestType_Side = "side";
            public const string QuestType_Matriarch = "matriarch";
            public const string QuestType_MatriarchDaily = "matriarch_daily";

            public const string ScreenTarget_Harem = "harem";
            public const string ScreenTarget_BattleGirls = "battle_girls";
            public const string ScreenTarget_GuestRoom = "guest_room";
            public const string ScreenTarget_MagicGuild = "magic_guild";
            public const string ScreenTarget_Municipality = "municipality";
            public const string ScreenTarget_Forge = "forge";
            public const string ScreenTarget_Laboratory = "laboratory";
            public const string ScreenTarget_Map = "map";
            public const string ScreenTarget_Portal = "portal";

            [JsonProperty(Required = Required.Default)]
            public bool isNew { get; set; }

            [JsonProperty(Required = Required.Default)]
            public bool markCompleted { get; set; }

            [JsonProperty(Required = Required.Default)]
            public bool isFTUE => type == Type_Ftue;

            [JsonProperty(Required = Required.Default)]
            public bool isEvent => type == Type_Event;

            [JsonProperty(Required = Required.Default)]
            public bool isFTUEMain => ftueQuestType == QuestType_Main;

            [JsonProperty(Required = Required.Default)]
            public bool isFTUESide => ftueQuestType == QuestType_Side;

            [JsonProperty(Required = Required.Default)]
            public bool isFTUEMatriarch => ftueQuestType == QuestType_Matriarch;

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

            [JsonProperty(Required = Required.Default)]
            public MatriarchItem matriarchData =>
                GameData.matriarchs.GetMatriarchById(matriarchId);
        }

        // //quests/{id}/claim-reward
        public static async Task<HttpCoreResponse> questClaimRewardAsync(int id) =>
            await HttpCore.PostAsync(make_url($"quests/{id}/claim-reward"));

        // /i18n
        public static async Task<HttpCoreResponse<List<LocalizationItem>>> localizationAsync(string locale) =>
            await HttpCore.GetAsync<List<LocalizationItem>>(make_url($"i18n?locale={locale}"));

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
        // /dialogs/{id}/start
        // /dialogs/{id}/end
        public static async Task<HttpCoreResponse<List<Dialog>>> dialogsAsync() =>
            await HttpCore.GetAsync<List<Dialog>>(make_url("dialogs"));
        public static async Task<HttpCoreResponse> dialogStartAsync(int id) =>
            await HttpCore.PostAsync(make_url($"dialogs/{id}/start"));
        public static async Task<HttpCoreResponse> dialogEndAsync(int id) =>
            await HttpCore.PostAsync<List<GenRewardItem>>(make_url($"dialogs/{id}/end"));


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
            public const string CharacterName_Dragon = "Dragon";
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
            public string postAction;

            public const string Type_Dialog = "dialog";
            public const string Type_Sex = "sex";
            public const string Type_Notification = "notification";

            public const string PostAction_Seduce = "seduce";

            [JsonProperty(Required = Required.Default)]
            public bool isTypeDialog => type == Type_Dialog;

            [JsonProperty(Required = Required.Default)]
            public bool isTypeSex => type == Type_Sex;

            [JsonProperty(Required = Required.Default)]
            public bool isTypeNotification => type == Type_Notification;
        }

        // /battles
        // /battles/{id}/start
        // /battles/{id}/end
        public static async Task<HttpCoreResponse<List<Battle>>> battlesAsync() =>
            await HttpCore.GetAsync<List<Battle>>(make_url("battles"));
        public static async Task<HttpCoreResponse> battleStartAsync(int id) =>
            await HttpCore.PostAsync(make_url($"battles/{id}/start"));
        public static async Task<HttpCoreResponse<List<GenRewardItem>>> battleEndAsync(int id, BattleEndData battleEndData = null) =>
            await HttpCore.PostAsync<List<GenRewardItem>>(make_url($"battles/{id}/end"), battleEndData?.ToWWWForm());

        public class BattleEndData
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

        // /battles/characters
        // /battles/my/characters
        // /battles/my/characters/{id}
        // /battles/my/characters/{id}/levelup
        // /battles/my/characters/{characterId}/skills/{skillId}/levelup
        // /battles/my/characters/{tgtId}/merge/{srcId}
        // /battles/skills/effects
        public static async Task<HttpCoreResponse<List<CharacterBase>>> charactersBaseAsync() =>
            await HttpCore.GetAsync<List<CharacterBase>>(make_url("battles/characters"));
        public static async Task<HttpCoreResponse<List<Character>>> charactersAsync() =>
            await HttpCore.GetAsync<List<Character>>(make_url("battles/my/characters"));
        public static async Task<HttpCoreResponse> characterToSlotAsync(int characterId, string slotId)
        {
            var form = new WWWForm();
            form.AddField("teamPosition", slotId);
            return await HttpCore.PostAsync(make_url($"battles/my/characters/{characterId}"), form);
        }
        public static async Task<HttpCoreResponse> characterLvlupAsync(int characterId) =>
            await HttpCore.PostAsync(make_url($"battles/my/characters/{characterId}/levelup"));
        public static async Task<HttpCoreResponse> chracterSkillLvlUp(int characterId, int skillId) =>
            await HttpCore.PostAsync(make_url($"battles/my/characters/{characterId}/skills/{skillId}/levelup"));
        public static async Task<HttpCoreResponse<Character>> charactersMrgAsync(int srcCharacterId, int trgtCharacterId) =>
            await HttpCore.PostAsync<Character>(make_url($"battles/my/characters/{trgtCharacterId}/merge/{srcCharacterId}"));
        public static async Task<HttpCoreResponse<List<SkillEffect>>> skillEffectsAsync() =>
            await HttpCore.GetAsync<List<SkillEffect>>(make_url("battles/skills/effects"));

        public class CharacterClass
        {
            public const string Assassin = "Assassin";
            public const string Bruiser = "Bruiser";
            public const string Tank = "Tank";
            public const string Caster = "Caster";
            public const string Healer = "Healer";
            public const string Overlord = "Overlord";

            public static int SortValue(string chClass) => chClass switch
            {
                Assassin => 1,
                Bruiser => 2,
                Tank => 3,
                Caster => 4,
                Healer => 5,
                Overlord => 6,
                _ => 0
            };
            public static string Marker(string chClass) => chClass switch
            {
                Assassin => TMPSprite.ClassAssassin,
                Bruiser => TMPSprite.ClassBruiser,
                Caster => TMPSprite.ClassCaster,
                Healer => TMPSprite.ClassHealer,
                Overlord => TMPSprite.ClassOverlord,
                Tank => TMPSprite.ClassTank,
                _ => ""
            };
        }

        [Serializable]
        public class CharacterBase
        {
            public int? id;
            public string basicIcon;
            public string advancedIcon;
            public string epicIcon;
            public string heroicIcon;
            public string teamEditSlotPersIcon;
            public string fullScreenPersIcon;
            public string battlePortraitIcon;
            public string name;
            public int characterClassId;
            public int? animationId;
            public float speed;
            public float power;
            public float constitution;
            public float agility;
            public int? skillNormalAttackId;
            public int? skillPassiveId;
            public int? skillSuperAttackId;
            public int mana;
            public int? sexSceneId;
            public string sexSceneVisibleByRarity;
            public string sexSceneClosedBanner;
            public string sexSceneOpenedBanner;
            public string key;
            public int? sfxAttack1Id;
            public int? sfxAttack2Id;
            public int? sfxDefeatId;
            public int? sfxDefenseId;
            public int? sfxIdleId;

            public string GetIconByRarity(string rarity) => rarity switch
            {
                Rarity.Basic => basicIcon,
                Rarity.Advanced => advancedIcon,
                Rarity.Epic => epicIcon,
                Rarity.Heroic => heroicIcon,
                _ => null
            };
        }

        [Serializable]
        public class Character
        {
            public int? id;
            public int? baseCharacterId;
            public string teamPosition;
            public string basicIcon;
            public string advancedIcon;
            public string epicIcon;
            public string heroicIcon;
            public string teamEditSlotPersIcon;
            public string fullScreenPersIcon;
            public string battlePortraitIcon;
            public string name;
            public string characterClass;
            public int characterClassId;
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
            public int potency;
            public int? sexSceneId;
            public string sexSceneVisibleByRarity;
            public string sexSceneClosedBanner;
            public string sexSceneOpenedBanner;
            public string key;
            public List<PriceItem> levelUpPrice;
            public List<PriceItem> mergePrice;
            public int? sfxAttack1Id;
            public int? sfxAttack2Id;
            public int? sfxDefeatId;
            public int? sfxDefenseId;
            public int? sfxIdleId;

            public const string TeamPosition_Slot1 = "slot1";
            public const string TeamPosition_Slot2 = "slot2";
            public const string TeamPosition_None = "none";

            [JsonProperty(Required = Required.Default)]
            public CharacterBase baseCharacterData =>
                GameData.characters.GetBaseById(baseCharacterId);

            [JsonProperty(Required = Required.Default)]
            public int raritySortLevel => Rarity.SortValue(rarity);

            [JsonProperty(Required = Required.Default)]
            public int raritySortLevelDesc => Rarity.DescSortValue(rarity);

            [JsonProperty(Required = Required.Default)]
            public int classSortLevel => CharacterClass.SortValue(characterClass);

            public string GetIconByRarity(string rarity) => rarity switch
            {
                Rarity.Basic => basicIcon,
                Rarity.Advanced => advancedIcon,
                Rarity.Epic => epicIcon,
                Rarity.Heroic => heroicIcon,
                _ => null
            };

            public CharacterSkill GetSkillByType(string type) =>
                skills.FirstOrDefault(s => s.type == type);

            [JsonProperty(Required = Required.Default)]
            public string iconUrl => GetIconByRarity(rarity);

            [JsonProperty(Required = Required.Default)]
            public string classMarker => CharacterClass.Marker(characterClass);

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

            public bool CanSkillLvlUpByPrice(CharacterSkill skill) =>
                GameData.player.CanBuy(skill.levelUpPrice);

            public bool CanSkillLvlUpByLevel(CharacterSkill skill) => level > skill.level;

            [JsonProperty(Required = Required.Default)]
            public int maxLvl => Rarity.MaxLvl(rarity);

            [JsonProperty(Required = Required.Default)]
            public bool isSexSceneOpen => sexSceneId.HasValue;

            [JsonProperty(Required = Required.Default)]
            public bool hasEquipment => equipment.Count > 0;

            [JsonProperty(Required = Required.Default)]
            public Equipment characterEquipmentData => GameData.equipment.GetById(equipment.FirstOrDefault());

            [JsonProperty(Required = Required.Default)]
            public Animation animationData => GameData.animations.GetById(animationId);

            [JsonProperty(Required = Required.Default)]
            public bool inTeam => teamPosition == TeamPosition_Slot1 || teamPosition == TeamPosition_Slot2;

            [JsonProperty(Required = Required.Default)]
            public CharacterSkill basicSkill =>
                skills.FirstOrDefault(s => s.type == AdminBRO.CharacterSkill.Type_Attack);

            [JsonProperty(Required = Required.Default)]
            public CharacterSkill ultimateSkill =>
                skills.FirstOrDefault(s => s.type == AdminBRO.CharacterSkill.Type_Enhanced);

            [JsonProperty(Required = Required.Default)]
            public CharacterSkill passiveSkill =>
                skills.FirstOrDefault(s => s.type == AdminBRO.CharacterSkill.Type_Passive);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxAttack1 => GameData.sounds.GetById(sfxAttack1Id);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxAttack2 => GameData.sounds.GetById(sfxAttack2Id);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxDefeat => GameData.sounds.GetById(sfxDefeatId);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxDefense => GameData.sounds.GetById(sfxDefenseId);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxIdle => GameData.sounds.GetById(sfxIdleId);
        }

        [Serializable]
        public class CharacterSkill
        {
            public int id;
            public int? characterId;
            public string name;
            private string _description;
            public string description
            {
                get => _description.
                    Replace("%amount%", ((int)(amount * 100.0f)).ToString() + "%").
                    Replace("%effect_amount%", ((int)(effectAmount * 100)).ToString() + "%").
                    Replace("%effect_prob%", ((int)(effectProb * 100)).ToString()).
                    Replace("%effect_acting_duration%", ((int)(effectActingDuration)).ToString()).
                    Replace("%effect_cooldown_duration%", ((int)(effectCooldownDuration)).ToString());
                set => _description = value;
            }
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
            public int? vfxSelfId;
            public int? vfxAOEId;
            public int? vfxTargetId;
            public int? sfxAttackId;
            public int? sfxTargetId;
            public bool shakeScreen;

            public const string Type_OverlordAttack = "overlord_attack";

            public const string Type_Passive = "passive_skill";
            public const string Type_Attack = "attack";
            public const string Type_Enhanced = "enhanced_attack";

            [JsonProperty(Required = Required.Default)]
            public string effectSprite => effect switch
            {
                SkillEffect.Key_Bless => TMPSprite.BuffBless,
                SkillEffect.Key_DefenceUp => TMPSprite.BuffDefenceUp,
                SkillEffect.Key_Dispel => TMPSprite.BuffDispell,
                SkillEffect.Key_Focus => TMPSprite.BuffFocus,
                SkillEffect.Key_Immunity => TMPSprite.BuffImmunity,
                SkillEffect.Key_Regeneration => TMPSprite.BuffRegeneration,
                SkillEffect.Key_Safeguard => TMPSprite.BuffSafeguard,
                SkillEffect.Key_Blind => TMPSprite.DebuffBlind,
                SkillEffect.Key_Curse => TMPSprite.DebuffCurse,
                SkillEffect.Key_DefenceDown => TMPSprite.DebuffDefenceDown,
                SkillEffect.Key_HealBlock => TMPSprite.DebuffHealBlock,
                SkillEffect.Key_Posion => TMPSprite.DebuffPoison,
                SkillEffect.Key_Silence => TMPSprite.DebuffSilence,
                SkillEffect.Key_Stun => TMPSprite.DebuffStun,
                _ => null
            };

            [JsonProperty(Required = Required.Default)]
            public Animation vfxSelf => GameData.animations.GetById(vfxSelfId);

            [JsonProperty(Required = Required.Default)]
            public Animation vfxAOE => GameData.animations.GetById(vfxAOEId);

            [JsonProperty(Required = Required.Default)]
            public Animation vfxTarget => GameData.animations.GetById(vfxTargetId);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxAttack => GameData.sounds.GetById(sfxAttackId);

            [JsonProperty(Required = Required.Default)]
            public Sound sfxTarget => GameData.sounds.GetById(sfxTargetId);

            [JsonProperty(Required = Required.Default)]
            public Character characterData => GameData.characters.GetById(characterId);

            public string GetDescription(float amount, float effectAmount) => _description.
                Replace("%amount%", ((int)(amount)).ToString()).
                Replace("%effect_amount%", ((int)(effectAmount)).ToString()).
                Replace("%effect_prob%", ((int)(effectProb * 100)).ToString()).
                Replace("%effect_acting_duration%", ((int)(effectActingDuration)).ToString()).
                Replace("%effect_cooldown_duration%", ((int)(effectCooldownDuration)).ToString());
        }

        [Serializable]
        public class SkillEffect
        {
            public string name;
            public string description;

            public const string Key_Bless = "bless";
            public const string Key_Blind = "blind";
            public const string Key_Curse = "curse";
            public const string Key_DefenceDown = "defence_down";
            public const string Key_DefenceUp = "defence_up";
            public const string Key_Dispel = "dispel";
            public const string Key_Focus = "focus";
            public const string Key_HealBlock = "heal_block";
            public const string Key_Immunity = "immunity";
            public const string Key_Posion = "poison";
            public const string Key_Regeneration = "regeneration";
            public const string Key_Safeguard = "safeguard";
            public const string Key_Silence = "silence";
            public const string Key_Stun = "stun";
            public const string Key_AttackUp = "attack_up";

        }

        // /battles/pass
        // /battles/pass/{id}/buy-premium
        // /battles/pass/{id}/claim
        public static async Task<HttpCoreResponse<List<BattlePass>>> battlePassesAsync() =>
            await HttpCore.GetAsync<List<BattlePass>>(make_url("battles/pass"));
        public static async Task<HttpCoreResponse> battlePassBuyPremiumAsync(int battlePassId) =>
            await HttpCore.PostAsync(make_url($"battles/pass/{battlePassId}/buy-premium"));
        public static async Task<HttpCoreResponse> battlePassClaimAsync(int battlePassId) =>
            await HttpCore.PostAsync(make_url($"battles/pass/{battlePassId}/claim"));


        [Serializable]
        public class BattlePass
        {
            public int id;
            public int? eventId;
            public List<Level> levels;
            public List<PriceItem> premiumPrice;
            public int currentPointsCount;
            public bool isPremium;

            public class Level
            {
                public int pointsThreshold;
                public List<RewardItem> defaultReward;
                public List<RewardItem> premiumReward;
                public bool isDefaultRewardClaimed;
                public bool isPremiumRewardClaimed;
            }
        }

        // /characters/equipment
        // /my/characters/equipment
        // /battles/my/characters/{id}/equip/{id} - post
        // /battles/my/characters/{id}/equip/{id} - delete
        public static async Task<HttpCoreResponse<List<EquipmentBase>>> equipmentBaseAsync() =>
            await HttpCore.GetAsync<List<EquipmentBase>>(make_url("battles/characters/equipment"));
        public static async Task<HttpCoreResponse<List<Equipment>>> equipmentAsync() =>
            await HttpCore.GetAsync<List<Equipment>>(make_url("battles/my/characters/equipment"));
        public static async Task<HttpCoreResponse> equipAsync(int characterId, int equipmentId) =>
            await HttpCore.PostAsync(make_url($"battles/my/characters/{characterId}/equip/{equipmentId}"));
        public static async Task<HttpCoreResponse> unequipAsync(int characterId, int equipmentId) =>
            await HttpCore.DeleteAsync(make_url($"battles/my/characters/{characterId}/equip/{equipmentId}"));

        public class EquipmentType
        {
            public const string CharacterWeapon = "battle_character_weapon";
            public const string OverlordThighs = "overlord_thighs";
            public const string OverlordHelmet = "overlord_helmet";
            public const string OverlordBoots = "overlord_boots";
            public const string OverlordHarness = "overlord_harness";
            public const string OverlordWeapon = "overlord_weapon";
            public const string OverlordGloves = "overlord_gloves";

            public static int SortValue(string eqType) => eqType switch
            {
                OverlordThighs => 1,
                OverlordHelmet => 2,
                OverlordBoots => 3,
                OverlordHarness => 4,
                OverlordWeapon => 5,
                OverlordGloves => 6,
                CharacterWeapon => 7,
                _ => 0
            };
        }

        [Serializable]
        public class EquipmentBase
        {
            public int id;
            public string name;
            public string key;
            public string type;
            public string basicIcon;
            public string advancedIcon;
            public string epicIcon;
            public string heroicIcon;
            public float speed;
            public float power;
            public int characterClassId;
            public float constitution;
            public float agility;
            public float accuracy;
            public float dodge;
            public float critrate;
            public float health;
            public float damage;
            public float mana;
            public float speedBoost;
            public float powerBoost;
            public float constitutionBoost;
            public float agilityBoost;
            public float accuracyBoost;
            public float dodgeBoost;
            public float critrateBoost;
            public float healthBoost;
            public float damageBoost;
            public float manaBoost;

            public string GetIconByRarity(string rarity) => rarity switch
            {
                Rarity.Basic => basicIcon,
                Rarity.Advanced => advancedIcon,
                Rarity.Epic => epicIcon,
                Rarity.Heroic => heroicIcon,
                _ => null
            };
        }

        [Serializable]
        public class Equipment
        {
            public int id;
            public int? baseEquipmentId;
            public int? characterId;
            public string characterClass;
            public string name;
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
            public string basicIcon;
            public string advancedIcon;
            public string epicIcon;
            public string heroicIcon;
            public string rarity;

            [JsonProperty(Required = Required.Default)]
            public EquipmentBase baseEquipmentData =>
                GameData.equipment.GetBaseById(baseEquipmentId);

            [JsonProperty(Required = Required.Default)]
            public bool isEquipped => characterId.HasValue;

            [JsonProperty(Required = Required.Default)]
            public int raritySortLevel => Rarity.SortValue(rarity);

            [JsonProperty(Required = Required.Default)]
            public int raritySortLevelDesc => Rarity.DescSortValue(rarity);

            [JsonProperty(Required = Required.Default)]
            public int classSortLevel => CharacterClass.SortValue(characterClass);

            [JsonProperty(Required = Required.Default)]
            public int typeSortLevel => EquipmentType.SortValue(equipmentType);

            [JsonProperty(Required = Required.Default)]
            public string classMarker => CharacterClass.Marker(characterClass);

            [JsonProperty(Required = Required.Default)]
            public string icon => GetIconByRarity(rarity);

            public string GetIconByRarity(string rarity) => rarity switch
            {
                Rarity.Basic => basicIcon,
                Rarity.Advanced => advancedIcon,
                Rarity.Epic => epicIcon,
                Rarity.Heroic => heroicIcon,
                _ => null
            };

            public bool IsMyClass(string chClass) => chClass == characterClass;

            public bool IsMy(int? myId) => isEquipped && myId == characterId;
        }

        //ftue
        public static async Task<HttpCoreResponse<FTUEInfo>> ftueAsync() =>
            await HttpCore.GetAsync<FTUEInfo>(make_url("ftue"));

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

            [JsonProperty(Required = Required.Default)]
            public List<FTUEStageItem> stagesData => stages.Select(sId => GetStageById(sId)).Where(s => s != null).ToList();

            [JsonProperty(Required = Required.Default)]
            public bool isComplete => !stagesData.Exists(s => !s.isComplete);

            [JsonProperty(Required = Required.Default)]
            public bool isActive => GameData.ftue.activeChapter?.id == id;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => GameData.devMode ? true : (isComplete || isActive);

            [JsonProperty(Required = Required.Default)]
            public FTUEChapter nextChapterData => GameData.ftue.GetChapterById(nextChapterId);

            public FTUENotificationItem GetNotifByKey(string key) => notifications.Find(n => n.key == key);
            public FTUEStageItem GetStageById(int? id) => GameData.ftue.GetStageById(id);
            public FTUEStageItem GetStageByKey(string key) => stagesData.Find(s => s.key == key);
            public FTUEChapter SetAsMapChapter() => GameData.ftue.mapChapter = this;
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
                    }).DoShow();

                if (notifData != null)
                {
                    notifData.isShown = true;
                }
            }
        }

        [Serializable]
        public class FTUEInfo
        {
            public List<FTUEChapter> chapters;
        }

        // ftue/stats
        public static async Task<HttpCoreResponse<FTUEStats>> ftueStatsAsync() =>
            await HttpCore.GetAsync<FTUEStats>(make_url("ftue/stats"));

        [Serializable]
        public class FTUEStats
        {
            public int? lastStartedStage;
            public int? lastUpdatedStage;
            public int? lastEndedStage;

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastStartedStageData =>
                GameData.ftue.GetStageById(lastStartedStage);

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastUpdatedStageData =>
                GameData.ftue.GetStageById(lastUpdatedStage);

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastEndedStageData =>
                GameData.ftue.GetStageById(lastEndedStage);
        }

        // /ftue-stages
        public static async Task<HttpCoreResponse<List<FTUEStageItem>>> ftueStagesAsync() =>
            await HttpCore.GetAsync<List<FTUEStageItem>>(make_url("ftue-stages"));

        [Serializable]
        public class FTUEStageItem
        {
            public int id;
            public string key;
            public int? ftueChapterId;
            public int? dialogId;
            public int? battleId;
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
                GameData.ftue.GetChapterById(ftueChapterId);

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
            public (string chKey, string sKey)? lerningKey =>
                GameData.devMode ? ((string, string)?)null : (ftueChapterData.key, key);

            [JsonProperty(Required = Required.Default)]
            public bool isLastEnded =>
                id == GameData.ftue.stats.lastEndedStage;
        }

        // /ftue-stages/{id}/start
        // /ftue-stages/{id}/end
        // /ftue-stages/{id}/replay
        public static async Task<HttpCoreResponse> ftueStageStartAsync(int stageId) =>
            await HttpCore.PostAsync(make_url($"ftue-stages/{stageId}/start"));

        public static async Task<HttpCoreResponse<List<GenRewardItem>>> ftueStageEndAsync(int stageId, BattleEndData battleEndData = null) =>
            await HttpCore.PostAsync<List<GenRewardItem>>(make_url($"ftue-stages/{stageId}/end"), battleEndData?.ToWWWForm());

        public static async Task<HttpCoreResponse> ftueStageReplayAsync(int stageId, int count)
        {
            var form = new WWWForm();
            form.AddField("count", count);
            return await HttpCore.PostAsync(make_url($"ftue-stages/{stageId}/replay"), form);
        }

        //animations
        public static async Task<HttpCoreResponse<List<Animation>>> animationsAsync() =>
            await HttpCore.GetAsync<List<Animation>>(make_url("animations"));

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
        public static async Task<HttpCoreResponse<List<Sound>>> soundsAsync() =>
            await HttpCore.GetAsync<List<Sound>>(make_url("sounds"));

        [Serializable]
        public class Sound
        {
            public int id;
            public string title;
            public string soundBankId;
            public string eventPath;

            public void Play() => SoundManager.PlayOneShot(eventPath, soundBankId);

            [JsonProperty(Required = Required.Default)]
            public FMODEvent instantiate => SoundManager.GetEventInstance(eventPath, soundBankId);
        }

        // /buildings
        // /buildings/{id}/build
        // /buildings/{id}/build-crystals
        public static async Task<HttpCoreResponse<List<Building>>> buildingsAsync() =>
            await HttpCore.GetAsync<List<Building>>(make_url("buildings"));

        public static async Task<HttpCoreResponse> buildingBuildAsync(int id) =>
            await HttpCore.PostAsync(make_url($"buildings/{id}/build"));

        public static async Task<HttpCoreResponse> buildingBuildCrystalsAsync(int id) =>
            await HttpCore.PostAsync(make_url($"buildings/{id}/build-crystals"));

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
        // /municipality/settings
        public static async Task<HttpCoreResponse<MunicipalityTimeLeft>> municipalityTimeLeftAsync() =>
            await HttpCore.GetAsync<MunicipalityTimeLeft>(make_url("municipality/time-left"));

        public static async Task<HttpCoreResponse> municipalityCollectAsync() =>
            await HttpCore.PostAsync(make_url("municipality/collect"));

        public static async Task<HttpCoreResponse<MunicipalitySettings>> municipalitySettingsAsync() =>
            await HttpCore.GetAsync<MunicipalitySettings>(make_url("municipality/settings"));

        [Serializable]
        public class MunicipalityTimeLeft
        {
            public float timeLeft;
        }

        [Serializable]
        public class MunicipalitySettings
        {
            public int currentLevelNumber;
            public int moneyPerPeriod;
            public int currencyPerHour;
            public int periodInSeconds;
            public int? currencyId;
        }

        // /magicguild/skills
        // /magicguild/{skillType}/levelup
        public static async Task<HttpCoreResponse<List<MagicGuildSkill>>> magicGuildSkillsAsync() =>
            await HttpCore.GetAsync<List<MagicGuildSkill>>(make_url("magicguild/skills"));

        public static async Task<HttpCoreResponse> magicGuildSkillLvlUpAsync(string skillType) =>
            await HttpCore.PostAsync(make_url($"magicguild/{skillType}/levelup"));

        public static async Task<HttpCoreResponse> magicGuildSkillLvlUpCrystalAsync(string skillType) =>
            await HttpCore.PostAsync(make_url($"magicguild/{skillType}/levelup-crystals"));

        [Serializable]
        public class MagicGuildSkill
        {
            public string type;
            public CharacterSkill current;
            public CharacterSkill next;
            public int? currentSkillLevel;
            public int? requiredBuildingLevel;
            public int? maxSkillLevel;
            public int skillId;
            public List<PriceItem> price;
            public List<PriceItem> priceCrystal;
            public bool locked;

            [JsonProperty(Required = Required.Default)]
            public bool isLvlMax => currentSkillLevel == maxSkillLevel;

            [JsonProperty(Required = Required.Default)]
            public bool canlvlUp => GameData.player.CanBuy(current.levelUpPrice);

            [JsonProperty(Required = Required.Default)]
            public bool canCrystallvlUp => GameData.player.CanBuy(priceCrystal);

            [JsonProperty(Required = Required.Default)]
            public bool canUpgrade =>
                GameData.buildings.magicGuild.meta.currentLevel >=
                requiredBuildingLevel && next != null;

            public const string Type_ActiveSkill = "overlord_enhanced_attack";
            public const string Type_UltimateSkill = "overlord_ultimate_attack";
            public const string Type_PassiveSkill1 = "overlord_first_passive_skill";
            public const string Type_PassiveSkill2 = "overlord_second_passive_skill";
        }

        // /forge/price
        // /forge/merge
        public static async Task<HttpCoreResponse<ForgePrice>> forgePrices() =>
            await HttpCore.GetAsync<ForgePrice>(make_url("forge/price"));

        public static async Task<HttpCoreResponse> forgeMergeEquipment(string mergeType, int[] mergeIds)
        {
            var form = new WWWForm();
            form.AddField("mergeType", mergeType);
            foreach (var mId in mergeIds)
            {
                form.AddField("ids[]", mId);
            }
            return await HttpCore.PostAsync(make_url("forge/merge/equipment"), form);
        }

        public static async Task<HttpCoreResponse> forgeMergeShard(int matriarchId, string rarity, int amount)
        {
            var form = new WWWForm();
            form.AddField("matriarchId", matriarchId);
            form.AddField("rarity", rarity);
            form.AddField("amount", amount);
            return await HttpCore.PostAsync(make_url("forge/merge/shard"), form);
        }

        public static async Task<HttpCoreResponse> forgeExchangeShard(int matriarchSourceId, int matriarchTargetId, string rarity, int amount)
        {
            var form = new WWWForm();
            form.AddField("matriarchSourceId", matriarchSourceId);
            form.AddField("matriarchTargetId", matriarchTargetId);
            form.AddField("rarity", rarity);
            form.AddField("amount", amount);
            return await HttpCore.PostAsync(make_url("forge/exchange/shard"), form);
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
                public int mergeAmount;
                public int maxPossibleResultAmount;
                public List<MergePrice> pricesOfMergeType;

                public List<PriceItem> GetPrice(string rarity) =>
                    pricesOfMergeType.Find(p => p.rarity == rarity)?.price ?? new List<PriceItem>();
            }

            public class MergeShardSettings
            {
                public int mergeAmount;
                public int maxPossibleResultAmount;
                public List<MergePrice> pricesOfMergeType;

                public List<PriceItem> GetPrice(string rarity) =>
                    pricesOfMergeType.Find(p => p.rarity == rarity)?.price ?? new List<PriceItem>();
            }

            public class ExchangeShardSettings
            {
                public int exchangeAmount;
                public int maxPossibleResultAmount;
                public List<MergePrice> pricesOfExchangeType;

                public List<PriceItem> GetPrice(string rarity) =>
                    pricesOfExchangeType.Find(p => p.rarity == rarity)?.price ?? new List<PriceItem>();
            }

            public class MergePrice
            {
                public string rarity;
                public List<PriceItem> price;
            }

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentCharacterSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_battle_character");

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentOverlordHelmetSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_overlord_helmet");

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentOverlordWeaponSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_overlord_weapon");

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentOverlordGlovesSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_overlord_gloves");

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentOverlordThighsSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_overlord_thighs");

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentOverlordBootsSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_overlord_boots");

            [JsonProperty(Required = Required.Default)]
            public MergeEquipmentSettings mergeEquipmentOverlordHarnessSettings =>
                mergeEquipmentSettings.Find(ms => ms.mergeType == "merge_overlord_harness");
        }

        // gacha
        public static async Task<HttpCoreResponse<List<GachaItem>>> gachaAsync() =>
            await HttpCore.GetAsync<List<GachaItem>>(make_url("gacha"));

        public static async Task<HttpCoreResponse<List<GachaBuyResult>>> gachaBuyAsync(int id) =>
            await HttpCore.PostAsync<List<GachaBuyResult>>(make_url($"gacha/{id}/buy"));

        public static async Task<HttpCoreResponse<List<GachaBuyResult>>> gachaBuyManyAsync(int id) =>
            await HttpCore.PostAsync<List<GachaBuyResult>>(make_url($"gacha/{id}/buy-many"));

        [Serializable]
        public class GachaBuyResult
        {
            public int? tradableId;
            public int? entityUserProgressId;
            public string rarity;
            public int amount;

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
        // /matriarchs/memories/{memoryId}/piece-of-glass/{shardKey}/buy
        // /matriarchs/{id}/seduce
        // /matriarchs/shards
        // /matriarchs/buffs

        public static async Task<HttpCoreResponse<List<MatriarchItem>>> matriarchsAsync() =>
            await HttpCore.GetAsync<List<MatriarchItem>>(make_url("matriarchs"));

        public static async Task<HttpCoreResponse<List<MemoryItem>>> memoriesAsync() =>
            await HttpCore.GetAsync<List<MemoryItem>>(make_url("matriarchs/memories"));

        public static async Task<HttpCoreResponse> memoryPieceOfGlassBuyAsync(int memoryId, string shardKey) =>
            await HttpCore.PostAsync(make_url($"matriarchs/memories/{memoryId}/piece-of-glass/{shardKey}/buy"));

        public static async Task<HttpCoreResponse> seduceMatriarchAsync(int id) =>
            await HttpCore.PostAsync(make_url($"matriarchs/{id}/seduce"));

        public static async Task<HttpCoreResponse<List<MemoryShardItem>>> memoryShardsAsync() =>
            await HttpCore.GetAsync<List<MemoryShardItem>>(make_url("matriarchs/shards"));

        public static async Task<HttpCoreResponse<List<BuffItem>>> buffsAsync() =>
            await HttpCore.GetAsync<List<BuffItem>>(make_url("matriarchs/buffs"));

        [Serializable]
        public class MatriarchItem
        {
            public int id;
            public string name;
            public int? paramAge;
            public string paramZodiac;
            public int? seduceSexSceneId;
            public int? dailyQuestGiverDialogId;
            public int seduceCooldown;
            public string status;
            public int? currentEmpathyPoints;
            public int? empathyLevelTargetPoints;
            public int? currentEmpathyLevel;
            public int? nextEmpathyLevel;
            public string rewardsClaimed;
            public string seduceAvailableAt;
            public string matriarchType;
            public string narratorIcon;

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem basicShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Basic);

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem advancedShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Advanced);

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem epicShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Epic);

            [JsonProperty(Required = Required.Default)]
            public MemoryShardItem heroicShard => GameData.matriarchs.GetShardByMatriarchId(id, Rarity.Heroic);

            [JsonProperty(Required = Required.Default)]
            public bool isMain => matriarchType == MatriarchType_Main;
            
            [JsonProperty(Required = Required.Default)]
            public bool isGuest => matriarchType == MatriarchType_Guest;

            public const string Key_Ulvi = "Ulvi";
            public const string Key_Adriel = "Adriel";
            public const string Key_Ingie = "Ingie";
            public const string Key_Faye = "Faye";
            public const string Key_Lili = "Lili";

            public const string RewardsClaimed_None = "none";
            public const string RewardsClaimed_TwentFive = "twenty_five";
            public const string RewardsClaimed_Fifty = "fifty";
            public const string RewardsClaimed_All = "all";

            public const string MatriarchType_Main = "main";
            public const string MatriarchType_Guest = "guest";

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
            public bool isOpen => GameData.devMode ? true :
                name switch
                {
                    Key_Ulvi => true,
                    Key_Adriel => GameData.ftue.chapter1_dialogue3.isComplete,
                    Key_Ingie => GameData.ftue.chapter2_dialogue4.isComplete,
                    Key_Faye => false,
                    Key_Lili => false,
                    _ => false
                };

            [JsonProperty(Required = Required.Default)]
            public BuffItem buff => GameData.matriarchs.GetBuffByMatriarchId(id);
        }

        [Serializable]
        public class MemoryItem
        {
            public int id;
            public int userId;
            public string status;
            public int? eventId;
            public int? matriarchId;
            public string title;
            public string label;
            public int? sexSceneId;
            public List<Piece> pieces;
            public string memoryType;
            public string memoryBackArt;
            public string sexSceneClosed;
            public string sexScenePreview;

            public class Piece
            {
                public string shardName;
                public bool isPurchased;
                public int shardId;
                public string rarity;
            }

            public const string Status_Visible = "visible";
            public const string Status_Open = "open";

            public const string MemoryType_Main = "main";
            public const string MemoryType_Story = "story";
            public const string MemoryType_Event = "event";

            [JsonProperty(Required = Required.Default)]
            public AdminBRO.MatriarchItem matriarchData => GameData.matriarchs.GetMatriarchById(matriarchId);
            
            [JsonProperty(Required = Required.Default)]
            public bool isVisible => status == Status_Visible;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;
            
            [JsonProperty(Required = Required.Default)]
            public bool isMain => memoryType == MemoryType_Main;
            
            [JsonProperty(Required = Required.Default)]
            public bool isStory => memoryType == MemoryType_Event;
            
            [JsonProperty(Required = Required.Default)]
            public bool isEvent => memoryType == Status_Open;

            [JsonProperty(Required = Required.Default)]
            public List<Piece> basicPieces => pieces.FindAll(p => p.rarity == Rarity.Basic);

            [JsonProperty(Required = Required.Default)]
            public List<Piece> advancedPieces => pieces.FindAll(p => p.rarity == Rarity.Advanced);

            [JsonProperty(Required = Required.Default)]
            public List<Piece> epicPieces => pieces.FindAll(p => p.rarity == Rarity.Epic);

            [JsonProperty(Required = Required.Default)]
            public List<Piece> heroicPieces => pieces.FindAll(p => p.rarity == Rarity.Heroic);
        }

        [Serializable]
        public class MemoryShardItem
        {
            public int id;
            public int? matriarchId;
            public string rarity;
            public string icon;
            public int amount;
            public string shardType;

            public const string ShardType_Main = "main";
            public const string ShardType_Guest = "guest";

            [JsonProperty(Required = Required.Default)]
            public MatriarchItem matriarchData =>
                GameData.matriarchs.GetMatriarchById(matriarchId);

            [JsonProperty(Required = Required.Default)]
            public string tmpSprite => rarity switch
            {
                Rarity.Basic => TMPSprite.ShardBasic,
                Rarity.Advanced => TMPSprite.ShardAdvanced,
                Rarity.Epic => TMPSprite.ShardEpic,
                Rarity.Heroic => TMPSprite.ShardHeroic,
                _ => ""
            };
        }

        [Serializable]
        public class BuffItem
        {
            public int id;
            public int? matriarchId;
            public string name;
            private string _description;
            public string description
            {
                get => _description.
                    Replace("%accuracy%", ((int)(accuracy * 100)).ToString()).
                    Replace("%dodge%", ((int)(dodge * 100)).ToString()).
                    Replace("%critrate%", ((int)(critrate * 100)).ToString()).
                    Replace("%health%", ((int)(health)).ToString()).
                    Replace("%damage%", ((int)(damage)).ToString()).
                    Replace("%mana%", ((int)(mana)).ToString());
                set => _description = value;
            }
            public string postDescription;
            public bool AOE;
            public string icon;
            public float accuracy;
            public float dodge;
            public float critrate;
            public float health;
            public float damage;
            public float mana;
            public bool active;

            [JsonProperty(Required = Required.Default)]
            public MatriarchItem matriarch =>
                GameData.matriarchs.GetMatriarchById(matriarchId);
        }

        // /potions
        // /potions/{type}/buy
        public static async Task<HttpCoreResponse<PotionsInfo>> potionsAsync() =>
            await HttpCore.GetAsync<PotionsInfo>(make_url("potions"));

        public static async Task<HttpCoreResponse> potionBuyAsync(string type, int count)
        {
            var form = new WWWForm();
            form.AddField("count", count);
            return await HttpCore.PostAsync(make_url($"potions/{type}/buy"), form);
        }

        public static async Task<HttpCoreResponse> potionEnergyUseAsync(int count)
        {
            var form = new WWWForm();
            form.AddField("count", count);
            return await HttpCore.PostAsync(make_url("potions/energy/use"), form);
        }


        [Serializable]
        public class PotionsInfo
        {
            public List<PotionInfo> prices;
            public int maxEnergyVolume;
            public int energyPerCan;
            public float energyRecoverySpeedPerMinute;

            public class PotionInfo
            {
                public string type;
                public List<PriceItem> price;
                public float magnitude;
            }

            public const string Type_hp = "hp";
            public const string Type_mana = "mana";
            public const string Type_energy = "energy";
            public const string Type_replay = "replay";
        }

        // /nutaku
        // /nutaku/settings
        public static async Task<HttpCoreResponse<NutakuSettings>> nutakuSettingsAsync() =>
            await HttpCore.GetAsync<NutakuSettings>(make_url("nutaku/settings"));

        [Serializable]
        public class NutakuSettings
        {
            public string callbackUrl;
            public string completeUrl;
        }
    }
}