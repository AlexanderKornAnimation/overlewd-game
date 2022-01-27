using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class AdminBRO
    {
        private static T DeserializeObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        Debug.LogError($"Deserealize entity \"{typeof(T).FullName}\" error:  {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Error,
                });
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogError($"Deserealize Error: {ex.Message}");
                return default(T);
            }
        }

        private static bool RequestCheckError(UnityWebRequest request)
        {
            if (request.result != UnityWebRequest.Result.ProtocolError &&
                request.result != UnityWebRequest.Result.ConnectionError)
            {
                return false;
            }
            return true;
        }

        public static string GetDeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        // auth/login; auth/refresh
        public static async Task<Tokens> authLoginAsync()
        {
            var postData = new WWWForm();
            postData.AddField("deviceId", GetDeviceId());
            using (var request = await NetworkHelper.PostAsync("https://overlude-api.herokuapp.com/auth/login", postData))
            {
                if (!RequestCheckError(request))
                {
                    tokens = DeserializeObject<Tokens>(request.downloadHandler.text);
                    return tokens;
                }
                return null;
            }
        }

        public static async Task<Tokens> authRefreshAsync()
        {
            var postData = new WWWForm();
            using (var request = await NetworkHelper.PostAsync("https://overlude-api.herokuapp.com/auth/refresh", postData))
            {
                if (!RequestCheckError(request))
                {
                    tokens = DeserializeObject<Tokens>(request.downloadHandler.text);
                    return tokens;
                }
                return null;
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
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/me", tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<PlayerInfo>(request.downloadHandler.text);
                }
                return null;   
            }
        }

        public static async Task<PlayerInfo> meAsync(string name)
        {
            var formMe = new WWWForm();
            formMe.AddField("name", name);
            using (var request = await NetworkHelper.PostAsync("https://overlude-api.herokuapp.com/me", formMe))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<PlayerInfo>(request.downloadHandler.text);
                }
                return null;
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
            var url = "https://overlude-api.herokuapp.com/markets";
            using (var request = await NetworkHelper.GetAsync(url, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<EventMarketItem>>(request.downloadHandler.text);
                }
                return null;
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
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/currencies", tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<CurrencyItem>>(request.downloadHandler.text);
                }
                return null;
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
            var url = $"https://overlude-api.herokuapp.com/markets/{marketId}/tradable/{tradableId}/buy";
            using (var request = await NetworkHelper.PostAsync(url, form, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<TradableBuyStatus>(request.downloadHandler.text);
                }
                return null;
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
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/resources", tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<ResourcesMetaResponse>(request.downloadHandler.text).items;
                }
                return null;
            }
        }

        [Serializable]
        public class NetworkResource
        {
            public string id;
            public string type;
            public string internalFormat;
            public string hash;
            public int size;
            public string url;
        }

        [Serializable]
        public class ResourcesMetaResponse
        {
            public List<NetworkResource> items;
        }

        // /events
        public static async Task<List<EventItem>> eventsAsync()
        {
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/events", tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<EventItem>>(request.downloadHandler.text);
                }
                return null;
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
        }

        // /event-stages
        public static async Task<List<EventStageItem>> eventStagesAsync()
        {
            var url = "https://overlude-api.herokuapp.com/event-stages";
            using (var request = await NetworkHelper.GetAsync(url, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<EventStageItem>>(request.downloadHandler.text);
                }
                return null;
            }
        }

        // /event-stages/reset
        public static async Task eventStagesResetAsync()
        {
            var form = new WWWForm();
            var url = "https://overlude-api.herokuapp.com/event-stages/reset";
            using (var request = await NetworkHelper.PostAsync(url, form, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    
                }
            }
        }

        // //event-stages/{id}/start
        public static async Task<EventStageItem> eventStageStartAsync(int eventStageId)
        {
            var form = new WWWForm();
            var url = $"https://overlude-api.herokuapp.com/event-stages/{eventStageId}/start";
            using (var request = await NetworkHelper.PostAsync(url, form, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<EventStageItem>(request.downloadHandler.text);
                }
                return null;
            }
        }

        // /event-stages/{id}/end
        public static async Task<EventStageItem> eventStageEndAsync(int eventStageId)
        {
            var form = new WWWForm();
            var url = $"https://overlude-api.herokuapp.com/event-stages/{eventStageId}/end";
            using (var request = await NetworkHelper.PostAsync(url, form, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<EventStageItem>(request.downloadHandler.text);
                }
                return null;
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
            var url = "https://overlude-api.herokuapp.com/event-quests";
            using (var request = await NetworkHelper.GetAsync(url, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<EventQuestItem>>(request.downloadHandler.text);
                }
                return null;
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
            var url = String.Format("https://overlude-api.herokuapp.com/i18n?locale={0}", locale);
            using (var request = await NetworkHelper.GetAsync(url, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<LocalizationItem>>(request.downloadHandler.text);
                }
                return null;                
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
            var url = "https://overlude-api.herokuapp.com/dialogs";
            using (var request = await NetworkHelper.GetAsync(url, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<Dialog>>(request.downloadHandler.text);
                }
                return null;
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
            public const string Wolf = "wolf";
        }

        public class DialogCharacterKey
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
            public string cutIn;
            public string message;
            public string animation;
            public string sceneOverlayImage;

            public string replicaSoundId;
            public string characterKey
            {
                get
                {
                    return characterName switch
                    {
                        DialogCharacterName.Overlord => characterSkin switch
                        {
                            _ => DialogCharacterKey.Overlord
                        },

                        DialogCharacterName.Ulvi => characterSkin switch
                        {
                            DialogCharacterSkin.Wolf => DialogCharacterKey.UlviWolf,
                            _ => DialogCharacterKey.Ulvi
                        },

                        DialogCharacterName.Faye => characterSkin switch
                        {
                            _ => DialogCharacterKey.Faye
                        },

                        DialogCharacterName.Adriel => characterSkin switch
                        {
                            _ => DialogCharacterKey.Adriel
                        },

                        _ => characterName
                    };
                }
            }
        }

        public class DialogType 
        {
            public const string Dialog = "dialog";
            public const string Sex = "sex";
        }

        [Serializable]
        public class Dialog
        {
            public int id;
            public string title;
            public string type;
            public List<DialogReplica> replicas;
        }

        // /battles
        public static async Task<List<Battle>> battlesAsync()
        {
            var url = "https://overlude-api.herokuapp.com/battles";
            using (var request = await NetworkHelper.GetAsync(url, tokens?.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return DeserializeObject<List<Battle>>(request.downloadHandler.text);
                }
                return null;
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
        }
    }
}
