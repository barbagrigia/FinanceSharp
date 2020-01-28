/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
using FinanceSharp.Data;

namespace FinanceSharp.Indicators {
    public abstract partial class IndicatorBase {
        public static bool operator ==(IndicatorBase lhs, IndicatorBase rhs) {
            if (lhs is null || rhs is null)
                return false;
            return lhs.Current.Equals(rhs.Current);
        }

        public static bool operator !=(IndicatorBase lhs, IndicatorBase rhs) {
            if (lhs is null || rhs is null)
                return false;
            return !lhs.Current.Equals(rhs.Current);
        }

        public static implicit operator DoubleArray(IndicatorBase scalar) => scalar.Current;
        public static implicit operator double(IndicatorBase scalar) => scalar.Current.Value;

#if _REGEN
        %possabilities = ["sbyte", "byte", "short", "ushort", "int", "uint", "ulong", "long", "float", "double"]
        %operators_sign = ["+", "-", "*", "/", "%"]
        %operators_comparers = [">", "<", ">=", "<="]

        %foreach operators_sign%
        public static double operator #1(IndicatorBase lhs, IndicatorBase rhs) { return lhs.Current.Value #1 rhs.Current.Value; }        
        %foreach possabilities%
        public static double operator #1(#101 lhs, IndicatorBase rhs) { return lhs #1 rhs.Current.Value; }        
        public static double operator #1(IndicatorBase lhs, #101 rhs) { return lhs.Current.Value #1 rhs; }        
        %
        %
        %foreach possabilities%
        public static bool operator ==(#1 lhs, IndicatorBase rhs) { return rhs.Current.IsScalar && lhs == rhs.Current.Value; }        
        public static bool operator !=(#1 lhs, IndicatorBase rhs) { return !rhs.Current.IsScalar || lhs != rhs.Current.Value; }        
        public static bool operator ==(IndicatorBase lhs, #1 rhs) { return lhs.Current.IsScalar && lhs.Current.Value == rhs; }        
        public static bool operator !=(IndicatorBase lhs, #1 rhs) { return !lhs.Current.IsScalar || lhs.Current.Value != rhs; }        
        %        
        %foreach possabilities%
        %foreach operators_comparers%
        public static bool operator #101(#1 lhs, IndicatorBase rhs) { return rhs.Current.IsScalar && lhs #101 rhs.Current.Value; }        
        public static bool operator #101(IndicatorBase lhs, #1 rhs) { return lhs.Current.IsScalar && lhs.Current.Value #101 rhs; }        
        %
        %
#else
        public static double operator +(IndicatorBase lhs, IndicatorBase rhs) {
            return lhs.Current.Value + rhs.Current.Value;
        }

        public static double operator +(sbyte lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(byte lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, byte rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(short lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, short rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(ushort lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(int lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, int rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(uint lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, uint rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(ulong lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(long lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, long rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(float lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, float rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator +(double lhs, IndicatorBase rhs) {
            return lhs + rhs.Current.Value;
        }

        public static double operator +(IndicatorBase lhs, double rhs) {
            return lhs.Current.Value + rhs;
        }

        public static double operator -(IndicatorBase lhs, IndicatorBase rhs) {
            return lhs.Current.Value - rhs.Current.Value;
        }

        public static double operator -(sbyte lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(byte lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, byte rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(short lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, short rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(ushort lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(int lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, int rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(uint lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, uint rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(ulong lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(long lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, long rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(float lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, float rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator -(double lhs, IndicatorBase rhs) {
            return lhs - rhs.Current.Value;
        }

        public static double operator -(IndicatorBase lhs, double rhs) {
            return lhs.Current.Value - rhs;
        }

        public static double operator *(IndicatorBase lhs, IndicatorBase rhs) {
            return lhs.Current.Value * rhs.Current.Value;
        }

        public static double operator *(sbyte lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(byte lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, byte rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(short lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, short rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(ushort lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(int lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, int rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(uint lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, uint rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(ulong lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(long lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, long rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(float lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, float rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator *(double lhs, IndicatorBase rhs) {
            return lhs * rhs.Current.Value;
        }

        public static double operator *(IndicatorBase lhs, double rhs) {
            return lhs.Current.Value * rhs;
        }

        public static double operator /(IndicatorBase lhs, IndicatorBase rhs) {
            return lhs.Current.Value / rhs.Current.Value;
        }

        public static double operator /(sbyte lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(byte lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, byte rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(short lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, short rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(ushort lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(int lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, int rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(uint lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, uint rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(ulong lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(long lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, long rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(float lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, float rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator /(double lhs, IndicatorBase rhs) {
            return lhs / rhs.Current.Value;
        }

        public static double operator /(IndicatorBase lhs, double rhs) {
            return lhs.Current.Value / rhs;
        }

        public static double operator %(IndicatorBase lhs, IndicatorBase rhs) {
            return lhs.Current.Value % rhs.Current.Value;
        }

        public static double operator %(sbyte lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(byte lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, byte rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(short lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, short rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(ushort lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(int lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, int rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(uint lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, uint rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(ulong lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(long lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, long rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(float lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, float rhs) {
            return lhs.Current.Value % rhs;
        }

        public static double operator %(double lhs, IndicatorBase rhs) {
            return lhs % rhs.Current.Value;
        }

        public static double operator %(IndicatorBase lhs, double rhs) {
            return lhs.Current.Value % rhs;
        }

        public static bool operator ==(sbyte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(sbyte lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, sbyte rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(byte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(byte lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, byte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, byte rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(short lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(short lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, short rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, short rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(ushort lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(ushort lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, ushort rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(int lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(int lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, int rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, int rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(uint lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(uint lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, uint rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, uint rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(ulong lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(ulong lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, ulong rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(long lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(long lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, long rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, long rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(float lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(float lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, float rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, float rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator ==(double lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs == rhs.Current.Value;
        }

        public static bool operator !=(double lhs, IndicatorBase rhs) {
            return !rhs.Current.IsScalar || lhs != rhs.Current.Value;
        }

        public static bool operator ==(IndicatorBase lhs, double rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value == rhs;
        }

        public static bool operator !=(IndicatorBase lhs, double rhs) {
            return !lhs.Current.IsScalar || lhs.Current.Value != rhs;
        }

        public static bool operator >(sbyte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(sbyte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(sbyte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(sbyte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, sbyte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(byte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, byte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(byte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, byte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(byte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, byte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(byte lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, byte rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(short lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, short rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(short lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, short rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(short lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, short rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(short lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, short rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(ushort lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(ushort lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(ushort lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(ushort lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, ushort rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(int lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, int rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(int lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, int rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(int lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, int rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(int lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, int rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(uint lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, uint rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(uint lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, uint rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(uint lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, uint rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(uint lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, uint rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(ulong lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(ulong lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(ulong lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(ulong lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, ulong rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(long lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, long rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(long lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, long rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(long lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, long rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(long lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, long rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(float lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, float rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(float lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, float rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(float lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, float rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(float lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, float rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }

        public static bool operator >(double lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs > rhs.Current.Value;
        }

        public static bool operator >(IndicatorBase lhs, double rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value > rhs;
        }

        public static bool operator <(double lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs < rhs.Current.Value;
        }

        public static bool operator <(IndicatorBase lhs, double rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value < rhs;
        }

        public static bool operator >=(double lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs >= rhs.Current.Value;
        }

        public static bool operator >=(IndicatorBase lhs, double rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value >= rhs;
        }

        public static bool operator <=(double lhs, IndicatorBase rhs) {
            return rhs.Current.IsScalar && lhs <= rhs.Current.Value;
        }

        public static bool operator <=(IndicatorBase lhs, double rhs) {
            return lhs.Current.IsScalar && lhs.Current.Value <= rhs;
        }
#endif
    }
}