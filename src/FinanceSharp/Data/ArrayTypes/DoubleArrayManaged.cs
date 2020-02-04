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
using System.Linq;
using System.Runtime.CompilerServices;

namespace FinanceSharp {
    public unsafe class DoubleArrayManaged : DoubleArray {
        protected internal double[] values;

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayManaged(params double[] values) : this(values, 1) { }

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayManaged(double[] values, int properties) : base(values.Length / properties, properties) {
            this.values = values;
        }

        public DoubleArrayManaged() : this(new double[1]) { }

        public override Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref values[0]), Count * Properties);

        public override double this[int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return values[property];
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                values[property] = value;
            }
        }


        public override double this[int index, int property] {
            get {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return values[index * Properties + property];
            }
            set {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                values[index * Properties + property] = value;
            }
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            ref var s = ref values;
            fixed (double* ptr = values) {
                var cnt = Properties * Count;
                for (int i = 0; i < cnt; i++) {
                    function(ref ptr[i]);
                }
            }
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            return values[offset];
        }

        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            values[offset] = value;
        }

        protected override bool IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArrayManaged o) {
                return o.values.Equals(values) || o.values.SequenceEqual(values);
            }

            return false;
        }

        protected override int ComputeHashCode() {
            return values.GetHashCode();
        }

        public override DoubleArray Clone() {
            return new DoubleArrayManaged((double[]) values.Clone());
        }

        #region Blunt Overloads

        public override double Value {
            get => this.values[0];
            set => this.values[0] = value;
        }

        public override double Close {
            get => this.values[0];
            set => this.values[0] = value;
        }

        #endregion
    }
}