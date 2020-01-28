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
using System.Linq;
using FinanceSharp.Data;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 Smooth and high sensitive moving Average. This moving average reduce lag of the information
    /// 	 but still being smooth to reduce noises.
    /// 	 Is a weighted moving average, which weights have a Normal shape;
    /// 	 the parameters Sigma and Offset affect the kurtosis and skewness of the weights respectively.
    /// 	 Source: http://www.arnaudlegoux.com/index.html
    /// </summary>
    /// <seealso cref="DoubleArray" />
    public class ArnaudLegouxMovingAverage : WindowIndicator {
        private readonly double[] _weightVector;

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod => _weightVector.Length;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="ArnaudLegouxMovingAverage" /> class.
        /// </summary>
        /// <param name="name">string - a name for the indicator</param>
        /// <param name="period">int - the number of periods to calculate the ALMA</param>
        /// <param name="sigma">
        /// 	 int - this parameter is responsible for the shape of the curve coefficients. It affects the weight vector kurtosis.
        /// </param>
        /// <param name="offset">
        /// 	 double - This parameter allows regulating the smoothness and high sensitivity of the
        /// 	 Moving Average. The range for this parameter is [0, 1]. It affects the weight vector skewness.
        /// </param>
        public ArnaudLegouxMovingAverage(string name, int period, int sigma = 6, double offset = 0.85d)
            : base(name, period) {
            if (offset < 0 || offset > 1) throw new ArgumentException($"Offset parameter range is [0,1]. Value: {offset}", "offset");

            var m = Math.Floor(offset * (period - 1));
            var s = period * 1d / sigma;

            var tmpVector = Enumerable.Range(0, period)
                .Select(i => Math.Exp(-(i - m) * (i - m) / (2 * s * s)))
                .ToArray();

            var vecSum = tmpVector.Sum();
            _weightVector = tmpVector
                .Select(i => i / vecSum).Reverse()
                .ToArray();
        }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="ArnaudLegouxMovingAverage" /> class.
        /// </summary>
        /// <param name="name">string - a name for the indicator</param>
        /// <param name="period">int - the number of periods to calculate the ALMA.</param>
        public ArnaudLegouxMovingAverage(string name, int period)
            : this(name, period, 6) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="ArnaudLegouxMovingAverage" /> class.
        /// </summary>
        /// <param name="period">int - the number of periods to calculate the ALMA</param>
        /// <param name="sigma">
        /// 	 int - this parameter is responsible for the shape of the curve coefficients. It affects the weight
        /// 	 vector kurtosis.
        /// </param>
        /// <param name="offset">
        /// 	 double -  This parameter allows regulating the smoothness and high sensitivity of the Moving
        /// 	 Average. The range for this parameter is [0, 1]. It affects the weight vector skewness.
        /// </param>
        public ArnaudLegouxMovingAverage(int period, int sigma, double offset = 0.85d)
            : this($"ALMA({period},{sigma},{offset})", period, sigma, offset) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="ArnaudLegouxMovingAverage" /> class.
        /// </summary>
        /// <param name="period">int - the number of periods to calculate the ALMA.</param>
        public ArnaudLegouxMovingAverage(int period)
            : this(period, 6) { }

        /// <summary>
        /// 	 Computes the next value for this indicator from the given state.
        /// </summary>
        /// <param name="timeWindow"></param>
        /// <param name="window">The window of data held in this indicator</param>
        /// <param name="time"></param>
        /// <param name="input">The input value to this indicator on this time step</param>
        /// <returns>
        /// 	 A new value for this indicator
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override DoubleArray Forward(IReadOnlyWindow<long> timeWindow, IReadOnlyWindow<DoubleArray> window,
            long time,
            DoubleArray input) {
            return IsReady
                ? window.Select((t, i) => t.Value * _weightVector[i]).Sum()
                : input;
        }
    }
}