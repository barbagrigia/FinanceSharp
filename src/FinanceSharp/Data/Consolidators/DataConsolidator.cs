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
 *
*/

using System;

namespace FinanceSharp.Data.Consolidators {
    /// <summary>
    /// 	 Represents a type that consumes BaseData instances and fires an event with consolidated
    /// 	 and/or aggregated data.
    /// </summary>
    /// <typeparam name="TInput">The type consumed by the consolidator</typeparam>
    public abstract class DataConsolidator : IDataConsolidator {
        /// <summary>
        /// 	 Scans this consolidator to see if it should emit a bar due to time passing
        /// </summary>
        /// <param name="currentLocalTime">The current time in the local time zone (same as <see cref="BaseData.Time"/>)</param>
        public abstract void Scan(long currentLocalTime);

        /// <summary>
        /// 	 Event handler that fires when a new piece of data is produced
        /// </summary>
        public event UpdatedHandler Updated;

        /// <summary>
        ///     Event handler that fires after this updatable is reset.
        /// </summary>
        public event ResettedHandler Resetted;

        /// <summary>
        /// 	 Gets the most recently consolidated piece of data. This will be null if this consolidator
        /// 	 has not produced any data yet.
        /// </summary>
        public DoubleArray Current { get; protected set; }

        /// <summary>
        /// 	 Gets the current time of <see cref="IUpdatable.Current"/>.
        /// </summary>
        public long CurrentTime { get; protected set; }

        /// <summary>
        /// 	 Gets a clone of the data being currently consolidated
        /// </summary>
        public abstract DoubleArray WorkingData { get; }

        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public virtual bool IsReady => Samples > 0;


        /// <summary>
        /// 	 Gets the number of samples processed by this indicator
        /// </summary>
        public long Samples { get; protected set; }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public void Reset() {
            Samples = 0;
            Current = null;
            CurrentTime = 0;
            Resetted?.Invoke(this);
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        public bool Update<TStruct>(long time, TStruct data) where TStruct : unmanaged, DataStruct {
            return Update(time, DoubleArray.FromStruct(data));
        }

        /// <summary>
        /// 	 Updates this consolidator with the specified data. This method is
        /// 	 responsible for raising the DataConsolidated event
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data">The new data for the consolidator</param>
        public abstract bool Update(long time, DoubleArray data);

        /// <summary>
        /// 	 Event invocator for the DataConsolidated event. This should be invoked
        /// 	 by derived classes when they have consolidated a new piece of data.
        /// </summary>
        /// <param name="consolidated">The newly consolidated data</param>
        protected virtual void OnDataConsolidated(long time, DoubleArray consolidated) {
            Updated?.Invoke(time, consolidated);

            // assign the Consolidated property after the event handlers are fired,
            // this allows the event handlers to look at the new consolidated data
            // and the previous consolidated data at the same time without extra bookkeeping
            Current = consolidated;
            CurrentTime = time;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            Updated = null;
        }
    }
}