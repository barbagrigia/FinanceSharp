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
    /// 	 This indicator computes the Heikin-Ashi bar (HA)
    /// 	 The Heikin-Ashi bar is calculated using the following formulas:
    /// 	 HA_Close[0] = (Open[0] + High[0] + Low[0] + Close[0]) / 4
    /// 	 HA_Open[0] = (HA_Open[1] + HA_Close[1]) / 2
    /// 	 HA_High[0] = MAX(High[0], HA_Open[0], HA_Close[0])
    /// 	 HA_Low[0] = MIN(Low[0], HA_Open[0], HA_Close[0])
    /// </summary>
    public class HeikinAshi : BarIndicator {
        /// <summary>
        /// 	 Gets the Heikin-Ashi Open
        /// </summary>
        public double Open => Current.Open;

        /// <summary>
        /// 	 Gets the Heikin-Ashi High
        /// </summary>
        public double High => Current.High;

        /// <summary>
        /// 	 Gets the Heikin-Ashi Low
        /// </summary>
        public double Low => Current.Low;

        /// <summary>
        /// 	 Gets the Heikin-Ashi Close
        /// </summary>
        public double Close => Current.Close;

        /// <summary>
        /// 	 Gets the Heikin-Ashi Volume
        /// </summary>
        public double Volume => Current.Volume;

        /// <summary>
        /// 	 Gets the Heikin-Ashi current TradeBar
        /// </summary>
        public TradeBarValue CurrentBar => Current.Get<TradeBarValue>(0);

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="HeikinAshi"/> class using the specified name.
        /// </summary> 
        /// <param name="name">The name of this indicator</param>
        public HeikinAshi(string name)
            : base(name) { }

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="HeikinAshi"/> class.
        /// </summary> 
        public HeikinAshi()
            : this("HA") { }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => Samples > 1;

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public override int WarmUpPeriod => 2;

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns> A new value for this indicator </returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            if (!IsReady) {
                Current = new DoubleArray2DManaged(1, TradeBarValue.Properties);
                Current.Open = (input.Open + input.Close) / 2d;
                Current.Close = (input.Open + input.High + input.Low + input.Close) / 4d;
                Current.High = input.High;
                Current.Low = input.Low;
            } else {
                Current.Open = (Current.Open + Current.Close) / 2d;
                Current.Close = (input.Open + input.High + input.Low + input.Close) / 4d;
                Current.High = Math.Max(input.High, Math.Max(Current.Open, Current.Close));
                Current.Low = Math.Min(input.Low, Math.Min(Current.Open, Current.Close));
            }

            return Current;
        }
    }
}