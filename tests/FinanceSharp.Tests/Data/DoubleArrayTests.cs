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
    }
}