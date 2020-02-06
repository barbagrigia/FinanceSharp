using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    public class DoubleArray2DManagedTests : DoubleArrayBaseTests {
        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void JaggedArray(double value1, double value2, double value3, double value4) {
            var d = new double[2][];
            d[0] = new[] {value1, value2};
            d[1] = new[] {value3, value4};

            var arr = new DoubleArray2DManaged(d);
            arr.ToArray().Should().BeEquivalentTo(value1, value2, value3, value4);
        }

        public override DoubleArray CreateDefault() {
            return new DoubleArray2DManaged();
        }

        public override DoubleArray CreateScalar1_1(double value1) {
            return new DoubleArray2DManaged(new double[1, 1] {{value1}});
        }

        public override DoubleArray CreateScalar1_2(double value1, double value2) {
            var d = new double[1, 2];

            d[0, 0] = value1;
            d[0, 1] = value2;

            return new DoubleArray2DManaged(d);
        }

        public override DoubleArray CreateArray2_1(double value1, double value2) {
            var d = new double[2, 1];

            d[0, 0] = value1;
            d[1, 0] = value2;

            return new DoubleArray2DManaged(d);
        }

        public override DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4) {
            var d = new double[1, 4];

            d[0, 0] = value1;
            d[0, 1] = value2;
            d[0, 2] = value3;
            d[0, 3] = value4;

            return new DoubleArray2DManaged(d);
        }

        public override DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4) {
            var d = new double[4, 1];

            d[0, 0] = value1;
            d[1, 0] = value2;
            d[2, 0] = value3;
            d[3, 0] = value4;

            return new DoubleArray2DManaged(d);
        }

        public override DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4) {
            var d = new double[2, 2];

            d[0, 0] = value1;
            d[0, 1] = value2;
            d[1, 0] = value3;
            d[1, 1] = value4;

            return new DoubleArray2DManaged(d);
        }
    }
}