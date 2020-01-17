using System.Runtime.InteropServices;

namespace FinanceSharp.Data {
    /// <summary>
    ///     Represents a CHLOV candle struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RenkoBarValue : DataStruct {
        public double Close;
        public double High;
        public double Low;
        public double Open;
        public double Volume;
        public double Size;

        int DataStruct.Properties => Properties;
        public const int Properties = 6;

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return new RenkoBarValue() {Close = Close, High = High, Low = Low, Open = Open, Size = Size, Volume = Volume};
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            return $"{nameof(Close)}: {Close}, {nameof(High)}: {High}, {nameof(Low)}: {Low}, {nameof(Open)}: {Open}, {nameof(Volume)}: {Volume}, {nameof(Size)}: {Size}, {nameof(Properties)}: {Properties}";
        }
    }
}