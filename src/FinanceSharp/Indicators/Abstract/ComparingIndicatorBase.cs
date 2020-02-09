using System;
using FinanceSharp.Delegates;

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     An indicator that holds a memory (a <see cref="CurrentPoint"/>) based on comparing current to the newly updated time and value.
    /// </summary>
    public abstract class ComparingIndicatorBase : IndicatorBase {
        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        protected ComparingIndicatorBase(string name) : base(name) {
            CurrentPoint = (0, Constants.Zero);
        }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => Samples > 0;

        /// <summary>
        /// 	 Gets the current state of this indicator. If the state has not been updated
        /// 	 then the time on the value will equal DateTime.MinValue.
        /// </summary>
        public override DoubleArray Current => CurrentPoint.Array;

        /// <summary>
        ///     The time of the currently stored data in milliseconds-epoch.
        /// </summary>
        public override long CurrentTime => CurrentPoint.Time;

        /// <summary>
        ///     The <see cref="Current"/> and <see cref="CurrentTime"/> coupled.
        /// </summary>
        protected (long Time, DoubleArray Array) CurrentPoint;

        /// <summary>
        ///     The number of items <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public override int OutputCount { get; protected set; } = 1;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public override int Properties => IndicatorValue.Properties;

        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        public override int InputProperties => Properties;

        /// <summary>
        /// 	 Required period, in data points (number of updates), for the indicator to be ready and fully initialized.
        /// </summary>
        public override int WarmUpPeriod => 1;


        /// <summary>
        /// 	 Updates the state of this indicator with the given value and returns true
        /// 	 if this indicator is ready, false otherwise
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The value to use to update this indicator</param>
        /// <returns>True if this indicator is ready, false otherwise</returns>
        public override bool Update(long time, DoubleArray input) {
            // compute a new value and update our previous time
            Samples++;

            ((long Time, DoubleArray Array) point, bool changed) = ValidateAndForward(time, input);

            if (changed) {
                CurrentPoint = point;

                // let others know we've produced a new data point
                OnUpdated(time, Current);
            }

            return IsReady;
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            CurrentPoint = (0, Constants.Zero);
            base.Reset();
        }

        /// <summary>
        ///     Compares <paramref name="current"/> against <paramref name="input"/> returning the new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> values.
        /// </summary>
        /// <param name="current">The <see cref="IUpdatable.Current"/> from the handling indicator.</param>
        /// <param name="input">The new input to compare against.</param>
        /// <returns>The new <see cref="IUpdatable.Current"/> and <see cref="IUpdatable.CurrentTime"/> to set.</returns>
        /// <remarks>Equivalent to <see cref="DoubleArrayComparingHandler"/></remarks>
        protected abstract (long Time, DoubleArray Array) Comparer((long time, DoubleArray Array) current, (long Time, DoubleArray Array) input);

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected sealed override DoubleArray Forward(long time, DoubleArray input) {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// 	 and returns an instance of the <see cref="IndicatorResult"/> class
        /// </summary>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>An IndicatorResult object including the status of the indicator</returns>
        protected virtual ((long Time, DoubleArray Array) Point, bool Changed) ValidateAndForward(long inputTime, DoubleArray input) {
#if DEBUG
            if (this.InputProperties > input.Properties)
                throw new ArgumentException($"Unable to update with given input because atleast {InputProperties} properties required but got input with {input.Properties} properties.");
#endif
            var newPoint = Comparer(CurrentPoint, (inputTime, input));
            return (newPoint, newPoint.Time == inputTime);
        }
    }
}