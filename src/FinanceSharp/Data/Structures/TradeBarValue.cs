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

        public int Properties => 4;
    }
}