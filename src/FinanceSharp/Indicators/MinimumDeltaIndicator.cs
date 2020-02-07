namespace FinanceSharp.Indicators {
    /// <summary>
    ///     Calculates the delta between <see cref="Minimum"/> and current price.
    /// </summary>
    public class MinimumDeltaIndicator : Indicator {
        /// <summary>
        ///     The maximum to calculate <see cref="Minimum"/> based on. 
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IIndicator Minimum { get; }

        /// <summary>
        ///     The delta between <see cref="Minimum"/> and current price.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase Delta { get; }

        public override bool IsReady => Minimum.IsReady;

        public MinimumDeltaIndicator(string name, int period) : base(name) {
            Minimum = period <= 0 ? (IIndicator) new PeriodlessMaximum($"min({name})") : new Maximum($"min({name})", period) as IndicatorBase;
            Delta = new Identity("min_delta");
        }

        public MinimumDeltaIndicator(int? period = null) : this(nameof(MaximumDeltaIndicator), period ?? 0) { }

        /// <inheritdoc />
        public override void Reset() {
            base.Reset();
            Minimum.Reset();
            Delta.Reset();
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            Minimum.Update(time, input);
            var val = input.Value - Minimum.Current.Value;
            Delta.Update(time, val);
            return val;
        }
    }
}