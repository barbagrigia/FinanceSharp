namespace FinanceSharp.Indicators {
    /// <summary>
    ///     A selector that calls <see cref="DoubleArray.SliceIndex(int)"/> on given <see cref="SelectedIndex"/>.
    /// </summary>
    public class CountSelector : IndicatorBase {
        /// <summary>
        ///     The index to get from <see cref="IUpdatable.OutputCount"/>.
        /// </summary>
        public int SelectedIndex { get; }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="index">The index to get from <see cref="IUpdatable.OutputCount"/>.</param>
        /// <param name="name">The name of this indicator</param>
        public CountSelector(int index, string name) : base(name) {
            SelectedIndex = index;
            OutputCount = 1;
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
            AssertTrue(SelectedIndex < input.Count, "Index is out of range from input's Count index.");
            return input.SliceIndex(SelectedIndex);
        }
    }
}