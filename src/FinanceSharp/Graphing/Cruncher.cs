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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FinanceSharp.Graphing {
    /// <summary>
    ///     Provides various methods to join <see cref="IUpdatable"/>'s output into a single <see cref="DoubleArray"/>.
    /// </summary>
    public partial class Cruncher {
        protected int counter;
        protected bool[] signalCounter;
        protected int length;
        protected IUpdatable[] observing;
        protected IUpdatable[] crunching;
        protected DoubleArrayPinned2DManaged workingTarget;
        protected int outputCount;

        /// <summary>
        ///     Should the cruncher clone <see cref="Current"/> when <see cref="Updated"/> is fired.
        /// </summary>
        public bool CloneCrunched { get; set; } = false;

        /// <summary>
        ///     The configuration type for this cruncher.
        /// </summary>
        public CrunchingOptions Options { get; protected set; }

        /// <summary>
        ///     The working target that gets updated and pushed/cloned on <see cref="Updated"/> event.
        /// </summary>
        public DoubleArray WorkingTarget => workingTarget;

        protected Cruncher() { }

        /// <summary>
        ///     Triggers <see cref="Updated"/>.
        /// </summary>
        /// <param name="time">The time </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void OnUpdated(long time) {
            Samples++;
            CurrentTime = time;
            Updated?.Invoke(time, CloneCrunched ? (DoubleArray) new DoubleArray2DManaged((double[,]) workingTarget.InternalArray.Clone()) : workingTarget);
        }

        protected static Cruncher BindValues(Cruncher c) {
            var crunching = c.crunching;
            var props = c.Properties;
            var workingTarget = c.workingTarget;
            var properties = c.Properties;

            unsafe {
                fixed (double* storageAddr = workingTarget.InternalArray) {
                    //the array is pinned so we can fix it and still use the address after we exit fixing.
                    for (var i = 0; i < crunching.Length; i++) {
                        IUpdatable srcUpdatable = crunching[i];
                        double* targetAddr = storageAddr
                                             + crunching.Take(i)
                                                 .Select(upd => upd.OutputCount)
                                                 .Append(0)
                                                 .Sum(outputCnt => outputCnt * properties);
                        var outputCount = srcUpdatable.OutputCount;
                        //case when values collected are indicator output (single value)
                        if (srcUpdatable.OutputCount == 1 && properties == 1) {
                            srcUpdatable.Updated += (time, updated) => { *targetAddr = updated.Value; };
                            srcUpdatable.Resetted += sender => { *targetAddr = 0d; };
                        } else {
                            //case when values collected are multi-valued output (tradebar value)
                            srcUpdatable.Updated += (time, updated) => {
                                Debug.Assert(updated.Properties >= props);
                                fixed (double* src = updated)
                                    Unsafe.CopyBlock(destination: targetAddr, source: src
                                        , (uint) (outputCount * (props > updated.Properties ? updated.Properties : props) * sizeof(double)));
                            };
                            srcUpdatable.Resetted += sender => { new Span<double>(targetAddr, props).Fill(0d); };
                        }
                    }
                }
            }

            return c;
        }

        /// <summary>
        ///     Crunches when all <see cref="IUpdatable"/> were updated atleast once.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnAllUpdatedOnce(IEnumerable<IUpdatable> updatables, int properties = 1, string name = null) {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher();
            c.Name = name ?? "Cruncher";
            c.Options = CrunchingOptions.OnAllUpdatedOnce;
            var obsing = c.observing = updatables.ToArray();
            c.crunching = c.observing.ToArray();
            var len = c.length = c.crunching.Length;
            var outputCount = c.outputCount = c.crunching.Sum(upd => upd.OutputCount);
            var props = c.Properties = properties;
            var cntr = c.signalCounter = new bool[len];
            var workingTarget = new DoubleArrayPinned2DManaged(outputCount, props);
            c.workingTarget = workingTarget;
            c.counter = len;

            //bind the values to their repsective memory address.
            BindValues(c);

            for (var i = 0; i < obsing.Length; i++) {
                IUpdatable srcUpdatable = obsing[i];
                int updId = i;
                //case when values collected are indicator output (single value)
                if (properties == 1) {
                    srcUpdatable.Updated += (time, updated) => {
                        if (!cntr[updId]) {
                            cntr[updId] = true;
                            if (--c.counter <= 0) {
                                c.OnUpdated(time);
                                Array.Clear(cntr, 0, len);
                                c.counter = len;
                            }
                        }
                    };
                    srcUpdatable.Resetted += sender => {
                        if (cntr[updId]) {
                            c.counter++;
                            cntr[updId] = false;
                        }
                    };
                } else {
                    //case when values collected are multi-valued output (tradebar value)
                    srcUpdatable.Updated += (time, updated) => {
                        if (!cntr[updId]) {
                            cntr[updId] = true;
                            if (--c.counter <= 0) {
                                c.OnUpdated(time);
                                Array.Clear(cntr, 0, len);
                                c.counter = len;
                            }
                        }
                    };
                    srcUpdatable.Resetted += sender => {
                        if (cntr[updId]) {
                            c.counter++;
                            cntr[updId] = false;
                        }
                    };
                }
            }

            return c;
        }

        /// <summary>
        ///     Crunches when any of <see cref="IUpdatable"/> are updated for every n <paramref name="interval"/>.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="interval">The interval for how many fires must any of <paramref name="updatables"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger Cruncher's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnEveryUpdate(IEnumerable<IUpdatable> updatables, int interval = 1, int properties = 1, string name = null) {
            if (interval <= 0) throw new ArgumentOutOfRangeException(nameof(interval));
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher();
            c.Name = name ?? "Cruncher";
            c.Options = CrunchingOptions.OnEveryUpdate;
            var obsing = c.observing = updatables.ToArray();
            c.crunching = c.observing.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.Properties = properties;
            c.signalCounter = null;
            var workingTarget = new DoubleArrayPinned2DManaged(len, props);
            c.workingTarget = workingTarget;
            c.counter = interval;

            BindValues(c);

            for (var i = 0; i < obsing.Length; i++) {
                IUpdatable srcUpdatable = obsing[i];
                if (properties == 1) {
                    if (interval == 1) {
                        srcUpdatable.Updated += (time, updated) => { c.OnUpdated(time); };
                    } else {
                        srcUpdatable.Updated += (time, updated) => {
                            if (--c.counter <= 0) {
                                c.OnUpdated(time);
                                c.counter = interval;
                            }
                        };
                        srcUpdatable.Resetted += sender => { c.counter = interval; };
                    }
                } else {
                    //case when values collected are multi-valued output (tradebar value)
                    if (interval == 1) {
                        srcUpdatable.Updated += (time, updated) => { c.OnUpdated(time); };
                    } else {
                        srcUpdatable.Updated += (time, updated) => {
                            if (--c.counter <= 0) {
                                c.OnUpdated(time);
                                c.counter = interval;
                            }
                        };
                        srcUpdatable.Resetted += sender => { c.counter = interval; };
                    }
                }
            }

            return c;
        }

        /// <summary>
        ///     Crunches <paramref name="updatables"/> whenever <paramref name="crunchTrigger"/> is updated for n <paramref name="interval"/> times.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="crunchTrigger">The <see cref="IUpdatable"/> to observe for fires of <see cref="IUpdatable.Updated"/>.</param>
        /// <param name="interval">The interval for how many fires must <paramref name="crunchTrigger"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger Cruncher's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="triggerMustBeReady">Does <paramref name="crunchTrigger"/> must be ready to trigger Cruncher's update event?</param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnSpecificUpdate(IEnumerable<IUpdatable> updatables, IUpdatable crunchTrigger, int interval = 1, int properties = 1, string name = null, bool triggerMustBeReady = true) {
            return OnSpecificUpdate(updatables, new IUpdatable[] {crunchTrigger}, interval, properties, name, new bool[] {triggerMustBeReady});
        }

        /// <summary>
        ///     Crunches <paramref name="updatables"/> whenever <paramref name="crunchTriggers"/> is updated for n <paramref name="interval"/> times.
        /// </summary>
        /// <param name="name">Name of the cruncher for debugging purposes.</param>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="crunchTriggers">The <see cref="IUpdatable"/>s to observe for fires of <see cref="IUpdatable.Updated"/>.</param>
        /// <param name="interval">The interval for how many fires must <paramref name="crunchTriggers"/> trigger <see cref="IUpdatable.Updated"/> in order to trigger Cruncher's update event.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <param name="triggerMustBeReady">Does <paramref name="crunchTriggers"/> must be ready to trigger Cruncher's update event? By default </param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnSpecificUpdate(IEnumerable<IUpdatable> updatables, IUpdatable[] crunchTriggers, int interval = 1, int properties = 1, string name = null, bool[] triggerMustBeReady = null) {
            if (interval <= 0) throw new ArgumentOutOfRangeException(nameof(interval));
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher {Name = name ?? "Cruncher"};
            c.Options = CrunchingOptions.OnSpecificUpdated;
            c.observing = crunchTriggers;
            c.crunching = updatables.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.Properties = properties;
            c.signalCounter = null;
            var workingTarget = new DoubleArrayPinned2DManaged(len, props);
            c.workingTarget = workingTarget;
            c.counter = interval;

            BindValues(c);
            for (var i = 0; i < crunchTriggers.Length; i++) {
                var onlyWhenReady = triggerMustBeReady[i];
                var crunchTrigger = crunchTriggers[i];
                if (interval == 1) {
                    if (onlyWhenReady) {
                        crunchTrigger.Updated += (time, updated) => {
                            if (crunchTrigger.IsReady)
                                c.OnUpdated(time);
                        };
                    } else {
                        crunchTrigger.Updated += (time, updated) => c.OnUpdated(time);
                    }
                } else {
                    if (onlyWhenReady) {
                        crunchTrigger.Updated += (time, updated) => {
                            if (!crunchTrigger.IsReady)
                                return;
                            if (--c.counter <= 0) {
                                c.OnUpdated(time);
                                c.counter = interval;
                            }
                        };
                    } else {
                        crunchTrigger.Updated += (time, updated) => {
                            if (--c.counter <= 0) {
                                c.OnUpdated(time);
                                c.counter = interval;
                            }
                        };
                    }
                }

                crunchTrigger.Resetted += _ => c.counter = interval;
            }

            return c;
        }
    }
}