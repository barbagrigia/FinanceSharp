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


using FinanceSharp.Data.Rolling;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators.CandlestickPatterns {
    /// <summary>
    /// 	 Two Crows candlestick pattern indicator
    /// </summary>
    /// <remarks>
    /// 	 Must have:
    /// - first candle: long white candle
    /// - second candle: black real body
    /// - gap between the first and the second candle's real bodies
    /// - third candle: black candle that opens within the second real body and closes within the first real body
    /// 	 The meaning of "long" is specified with SetCandleSettings
    /// 	 The returned value is negative (-1): two crows is always bearish;
    /// 	 The user should consider that two crows is significant when it appears in an uptrend, while this function
    /// 	 does not consider the trend.
    /// </remarks>
    public class TwoCrows : CandlestickPattern {
        private readonly int _bodyLongAveragePeriod;

        private double _bodyLongPeriodTotal;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="TwoCrows"/> class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        public TwoCrows(string name)
            : base(name, CandleSettings.Get(CandleSettingType.BodyLong).AveragePeriod + 2 + 1) {
            _bodyLongAveragePeriod = CandleSettings.Get(CandleSettingType.BodyLong).AveragePeriod;
        }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="TwoCrows"/> class.
        /// </summary>
        public TwoCrows()
            : this("TWOCROWS") { }

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
                if (Samples >= Period - _bodyLongAveragePeriod - 2 && Samples < Period - 2) {
                    _bodyLongPeriodTotal += GetCandleRange(CandleSettingType.BodyLong, input);
                }

                return Constants.Zero;
            }

            double value;
            if (
                // 1st: white
                GetCandleColor(window[2]) == CandleColor.White &&
                //      long
                GetRealBody(window[2]) > GetCandleAverage(CandleSettingType.BodyLong, _bodyLongPeriodTotal, window[2]) &&
                // 2nd: black
                GetCandleColor(window[1]) == CandleColor.Black &&
                //      gapping up
                GetRealBodyGapUp(window[1], window[2]) &&
                // 3rd: black
                GetCandleColor(input) == CandleColor.Black &&
                //      opening within 2nd rb
                input.Open < window[1].Open && input.Open > window[1].Close &&
                //      closing within 1st rb
                input[CloseIdx] > window[2].Open && input[CloseIdx] < window[2].Close
            )
                value = -1d;
            else
                value = Constants.Zero;

            // add the current range and subtract the first range: this is done after the pattern recognition 
            // when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)

            _bodyLongPeriodTotal += GetCandleRange(CandleSettingType.BodyLong, window[2]) -
                                    GetCandleRange(CandleSettingType.BodyLong, window[2 + _bodyLongAveragePeriod]);

            return value;
        }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            _bodyLongPeriodTotal = Constants.Zero;
            base.Reset();
        }
    }
}