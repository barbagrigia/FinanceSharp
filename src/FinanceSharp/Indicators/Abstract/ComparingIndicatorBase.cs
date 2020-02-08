using System;
using FinanceSharp.Delegates;

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     An indicator that holds a memory (a <see cref="CurrentPoint"/>) based on comparing current to the newly updated time and value.
    /// </summary>
    public abstract class ComparingIndicatorBase : IIndicator {
        /// <summary>
        /// 	 Event handler that fires after this indicator is updated
        /// </summary>
        public event UpdatedHandler Updated;

        /// <summary>
        ///     Event handler that fires after this updatable is reset.
        /// </summary>
        public event ResettedHandler Resetted;

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        protected ComparingIndicatorBase(string name) {
            Name = name;
            CurrentPoint = (0, Constants.Zero);
        }

        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public virtual bool IsReady => Samples > 0;

        /// <summary>
        /// 	 Gets the current state of this indicator. If the state has not been updated
        /// 	 then the time on the value will equal DateTime.MinValue.
        /// </summary>
        public DoubleArray Current => CurrentPoint.Array;

        /// <summary>
        ///     The time of the currently stored data in milliseconds-epoch.
        /// </summary>
        public long CurrentTime => CurrentPoint.Time;

        /// <summary>
        ///     The <see cref="Current"/> and <see cref="CurrentTime"/> coupled.
        /// </summary>
        protected (long Time, DoubleArray Array) CurrentPoint;

        /// <summary>
        ///     The number of items <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int OutputCount => 1;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public virtual int Properties => IndicatorValue.Properties;

        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        public virtual int InputProperties => Properties;

        /// <summary>
        /// 	 Gets the number of samples processed by this indicator
        /// </summary>
        public long Samples { get; protected set; }

        /// <summary>
        /// 	 Required period, in data points (number of updates), for the indicator to be ready and fully initialized.
        /// </summary>
        public virtual int WarmUpPeriod => 1;

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        public bool Update<TStruct>(long time, TStruct data) where TStruct : unmanaged, DataStruct {
            return Update(time, DoubleArray.From(data));
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        internal bool Update<TStruct>(DateTime time, TStruct data) where TStruct : unmanaged, DataStruct {
            return Update(time.ToEpochTime(), DoubleArray.From(data));
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        internal bool Update<TStruct>((DateTime Time, TStruct Value) tuple) where TStruct : unmanaged, DataStruct {
            return Update(tuple.Time.ToEpochTime(), DoubleArray.From(tuple.Value));
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        internal bool Update<TStruct>((long Time, TStruct Value) tuple) where TStruct : unmanaged, DataStruct {
            return Update(tuple.Time, DoubleArray.From(tuple.Value));
        }

        /// <summary>
        /// 	 Updates the state of this indicator with the given value and returns true
        /// 	 if this indicator is ready, false otherwise
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The value to use to update this indicator</param>
        /// <returns>True if this indicator is ready, false otherwise</returns>
        public bool Update(long time, DoubleArray input) {
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
        /// 	 Updates the state of this indicator with the given value and returns true
        /// 	 if this indicator is ready, false otherwise
        /// </summary>
        /// <param name="time">The time associated with the value</param>
        /// <param name="value">The value to use to update this indicator</param>
        /// <returns>True if this indicator is ready, false otherwise</returns>
        public bool Update(long time, double value) {
            return Update((long) time, (DoubleArray) value);
        }

        /// <summary>
        /// 	 Updates the state of this indicator with the given value and returns true
        /// 	 if this indicator is ready, false otherwise
        /// </summary>
        /// <param name="time">The time associated with the value</param>
        /// <param name="value">The value to use to update this indicator</param>
        /// <returns>True if this indicator is ready, false otherwise</returns>
        internal bool Update(DateTime time, double value) {
            return Update(time.ToEpochTime(), (DoubleArray) value);
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public virtual void Reset() {
            Samples = 0;
            CurrentPoint = (0, Constants.Zero);
            Resetted?.Invoke(this);
        }

        /// <summary>
        /// 	 Triggers event <see cref="Resetted"/>.
        /// </summary>
        protected void OnReset() {
            Resetted?.Invoke(this);
        }


        /// <summary>
        /// 	 Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// 	 true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            // this implementation acts as a liason to prevent inconsistency between the operators
            // == and != against primitive types. the core impl for equals between two indicators
            // is still reference equality, however, when comparing value types (floats/int, ect..)
            // we'll use value type semantics on Current.Value
            // because of this, we shouldn't need to override GetHashCode as well since we're still
            // solely relying on reference semantics (think hashset/dictionary impls)
            //TODO!: wtf is going on here
            if (ReferenceEquals(obj, null)) return false;
            var type = obj.GetType();

            while (type != null && type != typeof(object)) {
                if (ReferenceEquals(this, obj))
                    return true;

                type = type.BaseType;
            }

            try {
                // the obj is not an indicator, so let's check for value types, try converting to double
                var converted = obj.ConvertInvariant<double>();
                return Current == converted;
            } catch (InvalidCastException) {
                // conversion failed, return false
                return false;
            }
        }

        /// <summary>
        /// 	 ToString Overload for Indicator Base
        /// </summary>
        /// <returns>String representation of the indicator</returns>
        public override string ToString() {
            return Current.ToString();
        }

        /// <summary>
        /// 	 Provides a more detailed string of this indicator in the form of {Name} - {Value}
        /// </summary>
        /// <returns>A detailed string of this indicator's current state</returns>
        public string ToDetailedString() {
            return $"{Name} - {this}";
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

        /// <summary>
        /// 	 Event invocator for the Updated event
        /// </summary>
        /// <param name="consolidated">This is the new piece of data produced by this indicator</param>
        protected virtual void OnUpdated(long time, DoubleArray consolidated) {
            Updated?.Invoke(time, consolidated);
        }
    }
}