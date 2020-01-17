using System;
using System.Linq;
using FinanceSharp.Indicators;
using NUnit.Framework;

namespace FinanceSharp.Tests {
    public class DevTests {
        [Test]
        public unsafe void Dev() {
            var ema = new ExponentialMovingAverage(12);
            var factory = new TickFactory();
            ema.Updated += (time, updated) => { Console.WriteLine(updated.Value); };
            for (int i = 0; i < 100; i++) {
                var (time, tick) = factory.NewTick;
                ema.Update(time, tick);
            }
        }
    }
}