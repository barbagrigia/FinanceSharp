namespace FinanceSharp.Data {
    /// <summary>
    ///     Ensures that the given T structures are in fact unmanaged during compile time
    /// </summary>
    internal class UnmanagedEnsurer {
#pragma warning disable 169
        private MustBeUnmanaged<TradeBarValue> _c;
        private MustBeUnmanaged<TradeBarVolumedValue> _d;
        private MustBeUnmanaged<IndicatorValue> _e;
#pragma warning restore 169

        // ReSharper disable once ClassNeverInstantiated.Local
        private class MustBeUnmanaged<T> where T : unmanaged { }
    }
}