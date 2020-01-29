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
using FinanceSharp.Data;

namespace FinanceSharp.Tests.Indicators {
    [TestFixture]
    public class MoneyFlowIndexTests : CommonIndicatorTests<TradeBarValue> {
        protected override IndicatorBase CreateIndicator() {
            return new MoneyFlowIndex(20);
        }

        protected override string TestFileName => "spy_mfi.txt";

        protected override string TestColumnName => "Money Flow Index 20";

        [Test]
        public void TestTradeBarsWithNoVolume() {
            var mfi = new MoneyFlowIndex(3);
            foreach (var data in TestHelper.GetDataStream(4)) {
                var tradeBar = new TradeBarValue {
                    Open = data.Value.Value,
                    Close = data.Value.Value,
                    High = data.Value.Value,
                    Low = data.Value.Value,
                    Volume = 0
                };
                mfi.Update(DateTime.Now, tradeBar);
            }

            Assert.AreEqual(mfi.Current.Value, 100.0d);
        }
    }
}