using System;
using System.Runtime.CompilerServices;
using FinanceSharp.Exceptions;

namespace FinanceSharp {
    public unsafe class DoubleArrayStruct<TStruct> : DoubleArray where TStruct : unmanaged, DataStruct {
        protected internal TStruct[] values;

        static DoubleArrayStruct() {
            //verify staticly that this struct indeed has only made from doubles
            if (DataStructInfo<TStruct>.DoubleFieldsCount != DataStructInfo<TStruct>.FieldsCount)
                throw new InvalidDoubleArrayStructException();
        }

        /// <param name="struct">The structure value that'll be wrapped.</param>
        public DoubleArrayStruct(params TStruct[] @struct) : base(@struct.Length, DataStructInfo<TStruct>.Properties) {
            values = @struct;
        }

        public DoubleArrayStruct() : base(1, DataStructInfo<TStruct>.Properties) {
            values = new TStruct[] {new TStruct()};
        }

        public override void ForEach(ReferenceForFunctionHandler function) {
            ref var s = ref values;
            fixed (TStruct* struct_ptr = values) {
                var ptr = (double*) struct_ptr;
                var cnt = Properties * Count;
                for (int i = 0; i < cnt; i++) {
                    function(ref ptr[i]);
                }
            }
        }

        public override double this[int property] {
            get {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (TStruct* ptr = values) {
                    return ((double*) ptr)[property];
                }
            }
            set {
                AssertTrue(IsScalar, "Scalar only overload was called but the array is not scalar.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                fixed (TStruct* ptr = values) {
                    ((double*) ptr)[property] = value;
                }
            }
        }


        public override double this[int index, int property] {
            get {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                return ((double*) Unsafe.AsPointer(ref values[index]))[property];
            }
            set {
                AssertTrue(index < Count, "Index is out of range.");
                AssertTrue(property >= 0 && property < Properties, "Property index is out of range.");
                ((double*) Unsafe.AsPointer(ref values[index]))[property] = value;
            }
        }

        public override TDestStruct Get<TDestStruct>(int index) {
            AssertTrue(index >= 0 && index < Count, "Index is out of range.");
            if (DataStructInfo<TDestStruct>.Properties > Properties)
                return base.Get<TDestStruct>(index);
            return *(TDestStruct*) Unsafe.AsPointer(ref values[index]);
        }

        public override double GetLinear(int offset) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (TStruct* ptr = values) {
                return ((double*) ptr)[offset];
            }
        }

        public override void SetLinear(int offset, double value) {
            AssertTrue(offset >= 0 && offset < LinearLength, "Offset is out of range.");
            fixed (TStruct* ptr = values) {
                ((double*) ptr)[offset] = value;
            }
        }

        public virtual TStruct Get(int index) {
            AssertTrue(index >= 0 && index < Count, "Index is out of range.");
            return values[index];
        }

        protected override bool IsEqualExactlyTo(DoubleArray other) {
            if (other is DoubleArrayStruct<TStruct> o) {
                return o.values.Equals(values);
            }

            return false;
        }

        protected override int ComputeHashCode() {
            return values.GetHashCode();
        }

        public override Span<double> AsDoubleSpan => new Span<double>(Unsafe.AsPointer(ref values[0]), LinearLength);

        public override DoubleArray Clone() {
            return new DoubleArrayStruct<TStruct>((TStruct[]) values.Clone());
        }
    }
}