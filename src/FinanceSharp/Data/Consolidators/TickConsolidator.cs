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

namespace FinanceSharp.Data.Consolidators {
    /// <summary>
    /// 	 A data consolidator that can make bigger bars from ticks over a given
    /// 	 time span or a count of pieces of data.
    /// </summary>
    public class TickConsolidator : TradeBarConsolidatorBase {
        /// <summary>
        /// 	 Creates a consolidator to produce a new 'TradeBar' representing the period
        /// </summary>
        /// <param name="period">The minimum span of time before emitting a consolidated bar</param>
        public TickConsolidator(TimeSpan period)
            : base(period) { }

        /// <summary>
        /// 	 Creates a consolidator to produce a new 'TradeBar' representing the last count pieces of data
        /// </summary>
        /// <param name="maxCount">The number of pieces to accept before emitting a consolidated bar</param>
        public TickConsolidator(int maxCount)
            : base(maxCount) { }

        /// <summary>
        /// 	 Creates a consolidator to produce a new 'TradeBar' representing the last count pieces of data or the period, whichever comes first
        /// </summary>
        /// <param name="maxCount">The number of pieces to accept before emitting a consolidated bar</param>
        /// <param name="period">The minimum span of time before emitting a consolidated bar</param>
        public TickConsolidator(int maxCount, TimeSpan period)
            : base(maxCount, period) { }

        /// <summary>
        /// 	 Aggregates the new 'data' into the 'workingBar'. The 'workingBar' will be
        /// 	 null following the event firing
        /// </summary>
        /// <param name="workingBar">The bar we're building, null if the event was just fired and we're starting a new consolidated bar</param>
        /// <param name="data">The new data</param>
        protected override void AggregateBar(ref long workingTime, ref DoubleArray workingBar, long time, DoubleArray data) {
#if DEBUG
            if (data.Properties < TickValue.Properties)
                throw new ArgumentException($"data was expected to be TickValue with properties>={TickValue.Properties}", nameof(data));
#endif
            var value = data.Value;
            if (workingBar == null) {
                workingTime = GetRoundedBarTime(time);
                workingBar = new DoubleArray(1, TradeBarVolumedValue.Properties) {
                    Close = value,
                    High = value,
                    Low = value,
                    Open = value,
                    Volume = data.Volume
                };
            } else {
                //Aggregate the working bar
                workingBar.Close = value;
                workingBar.Volume += data.Volume;
                if (data.Value < workingBar.Low) workingBar.Low = value;
                if (data.Value > workingBar.High) workingBar.High = value;
            }
        }
    }
}

