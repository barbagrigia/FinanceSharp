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
    public class FunctionalIndicatorTests {
        [Test]
        public void ComputesDelegateCorrectly() {
            var func = new FunctionalIndicator("f", (time, data) => data.Value, @this => @this.Samples > 1, () => {
                /*no reset action required*/
            });
            func.Update(DateTime.Today, 1d);
            Assert.IsFalse(func.IsReady);
            Assert.AreEqual(1d, func.Current.Value);

            func.Update(DateTime.Today.AddSeconds(1), 2d);
            Assert.IsTrue(func.IsReady);
            Assert.AreEqual(2d, func.Current.Value);
        }

        [Test]
        public void ResetsProperly() {
            var inner = new SimpleMovingAverage(2);
            var func = new FunctionalIndicator("f", (time, data) => {
                    inner.Update(time, data);
                    return inner.Current.Value * 2;
                },
                @this => inner.IsReady,
                () => inner.Reset()
            );

            func.Update(DateTime.Today, 1d);
            func.Update(DateTime.Today.AddSeconds(1), 2d);
            Assert.IsTrue(func.IsReady);

            func.Reset();
            TestHelper.AssertIndicatorIsInDefaultState(inner);
            TestHelper.AssertIndicatorIsInDefaultState(func);
        }
    }
}