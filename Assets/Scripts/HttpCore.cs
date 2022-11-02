using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class HttpCore
    {
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
            request.timeout = 20;
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
            UIManager.PeakSystemNotif();
            return state;
        }

        public static async Task<UnityWebRequest> GetAsync(string url, bool lockUserInput = true)
        {
            while (true)
            {
                try
                {
                    var request = UnityWebRequest.Get(url);
                    return await Send(request, lockUserInput);
                }
                catch (UnityWebRequestException e)
                {
                    var errNotifState = await ExceptionHandling(e);
                    switch (errNotifState)
                    {
                        case BaseSystemNotif.State.Cancel:
                            return default;
                    }
                }
            }
        }

        
        public static async Task<UnityWebRequest> PostAsync(string url, WWWForm form, bool lockUserInput = true)
        {
            while (true)
            {
                try
                {
                    var request = UnityWebRequest.Post(url, form);
                    return await Send(request, lockUserInput);
                }
                catch (UnityWebRequestException e)
                {
                    var errNotifState = await ExceptionHandling(e);
                    switch (errNotifState)
                    {
                        case BaseSystemNotif.State.Cancel:
                            return default;
                    }
                }
            }
        }

        public static async Task<UnityWebRequest> DeleteAsync(string url, bool lockUserInput = true)
        {
            while (true)
            {
                try
                {
                    var request = UnityWebRequest.Delete(url);
                    return await Send(request, lockUserInput);
                }
                catch (UnityWebRequestException e)
                {
                    var errNotifState = await ExceptionHandling(e);
                    switch (errNotifState)
                    {
                        case BaseSystemNotif.State.Cancel:
                            return default;
                    }
                }
            }
        }
    }
}
