/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;

namespace FinanceSharp.Data {
    public delegate double BinaryFunctionHandler(double lhs, double rhs);

    public delegate double UnaryFunctionHandler(double lhs);

    public static class DoubleArrayMathExt {
        /// <summary>
        ///     Performs a binary function on lhs and rhs with broadcasting support.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static unsafe DoubleArray Function(this DoubleArray lhs, DoubleArray rhs, BinaryFunctionHandler function) {
            if (lhs.IsScalar && rhs.IsScalar)
                return function(*lhs.Address, *rhs.Address);

            DoubleArray ret;
            if (lhs.IsScalar) {
                ret = rhs.Clone();
                var len = ret.Count;
                var dstAndLhsAddr = ret.Address;
                var rhsAddr = rhs.Address;
                var propsRhs = rhs.Properties;
                var lhs_val = lhs.Value;
                for (int i = 0; i < len; i++) {
                    var offset = i * propsRhs;
                    dstAndLhsAddr[offset] = function(lhs_val, rhsAddr[i * propsRhs]);
                }
            } else if (rhs.IsScalar) {
                ret = lhs.Clone();
                var len = ret.Count;
                var dstAndLhsAddr = ret.Address;
                var lhsAddr = lhs.Address;
                var propsLhs = lhs.Properties;
                var rhsVal = rhs.Value;
                for (int i = 0; i < len; i++) {
                    var offset = i * propsLhs;
                    dstAndLhsAddr[offset] = function(lhsAddr[offset], rhsVal);
                }
            } else {
                ret = lhs.Clone();
                var len = ret.Count;
                var dstAndLhsAddr = ret.Address;
                var rhsAddr = rhs.Address;
                var propsLhs = lhs.Properties;
                var propsRhs = rhs.Properties;
                for (int i = 0; i < len; i++) {
                    var offset = i * propsLhs;
                    dstAndLhsAddr[offset] = function(dstAndLhsAddr[offset], rhsAddr[i * propsRhs]);
                }
            }

            return ret;
        }

        public static unsafe DoubleArray Function(this DoubleArray lhs, DoubleArray rhs, int property, BinaryFunctionHandler function) {
            if (lhs.IsScalar && rhs.IsScalar)
                return lhs[property] * rhs[property];

            DoubleArray ret;
            if (lhs.IsScalar) {
                ret = rhs.Clone();
                var len = ret.Count;
                var dstAndLhsAddr = ret.Address;
                var rhsAddr = rhs.Address;
                var propsRhs = rhs.Properties;
                var lhs_val = lhs.Value;
                for (int i = 0; i < len; i++) {
                    var offset = i * propsRhs + property;
                    dstAndLhsAddr[offset] = function(lhs_val, rhsAddr[i * propsRhs + property]);
                }
            } else if (rhs.IsScalar) {
                ret = lhs.Clone();
                var len = ret.Count;
                var dstAndLhsAddr = ret.Address;
                var lhsAddr = lhs.Address;
                var propsLhs = lhs.Properties;
                var rhsVal = rhs.Value;
                for (int i = 0; i < len; i++) {
                    var offset = i * propsLhs + property;
                    dstAndLhsAddr[offset] = function(lhsAddr[offset], rhsVal);
                }
            } else {
                ret = lhs.Clone();
                var len = ret.Count;
                var dstAndLhsAddr = ret.Address;
                var rhsAddr = rhs.Address;
                var propsLhs = lhs.Properties;
                var propsRhs = rhs.Properties;
                for (int i = 0; i < len; i++) {
                    var offset = i * propsLhs + property;
                    dstAndLhsAddr[offset] = function(dstAndLhsAddr[offset], rhsAddr[i * propsRhs + property]);
                }
            }

            return ret;
        }
    }

    public unsafe partial class DoubleArray {
        public DoubleArray Function(int property, UnaryFunctionHandler function, bool copy = false) {
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

        public DoubleArray Function(UnaryFunctionHandler function, bool copy = false) {
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
        public DoubleArray Function<TStruct>(ManipulateStructHandler<TStruct> function, bool copy = false) where TStruct : unmanaged, DataStruct {
            var @this = copy ? this.Clone() : this;
            var len = @this.Count;
            var ptr = (TStruct*) @this.Address;
            for (int i = 0; i < len; i++) {
                function(ptr++);
            }

            return @this;
        }

        public DoubleArray Sum(int property, UnaryFunctionHandler function, bool copy = false) {
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