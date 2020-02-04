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
using System.Linq;
using Python.Runtime;

namespace FinanceSharp.Tests.Indicators {
    [TestFixture]
    public class IndicatorExtensionsTests {
        [Test]
        public void PipesDataUsingOfFromFirstToSecond() {
            var first = new SimpleMovingAverage(2);
            var second = new Delay(1);

            // this is a configuration step, but returns the reference to the second for method chaining
            second.Of(first);

            var data1 = (Time: DateTime.UtcNow, Value: new IndicatorValue(1d));
            var data2 = (Time: DateTime.UtcNow, Value: new IndicatorValue(2d));
            var data3 = (Time: DateTime.UtcNow, Value: new IndicatorValue(3d));
            var data4 = (Time: DateTime.UtcNow, Value: new IndicatorValue(4d));

            // sma has one item
            first.Update(data1.Time, data1.Value);
            Assert.IsFalse(first.IsReady);
            Assert.AreEqual(0d, second.Current.Value);

            // sma is ready, delay will repeat this value
            first.Update(data2.Time, data2.Value);
            Assert.IsTrue(first.IsReady);
            Assert.IsFalse(second.IsReady);
            Assert.AreEqual(1.5d, second.Current.Value);

            // delay is ready, and repeats its first input
            first.Update(data3.Time, data3.Value);
            Assert.IsTrue(second.IsReady);
            Assert.AreEqual(1.5d, second.Current.Value);

            // now getting the delayed data
            first.Update(data4.Time, data4.Value);
            Assert.AreEqual(2.5d, second.Current.Value);
        }

        [Test]
        public void PipesDataFirstWeightedBySecond() {
            const int period = 4;
            var value = new Identity("Value");
            var weight = new Identity("Weight");

            var third = value.WeightedBy(weight, period);

            var data = Enumerable.Range(1, 10).ToList();
            var window = Enumerable.Reverse(data).Take(period);
            var current = window.Sum(x => 2 * x * x) / (double) window.Sum(x => x);

            foreach (var item in data) {
                value.Update(DateTime.UtcNow, new IndicatorValue(Convert.ToDouble(2 * item)));
                weight.Update(DateTime.UtcNow, new IndicatorValue(Convert.ToDouble(item)));
            }

            Assert.AreEqual(current, third.Current.Value);
        }

        [Test]
        public void NewDataPushesToDerivedIndicators() {
            var identity = new Identity("identity");
            var sma = new SimpleMovingAverage(3);

            identity.Updated += (time, consolidated) => { sma.Update(time, consolidated); };

            identity.Update(DateTime.UtcNow, 1d);
            identity.Update(DateTime.UtcNow, 2d);
            Assert.IsFalse(sma.IsReady);

            identity.Update(DateTime.UtcNow, 3d);
            Assert.IsTrue(sma.IsReady);
            Assert.AreEqual(2d, sma.Current.Value);
        }

        [Test]
        public void MultiChainSMA() {
            var identity = new Identity("identity");
            var delay = new Delay(2);

            // create the SMA of the delay of the identity
            var sma = delay.Of(identity).SMA(2);

            identity.Update(DateTime.UtcNow, 1d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(sma.IsReady);

            identity.Update(DateTime.UtcNow, 2d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(sma.IsReady);

            identity.Update(DateTime.UtcNow, 3d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsFalse(sma.IsReady);

            identity.Update(DateTime.UtcNow, 4d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsTrue(sma.IsReady);

            Assert.AreEqual(1.5d, sma.Current.Value);
        }

        [Test]
        public void MultiChainEMA() {
            var identity = new Identity("identity");
            var delay = new Delay(2);

            // create the EMA of chained methods
            var ema = delay.Of(identity).EMA(2, 1);

            // Assert.IsTrue(ema. == 1);
            identity.Update(DateTime.UtcNow, 1d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(ema.IsReady);

            identity.Update(DateTime.UtcNow, 2d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(ema.IsReady);

            identity.Update(DateTime.UtcNow, 3d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsFalse(ema.IsReady);

            identity.Update(DateTime.UtcNow, 4d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsTrue(ema.IsReady);
        }

        [Test]
        public void MultiChainMAX() {
            var identity = new Identity("identity");
            var delay = new Delay(2);

            // create the MAX of the delay of the identity
            var max = delay.Of(identity).MAX(2);

            identity.Update(DateTime.UtcNow, 1d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(max.IsReady);

            identity.Update(DateTime.UtcNow, 2d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(max.IsReady);

            identity.Update(DateTime.UtcNow, 3d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsFalse(max.IsReady);

            identity.Update(DateTime.UtcNow, 4d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsTrue(max.IsReady);
        }

        [Test]
        public void MultiChainMIN() {
            var identity = new Identity("identity");
            var delay = new Delay(2);

            // create the MIN of the delay of the identity
            var min = delay.Of(identity).MIN(2);

            identity.Update(DateTime.UtcNow, 1d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(min.IsReady);

            identity.Update(DateTime.UtcNow, 2d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsFalse(delay.IsReady);
            Assert.IsFalse(min.IsReady);

            identity.Update(DateTime.UtcNow, 3d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsFalse(min.IsReady);

            identity.Update(DateTime.UtcNow, 4d);
            Assert.IsTrue(identity.IsReady);
            Assert.IsTrue(delay.IsReady);
            Assert.IsTrue(min.IsReady);
        }

        [Test]
        public void PlusAddsLeftAndRightAfterBothUpdated() {
            var left = new Identity("left");
            var right = new Identity("right");
            var composite = left.Plus(right);

            left.Update(DateTime.Today, 1d);
            right.Update(DateTime.Today, 1d);
            Assert.AreEqual(2d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(2d, composite.Current.Value);

            left.Update(DateTime.Today, 3d);
            Assert.AreEqual(2d, composite.Current.Value);

            right.Update(DateTime.Today, 4d);
            Assert.AreEqual(7d, composite.Current.Value);
        }

        [Test]
        public void PlusAddsLeftAndConstant() {
            var left = new Identity("left");
            var composite = left.Plus(5);

            left.Update(DateTime.Today, 1d);
            Assert.AreEqual(6d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(7d, composite.Current.Value);
        }

        [Test]
        public void MinusSubtractsLeftAndRightAfterBothUpdated() {
            var left = new Identity("left");
            var right = new Identity("right");
            var composite = left.Minus(right);

            left.Update(DateTime.Today, 1d);
            right.Update(DateTime.Today, 1d);
            Assert.AreEqual(0d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(0d, composite.Current.Value);

            left.Update(DateTime.Today, 3d);
            Assert.AreEqual(0d, composite.Current.Value);

            right.Update(DateTime.Today, 4d);
            Assert.AreEqual(-1d, composite.Current.Value);
        }

        [Test]
        public void MinusSubtractsLeftAndConstant() {
            var left = new Identity("left");
            var composite = left.Minus(1);

            left.Update(DateTime.Today, 1d);
            Assert.AreEqual(0d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(1d, composite.Current.Value);
        }

        [Test]
        public void OverDividesLeftAndRightAfterBothUpdated() {
            var left = new Identity("left");
            var right = new Identity("right");
            var composite = left.Over(right);

            left.Update(DateTime.Today, 1d);
            right.Update(DateTime.Today, 1d);
            Assert.AreEqual(1d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(1d, composite.Current.Value);

            left.Update(DateTime.Today, 3d);
            Assert.AreEqual(1d, composite.Current.Value);

            right.Update(DateTime.Today, 4d);
            Assert.AreEqual(3d / 4d, composite.Current.Value);
        }

        [Test]
        public void OverDividesLeftAndConstant() {
            var left = new Identity("left");
            var composite = left.Over(2);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(1d, composite.Current.Value);

            left.Update(DateTime.Today, 4d);
            Assert.AreEqual(2d, composite.Current.Value);
        }

        [Test]
        public void OverHandlesDivideByZero() {
            var left = new Identity("left");
            var right = new Identity("right");
            var composite = left.Over(right);
            var updatedEventFired = false;
            composite.Updated += delegate { updatedEventFired = true; };

            left.Update(DateTime.Today, 1d);
            Assert.IsFalse(updatedEventFired);
            right.Update(DateTime.Today, 0d);
            Assert.IsFalse(updatedEventFired);

            // submitting another update to right won't cause an update without corresponding update to left
            right.Update(DateTime.Today, 1d);
            Assert.IsFalse(updatedEventFired);
            left.Update(DateTime.Today, 1d);
            Assert.IsTrue(updatedEventFired);
        }

        [Test]
        public void TimesMultipliesLeftAndRightAfterBothUpdated() {
            var left = new Identity("left");
            var right = new Identity("right");
            var composite = left.Times(right);

            left.Update(DateTime.Today, 1d);
            right.Update(DateTime.Today, 1d);
            Assert.AreEqual(1d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(1d, composite.Current.Value);

            left.Update(DateTime.Today, 3d);
            Assert.AreEqual(1d, composite.Current.Value);

            right.Update(DateTime.Today, 4d);
            Assert.AreEqual(12d, composite.Current.Value);
        }

        [Test]
        public void TimesMultipliesLeftAndConstant() {
            var left = new Identity("left");
            var composite = left.Times(10);

            left.Update(DateTime.Today, 1d);
            Assert.AreEqual(10d, composite.Current.Value);

            left.Update(DateTime.Today, 2d);
            Assert.AreEqual(20d, composite.Current.Value);
        }

        [Test]
        public void WorksForIndicatorsOfDifferentTypes() {
            var indicatorA1 = new TestIndicatorA("1");
            var indicatorA2 = new TestIndicatorA("2");

            indicatorA1.Over(indicatorA2);
            indicatorA1.Minus(indicatorA2);
            indicatorA1.Times(indicatorA2);
            indicatorA1.Plus(indicatorA2);
            indicatorA1.Of(indicatorA2);

            var indicatorB1 = new TestIndicatorB("1");
            var indicatorB2 = new TestIndicatorB("2");
            indicatorB1.Over(indicatorB2);
            indicatorB1.Minus(indicatorB2);
            indicatorB1.Times(indicatorB2);
            indicatorB1.Plus(indicatorB2);
            indicatorB1.Of(indicatorB2);
        }


        private class TestIndicatorA : IndicatorBase {
            public TestIndicatorA(string name) : base(name) { }
            public override bool IsReady { get; }


            protected override DoubleArray Forward(long time, DoubleArray input) {
                throw new NotImplementedException();
            }
        }

        private class TestIndicatorB : IndicatorBase {
            public TestIndicatorB(string name) : base(name) { }

            public override bool IsReady {
                get { throw new NotImplementedException(); }
            }

            /// <summary>
            /// 	 Computes the next value of this indicator from the given state
            /// </summary>
            /// <param name="time"></param>
            /// <param name="input">The input given to the indicator</param>
            /// <returns>A new value for this indicator</returns>
            protected override DoubleArray Forward(long time, DoubleArray input) {
                throw new NotImplementedException();
            }
        }
    }
}