namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Types of tick data
    /// </summary>
    /// <remarks>QuantConnect currently only has trade, quote, open interest tick data.</remarks>
    public enum TickType {
        /// 	 Trade type tick object.
        Trade,

        /// 	 Quote type tick object.
        Quote,

        /// 	 Open Interest type tick object (for options, futures)
        OpenInterest
    }
}