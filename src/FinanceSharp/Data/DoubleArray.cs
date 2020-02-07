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
using System.Threading;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    /// <summary>
    ///     A block of memory of doubles represented in two dimensions.
    /// </summary>
    /// <remarks>
    ///     First dimension of this array is <see cref="Count"/> and 2nd dimension is <see cref="Properties"/>,
    ///     which for a OHLC trade bar would be (n, 4) where n is the number of trade bars in the memory block.
    /// </remarks>
    public abstract unsafe partial class DoubleArray : ICloneable, IDisposable {
        /// The number of items in this array, each having n <see cref="Properties"/>.
        public int Count;

        /// The count of properties for every 
        public int Properties;

        /// <param name="count">The number of items in this array.</param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        protected DoubleArray(int count, int properties) {
            Count = count;
            Properties = properties;
        }

        /// <param name="values"></param>
        /// <param name="properties">How many properties typed double are for every <see cref="count"/></param>
        protected DoubleArray(double[] values, int properties = 1) : this(values.Length / properties, properties) {
            AssertTrue(values.Length % properties == 0, "It has to be evenly divided by the properties count.");
        }

        protected DoubleArray() { }

        /// <summary>
        ///     The bytes size of this array.
        /// </summary>
        public int SizeOf => sizeof(double) * LinearLength;

        /// <summary>
        ///     The total number of double values in the array.
        /// </summary>
        public int LinearLength => Count * Properties;

        /// <summary>
        ///     Wraps this DoubleArray with <see cref="Span{T}"/>.
        /// </summary>
        /// <remarks>Best practice to use AsDoubleSpan safely is first using fixed operator on the array and then access <see cref="AsDoubleSpan"/>. See examples.</remarks>
        /// <example>
        /// <code>
        ///     fixed (double* _ = doublearray) {
        ///         doublearray.AsDoubleSpan.Fill(10d);
        ///         double[] ret = doublearray.AsDoubleSpan.ToArray();
        ///     }
        /// </code>
        /// </example>
        public virtual Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref GetPinnableReference()), LinearLength);

        /// <summary>
        ///     Provides a pinnable reference for fixing a <see cref="DoubleArray"/>.
        /// </summary>
        public abstract ref double GetPinnableReference();

        /// <summary>
        ///     Provides a pinnable reference for fixing a <see cref="DoubleArray"/> at a specific index (of <see cref="Count"/> dimension).
        /// </summary>
        /// <example>
        /// <code>
        ///     fixed (double* pointer = &amp;arr.GetPinnableReference(0)) {
        ///         //use pointer to your needs
        ///     }
        /// </code>
        /// </example>
        public abstract ref double GetPinnableReference(int index);

        /// <summary>
        ///     Is this array a scalar by checking if <see cref="Count"/> is 1. (regardless to how many properties there are).
        /// </summary>
        public virtual bool IsScalar => Count == 1;

        /// <summary>
        ///     Clones current DoubleArray.
        /// </summary>
        /// <returns>A new copy of this.</returns>
        public abstract DoubleArray Clone();

        /// <summary>
        ///     Copies content of this array to <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The target array, must be matching <see cref="LinearLength"/> with this.</param>
        public virtual void CopyTo(DoubleArray target) {
            fixed (double* _ = this, __ = target) {
                //fix to prevent gc from moving them during the operation.
                AsDoubleSpan
                    .CopyTo(target.AsDoubleSpan);
            }
        }

        /// <summary>
        ///     Converts this <see cref="DoubleArray"/> to a linear <see cref="double"/>.
        /// </summary>
        /// <returns></returns>
        public virtual double[] ToArray() {
            fixed (double* _ = this) //fix to prevent gc from moving them during the operation.
                return AsDoubleSpan.ToArray();
        }

        /// <summary>
        ///     Converts current array to an Array of type <typeparamref name="TDestStruct"/>.
        /// </summary>
        /// <typeparam name="TDestStruct"></typeparam>
        /// <param name="method">ArrayConversionMethod,</param>
        /// <param name="propertiesPerItem">Used for ArrayConversionMethod.Cast: describes how many doubles to copy to and per TDestStruct. If -1 is specified then sizeof(TDestStruct)/sizeof(double) will be used.<br></br></param>
        public virtual unsafe TDestStruct[] ToArray<TDestStruct>(ArrayConversionMethod method, int propertiesPerItem = -1) where TDestStruct : unmanaged, DataStruct {
            if (typeof(TDestStruct) == typeof(double))
                fixed (double* _ = this)
                    return (TDestStruct[]) (object) AsDoubleSpan.ToArray();
            var totalBytes = this.SizeOf;
            if (method == ArrayConversionMethod.Reinterpret) {
                if (totalBytes % sizeof(TDestStruct) != 0)
                    throw new ArgumentException($"Can't reinterpret to TDestStruct because: totalBytes ({totalBytes}) % sizeof(TDestStruct) ({sizeof(TDestStruct)}) != 0 ({totalBytes % sizeof(TDestStruct)})");
                fixed (double* _ = this)
                    return MemoryMarshal.Cast<double, TDestStruct>(AsDoubleSpan).ToArray();
            } else if (method == ArrayConversionMethod.Cast) {
                if (propertiesPerItem == -1)
                    propertiesPerItem = sizeof(TDestStruct) / sizeof(double);
                if (totalBytes % (propertiesPerItem * sizeof(double)) != 0)
                    throw new ArgumentException($"Can't reinterpret to TDestStruct because: totalBytes ({totalBytes}) % (propertiesPerItem * sizeof(double)) (({propertiesPerItem} * sizeof(double))) != 0 ({totalBytes % sizeof(TDestStruct)})");
                var ret = new TDestStruct[(totalBytes) / (propertiesPerItem * sizeof(double))];
                TDestStruct tmp = new TDestStruct();
                double* tmpPtr = (double*) &tmp;
                var len = Count * (Properties / propertiesPerItem);
                for (int i = 0, linearI = 0; i < len; i++) {
                    for (int j = 0; j < propertiesPerItem; j++, linearI++) {
                        tmpPtr[j] = this.GetLinear(linearI);
                    }

                    ret[i] = tmp; //copies because tmp is struct.
                }

                return ret;
            } else {
                throw new NotSupportedException(method.ToString());
            }
        }

        /// <summary>
        ///     Copies this array into a multi-dimensional array, double[,].
        /// </summary>
        /// <returns></returns>
        public virtual double[,] To2DArray() {
            var ret = new double[Count, Properties];
            fixed (double* src = this, dst = ret) {
                Unsafe.CopyBlock(src, dst, (uint) (sizeof(double) * ret.Length));
                return ret;
            }
        }

        /// <summary>
        ///     Reshapes this <see cref="DoubleArray"/> to a shape of your choice, the shape must be 
        /// </summary>
        /// <param name="count">The count of items in this array.</param>
        /// <param name="properties">Number of properties for every item in this array.</param>
        /// <param name="copy">Should a copy be reshaped?</param>
        /// <exception cref="ReshapeException">When unable to reshape to reshape: <see cref="LinearLength"/> != (<paramref name="count"/> * <paramref name="properties"/>) then throws </exception>
        public abstract DoubleArray Reshape(int count, int properties, bool copy = true);

        /// <summary>
        ///     Slices (or wraps with a slice wrapper) the <see cref="Count"/> dimension.
        /// </summary>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <returns>Returns a sliced array shaped (newCount, <see cref="Properties"/>)</returns>
        /// <remarks>This slicing mechanism is similar to numpy's slice and will behave like the following: <code>thisArray[start:stop:1, :]</code></remarks>
        public virtual DoubleArray Slice(int start, int stop) {
            AssertTrue(start < Count, "Start index is out of range.");
            AssertTrue(stop <= Count, "Stop index is out of range.");
            return new SlicedDoubleArray(this, start, stop);
        }

        /// <summary>
        ///     Slices (or wraps with a slice wrapper) the <see cref="Count"/> dimension.
        /// </summary>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        /// <returns>Returns a sliced array shaped (newCount, <see cref="Properties"/>)</returns>
        /// <remarks>This slicing mechanism is similar to numpy's slice and will behave like the following: <code>thisArray[start:stop:1, :]</code></remarks>
        public DoubleArray Slice(int stop) {
            return Slice(0, stop);
        }

        /// <summary>
        ///     Asserts for DEBUG runs..
        /// </summary>
        [Conditional("DEBUG"), DebuggerHidden]
        protected void AssertTrue(bool condition) {
            if (!condition)
                throw new Exception();
        }

        /// <summary>
        ///     Asserts for DEBUG runs..
        /// </summary>
        [Conditional("DEBUG"), DebuggerHidden]
        protected void AssertTrue(bool condition, string reason) {
            if (!condition)
                throw new Exception(reason);
        }

        object ICloneable.Clone() {
            return Clone();
        }
    }
}