using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class AdminBRO
    {
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
                    tokens = JsonConvert.DeserializeObject<Tokens>(request.downloadHandler.text);
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
                    tokens = JsonConvert.DeserializeObject<Tokens>(request.downloadHandler.text);
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
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/me", tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<PlayerInfo>(request.downloadHandler.text);
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
                    return JsonConvert.DeserializeObject<PlayerInfo>(request.downloadHandler.text);
                }
                return null;
            }
        }

        [Serializable]
        public class PlayerInfo
        {
            public int id;
            public string name;
            public List<InventoryItem> inventory;
            public List<WalletItem> wallet;
        }

        [Serializable]
        public class InventoryItem
        {
            public int id;
            public string createdAt;
            public string updatedAt;
            public TradableItem tradable;
        }

        [Serializable]
        public class TradableItem
        {
            public int id;
            public string name;
            public string imageUrl;
            public string description;
            public string discount;
            public string specialOfferLabel;
            public string itemPack;
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

        // /markets/{id}
        public static async Task<MarketItem> marketsAsync(int id)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/markets/{0}", id);
            using (var request = await NetworkHelper.GetAsync(url, tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<MarketItem>(request.downloadHandler.text);
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
        public class MarketProductItem
        {
            public int id;
            public string name;
            public string imageUrl;
            public string description;
            public List<PriceItem> price;
            public string discount;
            public string specialOfferLabel;
            public string itemPack;
            public string dateStart;
            public string dateEnd;
            public string discountStart;
            public string discountEnd;
            public string sortPriority;
        }

        [Serializable]
        public class MarketItem
        {
            public int id;
            public string name;
            public string description;
            public string bannerImage;
            public string createdAt;
            public string updatedAt;
            public List<MarketProductItem> tradable;
            public List<int> currencies;
        }

        // /currencies
        public static async Task<List<CurrencyItem>> currenciesAsync()
        {
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/currencies", tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<List<CurrencyItem>>(request.downloadHandler.text);
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
            public string createdAt;
            public string updatedAt;
        }

        // /resources
        public static async Task<List<NetworkResource>> resourcesAsync()
        {
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/resources", tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<ResourcesMetaResponse>(request.downloadHandler.text).items;
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

        // /tradable/{id}/buy
        public static async Task<TradableBuyStatus> tradableBuyAsync(int tradable_id)
        {
            var form = new WWWForm();
            var url = String.Format("https://overlude-api.herokuapp.com/tradable/{0}/buy", tradable_id);
            using (var request = await NetworkHelper.PostAsync(url, form, tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<TradableBuyStatus>(request.downloadHandler.text);
                }
                return null;
            }
        }

        [Serializable]
        public class TradableBuyStatus
        {
            public bool status;
        }

        // /events
        public static async Task<List<EventItem>> eventsAsync()
        {
            using (var request = await NetworkHelper.GetAsync("https://overlude-api.herokuapp.com/events", tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<List<EventItem>>(request.downloadHandler.text);
                }
                return null;
            }
        }

        public class EventStageType
        {
            public const string Battle = "battle";
            public const string Boss = "boss";
            public const string Dialog = "dialog";
            public const string SexDialog = "act";
        }

        [Serializable]
        public class EventStageDialogInfo
        {
            public int id;
            public string title;
        }

        [Serializable]
        public class EventStageBattleInfo
        {
            public int id;
            public string title;
            public int? gachaId;
        }

        [Serializable]
        public class EventStageItem
        {
            public int id;
            public string title;
            public string type;
            public List<int> nextStages;
            public EventStageDialogInfo dialog;
            public EventStageBattleInfo battle;
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
            public string mapBackgroundUrl;
            public List<int> markets;
            public List<int> quests;
            public string createdAt;
            public string updatedAt;
            public List<EventStageItem> stages;
        }

        // /events/{id}/quests
        public static async Task<List<QuestItem>> questsAsync(int eventId)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/events/{0}/quests", eventId);
            using (var request = await NetworkHelper.GetAsync(url, tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<List<QuestItem>>(request.downloadHandler.text);
                }
                return null;
            }
        }

        [Serializable]
        public class QuestItem
        {
            public int id;
            public string name;
            public string type;
            public string description;
            public int goalCount;
            public string reward;
            public string createdAt;
            public string updatedAt;
        }

        // /i18n
        public static async Task<List<LocalizationItem>> localizationAsync(string locale)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/i18n?locale={0}", locale);
            using (var request = await NetworkHelper.GetAsync(url, tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<List<LocalizationItem>>(request.downloadHandler.text);
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

        // /dialog/{id}
        public static async Task<Dialog> dialogAsync(int id)
        {
            var url = String.Format("https://overlude-api.herokuapp.com/dialog/{0}", id);
            using (var request = await NetworkHelper.GetAsync(url, tokens.accessToken))
            {
                if (!RequestCheckError(request))
                {
                    return JsonConvert.DeserializeObject<Dialog>(request.downloadHandler.text);
                }
                return null;
            }
        }

        [Serializable]
        public class DialogReplica
        {
            public int id;
            public int sort;
            public string characterName;
            public string message;
            public string sceneOverlayImage;
        }

        [Serializable]
        public class Dialog
        {
            public int id;
            public string title;
            public List<DialogReplica> replicas;
        }

    }
}
