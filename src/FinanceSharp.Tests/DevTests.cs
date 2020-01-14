using System;
using System.Linq;
using NUnit.Framework;
using Torch;

namespace FinanceSharp.Tests {
    public class DevTests {

        [Test]
        public unsafe void Dev() {
            var device = torch.device(0);
            var a = torch.tensor(new int[] {1, 2});
            var b = a + 1;
            Console.WriteLine(b);
        }
    }
}