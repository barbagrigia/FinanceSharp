using System.Diagnostics;

namespace FinanceSharp.Tests.Data {
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public struct TestStructX1 : DataStruct {
        public const int Properties = 1;
        public double Value;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TestStructX1(double value) {
            Value = value;
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return new TestStructX1() {Value = Value};
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