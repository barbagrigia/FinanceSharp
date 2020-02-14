namespace FinanceSharp.Indicators {
    /// <summary>
    ///     A selector that calls <see cref="DoubleArray.Slice(int,int)"/> on given <see cref="Start"/> and <see cref="Stop"/>.
    /// </summary>
    public class SliceSelector : IndicatorBase {
        /// <summary>
        ///     Start of interval. The interval includes this value. The default start value is 0.
        /// </summary>
        public int Start { get; }

        /// <summary>
        ///     End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.
        /// </summary>
        public int Stop { get; }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="start">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stop">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        public SliceSelector(int start, int stop, string name) : base(name) {
            Start = start;
            Stop = stop;
        }

        /// <summary>
        /// 	 Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => Samples > 0;

        /// <summary>
        /// 	 Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override DoubleArray Forward(long time, DoubleArray input) {
            return input.Slice(Start, Stop);
        }
    }
}