namespace Nutaku.Unity
{
    public delegate void PaymentResultDelegate(WebViewEvent resultDelegate);

    /// <summary>
    /// Native platform interface for Android
    /// </summary>
    interface INativePlugin
    {
        /// <summary>
        /// Get the Login information
        /// </summary>
        LoginInfo loginInfo { get; }

        /// <summary>
        /// Get the SDK settings
        /// </summary>
        SdkSettings settings { get; }

        /// <summary>
        /// Get endpoint information for opensocial (OSAPI)
        /// </summary>
        ApiInfo OsapiInfo { get; }

        /// <summary>
        /// Get the endpoint information for mobileapi
        /// </summary>
        ApiInfo MobileApiInfo { get; }

        /// <summary>
        /// Open item purchase page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        void OpenPayment(string url, PaymentResultDelegate paymentResultDelegate);

        /// <summary>
        /// Logout and Exit Option
        /// </summary>
        void logoutAndExit();
    }
}
