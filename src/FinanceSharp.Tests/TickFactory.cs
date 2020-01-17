using System;
using FinanceSharp.Data;
using FinanceSharp.Data.Market;

namespace FinanceSharp.Tests {
    public class TickFactory {
        protected static Random rand = new Random();
        private long i = 1;
        public (long time, TickValue tick) NewTick => (i++, new TickValue(100d + rand.NextDouble(), 100d + rand.NextDouble(), 0, 100d + rand.NextDouble(), rand.Next(100, 200), 0));
    }
}