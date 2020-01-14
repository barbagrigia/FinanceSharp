using System.Runtime.Serialization;

namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Represents the types of environments supported by brokerages for trading
    /// </summary>
    public enum BrokerageEnvironment {
        /// <summary>
        /// 	 Live trading
        /// </summary>
        [EnumMember(Value = "live")] Live,

        /// <summary>
        /// 	 Paper trading
        /// </summary>
        [EnumMember(Value = "paper")] Paper
    }
}