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
using FinanceSharp;

namespace FinanceSharp.Data.Market {
    /// <summary>
    /// 	 QuoteBar class for second and minute resolution data:
    /// 	 An OHLC implementation of the QuantConnect BaseData class with parameters for candles.
    /// </summary>
    public class QuoteBar : BaseData, IBaseDataBar {
        // scale factor used in QC equity/forex data files
        private const double _scaleFactor = 1 / 1000.0d;

        /// <summary>
        /// 	 Average bid size
        /// </summary>
        public double LastBidSize { get; set; }

        /// <summary>
        /// 	 Average ask size
        /// </summary>
        public double LastAskSize { get; set; }

        /// <summary>
        /// 	 Bid OHLC
        /// </summary>
        public Bar Bid { get; set; }

        /// <summary>
        /// 	 Ask OHLC
        /// </summary>
        public Bar Ask { get; set; }

        /// <summary>
        /// 	 Opening price of the bar: Defined as the price at the start of the time period.
        /// </summary>
        public double Open {
            get {
                if (Bid != null && Ask != null) {
                    if (Bid.Open != Constants.Zero && Ask.Open != Constants.Zero)
                        return (Bid.Open + Ask.Open) / 2d;

                    if (Bid.Open != 0)
                        return Bid.Open;

                    if (Ask.Open != 0)
                        return Ask.Open;

                    return Constants.Zero;
                }

                if (Bid != null) {
                    return Bid.Open;
                }

                if (Ask != null) {
                    return Ask.Open;
                }

                return Constants.Zero;
            }
        }

        /// <summary>
        /// 	 High price of the QuoteBar during the time period.
        /// </summary>
        public double High {
            get {
                if (Bid != null && Ask != null) {
                    if (Bid.High != Constants.Zero && Ask.High != Constants.Zero)
                        return (Bid.High + Ask.High) / 2d;

                    if (Bid.High != 0)
                        return Bid.High;

                    if (Ask.High != 0)
                        return Ask.High;

                    return Constants.Zero;
                }

                if (Bid != null) {
                    return Bid.High;
                }

                if (Ask != null) {
                    return Ask.High;
                }

                return Constants.Zero;
            }
        }

        /// <summary>
        /// 	 Low price of the QuoteBar during the time period.
        /// </summary>
        public double Low {
            get {
                if (Bid != null && Ask != null) {
                    if (Bid.Low != Constants.Zero && Ask.Low != Constants.Zero)
                        return (Bid.Low + Ask.Low) / 2d;

                    if (Bid.Low != 0)
                        return Bid.Low;

                    if (Ask.Low != 0)
                        return Ask.Low;

                    return Constants.Zero;
                }

                if (Bid != null) {
                    return Bid.Low;
                }

                if (Ask != null) {
                    return Ask.Low;
                }

                return Constants.Zero;
            }
        }

        /// <summary>
        /// 	 Closing price of the QuoteBar. Defined as the price at Start Time + TimeSpan.
        /// </summary>
        public double Close {
            get {
                if (Bid != null && Ask != null) {
                    if (Bid.Close != Constants.Zero && Ask.Close != Constants.Zero)
                        return (Bid.Close + Ask.Close) / 2d;

                    if (Bid.Close != 0)
                        return Bid.Close;

                    if (Ask.Close != 0)
                        return Ask.Close;

                    return Constants.Zero;
                }

                if (Bid != null) {
                    return Bid.Close;
                }

                if (Ask != null) {
                    return Ask.Close;
                }

                return Value;
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
        /// 	 The period of this quote bar, (second, minute, daily, ect...)
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// 	 Default initializer to setup an empty quotebar.
        /// </summary>
        public QuoteBar() {
            Time = new DateTime();
            Bid = new Bar();
            Ask = new Bar();
            Value = 0;
            Period = TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// 	 Initialize Quote Bar with Bid(OHLC) and Ask(OHLC) Values:
        /// </summary>
        /// <param name="time">DateTime Timestamp of the bar</param>
        /// <param name="symbol">Market MarketType Symbol</param>
        /// <param name="bid">Bid OLHC bar</param>
        /// <param name="lastBidSize">Average bid size over period</param>
        /// <param name="ask">Ask OLHC bar</param>
        /// <param name="lastAskSize">Average ask size over period</param>
        /// <param name="period">The period of this bar, specify null for default of 1 minute</param>
        public QuoteBar(DateTime time, IBar bid, double lastBidSize, IBar ask, double lastAskSize, TimeSpan? period = null) {
            Time = time;
            Bid = bid == null ? null : new Bar(bid.Open, bid.High, bid.Low, bid.Close);
            Ask = ask == null ? null : new Bar(ask.Open, ask.High, ask.Low, ask.Close);
            if (Bid != null) LastBidSize = lastBidSize;
            if (Ask != null) LastAskSize = lastAskSize;
            Value = Close;
            Period = period ?? TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// 	 Update the quotebar - build the bar from this pricing information:
        /// </summary>
        /// <param name="lastTrade">The last trade price</param>
        /// <param name="bidPrice">Current bid price</param>
        /// <param name="askPrice">Current asking price</param>
        /// <param name="volume">Volume of this trade</param>
        /// <param name="bidSize">The size of the current bid, if available, if not, pass 0</param>
        /// <param name="askSize">The size of the current ask, if available, if not, pass 0</param>
        public override void Update(double lastTrade, double bidPrice, double askPrice, double volume, double bidSize, double askSize) {
            // update our bid and ask bars - handle null values, this is to give good values for midpoint OHLC
            if (Bid == null && bidPrice != 0) Bid = new Bar();
            if (Bid != null) Bid.Update(bidPrice);

            if (Ask == null && askPrice != 0) Ask = new Bar();
            if (Ask != null) Ask.Update(askPrice);

            if (bidSize > 0) {
                LastBidSize = bidSize;
            }

            if (askSize > 0) {
                LastAskSize = askSize;
            }

            // be prepared for updates without trades
            if (lastTrade != 0) Value = lastTrade;
            else if (askPrice != 0) Value = askPrice;
            else if (bidPrice != 0) Value = bidPrice;
        }


        /// <summary>
        /// 	 Return a new instance clone of this quote bar, used in fill forward
        /// </summary>
        /// <returns>A clone of the current quote bar</returns>
        public override BaseData Clone() {
            return new QuoteBar {
                Ask = Ask == null ? null : Ask.Clone(),
                Bid = Bid == null ? null : Bid.Clone(),
                LastAskSize = LastAskSize,
                LastBidSize = LastBidSize,

                Time = Time,
                Period = Period,
                Value = Value
            };
        }

        /// <summary>
        /// 	 Collapses QuoteBars into TradeBars object when
        ///  algorithm requires FX data, but calls OnData(<see cref="TradeBars"/>)
        /// 	 TODO: (2017) Remove this method in favor of using OnData(<see cref="Slice"/>)
        /// </summary>
        /// <returns><see cref="TradeBars"/></returns>
        public TradeBar Collapse() {
            return new TradeBar(Time, Open, High, Low, Close, Constants.Zero) {
                Period = Period
            };
        }

        public override string ToString() {
            return
                $"Bid: O: {Bid?.Open.SmartRounding()} " +
                $"Bid: H: {Bid?.High.SmartRounding()} " +
                $"Bid: L: {Bid?.Low.SmartRounding()} " +
                $"Bid: C: {Bid?.Close.SmartRounding()} " +
                $"Ask: O: {Ask?.Open.SmartRounding()} " +
                $"Ask: H: {Ask?.High.SmartRounding()} " +
                $"Ask: L: {Ask?.Low.SmartRounding()} " +
                $"Ask: C: {Ask?.Close.SmartRounding()} ";
        }
    }
}