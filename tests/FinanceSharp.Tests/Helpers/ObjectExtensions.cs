using System.Collections;
using System.Linq;

namespace FinanceSharp.Tests.Helpers {
    public static class ObjectExtensions {
        /// <summary>
        ///     Does nothing.
        /// </summary>
        public static void Void<T>(this T obj) { }

        /// <summary>
        ///     Evaluates <see cref="IEnumerable"/> and does nothing.
        /// </summary>
        public static void Void<T>(this IEnumerable objs) {
            objs.Cast<object>().ToArray();
        }
    }
}