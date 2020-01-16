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
using FinanceSharp.Data;
using FinanceSharp.Data.Market;
using FinanceSharp;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 Represents an indicator that is a ready after ingesting a single sample and
    /// 	 always returns the same value as it is given if it passes a filter condition
    /// </summary>
    public class FilteredIdentity : IndicatorBase {
        private DoubleArray _previousInput;
        private readonly Func<DoubleArray, bool> _filter;

        /// <summary>
        /// 	 Initializes a new instance of the FilteredIdentity indicator with the specified name
        /// </summary>
        /// <param name="name">The name of the indicator</param>
        /// <param name="filter">Filters the IBaseData send into the indicator, if null defaults to true (x => true) which means no filter</param>
        public FilteredIdentity(string name, Func<DoubleArray, bool> filter)
            : base(name) {
            // default our filter to true (do not filter)
            _filter = filter ?? (x => true);
        }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => Samples > 0 && (Current > 0).all();

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            if (_filter(input)) {
                _previousInput = input;
                return input;
            }

            if (_previousInput != null) {
                return _previousInput;
            }

            _previousInput = Constants.Empty;
            return _previousInput;
        }
    }
}