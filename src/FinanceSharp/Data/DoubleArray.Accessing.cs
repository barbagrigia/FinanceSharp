using System.Runtime.CompilerServices;

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray {
        public double Value {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => *Address;
        }

        public double Close {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => *Address;
        }

        public double Open {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Address[Constants.OpenIdx];
        }

        public double High {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Address[Constants.HighIdx];
        }

        public double Low {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Address[Constants.LowIdx]; }
        }

        public double Volume => Address[Constants.VolumeIdx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ValueAt(int index) => Address[index * Properties];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CloseAt(int index) => Address[index * Properties];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double OpenAt(int index) => Address[index * Properties + Constants.OpenIdx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double HighAt(int index) => Address[index * Properties + Constants.HighIdx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LowAt(int index) => Address[index * Properties + Constants.LowIdx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double VolumeAt(int index) => Address[index * Properties + Constants.VolumeIdx];

        /// <summary>
        ///     Scalar overload.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public double this[int property] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(IsScalar, "Scalar only overload was called but the array is not scalar.");
                return Address[property];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(IsScalar, "Scalar only overload was called but the array is not scalar.");
                Address[property] = value;
            }
        }

        public double this[int index, int property] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Address[index * Properties + property];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Address[index] = value;
        }

        public TStruct Get<TStruct>(int index) where TStruct : unmanaged, DataStruct {
            Assert(sizeof(TStruct) == sizeof(double) * Properties);
            return ((TStruct*) Address)[index];
        }
    }
}