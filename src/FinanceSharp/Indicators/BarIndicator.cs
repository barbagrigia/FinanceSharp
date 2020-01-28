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
using System.Diagnostics;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 The BarIndicator is an indicator that accepts DoubleArray data as its input.
    /// 
    /// 	 This type is more of a shim/typedef to reduce the need to refer to things as IndicatorBase&lt;DoubleArray&gt;
    /// </summary>
    public abstract class BarIndicator : IndicatorBase {
        protected readonly int _properties;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public override int Properties => _properties;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public override int InputProperties => _properties;

        /// <summary>
        /// 	 Creates a new TradeBarIndicator with the specified name
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        protected BarIndicator(string name, int properties = TradeBarValue.Properties)
            : base(name) {
            _properties = properties;
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// 	 and returns an instance of the <see cref="IndicatorResult"/> class
        /// </summary>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>An IndicatorResult object including the status of the indicator</returns>
        protected override IndicatorResult ValidateAndForward(long time, DoubleArray input) {
#if DEBUG

            if (input.Properties < 4) //4 for OHLC
                throw new ArgumentException("input doesn't have more than 4 values - therefore can't be considered a TradeBarValue.");
#endif
            // default implementation always returns IndicatorStatus.Success
            return new IndicatorResult(Forward(time, input));
        }
    }
}