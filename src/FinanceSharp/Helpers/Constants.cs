using System;
using FinanceSharp.Data;
using Numpy;
using Python.Runtime;

// ReSharper disable once CheckNamespace
namespace FinanceSharp {
    public static class Constants {
        public static readonly DoubleArray Empty = new DoubleArray(0);
        public const double Zero = 0;

        public const double One = 1;
        public const bool True = true;
        public const bool False = false;


        /// Close 
        public const int C = 0;

        /// High
        public const int H = 1;

        /// Low
        public const int L = 2;

        /// Open
        public const int O = 3;

        /// Volume
        public const int V = 4;

        /// Open
        public const int OpenIdx = O;

        /// High
        public const int HighIdx = H;

        /// Low
        public const int LowIdx = L;

        /// Close 
        public const int CloseIdx = C;

        /// Volume
        public const int VolumeIdx = V;


        static Constants() {
        }
    }
}