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

        /// <summary>
        ///     Should the cruncher clone <see cref="Current"/> when <see cref="Updated"/> is fired.
        /// </summary>
        public bool CloneCrunched { get; set; } = false;

        public CrunchingOptions Options { get; protected set; }

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
            var c = new Cruncher() {
                Name = name ?? "Cruncher",
                Options = CrunchingOptions.OnAllUpdatedOnce,
            };
            var obsing = c.observing = updatables.ToArray();
            var crunching = c.crunching = c.observing.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.Properties = properties;
            var cntr = c.signalCounter = new bool[len];
            var workingTarget = new DoubleArrayPinned2DManaged(len, props);
            c.workingTarget = workingTarget;
            c.counter = len;
            for (var i = 0; i < obsing.Length; i++) {
                unsafe {
                    IUpdatable upd = obsing[i];
                    fixed (double* storageAddr = workingTarget.InternalArray) {
                        double* addr = storageAddr;
                        int i_ = i;
                        //case when values collected are indicator output (single value)
                        if (properties == 1) {
                            upd.Updated += (time, updated) => {
                                *addr = updated.Value;
                                if (!cntr[i_]) {
                                    cntr[i_] = true;
                                    if (--c.counter <= 0) {
                                        c.OnUpdated(time);
                                        Array.Clear(cntr, 0, len);
                                        c.counter = len;
                                    }
                                }
                            };
                            upd.Resetted += sender => {
                                *addr = 0d;
                                if (cntr[i_]) {
                                    c.counter++;
                                    cntr[i_] = false;
                                }
                            };
                        } else {
                            //case when values collected are multi-valued output (tradebar value)
                            upd.Updated += (time, updated) => {
                                Debug.Assert(updated.Properties >= props);
                                var propCount = props > updated.Properties ? updated.Properties : props;
                                for (int j = 0; j < propCount; j++)
                                    addr[j] = updated[j];

                                if (!cntr[i_]) {
                                    cntr[i_] = true;
                                    if (--c.counter <= 0) {
                                        c.OnUpdated(time);
                                        Array.Clear(cntr, 0, len);
                                        c.counter = len;
                                    }
                                }
                            };
                            upd.Resetted += sender => {
                                var propCount = props;
                                for (int j = 0; j < propCount; j++)
                                    addr[j] = 0d;
                                if (cntr[i_]) {
                                    c.counter++;
                                    cntr[i_] = false;
                                }
                            };
                        }
                    }
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
            var c = new Cruncher() {Name = name ?? "Cruncher"};
            c.Options = CrunchingOptions.OnEveryUpdate;
            var obsing = c.observing = updatables.ToArray();
            var crunching = c.crunching = c.observing.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.Properties = properties;
            var cntr = c.signalCounter = null;
            var workingTarget = new DoubleArrayPinned2DManaged(len, props);
            c.workingTarget = workingTarget;
            c.counter = interval;
            for (var i = 0; i < obsing.Length; i++) {
                unsafe {
                    IUpdatable upd = obsing[i];
                    double* addr = workingTarget.Address + i * props;
                    //case when values collected are indicator output (single value)
                    if (properties == 1) {
                        if (interval == 1) {
                            upd.Updated += (time, updated) => {
                                *addr = updated.Value;
                                c.OnUpdated(time);
                            };
                            upd.Resetted += sender => { *addr = 0d; };
                        } else {
                            upd.Updated += (time, updated) => {
                                *addr = updated.Value;
                                if (--c.counter <= 0) {
                                    c.OnUpdated(time);
                                    c.counter = interval;
                                }
                            };
                            upd.Resetted += sender => {
                                *addr = 0d;
                                c.counter = interval;
                            };
                        }
                    } else {
                        //case when values collected are multi-valued output (tradebar value)
                        if (interval == 1) {
                            upd.Updated += (time, updated) => {
                                Debug.Assert(updated.Properties >= props);
                                var propCount = props > updated.Properties ? updated.Properties : props;
                                for (int j = 0; j < propCount; j++)
                                    addr[j] = updated[j];
                                if (--c.counter <= 0) {
                                    c.OnUpdated(time);
                                    c.counter = interval;
                                }
                            };
                            upd.Resetted += sender => {
                                var propCount = props;
                                for (int j = 0; j < propCount; j++)
                                    addr[j] = 0d;
                                c.counter = interval;
                            };
                        } else {
                            upd.Updated += (time, updated) => {
                                Debug.Assert(updated.Properties >= props);
                                var propCount = props > updated.Properties ? updated.Properties : props;
                                for (int j = 0; j < propCount; j++)
                                    addr[j] = updated[j];
                                c.OnUpdated(time);
                            };
                            upd.Resetted += sender => {
                                var propCount = props;
                                for (int j = 0; j < propCount; j++)
                                    addr[j] = 0d;
                            };
                        }
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
            if (interval <= 0) throw new ArgumentOutOfRangeException(nameof(interval));
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher() {Name = name ?? "Cruncher"};
            c.Options = CrunchingOptions.OnSpecificUpdated;
            var obsing = c.observing = new IUpdatable[] {crunchTrigger};
            var crunching = c.crunching = updatables.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.Properties = properties;
            var cntr = c.signalCounter = null;
            var workingTarget = new DoubleArrayPinned2DManaged(len, props);
            c.workingTarget = workingTarget;
            c.counter = interval;
            for (var i = 0; i < crunching.Length; i++) {
                unsafe {
                    IUpdatable upd = crunching[i];
                    double* addr = workingTarget.Address + i * props;
                    //case when values collected are indicator output (single value)
                    if (properties == 1) {
                        upd.Updated += (time, updated) => { *addr = updated.Value; };
                        upd.Resetted += _ => { *addr = 0d; };
                    } else {
                        //case when values collected are multi-valued output (tradebar value)
                        upd.Updated += (time, updated) => {
                            Debug.Assert(updated.Properties >= props);
                            var propCount = props > updated.Properties ? updated.Properties : props;
                            for (int j = 0; j < propCount; j++)
                                addr[j] = updated[j];
                        };

                        upd.Resetted += _ => {
                            for (int j = 0; j < props; j++)
                                addr[j] = 0d;
                        };
                    }
                }
            }

            if (interval == 1) {
                if (triggerMustBeReady) {
                    crunchTrigger.Updated += (time, updated) => {
                        if (crunchTrigger.IsReady)
                            c.OnUpdated(time);
                    };
                } else {
                    crunchTrigger.Updated += (time, updated) => c.OnUpdated(time);
                }
            } else {
                if (triggerMustBeReady) {
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

            return c;
        }
    }
}