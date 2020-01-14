namespace QuantConnect {
    /// <summary>
    /// 	 Specifies how data is normalized before being sent into an algorithm
    /// </summary>
    public enum DataNormalizationMode {
        /// <summary>
        /// 	 The raw price with dividends added to cash book
        /// </summary>
        Raw,

        /// <summary>
        /// 	 The adjusted prices with splits and dividendends factored in
        /// </summary>
        Adjusted,

        /// <summary>
        /// 	 The adjusted prices with only splits factored in, dividends paid out to the cash book
        /// </summary>
        SplitAdjusted,

        /// <summary>
        /// 	 The split adjusted price plus dividends
        /// </summary>
        TotalReturn
    }
}