using System.Linq;
using System.Reflection;

namespace FinanceSharp {
    public static class StructInfo<TStruct> where TStruct : struct {
        public static int DoubleFieldsCount;
        public static int FieldsCount;

        static StructInfo() {
            var observedFields = BindingFlags.Public | BindingFlags.Instance;
            DoubleFieldsCount = typeof(TStruct).GetFields(observedFields).Count(f => f.FieldType == typeof(double));
            FieldsCount = typeof(TStruct).GetFields(observedFields).Length;
        }
    }

    public static class DataStructInfo<TDataStruct> where TDataStruct : struct, DataStruct {
        public static int DoubleFieldsCount = StructInfo<TDataStruct>.DoubleFieldsCount;
        public static int FieldsCount = StructInfo<TDataStruct>.FieldsCount;
        public static int Properties;

        static DataStructInfo() {
            Properties = new TDataStruct().Properties;
        }
    }
}