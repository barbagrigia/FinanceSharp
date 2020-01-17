using System.Runtime.InteropServices;

namespace FinanceSharp.Data {
    /// <summary>
    ///     Represents a trade tick with quotation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TickValue : DataStruct {
        public double Value;
        public double BidPrice;
        public double BidSize;
        public double AskPrice;
        public double Volume;
        public double AskSize;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public TickValue(double value, double bidPrice, double bidSize, double askPrice, double volume, double askSize) {
            Value = value;
            BidPrice = bidPrice;
            BidSize = bidSize;
            AskPrice = askPrice;
            Volume = volume;
            AskSize = askSize;
        }

        int DataStruct.Properties => Properties;
        public const int Properties = 6;

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return new TickValue(Value, BidPrice, BidSize, AskPrice, Volume, AskSize);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{nameof(Value)}: {Value}, {nameof(BidPrice)}: {BidPrice}, {nameof(BidSize)}: {BidSize}, {nameof(AskPrice)}: {AskPrice}, {nameof(Volume)}: {Volume}, {nameof(AskSize)}: {AskSize}, {nameof(Properties)}: {Properties}";
        }
    }
}