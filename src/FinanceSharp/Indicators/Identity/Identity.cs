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

namespace FinanceSharp.Indicators {
    /// <summary>
    ///     Represents an indicator that is a ready after ingesting a single sample and
    ///     always returns the same value as it is given.
    /// </summary>
    public class Identity : Indicator {
        /// <summary>
        ///     The number of properties <see cref="IUpdatable.Current"/> will have.
        /// </summary>
        public override int Properties { get; }

        /// <summary>
        ///     Initializes a new instance of the Identity indicator with the specified name
        /// </summary>
        /// <param name="name">The name of the indicator</param>
        /// <param name="outputCount">Sets the <see cref="IUpdatable.OutputCount"/> of this <see cref="Identity"/></param>
        /// <param name="outputProperties">Sets the <see cref="IUpdatable.Properties"/> of this <see cref="Identity"/></param>
        public Identity(string name, int outputCount = 1, int outputProperties = 1)
            : base(name) {
            OutputCount = outputCount;
            Properties = outputProperties;
        }

        /// <summary>
        ///     Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady {
            get { return Samples > 0; }
        }

        /// <summary>
        /// 	 Updates the state of this indicator with the given value and returns true
        /// 	 if this indicator is ready, false otherwise
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The value to use to update this indicator</param>
        /// <returns>True if this indicator is ready, false otherwise</returns>
        public override void Update(long time, DoubleArray input) {
            // compute a new value and update our previous time
            Samples++;

            Current = input;
            CurrentTime = time;

            // let others know we've produced a new data point
            OnUpdated(time, Current);
        }

        /// <summary>
        ///     Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            return input;
        }
    }
}