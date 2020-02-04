using System;
using FinanceSharp.Consolidators;
using NUnit.Framework;

namespace FinanceSharp.Tests {
    public class IndicatorTests {
        [Test]
        public void METHOD() {
            var tickf = new TickFactory();
            var cons = new TickConsolidator(3);
            int i = 0;

            //cons.DataConsolidated += (sender, bar) => {
            //    i++;
            //    Console.WriteLine(bar);
            //};

            //for (int j = 0; j < 9; j++) {
            //    cons.Update(tickf.NewTick);
            //}
        }
    }
}