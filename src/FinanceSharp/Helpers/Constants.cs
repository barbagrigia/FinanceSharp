using Numpy;
using Python.Runtime;
using Torch;

namespace FinanceSharp.Helpers {
    public static class Constants {
        public static readonly Tensor<double> Empty;
        public static readonly Tensor<double> Zero;
        public static readonly Tensor<double> One;
        public static readonly Tensor<bool> True;
        public static readonly Tensor<bool> False;

        /// Open
        public static readonly PyObject O;

        /// High
        public static readonly PyObject H;

        /// Low
        public static readonly PyObject L;

        /// Close 
        public static readonly PyObject C;

        /// Volume
        public static readonly PyObject V;

        /// Open
        public static readonly PyObject Open;

        /// High
        public static readonly PyObject High;

        /// Low
        public static readonly PyObject Low;

        /// Close 
        public static readonly PyObject Close;

        /// Volume
        public static readonly PyObject Volume;

        public static readonly PyObject Elipsis;
        public static readonly PyObject All;

        static Constants() {
            using var _ = Py.GIL();

            Zero = (Tensor<double>) 0d;
            Zero.requires_grad_();
            One = (Tensor<double>) 1d;
            One.requires_grad_(); //TODO!: does this work

            False = (Tensor<bool>) false;
            False.requires_grad_(); //TODO!: does this work
            True = (Tensor<bool>) true;
            True.requires_grad_(); //TODO!: does this work

            Empty = (Tensor<double>) torch.tensor(np.arange(0), torch.@double, requires_grad: true);

            Open = O = new PyLong(0);
            High = H = new PyLong(1);
            Low = L = new PyLong(2);
            Close = C = new PyLong(3);
            Volume = V = new PyLong(4);

            Elipsis = PythonEngine.Eval("Ellipsis");
            All = PythonEngine.Eval("slice(None, None, 1)");
        }
    }
}