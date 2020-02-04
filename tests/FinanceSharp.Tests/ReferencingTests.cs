using System.Runtime.CompilerServices;
using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests {
    public class ReferencingTests {
        struct TestStruct {
            public double a;
            public double b;
        }

        struct InvalidStruct {
            public double a;
            public double b;
        }

        private double[] fieldArray = new double[10];
        private TestStruct fieldStruct = new TestStruct() {a = 1, b = 2};

        [Test]
        public unsafe void RefPointer() {
            double* ptr = (double*) Unsafe.AsPointer(ref fieldStruct);
            ptr[0].Should().Be(1d);
            ptr[1].Should().Be(2d);
        }
    }
}