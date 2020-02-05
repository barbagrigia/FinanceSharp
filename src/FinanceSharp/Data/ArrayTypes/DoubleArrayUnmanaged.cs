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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FinanceSharp {
    public unsafe class DoubleArrayUnmanaged : DoubleArray {
        /// The address for the memory block.
        public double* Address;

        /// The disposer method to deallocate <see cref="Address"/>, can be null.
        protected Action disposer;

        /// <summary>
        ///     
        /// </summary>
        /// <param name="count">The number of items in this array.</param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        public DoubleArrayUnmanaged(int count, int properties, bool zeroValues = true) : base(count, properties) {
            var addr = Address = (double*) Marshal.AllocHGlobal(count * properties * sizeof(double));
            disposer = () => DisposerThread.Enqueue(addr);
            if (zeroValues)
                AsDoubleSpan.Fill(0);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="pointer">A address to the wrapped memory block.</param>
        /// <param name="count">The number of items in this array.</param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        /// <param name="zeroValues">Should all the values in given <paramref name="pointer"/> </param>
        /// <param name="disposer">the disposing action for deallocating given <paramref name="pointer"/></param>
        public DoubleArrayUnmanaged(double* pointer, int count, int properties, bool zeroValues = true, Action disposer = null) : base(count, properties) {
            Address = pointer;
            this.disposer = disposer;
            if (zeroValues)
                AsDoubleSpan.Fill(0);
        }

        public DoubleArrayUnmanaged() : this(1, 1, true) { }

        public override Span<double> AsDoubleSpan => new Span<double>(Address, Count * Properties);

        public override ref double GetPinnableReference() {
            return ref Unsafe.AsRef<double>(Address);
        }

        public override ref double GetPinnableReference(int index) {
            AssertTrue(index == 0, "Index is out of range.");
            return ref Unsafe.AsRef<double>(Address + index * Properties);
        }

        /// <summary>
        ///     Nullifies disposer making this class no longer be responsible for the Address it contains.
        /// </summary>
        public void DiscardDisposer() {
            disposer = null;
        }

        public override DoubleArray Clone() {
            var ret = new DoubleArrayUnmanaged(Count, Properties);
            CopyTo(ret);
            return ret;
        }

        protected override void ReleaseUnmanagedResources() {
            disposer?.Invoke();
            disposer = null;
        }

        public TStruct[] ToArray<TStruct>() {
            return new Span<TStruct>(Address, Count).ToArray();
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            var cnt = Count * Properties;
            for (int i = 0; i < cnt; i++) {
                function(ref Address[i]);
            }
        }

        public override double this[int property] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                return Address[property];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                Address[property] = value;
            }
        }

        public override double this[int index, int property] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Address[index * Properties + property];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Address[index * Properties + property] = value;
        }

        protected override bool IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArrayUnmanaged uarr) {
                return uarr.Address == Address;
            }

            return false;
        }

        protected override int ComputeHashCode() {
            return ((int) Address * 397) % int.MaxValue;
        }

        public override TStruct Get<TStruct>(int index) {
            AssertTrue(sizeof(TStruct) == sizeof(double) * Properties);
            return ((TStruct*) Address)[index];
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            return Address[offset];
        }

        /// <summary>
        ///     Writes to this DoubleArray linearly regardless to shape.
        /// </summary>
        /// <param name="offset">Absolute offset to set <paramref name="value"/> at.</param>
        /// <param name="value">The value to write</param>
        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            Address[offset] = value;
        }
    }
}