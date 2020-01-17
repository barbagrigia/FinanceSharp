/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by: 
 * 
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
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

using FinanceSharp.Data;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 This indicator computes the Slow Stochastics %K and %D. The Fast Stochastics %K is is computed by 
    /// (Current Close Price - Lowest Price of given Period) / (Highest Price of given Period - Lowest Price of given Period)
    /// 	 multiplied by 100. Once the Fast Stochastics %K is calculated the Slow Stochastic %K is calculated by the average/smoothed price of
    /// 	 of the Fast %K with the given period. The Slow Stochastics %D is then derived from the Slow Stochastics %K with the given period.
    /// </summary>
    public class Stochastic : BarIndicator {
        private readonly IndicatorBase _maximum;
        private readonly IndicatorBase _minimum;
        private readonly IndicatorBase _sumFastK;
        private readonly IndicatorBase _sumSlowK;

        /// <summary>
        /// 	 Gets the value of the Fast Stochastics %K given Period.
        /// </summary>
        public IndicatorBase FastStoch { get; }

        /// <summary>
        /// 	 Gets the value of the Slow Stochastics given Period K.
        /// </summary>
        public IndicatorBase StochK { get; }

        /// <summary>
        /// 	 Gets the value of the Slow Stochastics given Period D.
        /// </summary>
        public IndicatorBase StochD { get; }

        /// <summary>
        /// 	 Creates a new Stochastics Indicator from the specified periods.
        /// </summary>
        /// <param name="name">The name of this indicator.</param>
        /// <param name="period">The period given to calculate the Fast %K</param>
        /// <param name="kPeriod">The K period given to calculated the Slow %K</param>
        /// <param name="dPeriod">The D period given to calculated the Slow %D</param>
        public Stochastic(string name, int period, int kPeriod, int dPeriod)
            : base(name) {
            _maximum = new Maximum(name + "_Max", period);
            _minimum = new Minimum(name + "_Min", period);
            _sumFastK = new Sum(name + "_SumFastK", kPeriod);
            _sumSlowK = new Sum(name + "_SumD", dPeriod);

            FastStoch = new FunctionalIndicator(name + "_FastStoch",
                (time, input) => ComputeFastStoch(period, time, input),
                fastStoch => _maximum.IsReady,
                () => { }
            );

            StochK = new FunctionalIndicator(name + "_StochK",
                (time, input) => ComputeStochK(period, kPeriod, time, input),
                stochK => _maximum.IsReady,
                () => { }
            );

            StochD = new FunctionalIndicator(
                name + "_StochD",
                (time, input) => ComputeStochD(period, kPeriod, dPeriod),
                stochD => _maximum.IsReady,
                () => { }
            );

            WarmUpPeriod = period;
        }

        /// <summary>
        /// 	 Creates a new <see cref="Stochastic"/> indicator from the specified inputs.
        /// </summary>
        /// <param name="period">The period given to calculate the Fast %K</param>
        /// <param name="kPeriod">The K period given to calculated the Slow %K</param>
        /// <param name="dPeriod">The D period given to calculated the Slow %D</param>
        public Stochastic(int period, int kPeriod, int dPeriod)
            : this($"STO({period},{kPeriod},{dPeriod})", period, kPeriod, dPeriod) { }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => FastStoch.IsReady && StochK.IsReady && StochD.IsReady;

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod { get; }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            _maximum.Update(time, input[HighIdx]);
            _minimum.Update(time, input[LowIdx]);
            FastStoch.Update(time, input);
            StochK.Update(time, input);
            StochD.Update(time, input);

            return FastStoch.Current;
        }

        /// <summary>
        /// 	 Computes the Fast Stochastic %K.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="input">The input.</param>
        /// <returns>The Fast Stochastics %K value.</returns>
        private double ComputeFastStoch(int period, long time, DoubleArray input) {
            var denominator = _maximum - _minimum;

            // if there's no range, just return constant zero
            if (denominator == Constants.Zero) {
                return Constants.Zero;
            }

            var numerator = input[CloseIdx] - _minimum;
            var fastStoch = _maximum.Samples >= period ? numerator / denominator : Constants.Zero;

            _sumFastK.Update(time, fastStoch);
            return fastStoch * 100;
        }

        /// <summary>
        /// 	 Computes the Slow Stochastic %K.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="constantK">The constant k.</param>
        /// <param name="input">The input.</param>
        /// <returns>The Slow Stochastics %K value.</returns>
        private double ComputeStochK(int period, int constantK, long time, DoubleArray input) {
            var stochK = _maximum.Samples >= (period + constantK - 1) ? _sumFastK / constantK : Constants.Zero;
            _sumSlowK.Update(time, stochK);
            return stochK * 100;
        }

        /// <summary>
        /// 	 Computes the Slow Stochastic %D.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="constantK">The constant k.</param>
        /// <param name="constantD">The constant d.</param>
        /// <returns>The Slow Stochastics %D value.</returns>
        private double ComputeStochD(int period, int constantK, int constantD) {
            var stochD = _maximum.Samples >= (period + constantK + constantD - 2) ? _sumSlowK / constantD : Constants.Zero;
            return stochD * 100;
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            FastStoch.Reset();
            StochK.Reset();
            StochD.Reset();
            _maximum.Reset();
            _minimum.Reset();
            _sumFastK.Reset();
            _sumSlowK.Reset();
            base.Reset();
        }
    }
}