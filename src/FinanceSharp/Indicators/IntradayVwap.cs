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
using FinanceSharp.Data;
using FinanceSharp;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


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
        /// 	 Computes the new VWAP
        /// </summary>
        protected override IndicatorResult ValidateAndForward(long time, DoubleArray input) {
            double volume, averagePrice;
            if (!TryGetVolumeAndAveragePrice(input, out volume, out averagePrice)) {
                return new IndicatorResult(0, IndicatorStatus.InvalidInput);
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

            if (_sumOfVolume == Constants.Zero) {
                // if we have no trade volume then use the current price as VWAP
                return input.Value;
            }

            return _sumOfPriceTimesVolume / _sumOfVolume;
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