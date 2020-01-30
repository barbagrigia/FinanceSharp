using System;
using System.Linq;
using FinanceSharp.Data;
using ScottPlot;

namespace FinanceSharp.Examples {
    public static class Plotter {
        private static (PltForm From, Plot Plot) PrepareForm() {
            var form = new PltForm();
            var plt = form.Plot.plt;
            plt.Title("Title");
            plt.YLabel("Y");
            plt.XLabel("X");
            return (form, plt);
        }

        public static void Show(DateTime[] times, DoubleArray[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(data));


            var (form, plt) = PrepareForm();

            var props = data[0].Properties;
            if (props == BarValue.Properties || props == TradeBarValue.Properties) {
                ScottPlot.OHLC[] ohlcs = data.Zip(times, (d, time) => new OHLC(d.Open, d.High, d.Low, d.Close, time)).ToArray();

                plt.PlotCandlestick(ohlcs);
                plt.Ticks(dateTimeX: true);
            } else {
                plt.PlotScatter(times.Select(t => t.ToOADate()).ToArray(), data.Select(d => d.Value).ToArray());
                plt.Ticks(dateTimeX: true, rulerModeX: true);
            }


            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(DateTime[] times, DoubleArray[][] ys) {
            if (ys == null) throw new ArgumentNullException(nameof(ys));
            if (ys.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(ys));
            if (ys[0].Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(ys));

            var (form, plt) = PrepareForm();


            foreach (var data in ys) {
                var props = data[0].Properties;
                if (props == BarValue.Properties || props == TradeBarValue.Properties) {
                    ScottPlot.OHLC[] ohlcs = data.Zip(times, (d, time) => new OHLC(d.Open, d.High, d.Low, d.Close, time)).ToArray();

                    plt.PlotCandlestick(ohlcs);
                    plt.Ticks(dateTimeX: true);
                } else {
                    plt.PlotScatter(times.Select(t => t.ToOADate()).ToArray(), data.Select(d => d.Value).ToArray());
                    plt.Ticks(dateTimeX: true, rulerModeX: true);
                }
            }


            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(double[] times, DoubleArray[] data, bool timeIsOleAut = false) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(data));

            var (form, plt) = PrepareForm();


            var props = data[0].Properties;
            if (props == BarValue.Properties || props == TradeBarValue.Properties) {
                ScottPlot.OHLC[] ohlcs = data.Zip(times, (d, time) => new OHLC(d.Open, d.High, d.Low, d.Close, time)).ToArray();

                plt.PlotCandlestick(ohlcs);
                plt.Ticks(dateTimeX: true);
            } else {
                plt.PlotScatter(times, data.Select(d => d.Value).ToArray());
                plt.Ticks(dateTimeX: timeIsOleAut, rulerModeX: true);
            }

            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(double[] times, DoubleArray[][] ys, bool timeIsOleAut = false) {
            if (ys == null) throw new ArgumentNullException(nameof(ys));
            if (ys.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(ys));
            if (ys[0].Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(ys));

            var (form, plt) = PrepareForm();

            foreach (var data in ys) {
                var props = data[0].Properties;
                if (props == BarValue.Properties || props == TradeBarValue.Properties) {
                    ScottPlot.OHLC[] ohlcs = data.Zip(times, (d, time) => new OHLC(d.Open, d.High, d.Low, d.Close, time)).ToArray();
                    plt.PlotCandlestick(ohlcs);

                    plt.Ticks(dateTimeX: true);
                } else {
                    plt.PlotScatter(times, data.Select(d => d.Value).ToArray());

                    plt.Ticks(dateTimeX: timeIsOleAut, rulerModeX: true);
                }
            }

            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(DoubleArray[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(data));
            var props = data[0].Properties;


            var form = new PltForm();
            var plt = form.Plot.plt;
            plt.Title("Title");
            plt.YLabel("Y");
            plt.XLabel("X");

            if (props == BarValue.Properties || props == TradeBarValue.Properties) {
                ScottPlot.OHLC[] ohlcs = data.Zip(DataGen.Consecutive(data.Length), (d, time) => new OHLC(d.Open, d.High, d.Low, d.Close, time)).ToArray();

                plt.PlotCandlestick(ohlcs);
                plt.Ticks(dateTimeX: true);
            } else {
                plt.PlotScatter(DataGen.Consecutive(data.Length), data.Select(d => d.Value).ToArray());
            }


            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(OHLC[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(data));

            var form = new PltForm();
            var plt = form.Plot.plt;
            plt.Title("Title");
            plt.YLabel("Y");
            plt.XLabel("X");

            plt.PlotCandlestick(data);
            plt.Ticks(dateTimeX: true);

            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(double[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(data));

            var form = new PltForm();
            var plt = form.Plot.plt;
            plt.Title("Title");
            plt.YLabel("Y");
            plt.XLabel("X");

            plt.PlotScatter(DataGen.Consecutive(data.Length), data);

            form.Plot.Render();
            form.ShowDialog();
        }

        public static void Show(double[] time, double[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(data));

            var form = new PltForm();
            var plt = form.Plot.plt;
            plt.Title("Title");
            plt.YLabel("Y");
            plt.XLabel("X");

            plt.PlotScatter(time, data);

            form.Plot.Render();
            form.ShowDialog();
        }
    }
}