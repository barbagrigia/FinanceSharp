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

using System;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 This indicator computes Average Directional Index which measures trend strength without regard to trend direction.
    /// 	 Firstly, it calculates the Directional Movement and the True Range value, and then the values are accumulated and smoothed
    /// 	 using a custom smoothing method proposed by Wilder. For an n period smoothing, 1/n of each period's value is added to the total period.
    /// 	 From these accumulated values we are therefore able to derived the 'Positive Directional Index' (+DI) and 'Negative Directional Index' (-DI)
    /// 	 which is used to calculate the Average Directional Index.
    /// 	 Computation source:
    /// 	 https://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:average_directional_index_adx
    /// </summary>
    public class AverageDirectionalIndex : BarIndicator {
        private readonly int _period;
        private readonly IndicatorBase _trueRange;
        private readonly IndicatorBase _directionalMovementPlus;
        private readonly IndicatorBase _directionalMovementMinus;
        private readonly IndicatorBase _smoothedTrueRange;
        private readonly IndicatorBase _smoothedDirectionalMovementPlus;
        private readonly IndicatorBase _smoothedDirectionalMovementMinus;
        private readonly IndicatorBase _averageDirectionalIndex;
        private DoubleArray _previousInput;

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => _averageDirectionalIndex.IsReady;

        /// <summary>
        /// 	 Gets the index of the Plus Directional Indicator
        /// </summary>
        /// <value>
        /// 	 The index of the Plus Directional Indicator.
        /// </value>
        public IndicatorBase PositiveDirectionalIndex { get; }

        /// <summary>
        /// 	 Gets the index of the Minus Directional Indicator
        /// </summary>
        /// <value>
        /// 	 The index of the Minus Directional Indicator.
        /// </value>
        public IndicatorBase NegativeDirectionalIndex { get; }

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public override int WarmUpPeriod => _period * 2;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="AverageDirectionalIndex"/> class.
        /// </summary>
        /// <param name="period">The period.</param>
        public AverageDirectionalIndex(int period)
            : this($"ADX({period})", period) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="AverageDirectionalIndex"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="period">The period.</param>
        public AverageDirectionalIndex(string name, int period)
            : base(name) {
            _period = period;

            _trueRange = new FunctionalIndicator(name + "_TrueRange",
                ComputeTrueRange,
                isReady => _previousInput != null
            );

            _directionalMovementPlus = new FunctionalIndicator(name + "_PositiveDirectionalMovement",
                ComputePositiveDirectionalMovement,
                isReady => _previousInput != null
            );

            _directionalMovementMinus = new FunctionalIndicator(name + "_NegativeDirectionalMovement",
                ComputeNegativeDirectionalMovement,
                isReady => _previousInput != null
            );

            PositiveDirectionalIndex = new FunctionalIndicator(name + "_PositiveDirectionalIndex",
                (time, input) => {
                    // Computes the Plus Directional Indicator(+DI period).
                    if (_smoothedTrueRange != 0 && _smoothedDirectionalMovementPlus.IsReady) {
                        return (DoubleArray) 100d * _smoothedDirectionalMovementPlus / _smoothedTrueRange;
                    }

                    return Constants.Zero;
                },
                positiveDirectionalIndex => _smoothedDirectionalMovementPlus.IsReady,
                () => {
                    _directionalMovementPlus.Reset();
                    _trueRange.Reset();
                }
            );

            NegativeDirectionalIndex = new FunctionalIndicator(name + "_NegativeDirectionalIndex",
                (time, input) => {
                    // Computes the Minus Directional Indicator(-DI period).
                    if (_smoothedTrueRange != 0 && _smoothedDirectionalMovementMinus.IsReady) {
                        return 100d * _smoothedDirectionalMovementMinus / _smoothedTrueRange;
                    }

                    return Constants.Zero;
                },
                negativeDirectionalIndex => _smoothedDirectionalMovementMinus.IsReady,
                () => {
                    _directionalMovementMinus.Reset();
                    _trueRange.Reset();
                }
            );

            _smoothedTrueRange = new FunctionalIndicator(name + "_SmoothedTrueRange",
                (time, input) => {
                    // Computes the Smoothed True Range value.
                    var value = Samples > _period + 1 ? _smoothedTrueRange / _period : Constants.Zero;
                    return _smoothedTrueRange + _trueRange - value;
                },
                isReady => Samples > period
            );

            _smoothedDirectionalMovementPlus = new FunctionalIndicator(name + "_SmoothedDirectionalMovementPlus",
                (time, input) => {
                    // Computes the Smoothed Directional Movement Plus value.
                    var value = Samples > _period + 1 ? _smoothedDirectionalMovementPlus / _period : Constants.Zero;
                    return _smoothedDirectionalMovementPlus + _directionalMovementPlus - value;
                },
                isReady => Samples > period
            );

            _smoothedDirectionalMovementMinus = new FunctionalIndicator(name + "_SmoothedDirectionalMovementMinus",
                (time, input) => {
                    // Computes the Smoothed Directional Movement Minus value.
                    var value = Samples > _period + 1 ? _smoothedDirectionalMovementMinus / _period : Constants.Zero;
                    return _smoothedDirectionalMovementMinus + _directionalMovementMinus - value;
                },
                isReady => Samples > period
            );

            _averageDirectionalIndex = new WilderMovingAverage(period);
        }

        /// <summary>
        /// 	 Computes the True Range value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private DoubleArray ComputeTrueRange(long time, DoubleArray input) {
            if (_previousInput == null) return Constants.Zero;

            var range1 = input.High - input.Low;
            var range2 = Math.Abs(input.High - _previousInput.Close);
            var range3 = Math.Abs(input.Low - _previousInput.Close);

            return Math.Max(range1, Math.Max(range2, range3));
        }

        /// <summary>
        /// 	 Computes the positive directional movement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private DoubleArray ComputePositiveDirectionalMovement(long time, DoubleArray input) {
            if (_previousInput != null &&
                input.High > _previousInput.High &&
                input.High - _previousInput.High >= _previousInput.Low - input.Low) {
                return input.High - _previousInput.High;
            }

            return 0d;
        }

        /// <summary>
        /// 	 Computes the negative directional movement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private DoubleArray ComputeNegativeDirectionalMovement(long time, DoubleArray input) {
            if (_previousInput != null &&
                _previousInput.Low > input.Low &&
                _previousInput.Low - input.Low > input.High - _previousInput.High) {
                return _previousInput.Low - input.Low;
            }

            return 0d;
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            _trueRange.Update(time, input);
            _directionalMovementPlus.Update(time, input);
            _directionalMovementMinus.Update(time, input);
            _smoothedTrueRange.Update(time, Current);
            _smoothedDirectionalMovementPlus.Update(time, Current);
            _smoothedDirectionalMovementMinus.Update(time, Current);
            _previousInput = input.Clone();

            PositiveDirectionalIndex.Update(time, Current);
            NegativeDirectionalIndex.Update(time, Current);

            var diff = Math.Abs(PositiveDirectionalIndex.Current.Value - NegativeDirectionalIndex);
            var sum = PositiveDirectionalIndex + NegativeDirectionalIndex;
            if (sum == 0) return (DoubleArray) 50d;

            _averageDirectionalIndex.Update(time, 100d * diff / sum);

            return _averageDirectionalIndex;
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            _previousInput = null;
            _trueRange.Reset();
            _directionalMovementPlus.Reset();
            _directionalMovementMinus.Reset();
            _smoothedTrueRange.Reset();
            _smoothedDirectionalMovementPlus.Reset();
            _smoothedDirectionalMovementMinus.Reset();
            _averageDirectionalIndex.Reset();
            PositiveDirectionalIndex.Reset();
            NegativeDirectionalIndex.Reset();
            base.Reset();
        }
    }
}