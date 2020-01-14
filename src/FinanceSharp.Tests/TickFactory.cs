using System;
using FinanceSharp.Data.Market;

namespace FinanceSharp.Tests {
    public class TickFactory {
        protected static Random rand = new Random();
        private int i = 1;
        public Tick NewTick => new Tick(new DateTime(2000, 1, 1, 0, 0, 0) + TimeSpan.FromSeconds(i++), (100d + rand.NextDouble()), (100d + rand.NextDouble()), (100d + rand.NextDouble()));
    }
}