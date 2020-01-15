/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by: 
 * 
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
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

using Torch;
using FinanceSharp.Helpers;

namespace FinanceSharp.Indicators {
    public abstract partial class IndicatorBase {
        /// <summary>
        /// 	 Returns the current value of this instance
        /// </summary>
        /// <param name="instance">The indicator instance</param>
        /// <returns>The current value of the indicator</returns>
        public static implicit operator Tensor<double>(IndicatorBase instance) {
            return instance.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than the specified value
        /// </summary>
        public static Tensor<bool> operator >(IndicatorBase left, double right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current > right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than the specified value
        /// </summary>
        public static Tensor<bool> operator <(IndicatorBase left, double right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current < right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >(double left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left > right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <(double left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left < right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator >=(IndicatorBase left, double right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current >= right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator <=(IndicatorBase left, double right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current <= right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >=(double left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left >= right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <=(double left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            
            return (Tensor<double>)left <= right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is equal to the specified value
        /// </summary>
        public static Tensor<bool> operator ==(IndicatorBase left, double right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return (Tensor<bool>) (left.Current == right);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is not equal to the specified value
        /// </summary>
        public static Tensor<bool> operator !=(IndicatorBase left, double right) {
            if (ReferenceEquals(left, null)) return Constants.True;
            return (Tensor<bool>) (left.Current != right);
        }

        /// <summary>
        /// 	 Determines if the specified value is equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator ==(double left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<bool>) ((Tensor<double>)left == right.Current);
        }

        /// <summary>
        /// 	 Determines if the specified value is not equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator !=(double left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.True;
            return (Tensor<bool>) ((Tensor<double>)left != right.Current);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than the specified value
        /// </summary>
        public static Tensor<bool> operator >(IndicatorBase left, float right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current > right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than the specified value
        /// </summary>
        public static Tensor<bool> operator <(IndicatorBase left, float right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current < right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >(float left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left > right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <(float left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left < right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator >=(IndicatorBase left, float right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current >= right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator <=(IndicatorBase left, float right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current <= right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >=(float left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left >= right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <=(float left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left <= right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is equal to the specified value
        /// </summary>
        public static Tensor<bool> operator ==(IndicatorBase left, float right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return (Tensor<bool>) (left.Current == right);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is not equal to the specified value
        /// </summary>
        public static Tensor<bool> operator !=(IndicatorBase left, float right) {
            if (ReferenceEquals(left, null)) return Constants.True;
            return (Tensor<bool>) (left.Current != right);
        }

        /// <summary>
        /// 	 Determines if the specified value is equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator ==(float left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<bool>) ((Tensor<double>)left == right.Current);
        }

        /// <summary>
        /// 	 Determines if the specified value is not equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator !=(float left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.True;
            return (Tensor<bool>) ((Tensor<double>)left != right.Current);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than the specified value
        /// </summary>
        public static Tensor<bool> operator >(IndicatorBase left, int right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current > right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than the specified value
        /// </summary>
        public static Tensor<bool> operator <(IndicatorBase left, int right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current < right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >(int left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left > right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <(int left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left < right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator >=(IndicatorBase left, int right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current >= right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator <=(IndicatorBase left, int right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current <= right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >=(int left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left >= right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <=(int left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left <= right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is equal to the specified value
        /// </summary>
        public static Tensor<bool> operator ==(IndicatorBase left, int right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return (Tensor<bool>) (left.Current == right);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is not equal to the specified value
        /// </summary>
        public static Tensor<bool> operator !=(IndicatorBase left, int right) {
            if (ReferenceEquals(left, null)) return Constants.True;
            return (Tensor<bool>) (left.Current != right);
        }

        /// <summary>
        /// 	 Determines if the specified value is equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator ==(int left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<bool>) ((Tensor<double>)left == right.Current);
        }

        /// <summary>
        /// 	 Determines if the specified value is not equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator !=(int left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.True;
            return (Tensor<bool>) ((Tensor<double>)left != right.Current);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than the specified value
        /// </summary>
        public static Tensor<bool> operator >(IndicatorBase left, long right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current > right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than the specified value
        /// </summary>
        public static Tensor<bool> operator <(IndicatorBase left, long right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current < right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >(long left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left > right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <(long left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left < right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is greater than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator >=(IndicatorBase left, long right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current >= right;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is less than or equal to the specified value
        /// </summary>
        public static Tensor<bool> operator <=(IndicatorBase left, long right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return left.Current <= right;
        }

        /// <summary>
        /// 	 Determines if the specified value is greater than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator >=(long left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left >= right.Current;
        }

        /// <summary>
        /// 	 Determines if the specified value is less than or equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator <=(long left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<double>)left <= right.Current;
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is equal to the specified value
        /// </summary>
        public static Tensor<bool> operator ==(IndicatorBase left, long right) {
            if (ReferenceEquals(left, null)) return Constants.False;
            return (Tensor<bool>) (left.Current == right);
        }

        /// <summary>
        /// 	 Determines if the indicator's current value is not equal to the specified value
        /// </summary>
        public static Tensor<bool> operator !=(IndicatorBase left, long right) {
            if (ReferenceEquals(left, null)) return Constants.True;
            return (Tensor<bool>) (left.Current != right);
        }

        /// <summary>
        /// 	 Determines if the specified value is equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator ==(long left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.False;
            return (Tensor<bool>) ((Tensor<double>)left == right.Current);
        }

        /// <summary>
        /// 	 Determines if the specified value is not equal to the indicator's current value
        /// </summary>
        public static Tensor<bool> operator !=(long left, IndicatorBase right) {
            if (ReferenceEquals(right, null)) return Constants.True;
            return (Tensor<bool>) ((Tensor<double>)left != right.Current);
        }
    }
}