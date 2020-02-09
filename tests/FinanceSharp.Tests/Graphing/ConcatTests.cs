using System;
using FinanceSharp.Graphing;
using FinanceSharp.Indicators;
using FluentAssertions;
using NUnit.Framework;
using static FinanceSharp.Tests.Helpers.IndicatorTestingHelper;

namespace FinanceSharp.Tests.Graphing {
    public class ConcatTests {
        [Test]
        public void UseCase1() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var a = Concat.OnAllUpdatedOnce(new IUpdatable[] {max1, max2});
            var b = Concat.OnAllUpdatedOnce(new IUpdatable[] {max2, max3, max3});
            var finalCruncher = Concat.OnAllUpdatedOnce(new IUpdatable[] {a, b});

            input.FeedTradeBar(200, 1, 0.1);
            Console.WriteLine(finalCruncher.Samples);
        }

        [Test]
        public void OnAllUpdatedOnce_Count2_Properties1() {
            var input = new Identity("Source");
            var max1 = new Maximum(3).Of(input);
            var max2 = new Maximum(3).Of(input);

            var a = Concat.OnAllUpdatedOnce(new IUpdatable[] {max1, max2});


            input.Feed(10, 1, 0.1);

            a.Current.Count.Should().Be(2);
            a.Current.Properties.Should().Be(1);
        }

        [Test]
        public void OnAllUpdatedOnce_Count2_Properties5() {
            var input = new Identity("Source");
            var max1 = new Maximum(3).Of(input);
            var max2 = new Maximum(3).Of(input);

            var a = Concat.OnAllUpdatedOnce(new IUpdatable[] {max1, max2});
            var ba = Concat.OnAllUpdatedOnce(new IUpdatable[] {max1, max2, a}, name: "Target");

            input.FeedTradeBar(10, 1, 0.1);
            input.Update(ba.CurrentTime + 1000, 100d);
            ba.Current.Count.Should().Be(4);
            ba.Current.Properties.Should().Be(1);
            ba.Current.AsDoubleSpan.ToArray().Should().AllBeEquivalentTo(100d);
        }

        [Test]
        public void OnAllUpdatedOnce() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var cruncher = Concat.OnAllUpdatedOnce(new IUpdatable[] {max1, max2, max3});
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
        public void OnAllUpdatedOnce_TradeBars() {
            var input = new Identity("Source");
            var ema = new ExponentialMovingAverage(12).Of(input);
            var max1 = new Maximum(6).Of(ema);
            var max2 = new Maximum(12).Of(ema);
            var max3 = new Maximum(24).Of(ema);

            var cruncher = Concat.OnAllUpdatedOnce(new IUpdatable[] {max1, max2, max3});
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

            var cruncher = Concat.OnEveryUpdate(new IUpdatable[] {max1, max2, max3});
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

            var cruncher = Concat.OnSpecificUpdate(new IUpdatable[] {max1, max2, max3}, max3, triggerMustBeReady: false);
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

            var cruncher = Concat.OnSpecificUpdate(new IUpdatable[] {max1, max2, max3}, max3, triggerMustBeReady: true);
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