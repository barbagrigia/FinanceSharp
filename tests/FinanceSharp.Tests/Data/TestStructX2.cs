using System.Diagnostics;

namespace FinanceSharp.Tests.Data {
    [DebuggerDisplay("{" + nameof(Value) + ("}, {" + nameof(AnotherValue) + "}"))]
    public struct TestStructX2 : DataStruct {
        public const int Properties = 2;
        public double Value;
        public double AnotherValue;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TestStructX2(double value, double anotherValue) {
            Value = value;
            AnotherValue = anotherValue;
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return new TestStructX2() {Value = Value, AnotherValue = AnotherValue};
        }

        /// <summary>
        ///     The number of properties this struct has, equivalent to sizeof(this)/sizeof(double).
        /// </summary>
        int DataStruct.Properties => Properties;

        /// <summary>
        ///     Returns the default value of given DataStruct.
        /// </summary>
        double DataStruct.Value {
            get => Value;
            set => Value = value;
        }
    }
}