namespace QuantConnect {
    /// <summary>
    /// 	 User / Algorithm Job Subscription Level
    /// </summary>
    public enum UserPlan {
        /// <summary>
        /// 	 Free User (Backtesting).
        /// </summary>
        Free,

        /// <summary>
        /// 	 Hobbyist User with Included 512db Server.
        /// </summary>
        Hobbyist,

        /// <summary>
        /// 	 Professional plan for financial advisors
        /// </summary>
        Professional
    }
}