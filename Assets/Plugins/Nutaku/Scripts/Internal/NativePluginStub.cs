#if UNITY_EDITOR
using UnityEngine;
using System;

namespace Nutaku.Unity
{
    /// <summary>
	/// Stub of the NativePlugin for running the SDK in Unity Editor.
    /// </summary>
    class NativePluginStub : INativePlugin
    {
        // You must configure the following 3 fields with your sandbox-specific values when running in Unity mode. When running on an android device, use the AndroidManifest meta-data instead.
        const string SandboxAppId = "83344";
        const string SandboxConsumerKey = "N0kKXEWoTxlSNFIh";
        const string SandboxConsumerSecret = "G#dxjtVepvPaV?8v3b$PJV6qda@WKJ5[";
        
        const int VersionCode = 1;
        const string MobileApiEndpoint = "https://sbox-mobileapi.nutaku.com/";
        const string OsapiEndpoint = "https://sbox-osapi.nutaku.com/social_android/rest/";

        SdkSettings _sdkSettings;
        public LoginInfo _loginInfo;
        ApiInfo _osapiInfo;
        ApiInfo _mobileApiInfo;
        bool _initialized = false;

        internal static NativePluginStub _instance = new NativePluginStub();

        /// <summary>
		/// Get a NativePluginStub instance.
        /// </summary>
        internal static NativePluginStub instance
        {
            get
            {
                return _instance;
            }
        }

        internal static void Initialize()
        {
            _instance.ReadSdkSettings();
            _instance._loginInfo = SandboxLoginView.Login();
			_instance._initialized = true;
        }

        public LoginInfo loginInfo
        {
            get
            {
                if (!_initialized)
                {
                    throw new InitializationException("Initialization has not been completed yet");
                }
                return _loginInfo;
            }
        }

        public SdkSettings settings
        {
            get
            {
                if (_sdkSettings == null)
                {
                    ReadSdkSettings();
                }

                return _sdkSettings;
            }
        }

        public ApiInfo OsapiInfo
        {
            get
            {
                if (_osapiInfo == null)
                {
                    var uri = new Uri(OsapiEndpoint);
                    _osapiInfo = new ApiInfo(uri.Scheme, uri.Host, uri.AbsolutePath);
                }
                return _osapiInfo;
            }
        }

        public ApiInfo MobileApiInfo
        {
            get
            {
                if (_mobileApiInfo == null)
                {
                    var uri = new Uri(MobileApiEndpoint);
                    _mobileApiInfo = new ApiInfo(uri.Scheme, uri.Host, uri.AbsolutePath);
                }
                return _mobileApiInfo;
            }
        }

		public void OpenPayment(string url, PaymentResultDelegate paymentDelegate)
        {
			const string message = "OpenPayment is not available in Unity Editor. You need to run on Android.";
			Debug.LogError(message);
			var result = new WebViewEvent()
			{
				kind = WebViewEventKind.Failed,
				message = message,
			};
			paymentDelegate (result);
        }

		public void logoutAndExit()
		{
			Debug.LogError("LogoutAndExit is not available in Unity Editor. You need to be running on a device.");
		}

        void ReadSdkSettings()
        {
            _sdkSettings = new SdkSettings(
                SandboxAppId,
                "sandbox",
                true,
                VersionCode,
                SandboxConsumerKey,
                SandboxConsumerSecret);
        }
    }
}
#endif
