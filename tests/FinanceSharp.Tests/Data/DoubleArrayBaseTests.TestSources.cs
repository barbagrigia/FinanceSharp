namespace FinanceSharp.Tests.Data {
    public abstract partial class DoubleArrayBaseTests {
        public object[] ScalarDataSet = {
            new object[] {5d},
            new object[] {-10d},
            new object[] {double.Epsilon},
            new object[] {double.MaxValue},
            new object[] {double.MinValue},
            new object[] {double.NaN},
            new object[] {double.PositiveInfinity},
            new object[] {double.NegativeInfinity},
        };

        public object[] DuoDataSet = {
            new object[] {5d, 6d},
            new object[] {-10d, -11d},
            new object[] {double.Epsilon, double.MaxValue},
            new object[] {double.MaxValue, double.MaxValue},
            new object[] {double.MinValue, double.MinValue},
            new object[] {double.NaN, double.NaN},
            new object[] {double.PositiveInfinity, double.PositiveInfinity},
            new object[] {double.NegativeInfinity, double.NegativeInfinity},
        };

        public object[] TripleDataSet = {
            new object[] {5d, 6d, 7d},
            new object[] {-10d, -11d, -12d},
            new object[] {double.Epsilon, double.MaxValue, 5d},
            new object[] {double.MaxValue, double.MaxValue, double.MaxValue},
            new object[] {double.MinValue, double.MinValue, double.MinValue},
            new object[] {double.NaN, double.NaN, double.NaN},
            new object[] {double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity},
            new object[] {double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity},
        };

        public object[] QuadDataSet = {
            new object[] {5d, 6d, 7d, 8d},
            new object[] {-10d, -11d, -12d, -13d},
            new object[] {double.Epsilon, double.MaxValue, 5d, 6d},
            new object[] {double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue},
            new object[] {double.MinValue, double.MinValue, double.MinValue, double.MinValue},
            new object[] {double.NaN, double.NaN, double.NaN, double.NaN},
            new object[] {double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity},
            new object[] {double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity},
        };
    }
}