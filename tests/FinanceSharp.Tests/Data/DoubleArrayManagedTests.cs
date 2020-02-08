using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    public class DoubleArrayManagedTests : DoubleArrayBaseTests {
        public override DoubleArray CreateDefault() {
            return new DoubleArrayManaged();
        }

        public override DoubleArray CreateScalar1_1(double value1) {
            return new DoubleArrayManaged(value1);
        }

        public override DoubleArray CreateScalar1_2(double value1, double value2) {
            var d = new double[2];

            d[0] = value1;
            d[1] = value2;

            return new DoubleArrayManaged(d, 2);
        }

        public override DoubleArray CreateArray2_1(double value1, double value2) {
            var d = new double[2];

            d[0] = value1;
            d[1] = value2;

            return new DoubleArrayManaged(d, 1);
        }

        public override DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4) {
            var d = new double[4];

            d[0] = value1;
            d[1] = value2;
            d[2] = value3;
            d[3] = value4;

            return new DoubleArrayManaged(d, 4);
        }

        public override DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4) {
            var d = new double[4];

            d[0] = value1;
            d[1] = value2;
            d[2] = value3;
            d[3] = value4;

            return new DoubleArrayManaged(d, 1);
        }

        public override DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4) {
            var d = new double[4];

            d[0] = value1;
            d[1] = value2;
            d[2] = value3;
            d[3] = value4;

            return new DoubleArrayManaged(d, 2);
        }

        public override DoubleArray CreateTensor4_4(double value1, double value2, double value3, double value4) {
            var d = new double[4 * 4];
            for (int i = 0; i < 4; i++) {
                d[i * 4 + 0] = value1;
                d[i * 4 + 1] = value2;
                d[i * 4 + 2] = value3;
                d[i * 4 + 3] = value4;
            }

            return new DoubleArrayManaged(d, 4);
        }
    }
}