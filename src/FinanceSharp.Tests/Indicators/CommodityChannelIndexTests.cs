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
using FinanceSharp.Data;
using NUnit.Framework;
using FinanceSharp.Indicators;
using FinanceSharp.Data;

namespace FinanceSharp.Tests.Indicators {
    [TestFixture]
    public class CommodityChannelIndexTests : CommonIndicatorTests<TradeBarVolumedValue> {
        protected override IndicatorBase CreateIndicator() {
            return new CommodityChannelIndex(14);
        }

        protected override string TestFileName => "spy_with_cci.txt";

        protected override string TestColumnName => "Commodity Channel Index (CCI) 14";

        protected override Action<IndicatorBase, double> Assertion =>
            (indicator, expected) =>
                Assert.AreEqual(expected, (double) indicator.Current.Value, 1e-2);

        [Test]
        public void ResetsProperly() {
            var cci = new CommodityChannelIndex(2);
            cci.Update(DateTime.Today, new TradeBarVolumedValue {
                Open = 3d,
                High = 7d,
                Low = 2d,
                Close = 5d,
                Volume = 10
            });
            Assert.IsFalse(cci.IsReady);
            cci.Update(DateTime.Today.AddSeconds(1), new TradeBarVolumedValue {
                Open = 3d,
                High = 7d,
                Low = 2d,
                Close = 5d,
                Volume = 10
            });
            Assert.IsTrue(cci.IsReady);

            cci.Reset();
            TestHelper.AssertIndicatorIsInDefaultState(cci);
            TestHelper.AssertIndicatorIsInDefaultState(cci.TypicalPriceAverage);
            TestHelper.AssertIndicatorIsInDefaultState(cci.TypicalPriceMeanDeviation);
        }
    }
}