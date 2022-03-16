using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class AdminBRO
    {
        public static string GetDeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
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
        public static async Task<PlayerInfo> meAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/me", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<PlayerInfo>(request?.downloadHandler.text);
            }
        }

        public static async Task<PlayerInfo> meAsync(string name)
        {
            var formMe = new WWWForm();
            formMe.AddField("name", name);
            using (var request = await HttpCore.PostAsync("https://overlewd-api.herokuapp.com/me", formMe))
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
            public List<InventoryItem> inventory;
            public List<WalletItem> wallet;
        }

        [Serializable]
        public class InventoryItem
        {
            public int id;
            public int amount;
            public string createdAt;
            public string updatedAt;
            public InventoryTradableItem tradable;
        }

        [Serializable]
        public class InventoryTradableItem
        {
            public int id;
            public string name;
            public string type;
            public bool promo;
            public bool donat;
            public string imageUrl;
            public string description;
            public string discount;
            public string specialOfferLabel;
            public List<int> itemPack;
            public int? currencyId;
            public int? currencyAmount;
            public int? limit;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
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
        public class PriceItem
        {
            public int currencyId;
            public int amount;
        }

        [Serializable]
        public class TradableItem
        {
            public int id;
            public string name;
            public string type;
            public bool promo;
            public bool donat;
            public string imageUrl;
            public string description;
            public List<PriceItem> price;
            public string discount;
            public string specialOfferLabel;
            public List<int> itemPack;
            public int? currencyId;
            public int? currencyAmount;
            public int? limit;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
            public int currentCount;
            public bool soldOut;
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
            public List<TradableItem> tradable;
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
            public string internalFormat;
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

        //event-chapters
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
            public string mapBackgroundImage;
            public int? chapterMapId;
            public int eventId;
            public int? nextChapter;
            public List<int> stages;
            public string status;
        }

        // /events
        public static async Task<List<EventItem>> eventsAsync()
        {
            using (var request = await HttpCore.GetAsync("https://overlewd-api.herokuapp.com/events", tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventItem>>(request?.downloadHandler.text);
            }
        }

        public class EventType
        {
            public const string Quarterly = "quarterly";
            public const string Monthly = "monthly";
            public const string Weekly = "weekly";
        }

        [Serializable]
        public class EventItem
        {
            public int id;
            public string type;
            public string name;
            public string description;
            public string dateStart;
            public string dateEnd;
            public string backgroundUrl;
            public List<int> currencies;
            public string mapBackgroundImage;
            public string mapBannerImage;
            public string eventListBannerImage;
            public string bannerOverlayText;
            public List<int> markets;
            public List<int> quests;
            public int? buyLimit;
            public string createdAt;
            public string updatedAt;
            public List<int> stages;
            public List<int> chapters;
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

        // //event-stages/{id}/start
        public static async Task<EventStageItem> eventStageStartAsync(int eventStageId)
        {
            var form = new WWWForm();
            var url = $"https://overlewd-api.herokuapp.com/event-stages/{eventStageId}/start";
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<EventStageItem>(request?.downloadHandler.text);
            }
        }

        // /event-stages/{id}/end
        public static async Task<EventStageItem> eventStageEndAsync(int eventStageId)
        {
            var form = new WWWForm();
            var url = $"https://overlewd-api.herokuapp.com/event-stages/{eventStageId}/end";
            using (var request = await HttpCore.PostAsync(url, form, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<EventStageItem>(request?.downloadHandler.text);
            }
        }

        public class EventStageType
        {
            public const string Battle = "battle";
            public const string Dialog = "dialog";
        }

        public class EventStageStatus
        {
            public const string Open = "open";
            public const string Started = "started";
            public const string Complete = "complete";
            public const string Closed =  "closed";
        }

        [Serializable]
        public class EventStageItem
        {
            public int index;
            public int id;
            public string title;
            public string type;
            public int? dialogId;
            public int? battleId;
            public string eventMapNodeName;
            public List<int> nextStages;
            public string status;
        }

        // /event-quests
        public static async Task<List<EventQuestItem>> eventQuestsAsync()
        {
            var url = "https://overlewd-api.herokuapp.com/event-quests";
            using (var request = await HttpCore.GetAsync(url, tokens?.accessToken))
            {
                return JsonHelper.DeserializeObject<List<EventQuestItem>>(request?.downloadHandler.text);
            }
        }

        [Serializable]
        public class EventQuestReward
        {
            public int currencyId;
            public int amount;
        }

        public class EventQuestType
        {
            public const string StandartHunt = "standart_hunt";
            public const string QuickQuest = "quick_quest";
            public const string UniversalAdventure = "universal_adventure";
        }

        [Serializable]
        public class EventQuestItem
        {
            public int id;
            public string name;
            public string type;
            public string description;
            public int? goalCount;
            public List<EventQuestReward> rewards;
            public string status;
            public int progressCount;
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

        public class DialogCharacterPosition 
        {
            public const string Left = "left";
            public const string Right = "right";
            public const string Middle = "middle";
        }

        public class DialogCharacterName
        {
            public const string Overlord = "Overlord";
            public const string Ulvi = "Ulvi";
            public const string Faye = "Faye";
            public const string Adriel = "Adriel";
        }

        public class DialogCharacterSkin
        {
            public const string Overlord = "Overlord";
            public const string Ulvi = "Ulvi";
            public const string UlviWolf = "UlviWolf";
            public const string Faye = "Faye";
            public const string Adriel = "Adriel";
        }

        public class DialogCharacterAnimation
        {
            public const string Idle = "idle";
            public const string Surprised = "surprised";
            public const string Happy = "happy";
            public const string Love = "love";
            public const string Angry = "angry";
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

            public string cutInSoundPath;
            public string mainSoundPath;
        }

        public class DialogType 
        {
            public const string Dialog = "dialog";
            public const string Sex = "sex";
            public const string Notification = "notification";
        }

        [Serializable]
        public class Dialog
        {
            public int id;
            public string title;
            public string type;
            public List<DialogReplica> replicas;
            public string status;
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
        public class BattleReward
        {
            public int currencyId;
            public int amount;
        }

        public class BattleType
        {
            public const string Battle = "battle";
            public const string Boss = "boss";
        }

        [Serializable]
        public class Battle
        {
            public int id;
            public string title;
            public string type;
            public List<BattleReward> rewards;
            public List<BattleReward> firstRewards;
            public string status;
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
        public class FTUEInfo
        {
            public List<FTUEChapter> chapters;
        }

        [Serializable]
        public class FTUEChapter
        {
            public int id;
            public string name;
            public int? chapterMapId;
            public List<FTUEStageInfo> dialogs;
            public List<FTUEStageInfo> battles;
        }

        [Serializable]
        public class FTUEStageInfo
        {
            public int id;
            public string key;
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
    }
}
