using System.Runtime.InteropServices;

namespace FinanceSharp.Data {
    /// <summary>
    ///     Represents a CHLOV candle struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TradeBarVolumedValue : DataStruct {
        public double Close;
        public double High;
        public double Low;
        public double Open;
        public double Volume;

        public int Properties => 5;
    }
}