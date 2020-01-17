using System.Runtime.InteropServices;

namespace FinanceSharp.Data {
    /// <summary>
    ///     Represents a CHLO candle struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TradeBarValue : DataStruct {
        public double Close;
        public double High;
        public double Low;
        public double Open;

        public TradeBarValue(double close, double high, double low, double open) {
            Close = close;
            High = high;
            Low = low;
            Open = open;
        }
        int DataStruct.Properties => Properties;
        public const int Properties = 4;

        public object Clone() {
            return new TradeBarValue(Close, High, Low, Open);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{nameof(Close)}: {Close}, {nameof(High)}: {High}, {nameof(Low)}: {Low}, {nameof(Open)}: {Open}, {nameof(Properties)}: {Properties}";
        }
    }
}