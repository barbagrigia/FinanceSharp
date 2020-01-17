using System;

namespace FinanceSharp.Data {
    public interface DataStruct : ICloneable {
        /// <summary>
        ///     The number of properties this struct has, equivalent to sizeof(this)/sizeof(double).
        /// </summary>
        int Properties { get; }
    }
}