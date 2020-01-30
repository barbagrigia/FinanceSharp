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
            var tickCons = new TickConsolidator(TimeSpan.FromMinutes(1d));
            var ema = new ExponentialMovingAverage(12).Of(tickCons);

            var closes = new List<DoubleArray>();
            var emas = new List<DoubleArray>();

            //bind model
            tickCons.Updated += (time, updated) => closes.Add(updated);
            ema.Updated += (time, updated) => emas.Add(updated);

            //feed the model data
            foreach (var tick in ticks)
            {
                tickCons.Update(tick.Time, tick.Value);
            }

            //plot
            Plotter.Show(Enumerable.Range(0, emas.Count).Select(i => (double) i).ToArray(),
                new DoubleArray[2][] {
                    closes.ToArray(),
                    emas.ToArray()
                }
            );
        }
    }
}