using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public class HttpCoreResponse
    {
        public string method { get; private set; }
        public UnityWebRequest.Result result { get; private set; }
        public long responseCode { get; private set; }
        public string error { get; private set; }
        public bool isDone { get; private set; }

        public byte[] data { get; private set; }
        public string text { get; private set; }

        public bool isSuccess => result == UnityWebRequest.Result.Success;

        private void _init(UnityWebRequest request)
        {
            method = request.method;
            result = request.result;
            responseCode = request.responseCode;
            error = request.error;
            isDone = request.isDone;

            data = request.downloadHandler.data;
            text = request.downloadHandler.text;
        }

        public virtual void Init(UnityWebRequest request)
        {
            _init(request);
        }

        public void Init(UnityWebRequestException e)
        {
            _init(e.UnityWebRequest);
        }
    }

    public class HttpCoreResponse<TData> : HttpCoreResponse
    {
        public TData dData { get; private set; }
        public override void Init(UnityWebRequest request)
        {
            base.Init(request);
            dData = JsonHelper.DeserializeObject<TData>(request.downloadHandler.text);
        }

        public static implicit operator TData(HttpCoreResponse<TData> param)
        {
            return param.dData;
        }
    }

    public static class HttpCore
    {
        private class RequestParams
        {
            private enum Type
            {
                GET,
                POST,
                DELETE
            }

            private Type type;
            private string url;
            private WWWForm formData;

            public UnityWebRequest Make() => type switch
            {
                Type.GET => UnityWebRequest.Get(url),
                Type.POST => UnityWebRequest.Post(url, formData),
                Type.DELETE => UnityWebRequest.Delete(url),
                _ => null
            };

            public static RequestParams InstGet(string url) =>
                new RequestParams { url = url, type = Type.GET };
            public static RequestParams InstPost(string url, WWWForm formData = null) =>
                new RequestParams { url = url, type = Type.POST, formData = formData ?? new WWWForm() };
            public static RequestParams InstDelete(string url) =>
                new RequestParams { url = url, type = Type.DELETE };
        }

        private static List<UnityWebRequest> activeRequests = new List<UnityWebRequest>();

        public static void PushRequest(UnityWebRequest request)
        {
            if (request != null)
            {
                if (!activeRequests.Contains(request))
                {
                    activeRequests.Add(request);
                    UIManager.ShowServerConnectionNotif();
                }
            }
        }

        public static void PopRequest(UnityWebRequest request)
        {
            if (request != null)
            {
                activeRequests.Remove(request);
                if (activeRequests.Count == 0)
                {
                    UIManager.HideServerConnectionNotif();
                }
            }
        }

        public static bool HasNetworkConection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        public static bool NetworkTypeMobile()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
        }
        public static bool NetworkTypeWIFI()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

        private static async Task<UnityWebRequest> Send(UnityWebRequest request, bool lockUserInput)
        {
            //request.timeout = 20;
            request.SetRequestHeader("Authorization", $"Bearer {AdminBRO.tokens?.accessToken}");
            request.SetRequestHeader("Version", AdminBRO.ApiVersion);

            PushRequest(request);
            if (lockUserInput)
            {
                UIManager.PushUserInputLocker(new UserInputLocker(request));
            }
            await request.SendWebRequest();
            PopRequest(request);
            UIManager.PopUserInputLocker(new UserInputLocker(request));

            return request;
        }

        private static async Task<BaseSystemNotif.State> ExceptionHandling(UnityWebRequestException e)
        {
            PopRequest(e.UnityWebRequest);
            UIManager.PopUserInputLocker(new UserInputLocker(e.UnityWebRequest));

            Debug.LogError(e.UnityWebRequest.url);
            Debug.LogError(e.Message);

            var errorNotif = UIManager.MakeSystemNotif<ServerErrorNotif>();
            errorNotif.message = $"{e.UnityWebRequest.url}\n{e.Message}";
            var state = await errorNotif.WaitChangeState();
            await errorNotif.CloseAsync();
            return state;
        }

        private static async Task<T> Send<T>(RequestParams requestParams, bool lockUserInput) where T : HttpCoreResponse, new()
        {
            while (true)
            {
                using var request = requestParams.Make();
                try
                {
                    await Send(request, lockUserInput);
                    var response = new T();
                    response.Init(request);
                    return response;
                }
                catch (UnityWebRequestException e)
                {
                    var errNotifState = await ExceptionHandling(e);
                    switch (errNotifState)
                    {
                        case BaseSystemNotif.State.Cancel:
                            var response = new T();
                            response.Init(e);
                            return response;
                    }
                }
            }
        }

        public static async Task<HttpCoreResponse> GetAsync(string url, bool lockUserInput = true) =>
            await Send<HttpCoreResponse>(RequestParams.InstGet(url), lockUserInput);
        public static async Task<HttpCoreResponse<TData>> GetAsync<TData>(string url, bool lockUserInput = true) =>
            await Send<HttpCoreResponse<TData>>(RequestParams.InstGet(url), lockUserInput);
        public static async Task<HttpCoreResponse> PostAsync(string url, WWWForm form = null, bool lockUserInput = true) =>
            await Send<HttpCoreResponse>(RequestParams.InstPost(url, form), lockUserInput);
        public static async Task<HttpCoreResponse<TData>> PostAsync<TData>(string url, WWWForm form = null, bool lockUserInput = true) =>
            await Send<HttpCoreResponse<TData>>(RequestParams.InstPost(url, form), lockUserInput);
        public static async Task<HttpCoreResponse> DeleteAsync(string url, bool lockUserInput = true) =>
            await Send<HttpCoreResponse>(RequestParams.InstDelete(url), lockUserInput);
        public static async Task<HttpCoreResponse<TData>> DeleteAsync<TData>(string url, bool lockUserInput = true) =>
            await Send<HttpCoreResponse<TData>>(RequestParams.InstDelete(url), lockUserInput);
    }
}
