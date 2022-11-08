using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public static class PlayFabSDKHelper
    {
        public static bool loggedIn => loginInfo != null;
        public static LoginResult loginInfo { get; private set; }

        public static async Task WaitLoggedIn()
        {
            if (!NutakuApiHelper.loggedIn)
                return;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = NutakuApiHelper.loginInfo.userId,
                CreateAccount = true
            };

            bool requestComplete = false;
            PlayFabClientAPI.LoginWithCustomID(
                request,
                (LoginResult info) =>
                {
                    Debug.Log("Congratulations, you made your first successful PlayFab API call!");
                    loginInfo = info;
                    requestComplete = true;
                },
                (PlayFabError error) =>
                {
                    Debug.LogWarning("Something went wrong with your first PlayFab API call.  :(");
                    Debug.LogError("Here's some debug information:");
                    Debug.LogError(error.GenerateErrorReport());
                    requestComplete = true;
                });

            await UniTask.WaitUntil(() => requestComplete);
        }
    }
}
