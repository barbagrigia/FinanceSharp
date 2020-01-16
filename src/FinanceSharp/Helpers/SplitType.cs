using FinanceSharp.Data.Market;

namespace FinanceSharp {
    /// <summary>
    /// 	 Specifies the type of <see cref="Split"/> data
    /// </summary>
    public enum SplitType {
        /// <summary>
        /// 	 Specifies a warning of an imminent split event
        /// </summary>
        Warning = 0,

        /// <summary>
        /// 	 Specifies the symbol has been split
        /// </summary>
        SplitOccurred = 1
    }
}