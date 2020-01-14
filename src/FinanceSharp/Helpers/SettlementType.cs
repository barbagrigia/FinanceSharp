namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Specifies the type of settlement in derivative deals
    /// </summary>
    public enum SettlementType {
        /// <summary>
        /// 	 Physical delivery of the underlying security
        /// </summary>
        PhysicalDelivery,

        /// <summary>
        /// 	 Cash is paid/received on settlement
        /// </summary>
        Cash
    }
}