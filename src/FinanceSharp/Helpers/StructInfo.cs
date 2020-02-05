using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace FinanceSharp {
    /// <summary>
    ///     Provides staticly cached variables about <typeparamref name="TStruct"/>.
    /// </summary>
    /// <typeparam name="TStruct">The type to provide info about.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static class StructInfo<TStruct> where TStruct : struct {
        /// <summary>
        ///     Number of public fields of type <see cref="double"/> in given <typeparamref name="TStruct"/>.
        /// </summary>
        public static readonly int DoubleFieldsCount;
        /// <summary>
        ///     Number of fields of type <see cref="double"/> in given <typeparamref name="TStruct"/>.
        /// </summary>
        public static readonly int FieldsCount;

        static StructInfo() {
            var observedFields = BindingFlags.Public | BindingFlags.Instance;
            DoubleFieldsCount = typeof(TStruct).GetFields(observedFields).Count(f => f.FieldType == typeof(double));
            FieldsCount = typeof(TStruct).GetFields(observedFields).Length;
        }
    }

    /// <summary>
    ///     Provides staticly cached variables about <typeparamref name="TDataStruct"/>.
    /// </summary>
    /// <typeparam name="TDataStruct">The type to provide info about.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static class DataStructInfo<TDataStruct> where TDataStruct : struct, DataStruct {
        public static int DoubleFieldsCount = StructInfo<TDataStruct>.DoubleFieldsCount;
        public static int FieldsCount = StructInfo<TDataStruct>.FieldsCount;
        /// <summary>
        ///     Number of Properties in type <typeparamref name="TDataStruct"/>.
        ///     The value is cached staticly.
        /// </summary>
        public static readonly int Properties;

        static DataStructInfo() {
            Properties = new TDataStruct().Properties;
        }
    }
}