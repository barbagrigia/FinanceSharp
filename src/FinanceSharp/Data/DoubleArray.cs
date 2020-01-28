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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static FinanceSharp.Constants;
using FinanceSharp.Data;

namespace FinanceSharp.Data {
    /// <summary>
    ///     A block of memory represented as two dimensions or a scalar.
    /// </summary>
    /// <remarks>First dimension of this array is <see cref="Count"/> and 2nd dimension is <see cref="Properties"/>, which for a OHLC trade bar would be (n, 4).</remarks>
    public unsafe partial class DoubleArray : ICloneable, IDisposable {
        /// The address for the memory block.
        public double* Address;

        /// The number of items in this array, each having n <see cref="Properties"/>.
        public int Count;

        /// The count of properties for every 
        public int Properties;

        /// <summary>
        ///     
        /// </summary>
        /// <param name="count">The number of items in this array.</param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        public DoubleArray(int count, int properties) {
            Count = count;
            Properties = properties;
            Address = (double*) Marshal.AllocHGlobal(count * properties * sizeof(double));
            AsDoubleSpan.Fill(0);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public DoubleArray(double value) : this(1, 1) {
            *Address = value;
        }

        protected DoubleArray() { }

        /// <summary>
        ///     The bytes size of this array.
        /// </summary>
        public int SizeOf => sizeof(double) * Count * Properties;

        public Span<double> AsDoubleSpan => new Span<double>(Address, Count * Properties);

        public bool IsScalar => Count == 1;

        public virtual DoubleArray Clone() {
            var ret = new DoubleArray(Count, Properties);
            CopyTo(ret);
            return ret;
        }

        public void CopyTo(DoubleArray target) {
            new Span<double>(Address, Count * Properties)
                .CopyTo(new Span<double>(target.Address, target.Count * target.Properties));
        }

        public TStruct[] ToArray<TStruct>() where TStruct : unmanaged, DataStruct {
            return new Span<TStruct>(Address, Count).ToArray();
        }

        public double[] ToArray() {
            return new Span<double>(Address, Count * Properties).ToArray();
        }

        object ICloneable.Clone() {
            return Clone();
        }

        [Conditional("DEBUG")]
        protected void Assert(bool condition) {
            if (!condition)
                throw new Exception();
        }

        [Conditional("DEBUG")]
        protected void Assert(bool condition, string reason) {
            if (!condition)
                throw new Exception(reason);
        }
    }
}