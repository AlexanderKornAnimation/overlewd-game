using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class AdminBRO
    {
        // auth/login; auth/refresh
        public static IEnumerator auth_login(Action success, Action error = null)
        {
            var postData = new WWWForm();
            postData.AddField("deviceId", GetDeviceId());
            yield return NetworkHelper.Post("https://overlude-api.herokuapp.com/auth/login", postData, downloadHandler =>
            {
                tokens = JsonUtility.FromJson<Tokens>(downloadHandler.text);
                success?.Invoke();
            },
            (msg) =>
            {
                error?.Invoke();
            });
        }

        public static IEnumerator auth_refresh(Action success, Action error = null)
        {
            var postData = new WWWForm();
            yield return NetworkHelper.Post("https://overlude-api.herokuapp.com/auth/refresh", postData, downloadHandler =>
            {
                tokens = JsonUtility.FromJson<Tokens>(downloadHandler.text);
                success?.Invoke();
            },
            (msg) =>
            {
                error?.Invoke();
            });
        }

        public static string GetDeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        [Serializable]
        public class Tokens
        {
            public string accessToken;
            public string refreshToken;
        }

        public static Tokens tokens;

        // GET /me; POST /me
        public static IEnumerator me(Action<PlayerInfo> success, Action error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/me", tokens.accessToken, downloadHandler =>
            {
                var playerInfo = JsonUtility.FromJson<PlayerInfo>(downloadHandler.text);
                success?.Invoke(playerInfo);
            },
            (msg) => 
            {
                error?.Invoke();
            });
        }

        public static IEnumerator me(string name, Action<PlayerInfo> success, Action error = null)
        {
            var formMe = new WWWForm();
            formMe.AddField("name", name);
            yield return NetworkHelper.PostWithToken("https://overlude-api.herokuapp.com/me", formMe, tokens.accessToken, downloadHandler =>
            {
                var playerInfo = JsonUtility.FromJson<PlayerInfo>(downloadHandler.text);
                success?.Invoke(playerInfo);
            }, 
            (msg) => 
            {
                error?.Invoke();
            });
        }

        [Serializable]
        public class PlayerInfo
        {
            public int id;
            public string name;
        }


        // /markets; /markets/{id}
        public static IEnumerator markets(Action<ShortMarketsData> success, Action error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/markets", tokens.accessToken, (downloadHandler) =>
            {
                var shortItemsJson = "{ \"items\" : " + downloadHandler.text + " }";
                var shortDataItems = JsonUtility.FromJson<ShortMarketsData>(shortItemsJson);
                success?.Invoke(shortDataItems);
            },
            (errorMsg) => {
                error?.Invoke();
            });
        }

        public static IEnumerator markets(int id, Action<FullMarketsData> success, Action error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/markets/" + id.ToString(),
            tokens.accessToken, (downloadHandler) => {
                var fullItemsJson = "{ \"items\" : " + downloadHandler.text + " }";
                var fullItems = JsonUtility.FromJson<FullMarketsData>(fullItemsJson);
                success?.Invoke(fullItems);
            },
            (errorMsg) =>
            {
                error?.Invoke();
            });
        }

        [Serializable]
        public class ShortMarketData
        {
            public int id;
            public string name;
            public string backgroundUrl;
            public string bannerUrl;
            public string dateStart;
            public string dateEnd;
            public string createdAt;
            public string updatedAt;
        }

        [Serializable]
        public class ShortMarketsData
        {
            public List<ShortMarketData> items;
        }

        [Serializable]
        public class PriceItem
        {
            public int id;
            public string name;
            public string iconUrl;
            public string createdAt;
            public string updatedAt;
            public int count;
        }

        [Serializable]
        public class FullMarketData
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
        public class FullMarketsData
        {
            public List<FullMarketData> items;
        }

        // /currencies
        public static IEnumerator currencies(Action<Currenies> success, Action error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/currencies", tokens.accessToken, (downloadHandler) =>
            {
                var currenciesJson = "{ \"items\" : " + downloadHandler.text + " }";
                var currencies = JsonUtility.FromJson<Currenies>(currenciesJson);
                success?.Invoke(currencies);
            },
            (errorMsg) => 
            {
                error?.Invoke();
            });
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

        [Serializable]
        public class Currenies
        {
            public List<CurrencyItem> items;
        }

        // /resources
        public static IEnumerator resources(Action<ResourcesMeta> success, Action<string> error = null)
        {
            yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/resources", tokens.accessToken, downloadHandler =>
            {
                var resources = JsonUtility.FromJson<ResourcesMeta>(downloadHandler.text);
                success?.Invoke(resources);
            },
            (msg) => 
            {
                error?.Invoke(msg);
            });
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
        public class ResourcesMeta
        {
            public List<NetworkResource> items;
        }
    }

}
