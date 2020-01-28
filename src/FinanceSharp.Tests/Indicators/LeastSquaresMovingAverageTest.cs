/*
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

using NUnit.Framework;
using FinanceSharp.Indicators;
using FinanceSharp.Data;
using System;
using FinanceSharp.Data;

namespace FinanceSharp.Tests.Indicators {
    /// <summary>
    /// Result tested vs. Python available at: http://tinyurl.com/o7redso
    /// </summary>
    [TestFixture]
    public class LeastSquaresMovingAverageTest : CommonIndicatorTests<IndicatorValue> {
        protected override IndicatorBase CreateIndicator() {
            return new LeastSquaresMovingAverage(20);
        }

        protected override string TestFileName => string.Empty;

        protected override string TestColumnName => string.Empty;

        #region Array input

        // Real AAPL minute data rounded to 2 decimals.
        public static double[] Prices = {
            125.99d, 125.91d, 125.75d, 125.62d, 125.54d, 125.45d, 125.47d,
            125.4d, 125.43d, 125.45d, 125.42d, 125.36d, 125.23d, 125.32d,
            125.26d, 125.31d, 125.41d, 125.5d, 125.51d, 125.41d, 125.54d,
            125.51d, 125.61d, 125.43d, 125.42d, 125.42d, 125.46d, 125.43d,
            125.4d, 125.35d, 125.3d, 125.28d, 125.21d, 125.37d, 125.32d,
            125.34d, 125.37d, 125.26d, 125.28d, 125.16d
        };

        #endregion Array input

        #region Array expected

        public static double[] Expected = {
            125.99d, 125.91d, 125.75d, 125.62d, 125.54d, 125.45d,
            125.47d, 125.4d, 125.43d, 125.45d, 125.42d, 125.36d,
            125.23d, 125.32d, 125.26d, 125.31d, 125.41d, 125.5d,
            125.51d, 125.41d, 125.328d, 125.381d, 125.4423d, 125.4591d,
            125.4689d, 125.4713d, 125.4836d, 125.4834d, 125.4803d, 125.4703d,
            125.4494d, 125.4206d, 125.3669d, 125.3521d, 125.3214d, 125.2986d,
            125.2909d, 125.2723d, 125.2619d, 125.2224d,
        };

        #endregion Array input

        protected override void RunTestIndicator(IndicatorBase indicator) {
            var time = DateTime.Now;

            for (var i = 0; i < Prices.Length; i++) {
                indicator.Update(time.AddMinutes(i), Prices[i]);
                Assert.AreEqual(Expected[i], Math.Round(indicator.Current.Value, 4));
            }
        }

        [Test]
        public override void ResetsProperly() {
            var indicator = CreateIndicator();
            var time = DateTime.Now;

            for (var i = 0; i < 20; i++) {
                indicator.Update(time.AddMinutes(i), Prices[i]);
                Assert.AreEqual(Expected[i], Math.Round(indicator.Current.Value, 4));
            }

            Assert.IsTrue(indicator.IsReady, "LeastSquaresMovingAverage Ready");
            indicator.Reset();
            TestHelper.AssertIndicatorIsInDefaultState(indicator);
        }
    }
}