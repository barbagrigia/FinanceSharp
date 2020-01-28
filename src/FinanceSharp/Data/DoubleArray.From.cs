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

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray {
        private class DoubleArrayManaged : DoubleArray {
            private readonly double[] _ref;
            private readonly GCHandle _handle;

            public DoubleArrayManaged(double[] array, int properties = 1) {
                _ref = array;
                Count = array.Length;
                Properties = properties;
                _handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                Address = (double*) _handle.AddrOfPinnedObject();
            }

            protected override void Dispose(bool disposing) {
                DisposerThread.Enqueue(_handle);
            }
        }

        private class DoubleArrayScalar : DoubleArray {
            // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
            private readonly IndicatorValue _ref;
            private readonly GCHandle _handle;

            public DoubleArrayScalar(double value) {
                //TODO: it might be much more performant with a preallocated buffer
                _ref = new IndicatorValue(value);
                Count = 1;
                Properties = 1;
                _handle = GCHandle.Alloc(_ref, GCHandleType.Pinned);
                Address = (double*) _handle.AddrOfPinnedObject();
            }

            protected override void Dispose(bool disposing) {
                DisposerThread.Enqueue(_handle);
            }
        }

        private class DoubleArrayStruct<TStruct> : StructArray<TStruct> where TStruct : unmanaged, DataStruct {
            private readonly TStruct[] _ref;
            private readonly GCHandle _handle;

            public DoubleArrayStruct(TStruct[] array) {
                _ref = array;
                Count = array.Length;
                Properties = sizeof(TStruct) / sizeof(double);
                _handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                Address = (TStruct*) _handle.AddrOfPinnedObject();
            }

            protected override void Dispose(bool disposing) {
                DisposerThread.Enqueue(_handle);
            }
        }

        private class DoubleArrayStructScalar<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
            private readonly TStruct _ref;
            private readonly GCHandle _handle;

            public DoubleArrayStructScalar(TStruct scalar) {
                _ref = scalar;
                Count = 1;
                Properties = scalar.Properties;
                _handle = GCHandle.Alloc(scalar, GCHandleType.Pinned);
                Address = (double*) _handle.AddrOfPinnedObject();
            }

            public DoubleArrayStructScalar(ref TStruct scalar) {
                _ref = scalar;
                Count = 1;
                Properties = scalar.Properties;
                _handle = GCHandle.Alloc(scalar, GCHandleType.Pinned);
                Address = (double*) _handle.AddrOfPinnedObject();
            }

            protected override void Dispose(bool disposing) {
                DisposerThread.Enqueue(_handle);
            }
        }

        /// <summary>
        ///     Copies or pins given <paramref name="array"/> into a <see cref="DoubleArray"/>.
        /// </summary>
        /// <param name="array">An array</param>
        /// <param name="copy">If true, <paramref name="array"/>'s contents will be copied to a newly allocated memory block, otherwise pinned and referenced. </param>
        /// <param name="properties"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DoubleArray FromArray(double[] array, bool copy, int properties = 1) {
            if (copy) {
                var ret = new DoubleArray(array.Length / properties, properties);
                new Span<double>(array).CopyTo(ret.AsDoubleSpan);
                return ret;
            }

            return new DoubleArrayManaged(array, properties);
        }

        /// <summary>
        ///     Copies or pins given <paramref name="array"/> into a <see cref="DoubleArray"/>.
        /// </summary>
        /// <param name="array">An array</param>
        /// <param name="copy">If true, <paramref name="array"/>'s contents will be copied to a newly allocated memory block, otherwise pinned and referenced. </param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DoubleArray FromStruct<TStruct>(TStruct[] array, bool copy) where TStruct : unmanaged, DataStruct {
            if (copy) {
                var properties = sizeof(TStruct) / sizeof(double);
                var ret = new DoubleArray(array.Length, properties);
                MemoryMarshal.AsBytes(new Span<TStruct>(array)).CopyTo(MemoryMarshal.AsBytes(ret.AsDoubleSpan));
                return ret;
            }

            return new DoubleArrayStruct<TStruct>(array);
        }

        /// <summary>
        ///     Pins a copy of given <typeparamref name="TStruct"/>.
        /// </summary>
        /// <typeparam name="TStruct"></typeparam>
        /// <param name="scalar"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DoubleArray FromStruct<TStruct>(TStruct scalar) where TStruct : unmanaged, DataStruct {
            return new DoubleArrayStructScalar<TStruct>(scalar);
        }

        /// <summary>
        ///     Pins given <typeparamref name="TStruct"/>.
        /// </summary>
        /// <typeparam name="TStruct"></typeparam>
        /// <param name="scalar"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DoubleArray FromStruct<TStruct>(ref TStruct scalar) where TStruct : unmanaged, DataStruct {
            return new DoubleArrayStructScalar<TStruct>(ref scalar);
        }

        /// <summary>
        ///     Creates a fast double scalar.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DoubleArray Scalar(double value) {
            return new DoubleArrayScalar(value);
        }
    }
}