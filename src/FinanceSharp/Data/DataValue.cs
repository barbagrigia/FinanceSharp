using System;

namespace FinanceSharp.Data {
    public interface IDataType { }

    public struct DataValue : IDataType {
        public double Value;

        public DataValue(double value) {
            Value = value;
        }
    }

    public struct DataPoint : IDataType {
        public DataValue Value;
        public long Time;

        public DataPoint(double value, long time) {
            Value = new DataValue(value);
            Time = time;
        }

        public DataPoint(double value, DateTime time) {
            Value = new DataValue(value);
            Time = time.ToEpochTime();
        }

        public DataPoint(double value, TimeSpan time) {
            Value = new DataValue(value);
            Time = time.ToEpochTime();
        }

        public DataPoint(DataValue value, long time) {
            Value = value;
            Time = time;
        }

        public DataPoint(DataValue value, DateTime time) {
            Value = value;
            Time = time.ToEpochTime();
        }

        public DataPoint(DataValue value, TimeSpan time) {
            Value = value;
            Time = time.ToEpochTime();
        }
    }

    public static class UnmanagedEnsurer {
#pragma warning disable 169
        private static MustBeUnmanaged<DataValue> _a;
        private static MustBeUnmanaged<DataPoint> _b;
#pragma warning restore 169

        // ReSharper disable once ClassNeverInstantiated.Local
        private class MustBeUnmanaged<T> where T : unmanaged { }
    }

    public static partial class Time {
        public static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
            return EpochStart.AddMilliseconds(epochMilliseconds).ToLocalTime();
        }
    }
}