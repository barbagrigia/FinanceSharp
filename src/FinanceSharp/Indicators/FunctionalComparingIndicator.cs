using System;
using FinanceSharp.Delegates;

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     A functional <see cref="ComparingIndicatorBase"/>. Pretty much saves time writing new classes.
    /// </summary>
    public class FunctionalComparingIndicator : ComparingIndicatorBase {
        private readonly DoubleArrayComparingHandler _comparer;

        /// <summary>
        ///     Compares <paramref name="current"/> against <paramref name="input"/> returning the new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> values.
        /// </summary>
        /// <param name="current">The <see cref="IUpdatable.Current"/> from the handling indicator.</param>
        /// <param name="input">The new input to compare against.</param>
        /// <returns>The new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> to set.</returns>
        /// <remarks>Equivalent to <see cref="DoubleArrayComparingHandler"/></remarks>
        protected override (long Time, DoubleArray Array) Comparer((long time, DoubleArray Array) current, (long Time, DoubleArray Array) input) {
            return _comparer(CurrentPoint, input);
        }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        public FunctionalComparingIndicator(DoubleArrayComparingHandler comparer, string name = "FUNC") : base(name) {
            _comparer = comparer ?? ((currentMax, input) => input.Array.Value > currentMax.Array.Value ? input : currentMax);
        }
    }
}