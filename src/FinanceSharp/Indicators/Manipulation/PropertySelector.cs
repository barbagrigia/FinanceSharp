namespace FinanceSharp.Indicators {
    public class PropertySelector : IndicatorBase {
        public int SelectedProperty { get; }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        public PropertySelector(int property, string name) : base(name) {
            SelectedProperty = property;
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
            AssertTrue(SelectedProperty < input.Properties, "Property is out of range from input's Properties.");
            if (input.Count==1) {
                return input[SelectedProperty];
            } else {
                var ret = new DoubleArray2DManaged(input.Count, 1);
                input.ForEach(SelectedProperty, (int index, double value) => ret[index] = value);
                return ret;
            }
        }
    }

    public class CountSelector : IndicatorBase {
        public int SelectedIndex { get; }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
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

    public class SliceSelector : IndicatorBase {
        public int Start { get; }
        public int Stop { get; }

        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
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