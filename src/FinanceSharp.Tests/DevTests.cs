using System;
using System.Linq;
using NUnit.Framework;
using Torch;
using TorchSharp.Tensor;

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

    public static unsafe class NumSharpExtensions {
        public static TorchTensor ToDoubleTensor(this NDArray nd) {
            nd = nd.typecode == NPTypeCode.Double ? nd : nd.astype(NPTypeCode.Double);
            return TorchSharp.Tensor.DoubleTensor.From((IntPtr) nd.Unsafe.Address, nd.Shape.Dimensions.Select(dim => (long) dim).ToArray(), true);
        }
    }
}