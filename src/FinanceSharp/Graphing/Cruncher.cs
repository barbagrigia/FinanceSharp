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
using FinanceSharp.Data;
using FinanceSharp.Indicators.CandlestickPatterns;

namespace FinanceSharp.Graphing {
    public class Cruncher {
        public CrunchingOptions Options { get; set; }
        public event UpdatedHandler Crunched;

        protected int counter;
        protected bool[] singelCounter;
        protected int length;
        protected int properties;
        protected IUpdatable[] observing;
        protected IUpdatable[] crunching;
        protected DoubleArray workingTarget;

        /// <summary>
        ///     Should the cruncher clone when passing to an event.
        /// </summary>
        public bool CloneCrunched { get; set; } = false;

        protected Cruncher() { }

        /// <summary>
        ///     Triggers <see cref="Crunched"/>.
        /// </summary>
        /// <param name="time">The time </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void OnCrunched(long time) {
            Crunched?.Invoke(time, CloneCrunched ? workingTarget.Clone() : workingTarget);
        }

        /// <summary>
        ///     Crunches when all <see cref="IUpdatable"/> were updated atleast once.
        /// </summary>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnAllUpdatedOnce(IEnumerable<IUpdatable> updatables, int properties = 1) {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher();
            c.Options = CrunchingOptions.OnAllUpdatedOnce;
            var obsing = c.observing = updatables.ToArray();
            var crunching = c.crunching = c.observing.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.properties = properties;
            var cntr = c.singelCounter = new bool[len];
            var workingTarget = c.workingTarget = new DoubleArray(len, props);
            c.counter = len;
            for (var i = 0; i < obsing.Length; i++) {
                unsafe {
                    IUpdatable upd = obsing[i];
                    double* addr = workingTarget.Address + i * props;
                    int i_ = i;
                    //case when values collected are indicator output (single value)
                    if (properties == 1) {
                        upd.Updated += (time, updated) => {
                            *addr = updated.Value;
                            if (!cntr[i_]) {
                                cntr[i_] = true;
                                if (--c.counter <= 0) {
                                    c.OnCrunched(time);
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
                                    c.OnCrunched(time);
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

            return c;
        }

        /// <summary>
        ///     Crunches when any of <see cref="IUpdatable"/> are updated for every n <paramref name="interval"/>.
        /// </summary>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnEveryUpdate(IEnumerable<IUpdatable> updatables, int interval = 1, int properties = 1) {
            if (interval <= 0) throw new ArgumentOutOfRangeException(nameof(interval));
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher();
            c.Options = CrunchingOptions.OnEveryUpdate;
            var obsing = c.observing = updatables.ToArray();
            var crunching = c.crunching = c.observing.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.properties = properties;
            var cntr = c.singelCounter = null;
            var workingTarget = c.workingTarget = new DoubleArray(len, props);
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
                                c.OnCrunched(time);
                            };
                            upd.Resetted += sender => { *addr = 0d; };
                        } else {
                            upd.Updated += (time, updated) => {
                                *addr = updated.Value;
                                if (--c.counter <= 0) {
                                    c.OnCrunched(time);
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
                                    c.OnCrunched(time);
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
                                c.OnCrunched(time);
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
        ///     Crunches <paramref name="updatables"/>when any of <see cref="IUpdatable"/> are updated for every n <paramref name="interval"/>.
        /// </summary>
        /// <param name="updatables">The updatables to observe and crunch.</param>
        /// <param name="properties">
        ///     How many properties all of the <paramref name="updatables"/> emit.
        ///     this can be less than their minimal properties.
        ///     e.g. if <paramref name="updatables"/> emit <see cref="BarValue"/> (4 properties), selecting 1 will take only <see cref="BarValue.Close"/>.
        /// </param>
        /// <returns>A new cruncher configured.</returns>
        public static Cruncher OnSpecificUpdate(IEnumerable<IUpdatable> updatables, IUpdatable crunchTrigger, int interval = 1, int properties = 1) {
            if (interval <= 0) throw new ArgumentOutOfRangeException(nameof(interval));
            // ReSharper disable once UseObjectOrCollectionInitializer
            var c = new Cruncher();
            c.Options = CrunchingOptions.OnSpecificUpdated;
            var obsing = c.observing = new IUpdatable[] {crunchTrigger};
            var crunching = c.crunching = updatables.ToArray();
            var len = c.length = c.crunching.Length;
            var props = c.properties = properties;
            var cntr = c.singelCounter = null;
            var workingTarget = c.workingTarget = new DoubleArray(len, props);
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
                crunchTrigger.Updated += (time, updated) => { c.OnCrunched(time); };

                crunchTrigger.Resetted += _ => { c.counter = interval; };
            } else {
                crunchTrigger.Updated += (time, updated) => {
                    if (--c.counter <= 0) {
                        c.OnCrunched(time);
                        c.counter = interval;
                    }
                };

                crunchTrigger.Resetted += _ => { c.counter = interval; };
            }

            return c;
        }
    }

    public enum CrunchingOptions {
        /// <summary>
        ///     All inputs must be updated atleast once.
        /// </summary>
        OnAllUpdatedOnce,

        /// <summary>
        ///     Every update a crunch is sent.
        /// </summary>
        OnEveryUpdate,

        /// <summary>
        ///     After a specific <see cref="IUpdatable"/> update a crunch will be called.
        /// </summary>
        OnSpecificUpdated
    }
}