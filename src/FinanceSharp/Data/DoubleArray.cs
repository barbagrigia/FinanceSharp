using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static FinanceSharp.Constants;
using FinanceSharp.Data;

namespace FinanceSharp.Data {
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
            AsDoubleSpan.Fill(0);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public StructArray(TStruct value) : this(1, value.Properties) {
            *Address = value;
        }

        public TStruct this[int i] {
            get => Address[i];
            set => Address[i] = value;
        }

        protected StructArray() { }
    }

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

        public DoubleArray Clone() {
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
        protected void Assert(bool condition, string reason = "") {
            if (!condition)
                throw new Exception(reason);
        }
    }
}