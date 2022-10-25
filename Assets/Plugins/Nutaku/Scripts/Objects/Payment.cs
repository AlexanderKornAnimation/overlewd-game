using System;
using System.Collections.Generic;

namespace Nutaku.Unity
{
    /// <summary>
	/// Used with Payment API
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Payment ID
        /// </summary>
        public string paymentId;

        /// <summary>
        /// App ID
        /// </summary>
        public string appId;

        /// <summary>
        /// User ID
        /// </summary>
        public string userId;

        /// <summary>
        /// Payment status
        /// 1: new/created, 2: completed, 3: canceled, 4: expired
        /// </summary>
        public int? status;

        /// <summary>
		/// URL for settlement confirmation notice to partner (AKA payment handler)
        /// </summary>
        public string callbackUrl;

        /// <summary>
		/// Partner's URL for displaying the end of settlement screen. Not used for APK games, but should be set to an empty string.
        /// </summary>
        public string finishPageUrl = "";

        /// <summary>
		/// Nutaku URL to execute transaction of settlement (where the user confirms the payment)
        /// </summary>
        public string transactionUrl;

		/// <summary>
		/// The user's PlayFabID. You'll need to provide the value received from PlayFab LoginWithCustomId
		/// </summary>
		public string playFabId;

		/// <summary>
		/// PlayFab Session Ticket. You'll need to provide the value received from PlayFab LoginWithCustomId
		/// </summary>
		public string sessionTicket;

		/// <summary>
		/// The PlayFab Catalog Version where to look for the item
		/// </summary>
		public string catalogVersion;

		/// <summary>
		/// The PlayFab Store ID (within the specified CatalogVersion) where to look for the item
		/// </summary>
		public string storeId;

        /// <summary>
        /// Message displayed on the transactionUrl
        /// </summary>
        public string message = "";

        /// <summary>
		/// Collection of PaymentItem objects
        /// </summary>
        public List<PaymentItem> paymentItems = new List<PaymentItem>();

        /// <summary>
		/// Payment creation datetime
        /// </summary>
        public DateTime? orderedTime;

        /// <summary>
        /// Payment completion datetime
        /// </summary>
        public DateTime? executedTime;
    }
}
