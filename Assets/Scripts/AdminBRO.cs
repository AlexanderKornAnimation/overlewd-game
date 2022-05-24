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

        // /me/init
        public static async Task initAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/me/init";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /me/reset
        public static async Task resetAsync(List<string> resetEntities)
        {
            var url = "https://overlewd-api.herokuapp.com/me/reset";
            var form = new WWWForm();
            foreach (var entityName in resetEntities)
            {
                form.AddField("modules[]", entityName);
            }
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        public class ResetEntityName
        {
            public const string Wallet = "wallet";
            public const string Inventory = "inventory";
            public const string Building = "building";
            public const string Battle = "battle";
            public const string Quest = "quest";
            public const string Event = "event";
            public const string FTUE = "ftue";
        }

        // GET /me; POST /me
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

            public bool CanBuy(List<PriceItem> price)
            {
                foreach (var priceItem in price)
                {
                    var walletCurrency = wallet.Find(item => item.currency.id == priceItem.currencyId);
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

        [Serializable]
        public class WalletItem
        {
            public int id;
            public int amount;
            public string createdAt;
            public string updatedAt;
            public CurrencyItem currency;
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
            public string iconUrl;
            public bool nutaku;
            public string createdAt;
            public string updatedAt;
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

            [JsonProperty(Required = Required.Default)]
            public bool canBuy => GameData.playerInfo.CanBuy(price);
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
        }

        [Serializable]
        public class EventChapterReward
        {
            public string icon;
            public int amount;
            public int currency;
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
            public bool win = true;

            public WWWForm ToWWWForm()
            {
                var form = new WWWForm();
                form.AddField("result", win ? "win" : "lose");
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
                dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;

            [JsonProperty(Required = Required.Default)]
            public Battle battleData =>
                battleId.HasValue ? GameData.GetBattleById(battleId.Value) : null;

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
            public List<int> animations;
            public int? level;
            public string rarity;
            public List<int> equipment;
            public List<int> skills;
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
            public int? sexSceneId;

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
                dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;
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
                nextChapterId.HasValue ? GameData.ftue.info.GetChapterById(nextChapterId.Value) : null;

            public FTUEStageItem GetStageById(int id) =>
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
            public FTUEChapter GetChapterById(int id) =>
                chapters.Find(ch => ch.id == id);
            public FTUEStageItem GetStageById(int id) =>
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
                lastStartedStage.HasValue ? GameData.ftue.info.GetStageById(lastStartedStage.Value) : null;

            [JsonProperty(Required = Required.Default)]
            public FTUEStageItem lastEndedStageData =>
                lastEndedStage.HasValue ? GameData.ftue.info.GetStageById(lastEndedStage.Value) : null;

            [JsonProperty(Required = Required.Default)]
            public (string stageKey, string chapterKey)? lastEndedState =>
                GameData.progressMode switch {
                    true => (lastEndedStageData?.key, lastEndedStageData?.ftueChapterData?.key),
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
                ftueChapterId.HasValue ? GameData.ftue.info.GetChapterById(ftueChapterId.Value) : null;

            [JsonProperty(Required = Required.Default)]
            public Dialog dialogData => 
                dialogId.HasValue ? GameData.GetDialogById(dialogId.Value) : null;

            [JsonProperty(Required = Required.Default)]
            public Battle battleData => 
                battleId.HasValue ? GameData.GetBattleById(battleId.Value) : null;

            [JsonProperty(Required = Required.Default)]
            public bool isOpen => status == Status_Open;

            [JsonProperty(Required = Required.Default)]
            public bool isStarted => status == Status_Started;
         
            [JsonProperty(Required = Required.Default)]
            public bool isComplete => status == Status_Complete;

            [JsonProperty(Required = Required.Default)]
            public bool isClosed => status == Status_Closed;

            [JsonProperty(Required = Required.Default)]
            public (string stageKey, string chapterKey) ftueState =>
               (key, ftueChapterData?.key);
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
            public bool win = true;

            public WWWForm ToWWWForm()
            {
                var form = new WWWForm();
                form.AddField("result", win ? "win" : "lose");
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
            public List<AnimationData> layouts;
        }

        [Serializable]
        public class AnimationData
        {
            public string assetBundleId;
            public string animationPath;
            public string animationName;
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
        public static async Task<List<Building>> buildingsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/buildings";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<Building>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class Building
        {
            public int id;
            public string key;
            public string name;
            public string description;
            public string image;
            public string icon;
            public List<Level> levels;
            public int? currentLevel;
            public int? nextLevel;
            public int? maxLevel;
            public string buildStartedAt;

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
                public List<PriceItem> momentPrice;
                public int minutesToBuild;

                [JsonProperty(Required = Required.Default)]
                public bool canBuild => GameData.playerInfo.CanBuy(price);

                [JsonProperty(Required = Required.Default)]
                public bool canBuildNow => GameData.playerInfo.CanBuy(momentPrice);
            }

            [JsonProperty(Required = Required.Default)]
            public bool isMax => isBuilt ? currentLevel == maxLevel : false;

            [JsonProperty(Required = Required.Default)]
            public bool isBuilt => currentLevel.HasValue;

            [JsonProperty(Required = Required.Default)]
            public bool isUnderConstruction => !String.IsNullOrEmpty(buildStartedAt);

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
            public bool canUpgradeNow =>
                nextLevelData?.canBuildNow ?? false;

            [JsonProperty(Required = Required.Default)]
            public bool canUpgrade =>
                nextLevelData?.canBuild ?? false;
        }

        // /buildings/{id}/build
        public static async Task buildingBuildAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/buildings/{id}/build";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

            }
        }

        // /buildings/{id}/build-now
        public static async Task buildingBuildNowAsync(int id)
        {
            var url = $"https://overlewd-api.herokuapp.com/buildings/{id}/build-now";
            var form = new WWWForm();
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {

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
    }
}
