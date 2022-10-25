namespace Nutaku.Unity
{
    /// <summary>
    /// Describes individual items for purchase in the Payment API
    /// </summary>
    public class PaymentItem
    {
        /// <summary>
		/// ItemId (AKA SKU ID). This is not shown to the user, but it is stored in the database for reporting purposes
        /// </summary>
        public string itemId;

        /// <summary>
        /// Item name
        /// </summary>
        public string itemName;

        /// <summary>
        /// Price for one unit of the item
        /// </summary>
        public int unitPrice;

        /// <summary>
        /// Quantity of this item that is to be purchased. Setting this to anything other than 1 is deprecated.
        /// </summary>
        public int quantity = 1;

        /// <summary>
        /// URL to an image of the item
        /// </summary>
        public string imageUrl;

        /// <summary>
        /// Item description
        /// </summary>
        public string description;
    }
}
