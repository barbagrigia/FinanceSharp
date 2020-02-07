using IndicatorBase = FinanceSharp.Indicators.IndicatorBase;

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     Belash is a softned oscillator (0 to 100) that shows how strongly the direction of the price is heading. If 100 then price is rising at full speed. If 0 the price is falling at full speed.
    /// </summary>
    public class Belash : Indicator {
        /// <summary>
        ///     The indicator that represents the maximal value of the price in a certain period of time. Can be anything but must be relative to <see cref="Min"/>.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase Max { get; }

        /// <summary>
        ///     The indicator that represents the maximal value of the price in a certain period of time. Can be anything but must be relative to <see cref="Max"/>.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase Min { get; }

        /// <summary>
        ///     A softned version of <see cref="Max"/>.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase SoftenedMax { get; }

        /// <summary>
        ///     A softned version of <see cref="Min"/>.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase SoftenedMin { get; }

        /// <summary>
        ///     A softned version of <see cref="Min"/>.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public IndicatorBase SoftenedInput { get; }

        /// <summary>
        ///     A regressed version of this's <see cref="Indicator.Current"/>.
        /// </summary>
        /// <remarks>Updated internally in <see cref="Forward"/>.</remarks>
        public LeastSquaresMovingAverage Regressed { get; }

        /// <summary>
        ///     Softning Period of all softened indicators in this class.
        /// </summary>
        public int SoftningPeriod { get; }

        /// <param name="period">The <see cref="Min"/> and <see cref="Max"/> period.</param>
        /// <param name="softningPeriod">The period of <see cref="ExponentialMovingAverage"/> applied on the <see cref="Min"/> and <see cref="Max"/>.</param>
        /// <param name="regressionPeriod">Regression period of <see cref="LeastSquaresMovingAverage"/>.</param>
        public Belash(int period, int softningPeriod, int? regressionPeriod = null) : this(new Maximum(period), new Minimum(period), softningPeriod, regressionPeriod) { }

        /// <param name="period">The <see cref="Min"/> and <see cref="Max"/> period.</param>
        /// <param name="max">The indicator that represents the maximal value of the price in a certain period of time. Can be anything but must be relative to <paramref name="min"/>.</param>
        /// <param name="min">The indicator that represents the minimal value of the price in a certain period of time. Can be anything but must be relative to <paramref name="max"/>.</param>
        /// <param name="softningPeriod">The period of <see cref="ExponentialMovingAverage"/> applied on the <see cref="Min"/> and <see cref="Max"/>.</param>
        /// <param name="regressionPeriod">Regression period of <see cref="LeastSquaresMovingAverage"/>.</param>
        /// <param name="softeningMovingAverage">The moving average to soften the price using <paramref name="softningPeriod"/>, <paramref name="max"/> and <paramref name="min"/>.</param>
        public Belash(IndicatorBase max, IndicatorBase min, int softningPeriod, int? regressionPeriod = null, MovingAverageType softeningMovingAverage = MovingAverageType.Exponential) : base("Blsh") {
            Min = min;
            Max = max;
            SoftningPeriod = softningPeriod;
            SoftenedMax = softeningMovingAverage.AsIndicator(softningPeriod).Of(Max, false);
            SoftenedMin = softeningMovingAverage.AsIndicator(softningPeriod).Of(Min, false);
            SoftenedInput = softeningMovingAverage.AsIndicator(softningPeriod);
            Regressed = new LeastSquaresMovingAverage(regressionPeriod ?? 8).Of(this, false);
        }

        /// <inheritdoc />
        public override bool IsReady => true;

        /// <inheritdoc />
        public override void Reset() {
            Min.Reset();
            Max.Reset();
            SoftenedMin.Reset();
            SoftenedMax.Reset();
            SoftenedInput.Reset();
            Regressed.Reset();
            base.Reset();
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            //todo pass tradebar and update min and max with high or low to allow turtle head for more accuracy.
            Max.Update(time, input);
            Min.Update(time, input);
            SoftenedInput.Update(time, input);
            var lower = SoftenedMax.Current.Value - SoftenedMin.Current.Value;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var @out = lower == 0 ? 0 : ((SoftenedInput - SoftenedMin) / lower) * 100d;

            return @out;
        }
    }
}