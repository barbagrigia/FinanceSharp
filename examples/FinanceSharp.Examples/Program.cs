using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using FinanceSharp.Data;
using FinanceSharp.Data.Consolidators;
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
                            Value: DoubleArray.FromStruct(new TickValue(tick.Price, tick.Price, 0, tick.Price, tick.Quantity, 0))))
                        .ToArray();
                }
            }

            //build model
            (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = HeikinAshi();
            //(IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = EMAx3();
            //(IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) model = EMA();


            //feed the model data
            foreach (var tick in ticks) {
                model.Input.Update(tick.Time, tick.Value);
            }

            //plot
            Plotter.Show(
                Enumerable.Range(0, model.Datas[0].Count).Select(i => (double) i).ToArray(),
                model.Datas.Select(m => m.ToArray()).ToArray()
            );
        }

        public static (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) EMA() {
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ema = new ExponentialMovingAverage(12).Of(tickCons);

            var bars = new List<DoubleArray>();
            var emas = new List<DoubleArray>();

            //bind model
            tickCons.Updated += (time, updated) => bars.Add(updated);
            ema.Updated += (time, updated) => emas.Add(updated);

            return (tickCons, new IUpdatable[] {ema}, new List<DoubleArray>[] {bars, emas});
        }

        public static (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) EMAx3() {
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ema6 = new ExponentialMovingAverage(6).Of(tickCons);
            var ema12 = new ExponentialMovingAverage(12).Of(tickCons);
            var ema24 = new ExponentialMovingAverage(24).Of(tickCons);

            var bars = new List<DoubleArray>();
            var emas6 = new List<DoubleArray>();
            var emas12 = new List<DoubleArray>();
            var emas24 = new List<DoubleArray>();

            //bind model
            tickCons.Updated += (time, updated) => bars.Add(updated);
            ema6.Updated += (time, updated) => emas6.Add(updated);
            ema12.Updated += (time, updated) => emas12.Add(updated);
            ema24.Updated += (time, updated) => emas24.Add(updated);

            return (tickCons, new IUpdatable[] {ema6, ema12, ema24}, new List<DoubleArray>[] {bars, emas6, emas12, emas24});
        }

        public static (IUpdatable Input, IUpdatable[] Outputs, List<DoubleArray>[] Datas) HeikinAshi() {
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ha = new HeikinAshi().Of(tickCons);

            var bars = new List<DoubleArray>();
            var has = new List<DoubleArray>();

            //bind model
            tickCons.Updated += (time, updated) => bars.Add(updated);
            ha.Updated += (time, updated) => has.Add(DoubleArray.FromStruct(ha.CurrentBar) + 25d); //note that we append +25 offset so ha will be visible

            return (tickCons, new IUpdatable[] {ha}, new List<DoubleArray>[] {bars, has});
        }
    }
}