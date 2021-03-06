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
using static FinanceSharp.Constants;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 The Aroon Oscillator is the difference between AroonUp and AroonDown. The value of this
    /// 	 indicator fluctuates between -100 and +100. An upward trend bias is present when the oscillator
    /// 	 is positive, and a negative trend bias is present when the oscillator is negative. AroonUp/Down
    /// 	 values over 75 identify strong trends in their respective direction.
    /// </summary>
    public class AroonOscillator : BarIndicator {
        /// <summary>
        /// 	 Gets the AroonUp indicator
        /// </summary>
        public IndicatorBase AroonUp { get; }

        /// <summary>
        /// 	 Gets the AroonDown indicator
        /// </summary>
        public IndicatorBase AroonDown { get; }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => AroonUp.IsReady && AroonDown.IsReady;

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public override int WarmUpPeriod { get; }

        /// <summary>
        /// 	 Creates a new AroonOscillator from the specified up/down periods.
        /// </summary>
        /// <param name="upPeriod">The lookback period to determine the highest high for the AroonDown</param>
        /// <param name="downPeriod">The lookback period to determine the lowest low for the AroonUp</param>
        public AroonOscillator(int upPeriod, int downPeriod)
            : this($"AROON({upPeriod},{downPeriod})", upPeriod, downPeriod) { }

        /// <summary>
        /// 	 Creates a new AroonOscillator from the specified up/down periods.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="upPeriod">The lookback period to determine the highest high for the AroonDown</param>
        /// <param name="downPeriod">The lookback period to determine the lowest low for the AroonUp</param>
        public AroonOscillator(string name, int upPeriod, int downPeriod)
            : base(name) {
            var max = new Maximum(name + "_Max", upPeriod + 1);
            AroonUp = new FunctionalIndicator(name + "_AroonUp",
                (time, input) => ComputeAroonUp(upPeriod, max, time, input),
                aroonUp => max.IsReady,
                () => max.Reset()
            );

            var min = new Minimum(name + "_Min", downPeriod + 1);
            AroonDown = new FunctionalIndicator(name + "_AroonDown",
                (time, input) => ComputeAroonDown(downPeriod, min, time, input),
                aroonDown => min.IsReady,
                () => min.Reset()
            );

            WarmUpPeriod = 1 + Math.Max(upPeriod, downPeriod);
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            AroonUp.Update(time, input[HighIdx]);
            AroonDown.Update(time, input[LowIdx]);

            return AroonUp.Current.Value - AroonDown.Current.Value;
        }

        /// <summary>
        /// 	 AroonUp = 100 * (period - {periods since max})/period
        /// </summary>
        /// <param name="upPeriod">The AroonUp period</param>
        /// <param name="max">A Maximum indicator used to compute periods since max</param>
        /// <param name="input">The next input data</param>
        /// <returns>The AroonUp value</returns>
        private static double ComputeAroonUp(int upPeriod, Maximum max, long time, DoubleArray input) {
            max.Update(time, input);
            return 100.0d * (upPeriod - max.PeriodsSinceMaximum) / upPeriod;
        }

        /// <summary>
        /// 	 AroonDown = 100 * (period - {periods since min})/period
        /// </summary>
        /// <param name="downPeriod">The AroonDown period</param>
        /// <param name="min">A Minimum indicator used to compute periods since min</param>
        /// <param name="input">The next input data</param>
        /// <returns>The AroonDown value</returns>
        private static double ComputeAroonDown(int downPeriod, Minimum min, long time, DoubleArray input) {
            min.Update(time, (DoubleArray) input);
            return 100.0d * (downPeriod - min.PeriodsSinceMinimum) / downPeriod;
        }

        /// <summary>
        /// 	 Resets this indicator and both sub-indicators (AroonUp and AroonDown)
        /// </summary>
        public override void Reset() {
            AroonUp.Reset();
            AroonDown.Reset();
            base.Reset();
        }
    }
}