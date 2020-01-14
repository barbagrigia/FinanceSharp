﻿/*
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
using System.Threading;

namespace QuantConnect.Data.Market {
    /// <summary>
    /// 	 TradeBar class for second and minute resolution data:
    /// 	 An OHLC implementation of the QuantConnect BaseData class with parameters for candles.
    /// </summary>
    public class TradeBar : BaseData, IBaseDataBar {
        // scale factor used in QC equity/forex data files
        private const double _scaleFactor = 1 / 10000d;

        private int _initialized;
        private double _open;
        private double _high;
        private double _low;

        /// <summary>
        /// 	 Volume:
        /// </summary>
        public virtual double Volume { get; set; }

        /// <summary>
        /// 	 Opening price of the bar: Defined as the price at the start of the time period.
        /// </summary>
        public virtual double Open {
            get { return _open; }
            set {
                Initialize(value);
                _open = value;
            }
        }

        /// <summary>
        /// 	 High price of the TradeBar during the time period.
        /// </summary>
        public virtual double High {
            get { return _high; }
            set {
                Initialize(value);
                _high = value;
            }
        }

        /// <summary>
        /// 	 Low price of the TradeBar during the time period.
        /// </summary>
        public virtual double Low {
            get { return _low; }
            set {
                Initialize(value);
                _low = value;
            }
        }

        /// <summary>
        /// 	 Closing price of the TradeBar. Defined as the price at Start Time + TimeSpan.
        /// </summary>
        public virtual double Close {
            get { return Value; }
            set {
                Initialize(value);
                Value = value;
            }
        }

        /// <summary>
        /// 	 The closing time of this bar, computed via the Time and Period
        /// </summary>
        public override DateTime EndTime {
            get { return Time + Period; }
            set { Period = value - Time; }
        }

        /// <summary>
        /// 	 The period of this trade bar, (second, minute, daily, ect...)
        /// </summary>
        public virtual TimeSpan Period { get; set; }

        //In Base Class: Alias of Closing:
        //public double Price;

        // Asset.
        //In Base Class: public ;

        //In Base Class: DateTime Of this TradeBar
        //public DateTime Time;

        /// <summary>
        /// 	 Default initializer to setup an empty tradebar.
        /// </summary>
        public TradeBar() {
            Period = TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// 	 Cloner constructor for implementing fill forward.
        /// 	 Return a new instance with the same values as this original.
        /// </summary>
        /// <param name="original">Original tradebar object we seek to clone</param>
        public TradeBar(TradeBar original) {
            Time = new DateTime(original.Time.Ticks);
            Value = original.Close;
            Open = original.Open;
            High = original.High;
            Low = original.Low;
            Close = original.Close;
            Volume = original.Volume;
            Period = original.Period;
            _initialized = 1;
        }

        /// <summary>
        /// 	 Initialize Trade Bar with OHLC Values:
        /// </summary>
        /// <param name="time">DateTime Timestamp of the bar</param>
        /// <param name="symbol">Market MarketType Symbol</param>
        /// <param name="open">Decimal Opening Price</param>
        /// <param name="high">Decimal High Price of this bar</param>
        /// <param name="low">Decimal Low Price of this bar</param>
        /// <param name="close">Decimal Close price of this bar</param>
        /// <param name="volume">Volume sum over day</param>
        /// <param name="period">The period of this bar, specify null for default of 1 minute</param>
        public TradeBar(DateTime time, double open, double high, double low, double close, double volume, TimeSpan? period = null) {
            Time = time;

            Value = close;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Period = period ?? TimeSpan.FromMinutes(1);
            _initialized = 1;
        }

        /// <summary>
        /// 	 Update the tradebar - build the bar from this pricing information:
        /// </summary>
        /// <param name="lastTrade">This trade price</param>
        /// <param name="bidPrice">Current bid price (not used) </param>
        /// <param name="askPrice">Current asking price (not used) </param>
        /// <param name="volume">Volume of this trade</param>
        /// <param name="bidSize">The size of the current bid, if available</param>
        /// <param name="askSize">The size of the current ask, if available</param>
        public override void Update(double lastTrade, double bidPrice, double askPrice, double volume, double bidSize, double askSize) {
            Initialize(lastTrade);
            if (lastTrade > High) High = lastTrade;
            if (lastTrade < Low) Low = lastTrade;
            //Volume is the total summed volume of trades in this bar:
            Volume += volume;
            //Always set the closing price;
            Close = lastTrade;
        }

        /// <summary>
        /// 	 Return a new instance clone of this object, used in fill forward
        /// </summary>
        /// <param name="fillForward">True if this is a fill forward clone</param>
        /// <returns>A clone of the current object</returns>
        public override BaseData Clone(bool fillForward) {
            var clone = base.Clone(fillForward);

            if (fillForward) {
                // zero volume out, since it would skew calculations in volume-based indicators
                ((TradeBar) clone).Volume = 0;
            }

            return clone;
        }

        /// <summary>
        /// 	 Return a new instance clone of this object
        /// </summary>
        public override BaseData Clone() {
            return (BaseData) MemberwiseClone();
        }

        /// <summary>
        /// 	 Formats a string with the symbol and value.
        /// </summary>
        /// <returns>string - a string formatted as SPY: 167.753</returns>
        public override string ToString() {
            return
                $"O: {Open.SmartRounding()} " +
                $"H: {High.SmartRounding()} " +
                $"L: {Low.SmartRounding()} " +
                $"C: {Close.SmartRounding()} " +
                $"V: {Volume.SmartRounding()}";
        }

        /// <summary>
        /// 	 Initializes this bar with a first data point
        /// </summary>
        /// <param name="value">The seed value for this bar</param>
        private void Initialize(double value) {
            if (Interlocked.CompareExchange(ref _initialized, 1, 0) == 0) {
                _open = value;
                _low = value;
                _high = value;
            }
        }
    }
}