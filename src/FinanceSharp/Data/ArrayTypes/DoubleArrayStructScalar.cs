using System;
using System.Runtime.CompilerServices;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    public unsafe class DoubleArrayStructScalar<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
        protected internal TStruct value;

        static DoubleArrayStructScalar() {
            //verify staticly that this struct indeed has only made from doubles
            if (DataStructInfo<TStruct>.DoubleFieldsCount != DataStructInfo<TStruct>.FieldsCount)
                throw new InvalidDoubleArrayStructException();
        }

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayStructScalar(TStruct @struct) : base(1, DataStructInfo<TStruct>.Properties) {
            value = @struct;
        }

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayStructScalar(ref TStruct @struct) : base(1, DataStructInfo<TStruct>.Properties) {
            value = @struct;
        }

        public DoubleArrayStructScalar() : base(1, DataStructInfo<TStruct>.Properties) {
            value = new TStruct();
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            double* ptr = (double*) Unsafe.AsPointer(ref value);
            var cnt = DataStructInfo<TStruct>.Properties;
            for (int i = 0; i < cnt; i++) {
                function(ref ptr[i]);
            }
        }

        public override double this[int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return ((double*) Unsafe.AsPointer(ref value))[property];
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                ((double*) Unsafe.AsPointer(ref this.value))[property] = value;
            }
        }

        public override double this[int index, int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(index == 0, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return ((double*) Unsafe.AsPointer(ref value))[property];
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(index == 0, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                ((double*) Unsafe.AsPointer(ref this.value))[property] = value;
            }
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            return this[offset];
        }

        /// <summary>
        ///     Writes to this DoubleArray linearly regardless to shape.
        /// </summary>
        /// <param name="offset">Absolute offset to set <paramref name="value"/> at.</param>
        /// <param name="value">The value to write</param>
        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            this[offset] = value;
        }

        public virtual TStruct Get(int index) {
            AssertTrue(index >= 0 && index < Properties, "Index is out of range.");
            return value;
        }

        /// <summary>
        ///     A comparison method used when <see cref="DoubleArray.Equals(DoubleArray)"/> is called from <see cref="IEquatable{T}"/>
        /// </summary>
        /// <param name="other">An DoubleArray to compare to this.</param>
        /// <returns>Are <see cref="this"/> equals to <see cref="other"/>.</returns>
        protected override bool IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArrayStructScalar<TStruct> o) {
                return o.value.Equals(value);
            }

            return false;
        }

        /// <summary>
        ///     Additional hashcode for inherieted classes.
        /// </summary>
        /// <returns></returns>
        protected override int ComputeHashCode() {
            return value.GetHashCode();
        }

        /// <summary>
        ///     Casts this DoubleArray with Span.
        /// </summary>
        public override Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref this.value), Count * Properties);

        /// <summary>
        ///     Clones current DoubleArray.
        /// </summary>
        /// <returns>A new copy of this.</returns>
        public override DoubleArray Clone() {
            return new DoubleArrayStructScalar<TStruct>(value);
        }
    }
}