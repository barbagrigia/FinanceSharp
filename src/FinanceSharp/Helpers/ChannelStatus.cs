namespace QuantConnect {
    /// <summary>
    /// 	 Defines the different channel status values
    /// </summary>
    public static class ChannelStatus {
        /// <summary>
        /// 	 The channel is empty
        /// </summary>
        public const string Vacated = "channel_vacated";

        /// <summary>
        /// 	 The channel has subscribers
        /// </summary>
        public const string Occupied = "channel_occupied";
    }
}