using System;

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     Calculates the delta between <see cref="Maximum"/> and current price.
    /// </summary>
    public class MaximumDeltaIndicator : Indicator {
        /// <summary>
        ///     The maximum to calculate <see cref="Delta"/> based on. 
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IIndicator Maximum { get; }

        /// <summary>
        ///     The delta between <see cref="Maximum"/> and current price.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase Delta { get; }

        public override bool IsReady => Maximum.IsReady;

        public MaximumDeltaIndicator(string name, int period) : base(name) {
            Maximum = period <= 0 ? (IIndicator) new PeriodlessMaximum($"max({name})") : new Maximum($"max({name})", period) as IndicatorBase;
            Delta = new Identity("max_delta");
        }

        public MaximumDeltaIndicator(int? period = null) : this(nameof(MaximumDeltaIndicator), period ?? 0) { }

        /// <inheritdoc />
        public override void Reset() {
            base.Reset();
            Maximum.Reset();
            Delta.Reset();
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            Maximum.Update(time, input);
            var val = Maximum.Current.Value - input.Value;
            Delta.Update(time, val);
            return val;
        }
    }
}