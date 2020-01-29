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
    public class AroonOscillatorTests : CommonIndicatorTests<TradeBarValue> {
        protected override IndicatorBase CreateIndicator() {
            return new AroonOscillator(14, 14);
        }

        protected override string TestFileName => "spy_aroon_oscillator.txt";

        protected override string TestColumnName => "Aroon Oscillator 14";

        [Test]
        public void ResetsProperly() {
            var aroon = new AroonOscillator(3, 3);
            aroon.Update(DateTime.Today.ToEpochTime(), new TradeBarValue {
                Open = 3d,
                High = 7d,
                Low = 2d,
                Close = 5d,
                Volume = 10
            });
            aroon.Update(DateTime.Today.AddSeconds(1).ToEpochTime(), new TradeBarValue {
                Open = 3d,
                High = 7d,
                Low = 2d,
                Close = 5d,
                Volume = 10
            });
            aroon.Update(DateTime.Today.AddSeconds(2).ToEpochTime(), new TradeBarValue {
                Open = 3d,
                High = 7d,
                Low = 2d,
                Close = 5d,
                Volume = 10
            });
            Assert.IsFalse(aroon.IsReady);
            aroon.Update(DateTime.Today.AddSeconds(3).ToEpochTime(), new TradeBarValue {
                Open = 3d,
                High = 7d,
                Low = 2d,
                Close = 5d,
                Volume = 10
            });
            Assert.IsTrue(aroon.IsReady);

            aroon.Reset();
            TestHelper.AssertIndicatorIsInDefaultState(aroon);
            TestHelper.AssertIndicatorIsInDefaultState(aroon.AroonUp);
            TestHelper.AssertIndicatorIsInDefaultState(aroon.AroonDown);
        }
    }
}