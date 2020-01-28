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
    public class AverageTrueRangeTests : CommonIndicatorTests<TradeBarVolumedValue> {
        protected override IndicatorBase CreateIndicator() {
            return new AverageTrueRange(14);
        }

        protected override string TestFileName => "spy_atr_wilder.txt";

        protected override string TestColumnName => "Average True Range 14";

        //[Test]
        //public void ComparesAgainstExternalData() {
        //    var atrSimple = new AverageTrueRange(14, MovingAverageType.Simple);
        //    TestHelper.TestIndicator(atrSimple, "spy_atr.txt", "Average True Range 14");
        //}

        [Test]
        public void ResetsProperly() {
            var atr = new AverageTrueRange(14, MovingAverageType.Simple);
            atr.Update(DateTime.Today, new TradeBarVolumedValue {
                Open = 1d,
                High = 3d,
                Low = .5d,
                Close = 2.75d,
                Volume = 1234567890
            });

            atr.Reset();

            TestHelper.AssertIndicatorIsInDefaultState(atr);
            TestHelper.AssertIndicatorIsInDefaultState(atr.TrueRange);
        }

        [Test]
        public void TrueRangePropertyIsReadyAfterOneSample() {
            var atr = new AverageTrueRange(14, MovingAverageType.Simple);
            Assert.IsFalse(atr.TrueRange.IsReady);

            atr.Update(DateTime.Today, new TradeBarVolumedValue {
                Open = 1d,
                High = 3d,
                Low = .5d,
                Close = 2.75d,
                Volume = 1234567890
            });

            Assert.IsTrue(atr.TrueRange.IsReady);
        }
    }
}