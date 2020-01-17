///*
// * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
// * Original code by: 
// * 
// * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
// * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
//*/

//using System;
//

//namespace FinanceSharp.Data.Consolidators {
//    /// <summary>
//    /// 	 This consolidator can transform a stream of <see cref="BaseData"/> instances into a stream of <see cref="DoubleArray"/>
//    /// </summary>
//    public class RenkoConsolidator : IDataConsolidator {
//        /// <summary>
//        /// 	 Event handler that fires when a new piece of data is produced
//        /// </summary>
//        public event EventHandler<DoubleArray> DataConsolidated;

//        private RenkoBarValue _currentBar;

//        private readonly double _barSize;
//        private readonly bool _evenBars;
//        private readonly Func<DoubleArray, double> _selector;
//        private readonly Func<DoubleArray, double> _volumeSelector;

//        private bool _firstTick = true;
//        private RenkoBarValue _lastWicko;

//        private DateTime _openOn;
//        private DateTime _closeOn;
//        private double _openRate;
//        private double _highRate;
//        private double _lowRate;
//        private double _closeRate;

//        /// <summary>
//        /// 	 Initializes a new instance of the <see cref="RenkoConsolidator"/> class using the specified <paramref name="barSize"/>.
//        /// </summary>
//        /// <param name="barSize">The constant value size of each bar</param>
//        /// <param name="type">The RenkoType of the bar</param>
//        public RenkoConsolidator(double barSize, RenkoType type) {
//            if (type != RenkoType.Wicked) {
//                throw new ArgumentException("RenkoConsolidator can only be initialized with RenkoType.Wicked. For RenkoType.Classic, please use the other constructor overloads.");
//            }

//            _barSize = barSize;
//            Type = type;
//        }

//        /// <summary>
//        /// 	 Initializes a new instance of the <see cref="RenkoConsolidator"/> class using the specified <paramref name="barSize"/>.
//        /// 	 The value selector will by default select <see cref="DoubleArray.Value"/>
//        /// 	 The volume selector will by default select zero.
//        /// </summary>
//        /// <param name="barSize">The constant value size of each bar</param>
//        /// <param name="evenBars">When true bar open/close will be a multiple of the barSize</param>
//        public RenkoConsolidator(double barSize, bool evenBars = true) {
//            _barSize = barSize;
//            _selector = x => x.Value;
//            _volumeSelector = x => 0;
//            _evenBars = evenBars;

//            Type = RenkoType.Classic;
//        }

//        /// <summary>
//        /// 	 Initializes a new instance of the <see cref="RenkoConsolidator" /> class.
//        /// </summary>
//        /// <param name="barSize">The size of each bar in units of the value produced by <paramref name="selector"/></param>
//        /// <param name="selector">Extracts the value from a data instance to be formed into a <see cref="DoubleArray"/>. The default
//        /// 	 value is (x => x.Value) the <see cref="DoubleArray.Value"/> property on <see cref="DoubleArray"/></param>
//        /// <param name="volumeSelector">Extracts the volume from a data instance. The default value is null which does
//        /// 	 not aggregate volume per bar.</param>
//        /// <param name="evenBars">When true bar open/close will be a multiple of the barSize</param>
//        public RenkoConsolidator(double barSize, Func<DoubleArray, double> selector, Func<DoubleArray, double> volumeSelector = null, bool evenBars = true) {
//            if (barSize < double.Epsilon) {
//                throw new ArgumentOutOfRangeException(nameof(barSize), "RenkoConsolidator bar size must be positve and greater than 1e-28");
//            }

//            _barSize = barSize;
//            _evenBars = evenBars;
//            _selector = selector ?? (x => x.Value);
//            _volumeSelector = volumeSelector ?? (x => 0);

//            Type = RenkoType.Classic;
//        }

//        /// <summary>
//        /// 	 Gets the kind of the bar
//        /// </summary>
//        public RenkoType Type { get; private set; }

//        /// <summary>
//        /// 	 Gets the bar size used by this consolidator
//        /// </summary>
//        public double BarSize => _barSize;

//        /// <summary>
//        /// 	 Gets the most recently consolidated piece of data. This will be null if this consolidator
//        /// 	 has not produced any data yet.
//        /// </summary>
//        public DoubleArray Current { get; private set; }

//        /// <summary>
//        /// 	 Gets a clone of the data being currently consolidated
//        /// </summary>
//        public DoubleArray Current => _currentBar.Clone();

//        /// <summary>
//        /// 	 Gets the type consumed by this consolidator
//        /// </summary>
//        public Type InputType => typeof(DoubleArray);

//        /// <summary>
//        /// 	 Gets <see cref="DoubleArray"/> which is the type emitted in the <see cref="IDataConsolidator.DataConsolidated"/> event.
//        /// </summary>
//        public Type OutputType => typeof(DoubleArray);

//        // Used for unit tests
//        internal DoubleArray OpenRenkoBar => new DoubleArray(_openOn, _closeOn, _barSize, _openRate, _highRate, _lowRate, _closeRate);

//        private void Rising(DoubleArray data) {
//            double limit;

//            while (_closeRate > (limit = _openRate + _barSize)) {
//                var wicko = new DoubleArray(_openOn, _closeOn, _barSize, _openRate, limit, _lowRate, limit);

//                _lastWicko = wicko;

//                OnDataConsolidated(wicko);

//                _openOn = _closeOn;
//                _openRate = limit;
//                _lowRate = limit;
//            }
//        }

//        private void Falling(DoubleArray data) {
//            double limit;

//            while (_closeRate < (limit = _openRate - _barSize)) {
//                var wicko = new DoubleArray(_openOn, _closeOn, _barSize, _openRate, _highRate, limit, limit);

//                _lastWicko = wicko;

//                OnDataConsolidated(wicko);

//                _openOn = _closeOn;
//                _openRate = limit;
//                _highRate = limit;
//            }
//        }

//        /// <summary>
//        /// 	 Updates this consolidator with the specified data. This method is
//        /// 	 responsible for raising the DataConsolidated event
//        /// </summary>
//        /// <param name="time"></param>
//        /// <param name="data">The new data for the consolidator</param>
//        public void Update(long time, DoubleArray data) {
//            if (Type == RenkoType.Classic) {
//                UpdateClassic(time, data);
//            } else {
//                UpdateWicked(time, data);
//            }
//        }

//        private void UpdateWicked(long time, DoubleArray data) {
//            var rate = data.Price;

//            if (_firstTick) {
//                _firstTick = false;

//                _openOn = time;
//                _closeOn = time;
//                _openRate = rate;
//                _highRate = rate;
//                _lowRate = rate;
//                _closeRate = rate;
//            } else {
//                _closeOn = time;

//                if (rate > _highRate) _highRate = rate;

//                if (rate < _lowRate) _lowRate = rate;

//                _closeRate = rate;

//                if (_closeRate > _openRate) {
//                    if (_lastWicko == null || _lastWicko.Direction == BarDirection.Rising) {
//                        Rising(data);
//                        return;
//                    }

//                    var limit = _lastWicko.Open + _barSize;

//                    if (_closeRate > limit) {
//                        var wicko = new DoubleArray(_openOn, _closeOn, _barSize, _lastWicko.Open, limit, _lowRate, limit);

//                        _lastWicko = wicko;

//                        OnDataConsolidated(wicko);

//                        _openOn = _closeOn;
//                        _openRate = limit;
//                        _lowRate = limit;

//                        Rising(data);
//                    }
//                } else if (_closeRate < _openRate) {
//                    if (_lastWicko == null || _lastWicko.Direction == BarDirection.Falling) {
//                        Falling(data);
//                        return;
//                    }

//                    var limit = _lastWicko.Open - _barSize;

//                    if (_closeRate < limit) {
//                        var wicko = new DoubleArray(_openOn, _closeOn, _barSize, _lastWicko.Open, _highRate, limit, limit);

//                        _lastWicko = wicko;

//                        OnDataConsolidated(wicko);

//                        _openOn = _closeOn;
//                        _openRate = limit;
//                        _highRate = limit;

//                        Falling(data);
//                    }
//                }
//            }
//        }

//        private void UpdateClassic(long time, DoubleArray data) {
//            var currentValue = _selector(data);
//            var volume = _volumeSelector(data);

//            double? close = null;

//            // if we're already in a bar then update it
//            if (_currentBar != null) {
//                _currentBar.Update(data.Time, currentValue, volume);

//                // if the update caused this bar to close, fire the event and reset the bar
//                if (_currentBar.IsClosed) {
//                    close = _currentBar.Close;
//                    OnDataConsolidated(_currentBar);
//                    _currentBar = null;
//                }
//            }

//            if (_currentBar == null) {
//                var open = close ?? currentValue;
//                if (_evenBars && !close.HasValue) {
//                    open = Math.Ceiling(open / _barSize) * _barSize;
//                }

//                _currentBar = new DoubleArray(data.Time, _barSize, open, volume);
//            }
//        }

//        /// <summary>
//        /// 	 Scans this consolidator to see if it should emit a bar due to time passing
//        /// </summary>
//        /// <param name="currentLocalTime">The current time in the local time zone (same as <see cref="BaseData.Time"/>)</param>
//        public void Scan(DateTime currentLocalTime) { }

//        /// <summary>
//        /// 	 Event invocator for the DataConsolidated event. This should be invoked
//        /// 	 by derived classes when they have consolidated a new piece of data.
//        /// </summary>
//        /// <param name="consolidated">The newly consolidated data</param>
//        protected virtual void OnDataConsolidated(DoubleArray consolidated) {
//            DataConsolidated?.Invoke(this, consolidated);

//            _dataConsolidatedHandler?.Invoke(this, consolidated);

//            Consolidated = consolidated;
//        }

//        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//        /// <filterpriority>2</filterpriority>
//        public void Dispose() {
//            DataConsolidated = null;
//            _dataConsolidatedHandler = null;
//        }
//    }
//}

