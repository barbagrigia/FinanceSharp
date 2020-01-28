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


namespace FinanceSharp.Data.Consolidators {
    /// <summary>
    /// 	 Represents the simplest DataConsolidator implementation, one that is defined
    /// 	 by a straight pass through of the data. No projection or aggregation is performed.
    /// </summary>
    /// <typeparam name="DoubleArray">The type of data</typeparam>
    public class IdentityDataConsolidator : DataConsolidator {
        private DoubleArray _last;
        private long _last_time;
        private int? _properties;
        public IdentityDataConsolidator() { }

        public IdentityDataConsolidator(int properties) {
            _properties = properties;
        }

        /// <summary>
        ///     The number of properties <see cref="DataConsolidator.Current"/> will have.
        /// </summary>
        public override int Properties => _properties ?? _last?.Properties ?? throw new Exception("Unable to resolve the properties size of this IdentityDataConsolidator.");

        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        public override int InputProperties => Properties;

        /// <summary>
        /// 	 Gets a clone of the data being currently consolidated
        /// </summary>
        public override DoubleArray WorkingData {
            get { return _last == null ? null : _last.Clone(); }
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data">The new data for the consolidator</param>
        public override bool Update(long time, DoubleArray data) {
            if (_last == null || _last_time != time) {
                OnDataConsolidated(time, data);
                _last = data;
                _last_time = time;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 	 Scans this consolidator to see if it should emit a bar due to time passing
        /// </summary>
        /// <param name="currentLocalTime">The current time in the local time zone (same as <see cref="BaseData.Time"/>)</param>
        public override void Scan(long currentLocalTime) { }
    }
}