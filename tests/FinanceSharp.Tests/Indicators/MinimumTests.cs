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
    public class MinimumTests : CommonIndicatorTests<IndicatorValue> {
        protected override IndicatorBase CreateIndicator() {
            return new Minimum(5);
        }

        protected override string TestFileName {
            get { return "spy_min.txt"; }
        }

        protected override string TestColumnName {
            get { return "MIN_5"; }
        }

        [Test]
        public void ComputesCorrectly() {
            var min = new Minimum(3);

            var reference = DateTime.UtcNow;

            min.Update(reference, 1d);
            Assert.AreEqual(1d, min.Current.Value);
            Assert.AreEqual(0, min.PeriodsSinceMinimum);

            min.Update(reference.AddDays(1), 2d);
            Assert.AreEqual(1d, min.Current.Value);
            Assert.AreEqual(1, min.PeriodsSinceMinimum);

            min.Update(reference.AddDays(2), -1d);
            Assert.AreEqual(-1d, min.Current.Value);
            Assert.AreEqual(0, min.PeriodsSinceMinimum);

            min.Update(reference.AddDays(3), 2d);
            Assert.AreEqual(-1d, min.Current.Value);
            Assert.AreEqual(1, min.PeriodsSinceMinimum);

            min.Update(reference.AddDays(4), 0d);
            Assert.AreEqual(-1d, min.Current.Value);
            Assert.AreEqual(2, min.PeriodsSinceMinimum);

            min.Update(reference.AddDays(5), 3d);
            Assert.AreEqual(0d, min.Current.Value);
            Assert.AreEqual(1, min.PeriodsSinceMinimum);

            min.Update(reference.AddDays(6), 2d);
            Assert.AreEqual(0d, min.Current.Value);
            Assert.AreEqual(2, min.PeriodsSinceMinimum);
        }

        [Test]
        public void ResetsProperlyMinimum() {
            var min = new Minimum(3);
            min.Update(DateTime.Today, 1d);
            min.Update(DateTime.Today.AddSeconds(1), 2d);
            min.Update(DateTime.Today.AddSeconds(2), 1d);
            Assert.IsTrue(min.IsReady);

            min.Reset();
            Assert.AreEqual(0, min.PeriodsSinceMinimum);
            TestHelper.AssertIndicatorIsInDefaultState(min);
        }
    }
}