using System;
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

        private class DoubleArrayStruct<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
            private readonly TStruct[] _ref;
            private readonly GCHandle _handle;

            public DoubleArrayStruct(TStruct[] array) {
                _ref = array;
                Count = array.Length;
                Properties = sizeof(TStruct) / sizeof(double);
                _handle = GCHandle.Alloc(array, GCHandleType.Pinned);
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
        public static DoubleArray FromStruct<TStruct>(TStruct[] array, bool copy) where TStruct : unmanaged, DataStruct {
            if (copy) {
                var properties = sizeof(TStruct) / sizeof(double);
                var ret = new DoubleArray(array.Length, properties);
                MemoryMarshal.AsBytes(new Span<TStruct>(array)).CopyTo(MemoryMarshal.AsBytes(ret.AsDoubleSpan));
                return ret;
            }

            return new DoubleArrayStruct<TStruct>(array);
        }

        public static DoubleArray Scalar(double value) {
            return new DoubleArrayScalar(value);
        }
    }
}