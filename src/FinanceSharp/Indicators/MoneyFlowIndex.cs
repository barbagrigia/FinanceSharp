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

using FinanceSharp.Data;
using static FinanceSharp.Constants;
using FinanceSharp.Data;


namespace FinanceSharp.Indicators {
    /// <summary>
    /// 	 The Money Flow Index (MFI) is an oscillator that uses both price and volume to
    /// 	 measure buying and selling pressure
    ///
    /// 	 Typical Price = (High + Low + Close)/3
    /// 	 Money Flow = Typical Price x Volume
    /// 	 Positive Money Flow = Sum of the money flows of all days where the typical
    ///     price is greater than the previous day's typical price
    /// 	 Negative Money Flow = Sum of the money flows of all days where the typical
    ///     price is less than the previous day's typical price
    /// 	 Money Flow Ratio = (14-period Positive Money Flow)/(14-period Negative Money Flow)
    ///
    /// 	 Money Flow Index = 100 x  Positive Money Flow / ( Positive Money Flow + Negative Money Flow)
    /// </summary>
    public class MoneyFlowIndex : TradeBarIndicator {
        /// <summary>
        /// 	 The sum of positive money flow to compute money flow ratio
        /// </summary>
        public IndicatorBase PositiveMoneyFlow { get; }

        /// <summary>
        /// 	 The sum of negative money flow to compute money flow ratio
        /// </summary>
        public IndicatorBase NegativeMoneyFlow { get; }

        /// <summary>
        /// 	 The current and previous typical price is used to determine positive or negative money flow
        /// </summary>
        public double PreviousTypicalPrice { get; private set; }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => PositiveMoneyFlow.IsReady && NegativeMoneyFlow.IsReady;

        /// <summary>
        /// 	 Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod { get; }

        /// <summary>
        /// 	 Resets this indicator to its initial state
        /// </summary>
        public override void Reset() {
            PreviousTypicalPrice = 0.0d;
            PositiveMoneyFlow.Reset();
            NegativeMoneyFlow.Reset();
            base.Reset();
        }

        /// <summary>
        /// 	 Initializes a new instance of the MoneyFlowIndex class
        /// </summary>
        /// <param name="period">The period of the negative and positive money flow</param>
        public MoneyFlowIndex(int period)
            : this($"MFI({period})", period) { }

        /// <summary>
        /// 	 Initializes a new instance of the MoneyFlowIndex class
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="period">The period of the negative and positive money flow</param>
        public MoneyFlowIndex(string name, int period)
            : base(name) {
            WarmUpPeriod = period;
            PositiveMoneyFlow = new Sum(name + "_PositiveMoneyFlow", period);
            NegativeMoneyFlow = new Sum(name + "_NegativeMoneyFlow", period);
        }

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            var typicalPrice = (input[HighIdx] + input[LowIdx] + input[CloseIdx]) / 3.0d;
            var moneyFlow = typicalPrice * input[VolumeIdx];

            PositiveMoneyFlow.Update(time, typicalPrice > PreviousTypicalPrice ? moneyFlow : 0.0d);
            NegativeMoneyFlow.Update(time, typicalPrice < PreviousTypicalPrice ? moneyFlow : 0.0d);
            PreviousTypicalPrice = typicalPrice;

            var totalMoneyFlow = PositiveMoneyFlow + NegativeMoneyFlow;
            if (totalMoneyFlow == 0.0d) {
                return 100.0d;
            }

            return 10.0d * PositiveMoneyFlow / totalMoneyFlow;
        }
    }
}