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


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 Defines the canonical intraday VWAP indicator
    /// </summary>
    public class IntradayVwap : IndicatorBase {
        private DateTime _lastDate;
        private double _sumOfVolume;
        private double _sumOfPriceTimesVolume;

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => _sumOfVolume > 0;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="IntradayVwap"/> class
        /// </summary>
        /// <param name="name">The name of the indicator</param>
        public IntradayVwap(string name)
            : base(name) { }

        /// <summary>
        /// 	 Updates the state of this indicator with the given value and returns true
        /// 	 if this indicator is ready, false otherwise
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The value to use to update this indicator</param>
        /// <returns>True if this indicator is ready, false otherwise</returns>
        public override bool Update(long time, DoubleArray input) {
#if DEBUG
            if (this.InputProperties > input.Properties)
                throw new ArgumentException($"Unable to update with given input because atleast {InputProperties} properties required but got input with {input.Properties} properties.");
#endif
            double volume, averagePrice;
            if (!TryGetVolumeAndAveragePrice(input, out volume, out averagePrice)) {
                return IsReady;
            }

            // reset vwap on daily boundaries
            var date = time.ToDateTime().Date;
            if (_lastDate != date) {
                _sumOfVolume = Constants.Zero;
                _sumOfPriceTimesVolume = Constants.Zero;
                _lastDate = date;
            }

            // running totals for Σ PiVi / Σ Vi
            _sumOfVolume += volume;
            _sumOfPriceTimesVolume += averagePrice * volume;
            CurrentTime = time;
            Samples++;
            if (_sumOfVolume == Constants.Zero) {
                // if we have no trade volume then use the current price as VWAP
                Current = input.Clone();
                OnUpdated(time, Current);
                return IsReady;
            }

            Current = _sumOfPriceTimesVolume / _sumOfVolume;
            
            OnUpdated(time, Current);
            return IsReady;
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state.
        /// 	 NOTE: This must be overriden since it's abstract in the base, but
        /// 	 will never be invoked since we've override the validate method above.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            throw new NotImplementedException($"{nameof(IntradayVwap)}.{nameof(Forward)} should never be invoked.");
        }

        /// <summary>
        /// 	 Determines the volume and price to be used for the current input in the VWAP computation
        /// </summary>
        protected bool TryGetVolumeAndAveragePrice(DoubleArray input, out double volume, out double averagePrice) {
            var tick = input;

            if (tick.Properties > 2) {
                volume = tick.Volume;
                averagePrice = tick.Value;
                return true;
            }

            //var tradeBar = input;
            //if (tradeBar?.IsFillForward == false) {
            //    volume = tradeBar.Volume;
            //    averagePrice = (tradeBar.High + tradeBar.Low + tradeBar.Close) / 3d;
            //    return true;
            //}

            volume = 0;
            averagePrice = 0;
            return false;
        }

        /// <summary>
        /// 	 Determines the volume and price to be used for the current input in the VWAP computation
        /// </summary>
        protected bool TryGetVolumeAndAveragePrice(long time, DoubleArray input, out double volume, out double averagePrice) {
            var tick = input;

            if (tick.Properties > 2) {
                volume = tick.Volume;
                averagePrice = tick.Value;
                return true;
            }

            //var tradeBar = input;
            //if (tradeBar?.IsFillForward == false) {
            //    volume = tradeBar.Volume;
            //    averagePrice = (tradeBar.High + tradeBar.Low + tradeBar.Close) / 3d;
            //    return true;
            //}

            volume = 0;
            averagePrice = 0;
            return false;
        }
    }
}