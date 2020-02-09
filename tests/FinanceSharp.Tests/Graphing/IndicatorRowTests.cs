using System;
using FinanceSharp.Graphing;
using FinanceSharp.Indicators;
using FluentAssertions;
using NUnit.Framework;
using static FinanceSharp.Tests.Helpers.IndicatorTestingHelper;

namespace FinanceSharp.Tests.Graphing {
    public class IndicatorRowTests
    {
        [Test]
        public void UseCase1() {
            var input = new Identity("Source");

            var periods = new[] {6, 12, 24};

            var ema = new ExponentialMovingAverage(12).Of(input);
            var row = IndicatorRow.OnAllUpdatedOnce(3, index => new Maximum(periods[index])).Of(ema);
            row.Concatenating.Length.Should().Be(3);
            row.Updated += (time, updated) => { Console.WriteLine(updated); };

            input.FeedTradeBar(200, 1, 0.1);
            Console.WriteLine(row.Samples);
        }
    }
}