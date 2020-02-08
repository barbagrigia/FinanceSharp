using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    public class DoubleArrayStructScalarTests : DoubleArrayBaseTests {
        public override DoubleArray CreateDefault() {
            return new DoubleArrayStructScalar<TestStructX1>();
        }

        public override DoubleArray CreateScalar1_1(double value1) {
            return new DoubleArrayStructScalar<TestStructX1>(new TestStructX1(value1));
        }

        public override DoubleArray CreateScalar1_2(double value1, double value2) {
            return new DoubleArrayStructScalar<TestStructX2>(new TestStructX2(value1, value2));
        }

        public override DoubleArray CreateArray2_1(double value1, double value2) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4) {
            return new DoubleArrayStructScalar<BarValue>(new BarValue(value1, value2, value3, value4));
        }

        public override DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateTensor4_4(double value1, double value2, double value3, double value4) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }
    }
}