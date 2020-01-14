namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Wrapper for algorithm status enum to include the charting subscription.
    /// </summary>
    public class AlgorithmControl {
        /// <summary>
        /// 	 Default initializer for algorithm control class.
        /// </summary>
        public AlgorithmControl() {
            // default to true, API can override
            Initialized = false;
            HasSubscribers = true;
            Status = AlgorithmStatus.Running;
            ChartSubscription = "Strategy Equity";
        }

        /// <summary>
        /// 	 Register this control packet as not defaults.
        /// </summary>
        public bool Initialized;

        /// <summary>
        /// 	 Current run status of the algorithm id.
        /// </summary>
        public AlgorithmStatus Status;

        /// <summary>
        /// 	 Currently requested chart.
        /// </summary>
        public string ChartSubscription;

        /// <summary>
        /// 	 True if there's subscribers on the channel
        /// </summary>
        public bool HasSubscribers;
    }
}