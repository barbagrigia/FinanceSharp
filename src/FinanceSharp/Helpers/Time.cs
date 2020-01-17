using System;
using System.Runtime.CompilerServices;
using FinanceSharp.Data;

// ReSharper disable once CheckNamespace
namespace FinanceSharp {
    /// <summary>
    ///     Conversion methods for epoch time and other time formats.
    /// </summary>
    public static partial class Time {
        public static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Provides UTC time now in milliseconds-epoch time.
        /// </summary>
        public static long Now => (long) DateTime.UtcNow.Subtract(EpochStart).TotalMilliseconds;

        /// <summary>
        ///     Provides local time now in milliseconds-epoch time.
        /// </summary>
        public static long LocalNow => (long) new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc).Subtract(EpochStart).TotalMilliseconds;

        /// <summary>
        ///     Converts <see cref="TimeSpan"/> to milliseconds-epoch time represented by <see cref="long"/>.
        /// </summary>
        public static long ToEpochTime(this TimeSpan ts) {
            return (long) ts.TotalMilliseconds;
        }

        /// <summary>
        ///     Converts <see cref="DateTime"/> to milliseconds-epoch time represented by <see cref="long"/>.
        /// </summary>
        public static long ToEpochTime(this DateTime ts) {
            return (long) new DateTime(ts.Ticks, DateTimeKind.Utc).Subtract(EpochStart).TotalMilliseconds;
        }

        /// <summary>
        ///     Converts <see cref="DateTime"/> to milliseconds-epoch time represented by <see cref="long"/>.
        /// </summary>
        public static long ToEpochTime(this DateTimeOffset ts) {
            return (long) new DateTime(ts.Ticks, DateTimeKind.Utc).Subtract(EpochStart).TotalMilliseconds;
        }

        /// <summary>
        ///     Converts milliseconds-epoch time to <see cref="DateTime"/> represented by <see cref="long"/>.
        /// </summary>
        public static DateTime ToDateTime(this long epochMilliseconds) {
            return EpochStart.AddMilliseconds(epochMilliseconds);
        }
    }
}