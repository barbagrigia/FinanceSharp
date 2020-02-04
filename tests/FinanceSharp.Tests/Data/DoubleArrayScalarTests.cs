using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    // ReSharper disable once UnusedMember.Global
    public class DoubleArrayScalarTests : DoubleArrayBaseTests {
        public override DoubleArray Create() {
            return new DoubleArrayScalar();
        }

        public override DoubleArray CreateScalar1_1(double value1) {
            return new DoubleArrayScalar(value1);
        }

        public override DoubleArray CreateScalar1_2(double value1, double value2) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateArray2_1(double value1, double value2) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }

        public override DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4) {
            Assert.Inconclusive("This DoubleArray is scalar only.");
            return null;
        }
    }
}