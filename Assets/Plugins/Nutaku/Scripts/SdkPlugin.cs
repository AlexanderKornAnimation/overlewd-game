using System;

namespace Nutaku.Unity
{
    public static class SdkPlugin
    {
        public const string sdkVersion = "3.0.0";
        public static void Initialize()
        {
            InitializeIfNeeded();
        }

		public static void OpenPaymentView(Payment payment, PaymentResultDelegate paymentResultDelegate)
        {
            if (payment == null)
                throw new ArgumentNullException("payment");
            if (string.IsNullOrEmpty(payment.transactionUrl))
                throw new ArgumentNullException("payment.transactionUrl");

            nativeBridge.OpenPayment(payment.transactionUrl, paymentResultDelegate);
        }

		public static void logoutAndExit()
		{
			nativeBridge.logoutAndExit();
		}


        /// <summary>
		/// Login information obtained from native bridge
        /// </summary>
        public static LoginInfo _loginInfo;

        /// <summary>
		/// Login information
        /// </summary>
        public static LoginInfo loginInfo
        {
            get
            {
                if (_loginInfo == null)
                    _loginInfo = nativeBridge.loginInfo;
                return _loginInfo;
            }

            private set
            {
                _loginInfo = value;
            }
        }

        internal static void UpdateSecurityToken(SecurityToken securityToken)
        {
            _loginInfo = new LoginInfo(loginInfo.userId, loginInfo.accessToken, securityToken);
        }

        /// <summary>
		/// SDK setting obtained from native bridge
        /// </summary>
        static SdkSettings _settings;

        /// <summary>
		/// Get the SdkSettings
        /// </summary>
        public static SdkSettings settings
        {
            get
            {
                if (_settings == null)
                    _settings = nativeBridge.settings;

                return _settings;
            }
        }

        /// <summary>
		/// Get the social API path information
        /// </summary>
        internal static ApiInfo socialApiInfo
        {
            get
            {
                return nativeBridge.OsapiInfo;
            }
        }

        /// <summary>
		/// Acquire the path information of the core API
        /// </summary>
        internal static ApiInfo coreApiInfo
        {
            get
            {
                return nativeBridge.MobileApiInfo;
            }
        }

        internal static INativePlugin nativeBridge
        {
            get
            {
#if UNITY_EDITOR
                return NativePluginStub.instance;
#elif UNITY_ANDROID
                return AndroidNativePlugin.instance;
#else
                throw new InvalidOperationException("Nutaku Unity SDK supports only Android device or Unity Editor.");
#endif
            }
        }

        static bool _initialized = false;

        static void InitializeIfNeeded()
        {
            if (!_initialized)
            {
                JsonMapperSettings.Initialize();
#if UNITY_EDITOR
                NativePluginStub.Initialize();
#elif UNITY_ANDROID
                AndroidNativePlugin.Initialize();
#endif
            }
            _initialized = true;
        }
    }
}
