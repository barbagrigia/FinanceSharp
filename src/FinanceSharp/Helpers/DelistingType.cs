namespace QuantConnect {
    /// <summary>
    /// 	 Specifies the type of <see cref="QuantConnect.Data.Market.Delisting"/> data
    /// </summary>
    public enum DelistingType {
        /// <summary>
        /// 	 Specifies a warning of an imminent delisting
        /// </summary>
        Warning = 0,

        /// <summary>
        /// 	 Specifies the symbol has been delisted
        /// </summary>
        Delisted = 1
    }
}