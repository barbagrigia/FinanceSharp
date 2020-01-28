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
using FinanceSharp.Data;
using FinanceSharp.Indicators.CandlestickPatterns;

namespace FinanceSharp.Indicators.CandlestickPatterns {
    /// <summary>
    /// Abstract base class for a candlestick pattern indicator
    /// </summary>
    public abstract class CandlestickPattern : WindowIndicator {
        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        public override int InputProperties => BarValue.Properties;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public override int Properties => IndicatorValue.Properties;

        /// <summary>
        /// Creates a new <see cref="CandlestickPattern"/> with the specified name
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="period">The number of data points to hold in the window</param>
        protected CandlestickPattern(string name, int period)
            : base(name, period) { }

        /// <summary>
        /// Returns the candle color of a candle
        /// </summary>
        /// <param name="tradeBar">The input candle</param>
        protected static CandleColor GetCandleColor(DoubleArray tradeBar) {
            return tradeBar.Close >= tradeBar.Open ? CandleColor.White : CandleColor.Black;
        }

        /// <summary>
        /// Returns the distance between the close and the open of a candle
        /// </summary>
        /// <param name="tradeBar">The input candle</param>
        protected static double GetRealBody(DoubleArray tradeBar) {
            return Math.Abs(tradeBar.Close - tradeBar.Open);
        }

        /// <summary>
        /// Returns the full range of the candle
        /// </summary>
        /// <param name="tradeBar">The input candle</param>
        protected static double GetHighLowRange(DoubleArray tradeBar) {
            return tradeBar.High - tradeBar.Low;
        }

        /// <summary>
        /// Returns the range of a candle
        /// </summary>
        /// <param name="type">The type of setting to use</param>
        /// <param name="tradeBar">The input candle</param>
        protected static double GetCandleRange(CandleSettingType type, DoubleArray tradeBar) {
            switch (CandleSettings.Get(type).RangeType) {
                case CandleRangeType.RealBody:
                    return GetRealBody(tradeBar);

                case CandleRangeType.HighLow:
                    return GetHighLowRange(tradeBar);

                case CandleRangeType.Shadows:
                    return GetUpperShadow(tradeBar) + GetLowerShadow(tradeBar);

                default:
                    return 0d;
            }
        }

        /// <summary>
        /// Returns true if the candle is higher than the previous one
        /// </summary>
        protected static bool GetCandleGapUp(DoubleArray tradeBar, DoubleArray previousBar) {
            return tradeBar.Low > previousBar.High;
        }

        /// <summary>
        /// Returns true if the candle is lower than the previous one
        /// </summary>
        protected static bool GetCandleGapDown(DoubleArray tradeBar, DoubleArray previousBar) {
            return tradeBar.High < previousBar.Low;
        }

        /// <summary>
        /// Returns true if the candle is higher than the previous one (with no body overlap)
        /// </summary>
        protected static bool GetRealBodyGapUp(DoubleArray tradeBar, DoubleArray previousBar) {
            return Math.Min(tradeBar.Open, tradeBar.Close) > Math.Max(previousBar.Open, previousBar.Close);
        }

        /// <summary>
        /// Returns true if the candle is lower than the previous one (with no body overlap)
        /// </summary>
        protected static bool GetRealBodyGapDown(DoubleArray tradeBar, DoubleArray previousBar) {
            return Math.Max(tradeBar.Open, tradeBar.Close) < Math.Min(previousBar.Open, previousBar.Close);
        }

        /// <summary>
        /// Returns the range of the candle's lower shadow
        /// </summary>
        /// <param name="tradeBar">The input candle</param>
        protected static double GetLowerShadow(DoubleArray tradeBar) {
            return (tradeBar.Close >= tradeBar.Open ? tradeBar.Open : tradeBar.Close) - tradeBar.Low;
        }

        /// <summary>
        /// Returns the range of the candle's upper shadow
        /// </summary>
        /// <param name="tradeBar">The input candle</param>
        protected static double GetUpperShadow(DoubleArray tradeBar) {
            return tradeBar.High - (tradeBar.Close >= tradeBar.Open ? tradeBar.Close : tradeBar.Open);
        }

        /// <summary>
        /// Returns the average range of the previous candles
        /// </summary>
        /// <param name="type">The type of setting to use</param>
        /// <param name="sum">The sum of the previous candles ranges</param>
        /// <param name="tradeBar">The input candle</param>
        protected static double GetCandleAverage(CandleSettingType type, double sum, DoubleArray tradeBar) {
            var defaultSetting = CandleSettings.Get(type);

            return defaultSetting.Factor *
                   (defaultSetting.AveragePeriod != 0 ? sum / defaultSetting.AveragePeriod : GetCandleRange(type, tradeBar)) /
                   (defaultSetting.RangeType == CandleRangeType.Shadows ? 2.0d : 1.0d);
        }
    }
}