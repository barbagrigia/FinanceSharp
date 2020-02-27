using System;
using FinanceSharp.Indicators;
using FinanceSharp.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    public class DoubleArrayTests {
        [Test]
        public void ARange() {
            var lhs = DoubleArray.ARange(0, 10, 1);
            lhs.AsDoubleSpan.ToArray().Should().BeEquivalentTo(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            lhs.Properties.Should().Be(1);
            lhs.Count.Should().Be(10);
        }

        [Test]
        public void WindowMathOperators() {
            var input = new Identity("Input");
            input.Updated += (time, updated) => Console.WriteLine(updated);

            var close = input.Select(value => value.Close, false);
            var closeWin = close.Delay(1).ArrayWindow(60);

            var C0_minus_Cn = close.Minus(closeWin, CompositionMethod.OnAnyUpdated); //TODO: verify that this outputs shape (closeWin.Count, 1) when computed!. then remove it

            input.Feed(3, 1, 0.1, timeStepMilliseconds: 1000);
            Console.WriteLine(C0_minus_Cn.Current);

            input.Current.Value.Should().BeApproximately(1.2);
            C0_minus_Cn.Samples.Should().Be(5);
            C0_minus_Cn.Current.ToString().Should()
                .Be("[0.1, 0.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2," +
                    " 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, " +
                    "1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, " +
                    "1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, " +
                    "1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2]");
        }
    }
}