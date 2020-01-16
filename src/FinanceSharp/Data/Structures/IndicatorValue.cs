using System.Runtime.InteropServices;

namespace FinanceSharp.Data {
    /// <summary>
    ///     Represents a simple 1-valued struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IndicatorValue : DataStruct {
        public double Value;

        public IndicatorValue(double value) {
            Value = value;
        }

        public int Properties => 1;
    }
}