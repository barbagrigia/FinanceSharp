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
using System.Runtime.InteropServices;

namespace FinanceSharp.Data {
    /// <summary>
    ///     A struct used to describe DoubleArray with an array of structs instead of an array of doubles.
    /// </summary>
    /// <typeparam name="TStruct"></typeparam>
    public unsafe class StructArray<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
        public new TStruct* Address;

        /// <summary>
        ///     
        /// </summary>
        /// <param name="count">The number of items in this array.</param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        public StructArray(int count, int properties) {
            Count = count;
            Properties = properties;
            Address = (TStruct*) Marshal.AllocHGlobal(count * properties * sizeof(double));
            base.Address = (double*) Address;
            AsDoubleSpan.Fill(0d);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public StructArray(TStruct value) : this(1, value.Properties) {
            *Address = value;
        }

        public new TStruct this[int i] {
            get => Address[i];
            set => Address[i] = value;
        }

        protected StructArray() { }
    }
}