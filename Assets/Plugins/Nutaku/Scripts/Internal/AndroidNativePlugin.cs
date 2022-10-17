#if UNITY_ANDROID && !UNITY_EDITOR
using Nutaku.Unity.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nutaku.Unity
{
    /// <summary>
    /// A class that communicates with Android Native when running on a device
    /// </summary>
    class AndroidNativePlugin : MonoBehaviour, INativePlugin
    {
        static AndroidNativePlugin _instance;
        static bool _initialized = false;
        AndroidJavaObject _nativeBridge;
		Dictionary<string, PaymentResultDelegate> _paymentResultDelegates = new Dictionary<string, PaymentResultDelegate>();

        /// <summary>
        /// Get the AndroidNativePlugin instance.
        /// </summary>
        public static AndroidNativePlugin instance
        {
            get
            {
                if (!_initialized)
                {
                    throw new InvalidOperationException("AndroidNativePlugin is not initialized");
                }
                return _instance;
            }
        }

        public LoginInfo loginInfo { get; private set; }

        public SdkSettings settings { get; private set; }

        public ApiInfo OsapiInfo { get; private set; }

        public ApiInfo MobileApiInfo { get; private set; }

		public void OpenPayment(string url, PaymentResultDelegate paymentResultDelegate)
        {
			var subjectId = Guid.NewGuid().ToString("N");
			_paymentResultDelegates.Add(subjectId, paymentResultDelegate);

			try
			{
				_nativeBridge.Call("openPayment", url, subjectId);
			}
			catch (Exception ex)
			{
				_paymentResultDelegates.Remove(subjectId);
				paymentResultDelegate (new WebViewEvent () { kind = WebViewEventKind.Failed, message = ex.Message });
			}
        }

		public void logoutAndExit()
		{
			_nativeBridge.Call("logoutAndExit");
		}

        internal static void Initialize()
        {
            var gameObject = new GameObject("Nutaku SDK Plugin Android Bridge");
            DontDestroyOnLoad(gameObject);
            var nativePlugin = gameObject.AddComponent<AndroidNativePlugin>();
            nativePlugin.SetUp();
            _instance = nativePlugin;
            _initialized = true;
        }

        AndroidNativePlugin()
        {
        }

        void SetUp()
        {
            _nativeBridge = new AndroidJavaObject("com.nutaku.unity.NativePlugin", gameObject.name);
            try
            {
                _nativeBridge.Call("setUp");
                loginInfo = JsonMapper.ToObject<LoginInfo>(_nativeBridge.Call<string>("getLoginInfoAsJson"));
                settings = JsonMapper.ToObject<SdkSettings>(_nativeBridge.Call<string>("getSdkSettingsAsJson"));
                OsapiInfo = JsonMapper.ToObject<ApiInfo>(_nativeBridge.Call<string>("getOsapiEndpointConfigAsJson"));
                MobileApiInfo = JsonMapper.ToObject<ApiInfo>(_nativeBridge.Call<string>("getMobileApiEndpointAsJson"));
            }
            catch (Exception ex)
            {
                _nativeBridge.Call("showUnitySetUpError", ex.Message);
            }
        }

        void OnDestroy()
        {
            if (_nativeBridge != null)
            {
                _nativeBridge.Call("tearDown");
                _instance = null;
                _initialized = false;
            }
        }

        class WebViewResult
        {
            public string subjectId = null;
			public WebViewEvent webViewEvent = new WebViewEvent();
        }

        void OnWebViewResult(string jsonFromAndroid)
        {
            var result = JsonMapper.ToObject<WebViewResult>(jsonFromAndroid);

            if (_paymentResultDelegates.ContainsKey(result.subjectId))
            {
				var paymentDelegate = _paymentResultDelegates[result.subjectId];
				_paymentResultDelegates.Remove(result.subjectId);
				paymentDelegate (result.webViewEvent);
            }
        }
    }
}
#endif
