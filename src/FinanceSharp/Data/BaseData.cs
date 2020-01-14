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

namespace QuantConnect.Data {
    /// <summary>
    /// 	 Abstract base data class of QuantConnect. It is intended to be extended to define
    /// 	 generic user customizable data types while at the same time implementing the basics of data where possible
    /// </summary>
    public abstract class BaseData : IBaseData {
        private double _value;

        /// <summary>
        /// 	 True if this is a fill forward piece of data
        /// </summary>
        public bool IsFillForward { get; private set; }

        /// <summary>
        /// 	 Current time marker of this data packet.
        /// </summary>
        /// <remarks>All data is timeseries based.</remarks>
        public DateTime Time { get; set; }

        /// <summary>
        /// 	 The end time of this data. Some data covers spans (trade bars) and as such we want
        /// 	 to know the entire time span covered
        /// </summary>
        public virtual DateTime EndTime {
            get { return Time; }
            set { Time = value; }
        }

        /// <summary>
        /// 	 Value representation of this data packet. All data requires a representative value for this moment in time.
        /// 	 For streams of data this is the price now, for OHLC packets this is the closing price.
        /// </summary>
        public virtual double Value {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 	 As this is a backtesting platform we'll provide an alias of value as price.
        /// </summary>
        public double Price => Value;


        /// <summary>
        /// 	 Updates this base data with a new trade
        /// </summary>
        /// <param name="lastTrade">The price of the last trade</param>
        /// <param name="tradeSize">The quantity traded</param>
        public void UpdateTrade(double lastTrade, double tradeSize) {
            Update(lastTrade, 0, 0, tradeSize, 0, 0);
        }

        /// <summary>
        /// 	 Updates this base data with new quote information
        /// </summary>
        /// <param name="bidPrice">The current bid price</param>
        /// <param name="bidSize">The current bid size</param>
        /// <param name="askPrice">The current ask price</param>
        /// <param name="askSize">The current ask size</param>
        public void UpdateQuote(double bidPrice, double bidSize, double askPrice, double askSize) {
            Update(0, bidPrice, askPrice, 0, bidSize, askSize);
        }

        /// <summary>
        /// 	 Updates this base data with the new quote bid information
        /// </summary>
        /// <param name="bidPrice">The current bid price</param>
        /// <param name="bidSize">The current bid size</param>
        public void UpdateBid(double bidPrice, double bidSize) {
            Update(0, bidPrice, 0, 0, bidSize, 0);
        }

        /// <summary>
        /// 	 Updates this base data with the new quote ask information
        /// </summary>
        /// <param name="askPrice">The current ask price</param>
        /// <param name="askSize">The current ask size</param>
        public void UpdateAsk(double askPrice, double askSize) {
            Update(0, 0, askPrice, 0, 0, askSize);
        }

        /// <summary>
        /// 	 Update routine to build a bar/tick from a data update.
        /// </summary>
        /// <param name="lastTrade">The last trade price</param>
        /// <param name="bidPrice">Current bid price</param>
        /// <param name="askPrice">Current asking price</param>
        /// <param name="volume">Volume of this trade</param>
        /// <param name="bidSize">The size of the current bid, if available</param>
        /// <param name="askSize">The size of the current ask, if available</param>
        public virtual void Update(double lastTrade, double bidPrice, double askPrice, double volume, double bidSize, double askSize) {
            Value = lastTrade;
        }

        /// <summary>
        /// 	 Return a new instance clone of this object, used in fill forward
        /// </summary>
        /// <remarks>
        /// 	 This base implementation uses reflection to copy all public fields and properties
        /// </remarks>
        /// <param name="fillForward">True if this is a fill forward clone</param>
        /// <returns>A clone of the current object</returns>
        public virtual BaseData Clone(bool fillForward) {
            var clone = Clone();
            clone.IsFillForward = fillForward;
            return clone;
        }

        /// <summary>
        /// 	 Return a new instance clone of this object, used in fill forward
        /// </summary>
        /// <remarks>
        /// 	 This base implementation uses reflection to copy all public fields and properties
        /// </remarks>
        /// <returns>A clone of the current object</returns>
        public virtual BaseData Clone() {
            return null;
            //TODO:
        }

        /// <summary>
        /// 	 Formats a string with the symbol and value.
        /// </summary>
        /// <returns>string - a string formatted as SPY: 167.753</returns>
        public override string ToString() {
            return $"{Value.ToStringInvariant("C")}";
        }
    }
}