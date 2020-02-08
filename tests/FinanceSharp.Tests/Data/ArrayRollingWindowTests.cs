using System;
using System.Linq;
using FinanceSharp.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    public class ArrayRollingWindowTests {
        [Test]
        public void Rolling_1Property() {
            var rolling = new ArrayRollingWindow(5, 1);
            rolling.OutputCount.Should().Be(5);
            rolling.Properties.Should().Be(1);
            rolling.InputProperties.Should().Be(1);

            rolling.Update(1, 1d);
            rolling.Current.ToArray().Should().BeEquivalentTo(1, 0, 0, 0, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(2, 2d);
            rolling.Current.ToArray().Should().BeEquivalentTo(2, 1, 0, 0, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(3, 3d);
            rolling.Current.ToArray().Should().BeEquivalentTo(3, 2, 1, 0, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();
            rolling.CurrentTime.Should().Be(3);

            rolling.Update(4, 4d);
            rolling.Current.ToArray().Should().BeEquivalentTo(4, 3, 2, 1, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(5, 5d);
            rolling.Current.ToArray().Should().BeEquivalentTo(5, 4, 3, 2, 1);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(6, 6d);
            rolling.Current.ToArray().Should().BeEquivalentTo(6, 5, 4, 3, 2);
            rolling.MostRecentlyRemoved.ToArray().Should().BeEquivalentTo(1);

            rolling.Update(6, 6d);
            rolling.Current.ToArray().Should().BeEquivalentTo(6, 6, 5, 4, 3);
            rolling.MostRecentlyRemoved.ToArray().Should().BeEquivalentTo(2);
        }

        [Test]
        public void Rolling_2Property_Downsampled() {
            var rolling = new ArrayRollingWindow(5, 1);
            rolling.OutputCount.Should().Be(5);
            rolling.Properties.Should().Be(1);
            rolling.InputProperties.Should().Be(1);
            TestStructX2 c(double v) => new TestStructX2(v, v);

            rolling.Update(1, DoubleArray.From(c(1)));
            rolling.Current.ToArray().Should().BeEquivalentTo(1, 0, 0, 0, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(2, DoubleArray.From(c(2d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(2, 1, 0, 0, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(3, DoubleArray.From(c(3d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(3, 2, 1, 0, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();
            rolling.CurrentTime.Should().Be(3);

            rolling.Update(4, DoubleArray.From(c(4d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(4, 3, 2, 1, 0);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(5, DoubleArray.From(c(5d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(5, 4, 3, 2, 1);
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(6, DoubleArray.From(c(6d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(6, 5, 4, 3, 2);
            rolling.MostRecentlyRemoved.ToArray().Should().BeEquivalentTo((1d));

            rolling.Update(7, DoubleArray.From(c(7d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(7, 6, 5, 4, 3);
            rolling.MostRecentlyRemoved.ToArray().Should().BeEquivalentTo((2d));
        }

        [Test]
        public void Rolling_2Property() {
            var rolling = new ArrayRollingWindow(5, DataStructInfo<TestStructX2>.Properties);
            rolling.OutputCount.Should().Be(5);
            rolling.Properties.Should().Be(2);
            rolling.InputProperties.Should().Be(2);
            TestStructX2 c(double v) => new TestStructX2(v, v);
            double[] flatten(params TestStructX2[] v) => v.SelectMany(val => new[] {val.Value, val.AnotherValue}).ToArray();

            rolling.Update(1, DoubleArray.From(c(1)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(1), c(0), c(0), c(0), c(0)));
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(2, DoubleArray.From(c(2d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(2d), c(1d), c(0d), c(0d), c(0d)));
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(3, DoubleArray.From(c(3d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(3d), c(2d), c(1d), c(0d), c(0d)));
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();
            rolling.CurrentTime.Should().Be(3);

            rolling.Update(4, DoubleArray.From(c(4d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(4d), c(3d), c(2d), c(1d), c(0d)));
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(5, DoubleArray.From(c(5d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(5d), c(4d), c(3d), c(2d), c(1d)));
            new Action(() => rolling.MostRecentlyRemoved.Void()).Should().Throw<InvalidOperationException>();

            rolling.Update(6, DoubleArray.From(c(6d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(6d), c(5d), c(4d), c(3d), c(2d)));
            rolling.MostRecentlyRemoved.ToArray().Should().BeEquivalentTo(flatten(c(1d)));

            rolling.Update(7, DoubleArray.From(c(6d)));
            rolling.Current.ToArray().Should().BeEquivalentTo(flatten(c(6d), c(6d), c(5d), c(4d), c(3d)));
            rolling.MostRecentlyRemoved.ToArray().Should().BeEquivalentTo(flatten(c(2d)));
        }
    }
}