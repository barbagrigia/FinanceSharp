using System.Runtime.CompilerServices;
using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests {
    public class ProofOfAbilityTests {
        [Test]
        public unsafe void CopyBlockToShiftDataInSameOffsettedPointer() {
            const int size = 1_000_000;
            var a = new DoubleArrayUnmanaged(size, 1, true);
            for (int i = 0; i < size; i++) {
                a.Address[i] = i;
            }

            Unsafe.CopyBlock(a.Address + 1, a.Address, (uint) (sizeof(double) * (a.Count - 1) * a.Properties));

            for (int i = 1; i < size; i++) {
                a.Address[i].Should().Be(i - 1);
            }
        }
    }
}