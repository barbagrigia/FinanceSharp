﻿/*
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
using System.Linq;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 This indicator computes the n-period mean absolute deviation.
    /// </summary>
    public class MeanAbsoluteDeviation : WindowIndicator {
        /// <summary>
        /// 	 Gets the mean used to compute the deviation
        /// </summary>
        public IndicatorBase Mean { get; }

        /// <summary>
        /// 	 Initializes a new instance of the MeanAbsoluteDeviation class with the specified period.
        ///
        /// 	 Evaluates the mean absolute deviation of samples in the lookback period.
        /// </summary>
        /// <param name="period">The sample size of the standard deviation</param>
        public MeanAbsoluteDeviation(int period)
            : this($"MAD({period})", period) { }

        /// <summary>
        /// 	 Initializes a new instance of the MeanAbsoluteDeviation class with the specified period.
        ///
        /// 	 Evaluates the mean absolute deviation of samples in the look-back period.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="period">The sample size of the mean absolute deviation</param>
        public MeanAbsoluteDeviation(string name, int period)
            : base(name, period) {
            Mean = new SimpleMovingAverage($"{name}_Mean", period);
        }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => Samples >= Period;

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
            Mean.Update(time, input);
            return Samples < 2 ? Constants.Zero : window.Average(v => Math.Abs(v.Value - Mean.Current.Value));
        }

        /// <summary>
        /// 	 Resets this indicator and its sub-indicator Mean to their initial state
        /// </summary>
        public override void Reset() {
            Mean.Reset();
            base.Reset();
        }
    }
}