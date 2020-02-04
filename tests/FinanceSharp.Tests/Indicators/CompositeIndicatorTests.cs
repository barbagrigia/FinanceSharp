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
    public class CompositeIndicatorTests {
        [Test]
        public void CompositeIsReadyWhenBothAre() {
            var left = new Delay(1);
            var right = new Delay(2);
            var composite = new CompositeIndicator(left, right, (l, r) => l + r);

            left.Update(DateTime.Today.AddSeconds(0), 1d);
            right.Update(DateTime.Today.AddSeconds(0), 1d);
            Assert.IsFalse(composite.IsReady);
            Assert.IsFalse(composite.Left.IsReady);
            Assert.IsFalse(composite.Right.IsReady);

            left.Update(DateTime.Today.AddSeconds(1), 2d);
            right.Update(DateTime.Today.AddSeconds(1), 2d);
            Assert.IsFalse(composite.IsReady);
            Assert.IsTrue(composite.Left.IsReady);
            Assert.IsFalse(composite.Right.IsReady);

            left.Update(DateTime.Today.AddSeconds(2), 3d);
            right.Update(DateTime.Today.AddSeconds(2), 3d);
            Assert.IsTrue(composite.IsReady);
            Assert.IsTrue(composite.Left.IsReady);
            Assert.IsTrue(composite.Right.IsReady);

            left.Update(DateTime.Today.AddSeconds(3), 4d);
            right.Update(DateTime.Today.AddSeconds(3), 4d);
            Assert.IsTrue(composite.IsReady);
            Assert.IsTrue(composite.Left.IsReady);
            Assert.IsTrue(composite.Right.IsReady);
        }

        [Test]
        public void CallsDelegateCorrectly() {
            var left = new Identity("left");
            var right = new Identity("right");
            var composite = new CompositeIndicator(left, right, (l, r) => {
                Assert.AreEqual(left, l);
                Assert.AreEqual(right, r);
                return l + r;
            });

            left.Update(DateTime.Today, 1d);
            right.Update(DateTime.Today, 1d);
            Assert.AreEqual(2d, composite.Current.Value);
        }

        [Test]
        public void ResetsProperly() {
            var left = new Maximum("left", 2);
            var right = new Minimum("right", 2);
            var composite = new CompositeIndicator(left, right, (l, r) => l + r);

            left.Update(DateTime.Today, 1d);
            right.Update(DateTime.Today, -1d);

            left.Update(DateTime.Today.AddDays(1), -1d);
            right.Update(DateTime.Today.AddDays(1), 1d);

            Assert.AreEqual(left.PeriodsSinceMaximum, 1);
            Assert.AreEqual(right.PeriodsSinceMinimum, 1);

            composite.Reset();
            TestHelper.AssertIndicatorIsInDefaultState(composite);
            TestHelper.AssertIndicatorIsInDefaultState(left);
            TestHelper.AssertIndicatorIsInDefaultState(right);
            Assert.AreEqual(left.PeriodsSinceMaximum, 0);
            Assert.AreEqual(right.PeriodsSinceMinimum, 0);
        }
    }
}