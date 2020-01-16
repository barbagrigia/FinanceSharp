namespace FinanceSharp.Data {
    public interface DataStruct {
        /// <summary>
        ///     The number of properties this struct has, equivalent to sizeof(this)/sizeof(double).
        /// </summary>
        int Properties { get; }
    }
}