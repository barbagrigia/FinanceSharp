using NUnit.Framework;
using FinanceSharp.Indicators;
using FinanceSharp;

namespace FinanceSharp.Tests.Indicators {
    [TestFixture]
    public class AccelerationBandsTests : CommonIndicatorTests<TradeBarValue> {
        protected override IndicatorBase CreateIndicator() {
            return new AccelerationBands(period: 20, width: 4d);
        }

        protected override string TestFileName => "spy_acceleration_bands_20_4.txt";

        protected override string TestColumnName => "MiddleBand";

        [Test]
        public void ComparesWithExternalDataLowerBand() {
            var abands = CreateIndicator();
            TestHelper.TestIndicator(
                abands,
                "spy_acceleration_bands_20_4.txt",
                "LowerBand",
                (ind, expected) => Assert.AreEqual(expected, (double) ((AccelerationBands) ind).LowerBand.Current.Value,
                    delta: 1e-3, message: "Lower band test fail.")
            );
        }

        [Test]
        public void ComparesWithExternalDataUpperBand() {
            var abands = CreateIndicator();
            TestHelper.TestIndicator(
                abands,
                "spy_acceleration_bands_20_4.txt",
                "UpperBand",
                (ind, expected) => Assert.AreEqual(expected, (double) ((AccelerationBands) ind).UpperBand.Current.Value,
                    delta: 1e-4, message: "Upper band test fail.")
            );
        }
    }
}