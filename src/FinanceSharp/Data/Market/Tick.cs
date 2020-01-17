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
//using FinanceSharp;

//namespace FinanceSharp.Data.Market {
//    /// <summary>
//    /// 	 Tick class is the base representation for tick data. It is grouped into a Ticks object
//    /// 	 which implements IDictionary and passed into an OnData event handler.
//    /// </summary>
//    public class Tick : BaseData {
//        /// <summary>
//        /// 	 Type of the Tick: Trade or Quote.
//        /// </summary>
//        public TickType TickType = TickType.Trade;

//        /// <summary>
//        /// 	 Quantity exchanged in a trade.
//        /// </summary>
//        public double Quantity;

//        /// <summary>
//        /// 	 Exchange we are executing on. String short code expanded in the MarketCodes.US global dictionary
//        /// </summary>
//        public string Exchange = "";

//        /// <summary>
//        /// 	 Sale condition for the tick.
//        /// </summary>
//        public string SaleCondition = "";

//        /// <summary>
//        /// 	 Bool whether this is a suspicious tick
//        /// </summary>
//        public bool Suspicious;

//        /// <summary>
//        /// 	 Bid Price for Tick
//        /// </summary>
//        /// <remarks>QuantConnect does not currently have quote data but was designed to handle ticks and quotes</remarks>
//        public double BidPrice;

//        /// <summary>
//        /// 	 Asking price for the Tick quote.
//        /// </summary>
//        /// <remarks>QuantConnect does not currently have quote data but was designed to handle ticks and quotes</remarks>
//        public double AskPrice;

//        /// <summary>
//        /// 	 Alias for "Value" - the last sale for this asset.
//        /// </summary>
//        public double LastPrice {
//            get { return Value; }
//        }

//        /// <summary>
//        /// 	 Size of bid quote.
//        /// </summary>
//        public double BidSize;

//        /// <summary>
//        /// 	 Size of ask quote.
//        /// </summary>
//        public double AskSize;

//        //In Base Class: Alias of Closing:
//        //public double Price;

//        // Asset.
//        //In Base Class: public ;

//        //In Base Class: DateTime Of this TradeBar
//        //public DateTime Time;

//        /// <summary>
//        /// 	 Initialize tick class with a default constructor.
//        /// </summary>
//        public Tick() {
//            Value = 0;
//            Time = new DateTime();
//            TickType = TickType.Trade;
//            Quantity = 0;
//            Exchange = "";
//            SaleCondition = "";
//            Suspicious = false;
//            BidSize = 0;
//            AskSize = 0;
//        }

//        /// <summary>
//        /// 	 Cloner constructor for fill forward engine implementation. Clone the original tick into this new tick:
//        /// </summary>
//        /// <param name="original">Original tick we're cloning</param>
//        public Tick(Tick original) {
//            Time = new DateTime(original.Time.Ticks);
//            Value = original.Value;
//            BidPrice = original.BidPrice;
//            AskPrice = original.AskPrice;
//            Exchange = original.Exchange;
//            SaleCondition = original.SaleCondition;
//            Quantity = original.Quantity;
//            Suspicious = original.Suspicious;
//            TickType = original.TickType;
//            BidSize = original.BidSize;
//            AskSize = original.AskSize;
//        }

//        /// <summary>
//        /// 	 Constructor for a FOREX tick where there is no last sale price. The volume in FX is so high its rare to find FX trade data.
//        /// 	 To fake this the tick contains bid-ask prices and the last price is the midpoint.
//        /// </summary>
//        /// <param name="time">Full date and time</param>
//        /// <param name="symbol">Underlying currency pair we're trading</param>
//        /// <param name="bid">FX tick bid value</param>
//        /// <param name="ask">FX tick ask value</param>
//        public Tick(DateTime time, double bid, double ask) {
//            Time = time;

//            Value = (bid + ask) / 2;
//            TickType = TickType.Quote;
//            BidPrice = bid;
//            AskPrice = ask;
//        }

//        /// <summary>
//        /// 	 Initializer for a last-trade equity tick with bid or ask prices.
//        /// </summary>
//        /// <param name="time">Full date and time</param>
//        /// <param name="symbol">Underlying equity security symbol</param>
//        /// <param name="bid">Bid value</param>
//        /// <param name="ask">Ask value</param>
//        /// <param name="last">Last trade price</param>
//        public Tick(DateTime time, double last, double bid, double ask) {
//            Time = time;

//            Value = last;
//            TickType = TickType.Quote;
//            BidPrice = bid;
//            AskPrice = ask;
//        }

//        /// <summary>
//        /// 	 Update the tick price information - not used.
//        /// </summary>
//        /// <param name="lastTrade">This trade price</param>
//        /// <param name="bidPrice">Current bid price</param>
//        /// <param name="askPrice">Current asking price</param>
//        /// <param name="volume">Volume of this trade</param>
//        /// <param name="bidSize">The size of the current bid, if available</param>
//        /// <param name="askSize">The size of the current ask, if available</param>
//        public override void Update(double lastTrade, double bidPrice, double askPrice, double volume, double bidSize, double askSize) {
//            Value = lastTrade;
//            BidPrice = bidPrice;
//            AskPrice = askPrice;
//            BidSize = bidSize;
//            AskSize = askSize;
//            Quantity = Convert.ToDouble(volume);
//        }

//        /// <summary>
//        /// 	 Check if tick contains valid data (either a trade, or a bid or ask)
//        /// </summary>
//        public bool IsValid() {
//            return (TickType == TickType.Trade && LastPrice > 0.0d && Quantity > 0) ||
//                   (TickType == TickType.Quote && AskPrice > 0.0d && AskSize > 0) ||
//                   (TickType == TickType.Quote && BidPrice > 0.0d && BidSize > 0) ||
//                   (TickType == TickType.OpenInterest && Value > 0);
//        }

//        /// <summary>
//        /// 	 Clone implementation for tick class:
//        /// </summary>
//        /// <returns>New tick object clone of the current class values.</returns>
//        public override BaseData Clone() {
//            return new Tick(this);
//        }

//        /// <summary>
//        /// 	 Formats a string with the symbol and value.
//        /// </summary>
//        /// <returns>string - a string formatted as SPY: 167.753</returns>
//        public override string ToString() {
//            switch (TickType) {
//                case TickType.Trade:
//                    return $"Price: {Price} Quantity: {Quantity}";

//                case TickType.Quote:
//                    return $"Bid: {BidSize}@{BidPrice} Ask: {AskSize}@{AskPrice}";

//                case TickType.OpenInterest:
//                    return $"OpenInterest: {Value}";

//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }

//        private static double GetScaleFactor(SecurityType securityType) {
//            return securityType == SecurityType.Equity || securityType == SecurityType.Option ? 1000.0d : 1;
//        }
//    }
//}