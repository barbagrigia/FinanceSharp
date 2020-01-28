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
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray {
        public virtual double Value {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => *Address;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => *Address = value;
        }

        public virtual double Close {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(Properties >= Constants.CloseIdx + 1);
                return Address[Constants.CloseIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(Properties >= Constants.CloseIdx + 1);
                Address[Constants.CloseIdx] = value;
            }
        }

        public virtual double Open {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
                return Address[Constants.OpenIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
                Address[Constants.OpenIdx] = value;
            }
        }

        public virtual double High {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
                return Address[Constants.HighIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
                Address[Constants.HighIdx] = value;
            }
        }

        public virtual double Low {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
                return Address[Constants.LowIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
                Address[Constants.LowIdx] = value;
            }
        }

        public virtual double Volume {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
                return Address[Constants.VolumeIdx];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
                Address[Constants.VolumeIdx] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double ValueAt(int index) {
            return Address[index * Properties];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CloseAt(int index) {
            Assert(index < Count);
            Assert(Properties >= Constants.CloseIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.CloseIdx");
            return Address[index * Properties + Constants.CloseIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double OpenAt(int index) {
            Assert(index < Count);
            Assert(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
            return Address[index * Properties + Constants.OpenIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double HighAt(int index) {
            Assert(index < Count);
            Assert(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
            return Address[index * Properties + Constants.HighIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double LowAt(int index) {
            Assert(index < Count);
            Assert(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
            return Address[index * Properties + Constants.LowIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double VolumeAt(int index) {
            Assert(index < Count);
            Assert(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
            return Address[index * Properties + Constants.VolumeIdx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetValueAt(int index, double value) {
            Assert(index < Count);
            return Address[index * Properties] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetCloseAt(int index, double value) {
            Assert(index < Count);
            Assert(Properties >= Constants.CloseIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.CloseIdx");
            return Address[index * Properties + Constants.CloseIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetOpenAt(int index, double value) {
            Assert(index < Count);
            Assert(Properties >= Constants.OpenIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.OpenIdx");
            return Address[index * Properties + Constants.OpenIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetHighAt(int index, double value) {
            Assert(index < Count);
            Assert(Properties >= Constants.HighIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.HighIdx");
            return Address[index * Properties + Constants.HighIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetLowAt(int index, double value) {
            Assert(index < Count);
            Assert(Properties >= Constants.LowIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.LowIdx");
            return Address[index * Properties + Constants.LowIdx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double SetVolumeAt(int index, double value) {
            Assert(index < Count);
            Assert(Properties >= Constants.VolumeIdx + 1, $"Properties count is too small ({Properties}) to be able to contain Constants.VolumeIdx");
            return Address[index * Properties + Constants.VolumeIdx] = value;
        }

        /// <summary>
        ///     Scalar overload.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual double this[int property] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Assert(IsScalar, "Scalar only overload was called but the array is not scalar.");
                return Address[property];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                Assert(IsScalar, "Scalar only overload was called but the array is not scalar.");
                Address[property] = value;
            }
        }

        public virtual double this[int index, int property] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Address[index * Properties + property]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Address[index] = value; }
        }

        public virtual TStruct Get<TStruct>(int index) where TStruct : unmanaged, DataStruct {
            Assert(sizeof(TStruct) == sizeof(double) * Properties);
            return ((TStruct*) Address)[index];
        }
    }
}