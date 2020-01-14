using System;

namespace QuantConnect {
    public class Log {
        public static void Error(Exception exception, string workerthreadExceptionThrownWhenRunningTask = null) {
            throw exception; //TODO
        }

        public static void Trace(string txt) {
            Console.WriteLine(txt);
        }

        internal static void Error(string txt) {
            throw new Exception(txt);
        }
    }
}