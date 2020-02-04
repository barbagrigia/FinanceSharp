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
    /// 	 This indicator computes the upper and lower band of the Donchian Channel.
    /// 	 The upper band is computed by finding the highest high over the given period.
    /// 	 The lower band is computed by finding the lowest low over the given period.
    /// 	 The primary output value of the indicator is the mean of the upper and lower band for 
    /// 	 the given timeframe.
    /// </summary>
    public class DonchianChannel : BarIndicator {
        private DoubleArray _previousInput;
        private long _previousTime;

        /// <summary>
        /// 	 Gets the upper band of the Donchian Channel.
        /// </summary>
        public IndicatorBase UpperBand { get; }

        /// <summary>
        /// 	 Gets the lower band of the Donchian Channel.
        /// </summary>
        public IndicatorBase LowerBand { get; }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="DonchianChannel"/> class.
        /// </summary>
        /// <param name="period">The period for both the upper and lower channels.</param>
        public DonchianChannel(int period)
            : this(period, period) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="DonchianChannel"/> class.
        /// </summary>
        /// <param name="upperPeriod">The period for the upper channel.</param>
        /// <param name="lowerPeriod">The period for the lower channel</param>
        public DonchianChannel(int upperPeriod, int lowerPeriod)
            : this($"DCH({lowerPeriod},{lowerPeriod})", upperPeriod, lowerPeriod) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="DonchianChannel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="period">The period for both the upper and lower channels.</param>
        public DonchianChannel(string name, int period)
            : this(name, period, period) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="DonchianChannel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="upperPeriod">The period for the upper channel.</param>
        /// <param name="lowerPeriod">The period for the lower channel</param>
        public DonchianChannel(string name, int upperPeriod, int lowerPeriod)
            : base(name) {
            WarmUpPeriod = 1 + Math.Max(upperPeriod, lowerPeriod);
            UpperBand = new Maximum(name + "_UpperBand", upperPeriod);
            LowerBand = new Minimum(name + "_LowerBand", lowerPeriod);
        }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => UpperBand.IsReady && LowerBand.IsReady;

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod { get; }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator, which by convention is the mean value of the upper band and lower band.</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            if (_previousInput != null) {
                UpperBand.Update(_previousTime, _previousInput.High);
                LowerBand.Update(_previousTime, _previousInput.Low);
            }

            _previousInput = input.Clone();
            _previousTime = time;
            return (UpperBand + LowerBand) / 2;
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            UpperBand.Reset();
            LowerBand.Reset();
            _previousInput = null;
            _previousTime = 0;
            base.Reset();
        }
    }
}