/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;

namespace FinanceSharp.Consolidators {
    /// <summary>
    /// 	 Provides a base class for consolidators that emit data based on the passing of a period of time
    /// 	 or after seeing a max count of data points.
    /// </summary>
    /// <typeparam name="T">The input type of the consolidator</typeparam>
    /// <typeparam name="TConsolidated">The output type of the consolidator</typeparam>
    public abstract class PeriodCountConsolidatorBase : DataConsolidator {
        // The symbol that we are consolidating for.

        //The number of data updates between creating new bars.
        private readonly int? _maxCount;

        //
        private readonly IPeriodSpecification _periodSpecification;

        //The minimum timespan between creating new bars.
        private TimeSpan? _period;

        //The number of pieces of data we've accumulated since our last emit
        private int _currentCount;

        //The working bar used for aggregating the data
        private DoubleArray _workingBar;

        //The working time used for aggregating the data
        private long _workingTime;

        //The last time we emitted a consolidated bar
        private long? _lastEmit;

        private PeriodCountConsolidatorBase(IPeriodSpecification periodSpecification) {
            _periodSpecification = periodSpecification;
        }

        /// <summary>
        /// 	 Creates a consolidator to produce a new <typeparamref name="TConsolidated"/> instance representing the period
        /// </summary>
        /// <param name="period">The minimum span of time before emitting a consolidated bar</param>
        protected PeriodCountConsolidatorBase(TimeSpan period)
            : this(new TimeSpanPeriodSpecification(period)) {
            _period = _periodSpecification.Period;
        }

        /// <summary>
        /// 	 Creates a consolidator to produce a new <typeparamref name="TConsolidated"/> instance representing the last count pieces of data
        /// </summary>
        /// <param name="maxCount">The number of pieces to accept before emiting a consolidated bar</param>
        protected PeriodCountConsolidatorBase(int maxCount)
            : this(new BarCountPeriodSpecification()) {
            _maxCount = maxCount;
        }

        /// <summary>
        /// 	 Creates a consolidator to produce a new <typeparamref name="TConsolidated"/> instance representing the last count pieces of data or the period, whichever comes first
        /// </summary>
        /// <param name="maxCount">The number of pieces to accept before emiting a consolidated bar</param>
        /// <param name="period">The minimum span of time before emitting a consolidated bar</param>
        protected PeriodCountConsolidatorBase(int maxCount, TimeSpan period)
            : this(new MixedModePeriodSpecification(period)) {
            _maxCount = maxCount;
            _period = _periodSpecification.Period;
        }

        /// <summary>
        /// <summary>
        /// 	 Gets a clone of the data being currently consolidated
        /// </summary>
        public override DoubleArray WorkingData => _workingBar;


        /// <summary>
        /// 	 Updates this consolidator with the specified data. This method is
        /// 	 responsible for raising the DataConsolidated event
        /// 	 In time span mode, the bar range is closed on the left and open on the right: [T, T+TimeSpan).
        /// 	 For example, if time span is 1 minute, we have [10:00, 10:01): so data at 10:01 is not 
        /// 	 included in the bar starting at 10:00.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when multiple symbols are being consolidated.</exception>
        /// <param name="data">The new data for the consolidator</param>
        public override bool Update(long time, DoubleArray data) {
            Samples++;
            //Decide to fire the event
            var fireDataConsolidated = false;

            // decide to aggregate data before or after firing OnDataConsolidated event
            // always aggregate before firing in counting mode
            bool aggregateBeforeFire = _maxCount.HasValue;

            if (_maxCount.HasValue) {
                // we're in count mode
                _currentCount++;
                if (_currentCount >= _maxCount.Value) {
                    _currentCount = 0;
                    fireDataConsolidated = true;
                }
            }

            if (!_lastEmit.HasValue) {
                // initialize this value for period computations
                _lastEmit = IsTimeBased ? 0 : time;
            }

            if (_period.HasValue) {
                // we're in time span mode and initialized
                if (_workingBar != null && (time - _workingTime) >= _period.Value.TotalMilliseconds && GetRoundedBarTime(time) > _lastEmit) {
                    fireDataConsolidated = true;
                }

                // special case: always aggregate before event trigger when TimeSpan is zero
                if (_period.Value == TimeSpan.Zero) {
                    fireDataConsolidated = true;
                    aggregateBeforeFire = true;
                }
            }

            if (aggregateBeforeFire) {
                if (time >= _lastEmit) {
                    AggregateBar(ref _workingTime, ref _workingBar, time, data);
                }
            }

            //Fire the event
            if (fireDataConsolidated) {
                OnDataConsolidated(_workingTime, _workingBar);
                _lastEmit = IsTimeBased && _workingBar != null ? (long) (_workingTime + (Period ?? TimeSpan.Zero).TotalMilliseconds) : time;
                _workingBar = null;
            }

            if (!aggregateBeforeFire) {
                if (time >= _lastEmit) {
                    AggregateBar(ref _workingTime, ref _workingBar, time, data);
                }
            }

            return fireDataConsolidated;
        }

        /// <summary>
        /// 	 Scans this consolidator to see if it should emit a bar due to time passing
        /// </summary>
        /// <param name="currentLocalTime">The current time in the local time zone (same as <see cref="BaseData.Time"/>)</param>
        public override void Scan(long currentLocalTime) {
            if (_period.HasValue && _workingBar != null) {
                currentLocalTime = GetRoundedBarTime(currentLocalTime);

                if (_period.Value != TimeSpan.Zero && (currentLocalTime - _workingTime) >= _period.Value.TotalMilliseconds && currentLocalTime > _lastEmit) {
                    OnDataConsolidated(_workingTime, _workingBar);
                    _lastEmit = currentLocalTime;
                    _workingBar = null;
                    _workingTime = 0;
                }
            }
        }

        /// <summary>
        /// 	 Returns true if this consolidator is time-based, false otherwise
        /// </summary>
        protected bool IsTimeBased => !_maxCount.HasValue;

        /// <summary>
        /// 	 Gets the time period for this consolidator
        /// </summary>
        protected TimeSpan? Period => _period;

        /// <summary>
        /// 	 Aggregates the new 'data' into the 'workingBar'. The 'workingBar' will be
        /// 	 null following the event firing
        /// </summary>
        /// <param name="workingBar">The bar we're building, null if the event was just fired and we're starting a new consolidated bar</param>
        /// <param name="data">The new data</param>
        protected abstract void AggregateBar(ref long workingTime, ref DoubleArray workingBar, long time, DoubleArray data);

        /// <summary>
        /// 	 Gets a rounded-down bar time. Called by AggregateBar in derived classes.
        /// </summary>
        /// <param name="time">The bar time to be rounded down</param>
        /// <returns>The rounded bar time</returns>
        protected long GetRoundedBarTime(long time) {
            var barTime = _periodSpecification.GetRoundedBarTime(time);

            // In the case of a new bar, define the period defined at opening time
            if (_workingBar == null) {
                _period = _periodSpecification.Period;
            }

            return barTime;
        }

        /// <summary>
        /// 	 Distinguishes between the different ways a consolidated data start time can be specified
        /// </summary>
        private interface IPeriodSpecification {
            TimeSpan? Period { get; }
            long GetRoundedBarTime(long time);
        }

        /// <summary>
        /// 	 User defined the bars period using a counter
        /// </summary>
        private class BarCountPeriodSpecification : IPeriodSpecification {
            public TimeSpan? Period { get; } = null;

            public long GetRoundedBarTime(long time) => time;
        }

        /// <summary>
        /// 	 User defined the bars period using a counter and a period (mixed mode)
        /// </summary>
        private class MixedModePeriodSpecification : IPeriodSpecification {
            public TimeSpan? Period { get; }

            public MixedModePeriodSpecification(TimeSpan period) {
                Period = period;
            }

            public long GetRoundedBarTime(long time) => time;
        }

        /// <summary>
        /// 	 User defined the bars period using a time span
        /// </summary>
        private class TimeSpanPeriodSpecification : IPeriodSpecification {
            public TimeSpan? Period { get; }

            public TimeSpanPeriodSpecification(TimeSpan period) {
                Period = period;
            }

            public long GetRoundedBarTime(long time) => time.RoundDown(Period.Value);
        }
    }
}