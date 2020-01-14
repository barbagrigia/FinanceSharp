namespace QuantConnect {
    /// <summary>
    /// 	 Datafeed enum options for selecting the source of the datafeed.
    /// </summary>
    public enum DataFeedEndpoint {
        /// 	 Backtesting Datafeed Endpoint
        Backtesting,

        /// 	 Loading files off the local system
        FileSystem,

        /// 	 Getting datafeed from a QC-Live-Cloud
        LiveTrading,

        /// 	 Database
        Database
    }
}