using System;
using System.Runtime.CompilerServices;

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray {

#if _REGEN
        #region Compute
        %operators = ["add", "sub", "mul", "div", "mod"]
        %operators_sign = ["+", "-", "*", "/", "%"]
        %operators_comparers = [">", "<", ">=", "<="]
        %operators_comparers_names = ["greater", "less", "greater_equal", "less_equal"]

        %possabilities = ["sbyte", "byte", "short", "ushort", "int", "uint", "ulong", "long", "float", "double"]
		
        %foreach operators, operators_sign%
            %foreach possabilities%
        public static DoubleArray operator #2(DoubleArray lhs, #101 rhs) => lhs.Select(d => d #2 rhs, true);
        public static DoubleArray operator #2(#101 lhs, DoubleArray rhs) => rhs.Select(d => d #2 lhs, true);
            %
        %				

        %foreach operators, operators_sign%
            %foreach ["decimal"]%
        public static DoubleArray operator #2(DoubleArray lhs, #101 rhs) => lhs #2 (double) rhs;
        public static DoubleArray operator #2(#101 lhs, DoubleArray rhs) => (double) lhs #2 rhs;
            %
        %		

        %foreach possabilities%
        public static explicit operator DoubleArray(#1 scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator #1(DoubleArray scalar) => (#1)scalar.Value;
        %
        #endregion
#else

        #region Compute

		
        public static DoubleArray operator +(DoubleArray lhs, sbyte rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(sbyte lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, byte rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(byte lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, short rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(short lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, ushort rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(ushort lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, int rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(int lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, uint rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(uint lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, ulong rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(ulong lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, long rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(long lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, float rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(float lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator +(DoubleArray lhs, double rhs) => lhs.Select(d => d + rhs, true);
        public static DoubleArray operator +(double lhs, DoubleArray rhs) => rhs.Select(d => d + lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, sbyte rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(sbyte lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, byte rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(byte lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, short rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(short lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, ushort rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(ushort lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, int rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(int lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, uint rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(uint lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, ulong rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(ulong lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, long rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(long lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, float rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(float lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator -(DoubleArray lhs, double rhs) => lhs.Select(d => d - rhs, true);
        public static DoubleArray operator -(double lhs, DoubleArray rhs) => rhs.Select(d => d - lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, sbyte rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(sbyte lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, byte rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(byte lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, short rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(short lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, ushort rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(ushort lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, int rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(int lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, uint rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(uint lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, ulong rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(ulong lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, long rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(long lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, float rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(float lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator *(DoubleArray lhs, double rhs) => lhs.Select(d => d * rhs, true);
        public static DoubleArray operator *(double lhs, DoubleArray rhs) => rhs.Select(d => d * lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, sbyte rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(sbyte lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, byte rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(byte lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, short rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(short lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, ushort rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(ushort lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, int rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(int lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, uint rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(uint lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, ulong rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(ulong lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, long rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(long lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, float rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(float lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator /(DoubleArray lhs, double rhs) => lhs.Select(d => d / rhs, true);
        public static DoubleArray operator /(double lhs, DoubleArray rhs) => rhs.Select(d => d / lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, sbyte rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(sbyte lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, byte rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(byte lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, short rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(short lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, ushort rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(ushort lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, int rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(int lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, uint rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(uint lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, ulong rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(ulong lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, long rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(long lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, float rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(float lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);
        public static DoubleArray operator %(DoubleArray lhs, double rhs) => lhs.Select(d => d % rhs, true);
        public static DoubleArray operator %(double lhs, DoubleArray rhs) => rhs.Select(d => d % lhs, true);

        public static DoubleArray operator +(DoubleArray lhs, decimal rhs) => lhs + (double) rhs;
        public static DoubleArray operator +(decimal lhs, DoubleArray rhs) => (double) lhs + rhs;
        public static DoubleArray operator -(DoubleArray lhs, decimal rhs) => lhs - (double) rhs;
        public static DoubleArray operator -(decimal lhs, DoubleArray rhs) => (double) lhs - rhs;
        public static DoubleArray operator *(DoubleArray lhs, decimal rhs) => lhs * (double) rhs;
        public static DoubleArray operator *(decimal lhs, DoubleArray rhs) => (double) lhs * rhs;
        public static DoubleArray operator /(DoubleArray lhs, decimal rhs) => lhs / (double) rhs;
        public static DoubleArray operator /(decimal lhs, DoubleArray rhs) => (double) lhs / rhs;
        public static DoubleArray operator %(DoubleArray lhs, decimal rhs) => lhs % (double) rhs;
        public static DoubleArray operator %(decimal lhs, DoubleArray rhs) => (double) lhs % rhs;

        public static explicit operator DoubleArray(sbyte scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator sbyte(DoubleArray scalar) => (sbyte)scalar.Value;
        public static explicit operator DoubleArray(byte scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator byte(DoubleArray scalar) => (byte)scalar.Value;
        public static explicit operator DoubleArray(short scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator short(DoubleArray scalar) => (short)scalar.Value;
        public static explicit operator DoubleArray(ushort scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator ushort(DoubleArray scalar) => (ushort)scalar.Value;
        public static explicit operator DoubleArray(int scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator int(DoubleArray scalar) => (int)scalar.Value;
        public static explicit operator DoubleArray(uint scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator uint(DoubleArray scalar) => (uint)scalar.Value;
        public static explicit operator DoubleArray(ulong scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator ulong(DoubleArray scalar) => (ulong)scalar.Value;
        public static explicit operator DoubleArray(long scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator long(DoubleArray scalar) => (long)scalar.Value;
        public static explicit operator DoubleArray(float scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator float(DoubleArray scalar) => (float)scalar.Value;
        public static implicit operator DoubleArray(double scalar) => new DoubleArrayScalar(scalar);
        public static explicit operator double(DoubleArray scalar) => (double)scalar.Value;
        #endregion
#endif
    }
}