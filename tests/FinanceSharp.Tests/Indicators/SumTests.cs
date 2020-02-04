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

using System;
using NUnit.Framework;
using FinanceSharp.Indicators;
using FinanceSharp;

namespace FinanceSharp.Tests.Indicators {
    [TestFixture]
    public class SumTests : CommonIndicatorTests<IndicatorValue> {
        protected override IndicatorBase CreateIndicator() {
            return new Sum(2);
        }

        protected override string TestFileName => "";

        protected override string TestColumnName => "";

        protected override void RunTestIndicator(IndicatorBase indicator) {
            var time = DateTime.UtcNow;

            foreach (var i in new[] {1, 2, 3}) {
                indicator.Update(time.AddDays(i), i);
            }

            Assert.AreEqual(indicator.Current.Value, 2d + 3d);
        }

        [Test]
        public override void ResetsProperly() {
            var sum = (Sum) CreateIndicator();
            RunTestIndicator(sum);

            Assert.IsTrue(sum.IsReady);

            sum.Reset();

            TestHelper.AssertIndicatorIsInDefaultState(sum);
            Assert.AreEqual(sum.Current.Value, 0d);
            sum.Update(DateTime.UtcNow, 1);
            Assert.AreEqual(sum.Current.Value, 1d);
        }
    }
}