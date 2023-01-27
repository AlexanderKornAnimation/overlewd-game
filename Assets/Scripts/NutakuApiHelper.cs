using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using Nutaku.Unity;
using System.Net;
using System.Text;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public static class NutakuApiHelper
    {
        public static void Initialize()
        {
#if UNITY_EDITOR || UNITY_ANDROID
            SdkPlugin.Initialize();
#endif
        }

        public static bool loggedIn =>
            !String.IsNullOrEmpty(loginInfo?.userId);
        public static LoginInfo loginInfo =>
#if UNITY_EDITOR || UNITY_ANDROID
            SdkPlugin.loginInfo;
#else
            null;
#endif
        public static Person userInfo { get; private set; }

        public static async Task WaitLoggedIn(MonoBehaviour myMonoBehaviour)
        {
#if UNITY_EDITOR || UNITY_ANDROID
            await UniTask.WaitUntil(() => loggedIn);
            userInfo = await GetMyProfileAsync(myMonoBehaviour);
#else
            await Task.CompletedTask;
#endif
        }

        public static void LogOut()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            SdkPlugin.logoutAndExit();
#elif UNITY_EDITOR
            PlayerPrefs.DeleteKey(SandboxLoginView.PrefKeyAutologinToken);
#endif
        }

        private static async Task<BaseSystemNotif.State> ErrorHandling(string errorMsg)
        {
            var errorNotif = UIManager.MakeSystemNotif<ServerErrorNotif>();
            errorNotif.message = errorMsg;
            var state = await errorNotif.WaitChangeState();
            await errorNotif.CloseAsync();
            return state;
        }

        public class NutakuPayment
        {
            public Payment payment;
            public WebViewEvent webViewResult;
            public Exception exception;
            public RawResult rawResult;
            public string message;

            public bool isValid => payment != null;
            public string rawResultMsg => 
                $"Http Status Code: {rawResult.statusCode}\n" +
                $"Http Status Message: {Encoding.UTF8.GetString(rawResult.body)}";
        }

        public static async Task<NutakuPayment> PaymentAsync(MonoBehaviour myMonoBehaviour, AdminBRO.TradableItem tradable)
        {
            if (!loggedIn)
            {
                var errMsg = "Nutaku user in not loggen in";
                logMessage(errMsg);
                return new NutakuPayment { message = errMsg };
            }

            if (!tradable?.nutakuPriceValid ?? true)
            {
                var errMsg = "Nutaku tradable data is not valid";
                logMessage(errMsg);
                return new NutakuPayment { message = errMsg };
            }

            while (true)
            {
                var paymentRequest = new UnityWebRequest("Nutaku payment");
                BeginLoading(paymentRequest);
                try
                {
                    Payment payment = new Payment();
                    logMessage("");
                    logMessage("PostPayment Start");

                    //This is just a documented shortcut available on sandbox
                    //payment.callbackUrl = "https://skip.payment.handler";
                    //payment.finishPageUrl = "http://www.nutaku.net/";
                    //payment.message = "Test Payment";
                    payment.callbackUrl = GameData.nutaku.settings.callbackUrl;
                    payment.finishPageUrl = GameData.nutaku.settings.completeUrl;
                    payment.message = tradable.name;

                    PaymentItem item = new PaymentItem
                    {
                        itemId = tradable.id.ToString(),
                        itemName = tradable.name,
                        unitPrice = tradable.price.First().amount,
                        imageUrl = GameData.resources.GetById(tradable.imageUrl)?.url,// "https://dogecoin.com/imgs/dogecoin-300.png",
                        description = tradable.description
                    };
                    payment.paymentItems.Add(item);

                    bool ppComplete = false;
                    NutakuPayment resultPayment = null;
                    RestApiHelper.PostPayment(payment, myMonoBehaviour, (RawResult rawResult) =>
                    {
                        resultPayment = PostPaymentRawEncode(rawResult);
                        ppComplete = true;
                    });
                    await UniTask.WaitUntil(() => ppComplete);
                    EndLoading(paymentRequest);

                    if (resultPayment.isValid)
                    {
                        bool opComplete = false;
                        SdkPlugin.OpenPaymentView(resultPayment.payment, (WebViewEvent result) =>
                        {
                            resultPayment.webViewResult = result;
                            opComplete = true;
                        });
                        await UniTask.WaitUntil(() => opComplete);

                        switch (resultPayment.webViewResult.kind)
                        {
                            case WebViewEventKind.Succeeded:
                                await GameData.player.Get();
                                UIManager.ThrowGameDataEvent(new GameDataEvent
                                {
                                    id = GameDataEventId.NutakuPayment
                                });
                                return resultPayment;
                            case WebViewEventKind.Failed:
                                resultPayment.message = "Web View Failed";
                                return resultPayment;
                            case WebViewEventKind.Cancelled:
                                resultPayment.message = "Web View Cancelled";
                                return resultPayment;
                        }
                    }

                    var errMsg = "PostPayment Failure\n" + resultPayment.rawResultMsg;
                    var errNotifState = await ErrorHandling(errMsg);
                    switch (errNotifState)
                    {
                        case BaseSystemNotif.State.Cancel:
                            return resultPayment;
                    }
                }
                catch (Exception ex)
                {
                    logError("Payment Failure");
                    DumpError(ex);
                    EndLoading(paymentRequest);

                    var errNotifState = await ErrorHandling(ex.Message);
                    switch (errNotifState)
                    {
                        case BaseSystemNotif.State.Cancel:
                            return new NutakuPayment { exception = ex };
                    }
                }
            }
        }

        private static NutakuPayment PostPaymentRawEncode(RawResult rawResult)
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                Payment payment = new Payment();
                var result = RestApi.HandleRequestCallback<Payment>(rawResult);

                logMessage("PostPayment Success");
                logMessage("Http Status Code: " + result.statusCode);

                payment = result.GetFirstEntry();
                OutputPaymentData(payment);
                return new NutakuPayment { payment = payment, rawResult = rawResult };
            }

            logError("PostPayment Failure");
            logError("Http Status Code: " + (int)rawResult.statusCode);
            logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            return new NutakuPayment { rawResult = rawResult };
        }

        public static async Task<Person> GetMyProfileAsync(MonoBehaviour myMonoBehaviour)
        {
            BeginLoading();
            try
            {
                logMessage("");
                logMessage("GetMyProfile Start");
                logMessage(string.Format("Get Your(ID:{0}) Profile", SdkPlugin.loginInfo.userId));
                
                bool rComplete = false;
                Person profile = null;
                RestApiHelper.GetMyProfile(myMonoBehaviour, (RawResult rawResult) =>
                {
                    profile = GetMyProfileCallback(rawResult);
                    rComplete = true;
                });
                await UniTask.WaitUntil(() => rComplete);
                return profile;
            }
            catch (Exception ex)
            {
                logError("GetMyProfile Failure");
                DumpError(ex);
                EndLoading();
                return default;
            }
        }

        private static Person GetMyProfileCallback(RawResult rawResult)
        {
            try
            {
                if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
                {
                    var result = RestApi.HandleRequestCallback<Person>(rawResult);
                    logMessage("GetMyProfile Success");
                    logMessage("Http Status Code: " + result.statusCode);
                    var profile = result.GetFirstEntry();
                    OutputPersonData(profile);
                    return profile;
                }
                else
                {
                    logError("GetMyProfile Failure");
                    logError("Http Status Code: " + (int)rawResult.statusCode);
                    logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
                    return default;
                }
            }
            catch (Exception ex)
            {
                logError("GetMyProfile Failure");
                DumpError(ex);
                return default;
            }
            finally
            {
                EndLoading();
            }
        }

        private static void OutputPaymentData(Payment payment)
        {
            logMessage("paymentId: " + payment.paymentId);
            logMessage("status: " + payment.status);
            logMessage("transactionUrl: " + payment.transactionUrl);
            logMessage("orderedTime: " + payment.orderedTime);
            logMessage("executedTime: " + payment.executedTime);

            foreach (var items in payment.paymentItems)
            {
                logMessage("\titemId: " + items.itemId);
                logMessage("\titemName: " + items.itemName);
                logMessage("\tunitPrice: " + items.unitPrice);
                logMessage("\timageUrl: " + items.imageUrl);
                logMessage("\tdescription: " + items.description);
            }
        }

        private static void OutputPersonData(Person person)
        {
            logMessage("id: " + person.id);
            logMessage("nickname: " + person.nickname);
            logMessage("profileUrl: " + person.profileUrl);
            logMessage("thumbnailUrl: " + person.thumbnailUrl);
            logMessage("thumbnailUrlSmall: " + person.thumbnailUrlSmall);
            logMessage("thumbnailUrlLarge: " + person.thumbnailUrlLarge);
            logMessage("thumbnailUrlHuge: " + person.thumbnailUrlHuge);
            logMessage("birthday: " + person.birthday);
            logMessage("gender: " + person.gender);
            logMessage("age: " + person.age);
            logMessage("addresses (country): " + person.addresses.country);
            logMessage("hasApp: " + person.hasApp);
            logMessage("userType: " + person.userType);
            logMessage("grade: " + person.grade);
            logMessage("languagesSpoken: " + person.languagesSpoken);
        }

        private static void DumpError(Exception ex)
        {
            var webEx = ex as WebException;
            if (webEx != null)
            {
                var response = webEx.Response as HttpWebResponse;
                if (response != null)
                {
                    logError("Http Status Code: " + (int)response.StatusCode);
                }
            }
            logException(ex);
        }

        private static void BeginLoading(UnityWebRequest request = null)
        {
            if (request == null)
                return;
            HttpCore.PushRequest(request);
            UIManager.PushUserInputLocker(new UserInputLocker(request));
        }

        private static void EndLoading(UnityWebRequest request = null)
        {
            if (request == null)
                return;
            HttpCore.PopRequest(request);
            UIManager.PopUserInputLocker(new UserInputLocker(request));
        }

        private static void logMessage(string message, LogType logType = LogType.Log)
        {
            switch (logType)
            {
                case LogType.Exception:
                    Debug.LogException(new Exception(message));
                    break;

                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;

                case LogType.Error:
                    Debug.LogError(message);
                    break;

                case LogType.Assert:
                    Debug.LogAssertion(message);
                    break;

                default:
                    Debug.Log(message);
                    break;
            }
        }

        private static void logError(string message)
        {
            logMessage("Error: " + message, LogType.Error);
        }

        private static void logException(Exception ex)
        {
            if (ex.StackTrace == null)
                logMessage("Exception: " + ex.Message, LogType.Exception);
            else
                logMessage("Exception: " + ex.Message + ".\r\nStack trace: " + ex.StackTrace, LogType.Exception);
        }
    }
}