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
    /// 	 This indicator computes the n-period population variance.
    /// </summary>
    public class Variance : WindowIndicator {
        private double _rollingSum;
        private double _rollingSumOfSquares;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="Variance"/> class using the specified period.
        /// </summary> 
        /// <param name="period">The period of the indicator</param>
        public Variance(int period)
            : this($"VAR({period})", period) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="Variance"/> class using the specified name and period.
        /// </summary> 
        /// <param name="name">The name of this indicator</param>
        /// <param name="period">The period of the indicator</param>
        public Variance(string name, int period)
            : base(name, period) { }

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public override int WarmUpPeriod => Period;

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="timeWindow"></param>
        /// <param name="window">The window for the input history</param>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(IReadOnlyWindow<long> timeWindow, IReadOnlyWindow<DoubleArray> window, long time, DoubleArray input) {
            _rollingSum += input.Value;
            _rollingSumOfSquares += input.Value * input.Value;

            if (Samples < 2)
                return Constants.Zero;

            var n = Math.Min(Period, Samples);
            var meanValue1 = _rollingSum / n;
            var meanValue2 = _rollingSumOfSquares / n;

            if (n == Period) {
                var removedValue = window[Period - 1].Value;
                _rollingSum -= removedValue;
                _rollingSumOfSquares -= removedValue * removedValue;
            }

            // Ensure non-negative variance
            return Math.Max(Constants.Zero, meanValue2 - meanValue1 * meanValue1);
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            _rollingSum = 0;
            _rollingSumOfSquares = 0;
            base.Reset();
        }
    }
}