using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Numeric;

namespace FinanceSharp.Tests.Helpers {
    public static class FluentAssertionsExt {
        /// <summary>
        /// Asserts a double value approximates another value as close as possible.
        /// </summary>
        /// <param name="parent">The <see cref="T:FluentAssertions.Numeric.NumericAssertions`1" /> object that is being extended.</param>
        /// <param name="expectedValue">
        /// The expected value to compare the actual value with.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of which the two values may differ.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="!:because" />.
        /// </param>
        public static AndConstraint<NumericAssertions<double>> BeApproximately(
            this NumericAssertions<double> parent,
            double expectedValue,
            string because = "",
            params object[] becauseArgs) {
            if (double.IsNaN(expectedValue) && double.IsNaN(expectedValue))
                return new AndConstraint<NumericAssertions<double>>(parent);
            if (double.IsPositiveInfinity(expectedValue) && double.IsPositiveInfinity(expectedValue))
                return new AndConstraint<NumericAssertions<double>>(parent);
            if (double.IsNegativeInfinity(expectedValue) && double.IsNegativeInfinity(expectedValue))
                return new AndConstraint<NumericAssertions<double>>(parent);
            double actualDifference = Math.Abs(expectedValue - (double) parent.Subject);
            FailIfDifferenceOutsidePrecision<double>(actualDifference <= Constants.ZeroEpsilon, parent, expectedValue, Constants.ZeroEpsilon, actualDifference, because, becauseArgs);
            return new AndConstraint<NumericAssertions<double>>(parent);
        }

        private static void FailIfDifferenceOutsidePrecision<T>(
            bool differenceWithinPrecision,
            NumericAssertions<T> parent,
            T expectedValue,
            T precision,
            T actualDifference,
            string because,
            object[] becauseArgs)
            where T : struct {
            Execute.Assertion.ForCondition(differenceWithinPrecision).BecauseOf(because, becauseArgs).FailWith("Expected {context:value} to approximate {1} +/- {2}{reason}, but {0} differed by {3}.", (object) parent.Subject, (object) expectedValue, (object) precision, (object) actualDifference);
        }
    }
}