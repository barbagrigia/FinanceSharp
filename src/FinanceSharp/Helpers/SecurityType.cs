namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Type of tradable security / underlying asset
    /// </summary>
    public enum SecurityType {
        /// <summary>
        /// 	 Base class for all security types:
        /// </summary>
        Base,

        /// <summary>
        /// 	 US Equity Security
        /// </summary>
        Equity,

        /// <summary>
        /// 	 Option Security Type
        /// </summary>
        Option,

        /// <summary>
        /// 	 Commodity Security Type
        /// </summary>
        Commodity,

        /// <summary>
        /// 	 FOREX Security
        /// </summary>
        Forex,

        /// <summary>
        /// 	 Future Security Type
        /// </summary>
        Future,

        /// <summary>
        /// 	 Contract For a Difference Security Type.
        /// </summary>
        Cfd,

        /// <summary>
        /// 	 Cryptocurrency Security Type.
        /// </summary>
        Crypto
    }
}