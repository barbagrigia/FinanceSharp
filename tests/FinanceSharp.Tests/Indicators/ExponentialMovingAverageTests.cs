﻿/*
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

using System;
using NUnit.Framework;
using FinanceSharp.Indicators;
using FinanceSharp;

namespace FinanceSharp.Tests.Indicators {
    [TestFixture]
    public class ExponentialMovingAverageTests : CommonIndicatorTests<IndicatorValue> {
        protected override IndicatorBase CreateIndicator() {
            return new ExponentialMovingAverage(14);
        }

        protected override string TestFileName => "spy_with_indicators.txt";

        protected override string TestColumnName => "EMA14";

        protected override Action<IndicatorBase, double> Assertion => TestHelper.AssertDeltaDecreases(2.5e-2);

        [Test]
        public void EmaComputesCorrectly() {
            const int period = 4;
            double[] values = {1d, 10d, 100d, 1000d};
            const double expFactor = 2d / (1d + period);

            var ema4 = new ExponentialMovingAverage(period);

            double current = 0d;
            for (int i = 0; i < values.Length; i++) {
                ema4.Update(DateTime.UtcNow.AddSeconds(i), new IndicatorValue(values[i]));
                if (i == 0) {
                    current = values[i];
                } else {
                    current = values[i] * expFactor + (1 - expFactor) * current;
                }

                Assert.AreEqual(current, ema4.Current.Value);
            }
        }

        [Test]
        public override void ResetsProperly() {
            // ema reset is just setting the value and samples back to 0
            var ema = new ExponentialMovingAverage(3);

            foreach (var data in TestHelper.GetDataStream(5)) {
                ema.Update(data);
            }

            Assert.IsTrue(ema.IsReady);
            Assert.AreNotEqual(0d, ema.Current.Value);
            Assert.AreNotEqual(0, ema.Samples);

            ema.Reset();

            TestHelper.AssertIndicatorIsInDefaultState(ema);
        }
    }
}