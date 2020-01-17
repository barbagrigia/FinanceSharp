using System;

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray : IEquatable<DoubleArray> {
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(DoubleArray other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Address == other.Address && Count == other.Count && Properties == other.Properties;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DoubleArray) obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            unchecked {
                var hashCode = unchecked((int) (long) Address);
                hashCode = (hashCode * 397) ^ Count;
                hashCode = (hashCode * 397) ^ Properties;
                return hashCode;
            }
        }

        /// <summary>Returns a value that indicates whether the values of two <see cref="T:FinanceSharp.Data.DoubleArray" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(DoubleArray left, DoubleArray right) {
            return Equals(left, right);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:FinanceSharp.Data.DoubleArray" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(DoubleArray left, DoubleArray right) {
            return !Equals(left, right);
        }
    }
}