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
using FinanceSharp;
using System;
using System.Collections;
using System.Linq;

namespace FinanceSharp.Tests.Indicators {
    /// <summary>
    /// Result tested vs. Python and Excel available in http://tinyurl.com/ob5tslj
    /// </summary>
    [TestFixture]
    public class MomersionTests {
        #region Array input

        // Real AAPL minute data rounded to 2 decimals.
        private readonly double[] _prices = {
            125.99d, 125.91d, 125.75d, 125.62d, 125.54d, 125.45d, 125.47d,
            125.4d, 125.43d, 125.45d, 125.42d, 125.36d, 125.23d, 125.32d,
            125.26d, 125.31d, 125.41d, 125.5d, 125.51d, 125.41d, 125.54d,
            125.51d, 125.61d, 125.43d, 125.42d, 125.42d, 125.46d, 125.43d,
            125.4d, 125.35d
        };

        private readonly double[] _expectedMinPeriod = {
            50.00d, 50.00d, 50.00d, 50.00d, 50.00d, 50.00d, 50.00d, 50.00d, 57.14d, 62.50d,
            55.56d, 60.00d, 63.64d, 58.33d, 53.85d, 50.00d, 53.33d, 56.25d, 58.82d, 55.56d,
            52.63d, 50.00d, 45.00d, 40.00d, 40.00d, 36.84d, 38.89d, 38.89d, 44.44d, 44.44d
        };

        private readonly double[] _expectedFullPeriod = {
            50d, 50d, 50d, 50d, 50d, 50d, 50d, 50d,
            50d, 50d, 50d, 50d, 60d, 50d, 40d, 30d,
            40d, 50d, 60d, 50d, 50d, 40d, 30d, 30d,
            40d, 44.44d, 37.5d, 25d, 25d, 37.5d,
        };

        #endregion Array input

        [TestCase(7, 20)]
        [TestCase(null, 10)]
        public void ComputesCorrectly(int? minPeriod, int fullPeriod) {
            var momersion = new MomersionIndicator(minPeriod, fullPeriod);
            var expected = minPeriod.HasValue ? _expectedMinPeriod : _expectedFullPeriod;

            RunTestIndicator(momersion, expected);
        }

        [TestCase(7, 20)]
        [TestCase(null, 10)]
        public void ResetsProperly(int? minPeriod, int fullPeriod) {
            var momersion = new MomersionIndicator(minPeriod, fullPeriod);
            var expected = minPeriod.HasValue ? _expectedMinPeriod : _expectedFullPeriod;

            RunTestIndicator(momersion, expected);

            Assert.IsTrue(momersion.IsReady);

            momersion.Reset();

            TestHelper.AssertIndicatorIsInDefaultState(momersion);
        }

        private void RunTestIndicator(MomersionIndicator momersion, IEnumerable expected) {
            var time = DateTime.Now;
            var actual = new double[_prices.Length];

            for (var i = 0; i < _prices.Length; i++) {
                momersion.Update(time.AddMinutes(i).ToEpochTime(), _prices[i]);
                actual[i] = Math.Round(momersion.Current.Value, 2);

                Console.WriteLine($"Bar : {i} | {momersion}, Is ready? {momersion.IsReady}");
            }

            Assert.AreEqual(expected, actual);
        }
    }
}