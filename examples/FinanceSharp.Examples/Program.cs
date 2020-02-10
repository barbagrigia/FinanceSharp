using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using FinanceSharp;
using FinanceSharp.Consolidators;
using FinanceSharp.Examples.Data;
using FinanceSharp.Indicators;
using Ionic.Zip;

namespace FinanceSharp.Examples {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            //load data
            (long Time, DoubleArray Value)[] ticks;
            using (var zip = new ZipFile("./Data/BTCUSDT.2018-10-04.csv.zip"))
            using (var zipfile = zip.Entries.First().OpenReader())
            using (var reader = new StreamReader(zipfile)) {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                    csv.Configuration.PrepareHeaderForMatch = (string header, int index) =>
                        char.ToUpperInvariant(header[0]) + header.Substring(1);
                    csv.Configuration.MissingFieldFound = null;
                    ticks = csv.GetRecords<BinanceTick>()
                        .Select(tick => (
                            Time: (long) (tick.time * 1000d), //we add 3 zeros to turn it from seconds to milliseconds.
                            Value: DoubleArray.From(new TickValue(tick.Price, tick.Price, 0, tick.Price, tick.Quantity, 0))))
                        .ToArray();
                }
            }

            //build model
            (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = MuhModel.Create();
            //(IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = EMAx3();
            //(IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = HeikinAshi();
            //(IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = EMA();


            //feed the model data
            for (var i = 0; i < ticks.Length; i++) {
                var tick = ticks[i];
                model.Input.Update(tick.Time, tick.Value);
            }

            //plot
            var x = Enumerable.Range(0, model.Datas.Min(l => l.Count)).Select(i => (double) i).ToArray();
            var y = model.Datas.Select(m => m.Take(x.Length).ToArray()).ToArray();
            Plotter.Show(
                x, y
            );
        }

        public static (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) EMA() {
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ema = new ExponentialMovingAverage(12).Of(tickCons);

            var bars = tickCons.ThenToList();
            var emas = ema.ThenToList();

            return (tickCons, new IUpdatable[] {ema}, new List<DoubleArray>[] {bars, emas});
        }

        public static (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) EMAx3() {
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ema6 = tickCons.Then(new ExponentialMovingAverage(6));
            var ema12 = tickCons.Then(new ExponentialMovingAverage(12));
            var ema24 = tickCons.Then(new ExponentialMovingAverage(24));

            var bars = tickCons.ThenToList();
            var emas6 = ema6.ThenToList();
            var emas12 = ema12.ThenToList();
            var emas24 = ema24.ThenToList();

            return (tickCons, new IUpdatable[] {ema6, ema12, ema24}, new List<DoubleArray>[] {bars, emas6, emas12, emas24});
        }

        public static (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) HeikinAshi() {
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ha = new HeikinAshi().Of(tickCons);

            var bars = tickCons.ThenToList();
            var has = new List<DoubleArray>();

            //bind model manually
            ha.Updated += (time, updated) => has.Add(DoubleArray.From(ha.CurrentBar) + 25d); //note that we append +25 offset so ha will be visible

            return (tickCons, new IUpdatable[] {ha}, new List<DoubleArray>[] {bars, has});
        }
    }
}