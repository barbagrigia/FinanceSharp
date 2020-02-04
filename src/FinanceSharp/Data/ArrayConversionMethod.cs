namespace FinanceSharp {
    /// <summary>
    ///     Provides options of how to convert a <see cref="DoubleArray"/> to an NDArray.
    /// </summary>
    public enum ArrayConversionMethod {
        /// <summary>
        ///     Reinterprets the entire array to be as TDestStruct. Similar to casting address pointer. returned array's Length will be total-bytes/sizeof(TDestStruct)
        /// </summary>
        /// <remarks>total_bytes % sizeof(TDestStruct) must be 0.</remarks>
        Reinterpret,

        /// <summary>
        ///     Casts the entire array to TDestStruct by reading propertiesPerItem properties. See remarks.
        /// </summary>
        /// <remarks>
        ///     propertiesPerItem describes how many doubles to copy to and per TDestStruct. <br></br>If -1 is specified then sizeof(TDestStruct)/sizeof(double) will be used.<br></br><br></br>
        ///     If TDestStruct has 2 double fields and propertiesPerItem specified is 1 then all new TDestStruct will have only their first field written to and rest are zero'ed.
        /// </remarks>
        Cast,
    }
}