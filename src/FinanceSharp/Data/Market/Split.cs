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
// *
//*/

//using System;
//using FinanceSharp;
//using Newtonsoft.Json;
//using static FinanceSharp.StringExtensions;

//namespace FinanceSharp.Data.Market {
//    /// <summary>
//    /// 	 Split event from a security
//    /// </summary>
//    public class Split : BaseData {
//        /// <summary>
//        ///Gets the type of split event, warning or split.
//        /// </summary>
//        [JsonProperty]
//        public SplitType Type { get; private set; }

//        /// <summary>
//        /// 	 Gets the split factor
//        /// </summary>
//        [JsonProperty]
//        public double SplitFactor { get; private set; }

//        /// <summary>
//        /// 	 Gets the price at which the split occurred
//        /// 	 This is typically the previous day's closing price
//        /// </summary>
//        public double ReferencePrice {
//            get { return Value; }
//            set { Value = value; }
//        }

//        /// <summary>
//        /// 	 Initializes a new instance of the Split class
//        /// </summary>
//        public Split() {
//            Type = SplitType.SplitOccurred;
//        }

//        /// <summary>
//        /// 	 Initializes a new instance of the Split class
//        /// </summary>
//        /// <param name="symbol">The symbol</param>
//        /// <param name="date">The date</param>
//        /// <param name="price">The price at the time of the split</param>
//        /// <param name="splitFactor">The split factor to be applied to current holdings</param>
//        /// <param name="type">The type of split event, warning or split occurred</param>
//        public Split(DateTime date, double price, double splitFactor, SplitType type)
//            : this() {
//            Type = type;
//            Time = date;

//            ReferencePrice = price;
//            SplitFactor = splitFactor;
//        }


//        /// <summary>
//        /// 	 Formats a string with the symbol and value.
//        /// </summary>
//        /// <returns>string - a string formatted as SPY: 167.753</returns>
//        public override string ToString() {
//            var type = Type == SplitType.Warning ? "Split Warning" : "Split";
//            return Invariant($"{type}: {SplitFactor} | {ReferencePrice}");
//        }

//        /// <summary>
//        /// 	 Return a new instance clone of this object, used in fill forward
//        /// </summary>
//        /// <remarks>
//        /// 	 This base implementation uses reflection to copy all public fields and properties
//        /// </remarks>
//        /// <returns>A clone of the current object</returns>
//        public override BaseData Clone() {
//            return new Split(Time, Price, SplitFactor, Type);
//        }
//    }
//}