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

        int DataStruct.Properties => Properties;
        public const int Properties = 1;

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return new IndicatorValue(Value);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{nameof(Value)}: {Value}, {nameof(Properties)}: {Properties}";
        }
    }
}