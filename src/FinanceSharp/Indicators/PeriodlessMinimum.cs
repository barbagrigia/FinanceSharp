using System;
using FinanceSharp.Delegates;

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     Identical to <see cref="Maximum"/> but without Period.
    /// </summary>
    public class PeriodlessMinimum : ComparingIndicatorBase {
        /// <summary>
        ///     Compares <paramref name="current"/> against <paramref name="input"/> returning the new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> values.
        /// </summary>
        /// <param name="current">The <see cref="IUpdatable.Current"/> from the handling indicator.</param>
        /// <param name="input">The new input to compare against.</param>
        /// <returns>The new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> to set.</returns>
        protected override (long Time, DoubleArray Array) Comparer((long time, DoubleArray Array) current, (long Time, DoubleArray Array) input) {
            return input.Array.Value < current.Array.Value ? input : current;
        }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        public PeriodlessMinimum(string name) : base(name) { }

        public PeriodlessMinimum() : base("MAX") { }
    }
}