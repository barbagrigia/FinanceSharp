using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    [SuppressMessage("ReSharper", "LocalVariableHidesMember")]
    public unsafe class DoubleArrayUnmanagedTests : DoubleArrayBaseTests {
        public double* block;
        public int offset;
        public static readonly object _lock = new object();

        public double* Offset(int doubles) {
            lock (_lock) {
                var offsetStart = this.offset;
                offset += doubles;
                return block + offsetStart;
            }
        }

        [TestFixtureSetUp]
        public void SetUp() {
            block = (double*) Marshal.AllocHGlobal(sizeof(double) * 1000);
            offset = 0;
        }

        [TestFixtureTearDown]
        public void TearDown() {
            Marshal.FreeHGlobal((IntPtr) block);
            offset = 0;
        }

        public override DoubleArray CreateDefault() {
            return new DoubleArrayUnmanaged();
        }

        public override DoubleArray CreateScalar1_1(double value1) {
            var ptr = Offset(1);

            *ptr = value1;

            return new DoubleArrayUnmanaged(ptr, 1, 1, false);
        }

        public override DoubleArray CreateScalar1_2(double value1, double value2) {
            var ptr = Offset(2);

            ptr[0] = value1;
            ptr[1] = value2;

            return new DoubleArrayUnmanaged(ptr, 1, 2, false);
        }

        public override DoubleArray CreateArray2_1(double value1, double value2) {
            var ptr = Offset(2);

            ptr[0] = value1;
            ptr[1] = value2;

            return new DoubleArrayUnmanaged(ptr, 2, 1, false);
        }

        public override DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4) {
            var ptr = Offset(4);

            ptr[0] = value1;
            ptr[1] = value2;
            ptr[2] = value3;
            ptr[3] = value4;

            return new DoubleArrayUnmanaged(ptr, 1, 4, false);
        }

        public override DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4) {
            var ptr = Offset(4);

            ptr[0] = value1;
            ptr[1] = value2;
            ptr[2] = value3;
            ptr[3] = value4;

            return new DoubleArrayUnmanaged(ptr, 4, 1, false);
        }

        public override DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4) {
            var ptr = Offset(4);

            ptr[0] = value1;
            ptr[1] = value2;
            ptr[2] = value3;
            ptr[3] = value4;

            return new DoubleArrayUnmanaged(ptr, 2, 2, false);
        }

        public override DoubleArray CreateTensor4_4(double value1, double value2, double value3, double value4) {
            var ptr = Offset(16);
            for (int i = 0; i < 4; i++) {
                ptr[i * 4 + 0] = value1;
                ptr[i * 4 + 1] = value2;
                ptr[i * 4 + 2] = value3;
                ptr[i * 4 + 3] = value4;
            }

            return new DoubleArrayUnmanaged(ptr, 4, 4, false);
        }
    }
}