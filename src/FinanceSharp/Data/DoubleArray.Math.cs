using System;

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray {
        public DoubleArray Select(int property, Func<double, double> function, bool copy = false) {
            var @this = copy ? this.Clone() : this;
            var len = @this.Count;
            var ptr = @this.Address;
            var props = @this.Properties;
            for (int i = 0; i < len; i++) {
                var offset = i * props + property;
                ptr[offset] = function(ptr[offset]);
            }

            return @this;
        }

        public DoubleArray Select(Func<double, double> function, bool copy = false) {
            var @this = copy ? this.Clone() : this;
            var len = @this.Count * @this.Properties;
            var ptr = @this.Address;
            for (int i = 0; i < len; i++, ptr++)
                *ptr = function(*ptr);

            return @this;
        }

        /// <summary>
        ///     Performs a function on the entire array
        /// </summary>
        /// <typeparam name="TStruct"></typeparam>
        /// <param name="function"></param>
        public DoubleArray Select<TStruct>(ManipulateStructHandler<TStruct> function, bool copy = false) where TStruct : unmanaged, DataStruct {
            var @this = copy ? this.Clone() : this;
            var len = @this.Count;
            var ptr = (TStruct*) @this.Address;
            for (int i = 0; i < len; i++) {
                function(ptr++);
            }

            return @this;
        }

        public DoubleArray Sum(int property, Func<double, double> function, bool copy = false) {
            var @this = copy ? this.Clone() : this;
            var len = @this.Count;
            var ptr = @this.Address;
            var props = @this.Properties;
            for (int i = 0; i < len; i++) {
                var offset = i * props + property;
                ptr[offset] = function(ptr[offset]);
            }

            return @this;
        }

        public double Sum(int property) {
            var len = Count;
            var ptr = Address + property;
            var props = Properties;
            double sum = 0;
            for (int i = 0; i < len; i++, ptr += props)
                sum += *ptr;

            return sum;
        }

        public double Sum() {
            var len = Count * Properties;
            var ptr = Address;
            double sum = 0;
            for (int i = 0; i < len; i++, ptr++)
                sum += *ptr;

            return sum;
        }

        public double Mean(int property) {
            return Sum(property) / Count;
        }

        public double Mean() {
            return Sum() / Count;
        }

        public double Median(int property) {
            return Address[((Count + 1) / 2) * Properties + property];
        }

        public double Median() {
            return Address[(Count + 1) / 2];
        }
    }
}