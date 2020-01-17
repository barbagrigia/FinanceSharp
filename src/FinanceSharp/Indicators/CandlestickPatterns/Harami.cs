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

using System;
using FinanceSharp.Data.Market;
using FinanceSharp.Data.Rolling;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators.CandlestickPatterns {
    /// <summary>
    /// 	 Harami candlestick pattern indicator
    /// </summary>
    /// <remarks>
    /// 	 Must have:
    /// - first candle: long white (black) real body
    /// - second candle: short real body totally engulfed by the first
    /// 	 The meaning of "short" and "long" is specified with SetCandleSettings
    /// 	 The returned value is positive(+1) when bullish or negative(-1) when bearish;
    /// 	 The user should consider that a harami is significant when it appears in a downtrend if bullish or
    /// 	 in an uptrend when bearish, while this function does not consider the trend
    /// </remarks>
    public class Harami : CandlestickPattern {
        private readonly int _bodyLongAveragePeriod;
        private readonly int _bodyShortAveragePeriod;

        private double _bodyLongPeriodTotal;
        private double _bodyShortPeriodTotal;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="Harami"/> class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        public Harami(string name)
            : base(name, Math.Max(CandleSettings.Get(CandleSettingType.BodyLong).AveragePeriod, CandleSettings.Get(CandleSettingType.BodyShort).AveragePeriod) + 1 + 1) {
            _bodyLongAveragePeriod = CandleSettings.Get(CandleSettingType.BodyLong).AveragePeriod;
            _bodyShortAveragePeriod = CandleSettings.Get(CandleSettingType.BodyShort).AveragePeriod;
        }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="Harami"/> class.
        /// </summary>
        public Harami()
            : this("HARAMI") { }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady {
            get { return Samples >= Period; }
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="timeWindow"></param>
        /// <param name="window">The window of data held in this indicator</param>
        /// <param name="time"></param>
        /// <param name="input"></param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(IReadOnlyWindow<long> timeWindow, IReadOnlyWindow<DoubleArray> window, long time, DoubleArray input) {
            if (!IsReady) {
                if (Samples >= Period - _bodyLongAveragePeriod - 1 && Samples < Period - 1) {
                    _bodyLongPeriodTotal += GetCandleRange(CandleSettingType.BodyLong, input);
                }

                if (Samples >= Period - _bodyShortAveragePeriod) {
                    _bodyShortPeriodTotal += GetCandleRange(CandleSettingType.BodyShort, input);
                }

                return Constants.Zero;
            }

            double value;
            if (
                // 1st: long
                GetRealBody(window[1]) > GetCandleAverage(CandleSettingType.BodyLong, _bodyLongPeriodTotal, window[1]) &&
                // 2nd: short
                GetRealBody(input) <= GetCandleAverage(CandleSettingType.BodyShort, _bodyShortPeriodTotal, input) &&
                //      engulfed by 1st
                Math.Max(input[CloseIdx], input.Open) < Math.Max(window[1].Close, window[1].Open) &&
                Math.Min(input[CloseIdx], input.Open) > Math.Min(window[1].Close, window[1].Open)
            )
                value = -(int) GetCandleColor(window[1]);
            else
                value = Constants.Zero;

            // add the current range and subtract the first range: this is done after the pattern recognition 
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)

            _bodyLongPeriodTotal += GetCandleRange(CandleSettingType.BodyLong, window[1]) -
                                    GetCandleRange(CandleSettingType.BodyLong, window[_bodyLongAveragePeriod + 1]);

            _bodyShortPeriodTotal += GetCandleRange(CandleSettingType.BodyShort, input) -
                                     GetCandleRange(CandleSettingType.BodyShort, window[_bodyShortAveragePeriod]);

            return value;
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            _bodyLongPeriodTotal = Constants.Zero;
            _bodyShortPeriodTotal = Constants.Zero;
            base.Reset();
        }
    }
}