using System;
using FinanceSharp.Graphing;
using FinanceSharp.Indicators;
using FluentAssertions;
using NUnit.Framework;
using static FinanceSharp.Tests.Helpers.IndicatorTestingHelper;

namespace FinanceSharp.Tests.Graphing {
    public class CruncherTests {
        //TODO: test cruncher when properties > 1.

        [Test]
        public void OnAllUpdatedOnce() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var cruncher = Cruncher.OnAllUpdatedOnce(new IUpdatable[] {max1, max2, max3});
            cruncher.Samples.Should().Be(0);
            max1.Feed(6, 1d, 0.1);
            cruncher.Samples.Should().Be(0);
            max2.Feed(12, 1d, 0.1);
            cruncher.Samples.Should().Be(0);
            max3.Feed(24, 1d, 0.1);
            cruncher.Samples.Should().Be(1);
            max1.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(1);
            max3.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(1);
            max2.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(2);
        }

        [Test]
        public void OnEveryUpdate() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var cruncher = Cruncher.OnEveryUpdate(new IUpdatable[] {max1, max2, max3});
            cruncher.Samples.Should().Be(0);
            input.FeedToReady(mustBeReady: new IUpdatable[] {max1, max2, max3});
            var baseSamples = cruncher.Samples;
            cruncher.Samples.Should().Be(baseSamples);
            Console.WriteLine($"{cruncher.Samples}");

            max1.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(baseSamples + 1);
            max3.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(baseSamples + 2);
            max2.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(baseSamples + 3);
            max2.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(baseSamples + 4);
            max1.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(baseSamples + 5);
            Console.WriteLine($"{cruncher.Samples}");
        }

        [Test]
        public void OnSpecificUpdate_triggerMustBeReady_False() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var cruncher = Cruncher.OnSpecificUpdate(new IUpdatable[] {max1, max2, max3}, max3, triggerMustBeReady: false);
            cruncher.Samples.Should().Be(0);
            //input.FeedToReady(mustBeReady: new IUpdatable[] {max1, max2, max3});
            var baseSamples = cruncher.Samples;
            cruncher.Samples.Should().Be(baseSamples);
            Console.WriteLine($"{cruncher.Samples}");

            cruncher.Samples.Should().Be(0);
            max1.Feed(6, 1d, 0.1);
            cruncher.Samples.Should().Be(0);
            max2.Feed(12, 1d, 0.1);
            cruncher.Samples.Should().Be(0);
            max3.Feed(24, 1d, 0.1);
            cruncher.Samples.Should().Be(24);
            max1.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(24);
            max3.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(25);
            max2.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(25);
            Console.WriteLine($"{cruncher.Samples}");
        }

        [Test]
        public void OnSpecificUpdate_triggerMustBeReady_True() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var cruncher = Cruncher.OnSpecificUpdate(new IUpdatable[] {max1, max2, max3}, max3, triggerMustBeReady: true);
            cruncher.Samples.Should().Be(0);
            //input.FeedToReady(mustBeReady: new IUpdatable[] {max1, max2, max3});
            var baseSamples = cruncher.Samples;
            cruncher.Samples.Should().Be(baseSamples);
            Console.WriteLine($"{cruncher.Samples}");

            cruncher.Samples.Should().Be(0);
            max1.Feed(6, 1d, 0.1);
            cruncher.Samples.Should().Be(0);
            max2.Feed(12, 1d, 0.1);
            cruncher.Samples.Should().Be(0);
            max3.Feed(24, 1d, 0.1);
            cruncher.Samples.Should().Be(1);
            max1.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(1);
            max3.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(2);
            max2.Feed(1, 1d, 0.1);
            cruncher.Samples.Should().Be(2);
            Console.WriteLine($"{cruncher.Samples}");
        }
    }
}