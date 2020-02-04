using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    public class DoubleArrayStructTests : DoubleArrayBaseTests {
        public override DoubleArray Create() {
            return new DoubleArrayStruct<TestStructX1>();
        }

        public override DoubleArray CreateScalar1_1(double value1) {
            return new DoubleArrayStruct<TestStructX1>(new TestStructX1(value1));
        }

        public override DoubleArray CreateScalar1_2(double value1, double value2) {
            return new DoubleArrayStruct<TestStructX2>(new TestStructX2(value1, value2));
        }

        public override DoubleArray CreateArray2_1(double value1, double value2) {
            return new DoubleArrayStruct<TestStructX1>(new TestStructX1(value1), new TestStructX1(value2));
        }

        public override DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4) {
            return new DoubleArrayStruct<BarValue>(new BarValue(value1, value2, value3, value4));
        }

        public override DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4) {
            return new DoubleArrayStruct<TestStructX1>(new TestStructX1(value1), new TestStructX1(value2), new TestStructX1(value3), new TestStructX1(value4));
        }

        public override DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4) {
            return new DoubleArrayStruct<TestStructX2>(new TestStructX2(value1, value2), new TestStructX2(value3, value4));
        }
    }
}