namespace QuantConnect {
    /// <summary>
    /// 	 Specifies where a subscription's data comes from
    /// </summary>
    public enum SubscriptionTransportMedium {
        /// <summary>
        /// 	 The subscription's data comes from disk
        /// </summary>
        LocalFile,

        /// <summary>
        /// 	 The subscription's data is downloaded from a remote source
        /// </summary>
        RemoteFile,

        /// <summary>
        /// 	 The subscription's data comes from a rest call that is polled and returns a single line/data point of information
        /// </summary>
        Rest,

        /// <summary>
        /// 	 The subscription's data is streamed
        /// </summary>
        Streaming
    }
}