using Numpy;
using Torch;

namespace FinanceSharp.Helpers {
    public static class Constants {
        public static readonly Tensor<double> Empty;
        public static readonly Tensor<double> Zero;
        public static readonly Tensor<double> One;
        public static readonly Tensor<bool> True;
        public static readonly Tensor<bool> False;

        static Constants() {
            Zero = (Tensor<double>) 0d;
            Zero.requires_grad_();
            One = (Tensor<double>) 1d;
            One.requires_grad_(); //TODO!: does this work

            False = (Tensor<bool>) false;
            False.requires_grad_(); //TODO!: does this work
            True = (Tensor<bool>) true;
            True.requires_grad_(); //TODO!: does this work

            Empty = (Tensor<double>) torch.tensor(np.arange(0), torch.@double, requires_grad: true);
        }
    }
}
