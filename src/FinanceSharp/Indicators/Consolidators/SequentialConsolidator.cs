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

namespace FinanceSharp.Consolidators {
    /// <summary>
    /// 	 This consolidator wires up the events on its First and Second consolidators
    /// 	 such that data flows from the First to Second consolidator. It's output comes
    /// 	 from the Second.
    /// </summary>
    public class SequentialConsolidator : IDataConsolidator {
        /// <summary>
        /// 	 Gets the first consolidator to receive data
        /// </summary>
        public IDataConsolidator First { get; private set; }

        /// <summary>
        /// 	 Gets the second consolidator that ends up receiving data produced
        /// 	 by the first
        /// </summary>
        public IDataConsolidator Second { get; private set; }

        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 	 Gets the number of samples processed by this indicator
        /// </summary>
        public long Samples { get; protected set; }

        /// <summary>
        /// 	 Required period, in data points (number of updates), for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod => First.WarmUpPeriod + Second.WarmUpPeriod;

        /// <summary>
        /// 	 Gets the most recently consolidated piece of data. This will be null if this consolidator
        /// 	 has not produced any data yet.
        ///
        /// 	 For a SequentialConsolidator, this is the output from the 'Second' consolidator.
        /// </summary>
        public DoubleArray Current => Second.Current;

        /// <summary>
        /// 	 Gets the current time of <see cref="IUpdatable.Current"/>.
        /// </summary>
        public long CurrentTime => Second.CurrentTime;

        /// <summary>
        ///     The number of items <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int OutputCount => 1;

        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public int Properties => Second.Properties;

        /// <summary>
        ///     The number of properties of input argument of <see cref="IUpdatable.Update"/> must have.
        /// </summary>
        public int InputProperties => First.InputProperties;

        /// <summary>
        /// 	 Gets a clone of the data being currently consolidated
        /// </summary>
        public DoubleArray WorkingData => Second.WorkingData;

        /// <summary>
        /// 	 Event handler that fires after this updatable is updated.
        /// </summary>
        public event UpdatedHandler Updated;

        /// <summary>
        ///     Event handler that fires after this updatable is reset.
        /// </summary>
        public event ResettedHandler Resetted;

        /// <summary>
        /// 	 Updates this consolidator with the specified data
        /// </summary>
        /// <param name="time"></param>
        /// <param name="data">The new data for the consolidator</param>
        public bool Update(long time, DoubleArray data) {
            return First.Update(time, data);
        }

        /// <summary>
        /// 	 Resets this updatable to its initial state
        /// </summary>
        public void Reset() {
            First.Reset();
            Second.Reset();
            Samples = 0;
            Resetted?.Invoke(this);
        }

        /// <summary>
        /// 	 Gets a flag indicating when this updatable is ready and fully initialized
        /// </summary>
        public bool IsReady => Second.IsReady;

        /// <summary>
        /// 	 Scans this consolidator to see if it should emit a bar due to time passing
        /// </summary>
        /// <param name="currentLocalTime">The current time in the local time zone (same as <see cref="BaseData.Time"/>)</param>
        public void Scan(long currentLocalTime) {
            First.Scan(currentLocalTime);
        }

        /// <summary>
        /// 	 Creates a new consolidator that will pump date through the first, and then the output
        /// 	 of the first into the second. This enables 'wrapping' or 'composing' of consolidators
        /// </summary>
        /// <param name="first">The first consolidator to receive data</param>
        /// <param name="second">The consolidator to receive first's output</param>
        public SequentialConsolidator(IDataConsolidator first, IDataConsolidator second, string name = "") {
            First = first;
            Second = second;
            Name = name;
            // wire up the second one to get data from the first
            first.Updated += (time, updated) => second.Update(time, updated);

            // wire up the second one's events to also fire this consolidator's event so consumers
            // can attach
            second.Updated += (time, updated) => OnDataConsolidated(time, updated);
        }

        /// <summary>
        /// 	 Event invocator for the DataConsolidated event. This should be invoked
        /// 	 by derived classes when they have consolidated a new piece of data.
        /// </summary>
        /// <param name="consolidated">The newly consolidated data</param>
        protected virtual bool OnDataConsolidated(long time, DoubleArray consolidated) {
            Samples++;
            Updated?.Invoke(time, consolidated);
            return true;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            First.Dispose();
            Second.Dispose();
            Updated = null;
        }
    }
}