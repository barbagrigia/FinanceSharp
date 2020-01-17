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
using FinanceSharp;

namespace FinanceSharp.Data.Consolidators {
    /// <summary>
    /// 	 Provides an implementation of <see cref="IDataConsolidator"/> that preserve the input
    /// 	 data unmodified. The input data is filtering by the specified predicate function
    /// </summary>
    /// <typeparam name="DoubleArray">The type of data</typeparam>
    public partial class FilteredIdentityDataConsolidator : IdentityDataConsolidator {
        private readonly Func<long, DoubleArray, bool> _predicate;

        /// <summary>
        /// 	 Initializes a new instance of the <see cref="FilteredIdentityDataConsolidator{DoubleArray}"/> class
        /// </summary>
        /// <param name="predicate">The predicate function, returning true to accept data and false to reject data</param>
        public FilteredIdentityDataConsolidator(Func<long, DoubleArray, bool> predicate) {
            _predicate = predicate;
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        public override bool Update(long time, DoubleArray data) {
            // only permit data that passes our predicate function to be passed through
            if (_predicate(time, data)) {
                base.Update(time, data);
                return true;
            }

            return false;
        }
    }
}