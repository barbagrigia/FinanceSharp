namespace FinanceSharp.Indicators {
    /// <summary>
    ///     Selects a specific range of (<see cref="Start"/> and <see cref="Stop"/>) of what properties to select.
    /// </summary>
    /// <remarks>To select property index 1 (2nd property) then call <code>new PropertySelector(1) or new PropertySelector(1, 2)</code></remarks>
    public class PropertySelector : IndicatorBase {
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
        /// <param name="property">The index of the property to slice.</param>
        /// <param name="name">The name of this indicator</param>
        public PropertySelector(int property, string name) : this(property, property + 1, name) { }


        /// <summary>
        /// 	 Initializes a new instance of the Indicator class using the specified name.
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="startProperty">Start of interval. The interval includes this value. The default start value is 0.</param>
        /// <param name="stopProperty">End of interval. The interval does not include this value, except in some cases where step is not an integer and floating point round-off affects the length of out.</param>
        public PropertySelector(int startProperty, int stopProperty, string name) : base(name) {
            Start = startProperty;
            Stop = stopProperty;
            AssertTrue(Start >= Stop, "Start is out of range from input's Properties.");
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
            AssertTrue(Start >= 0 && Start < input.Properties, "Start is out of range from input's Properties.");
            AssertTrue(Stop >= 0 && Stop < input.Properties, "Stop is out of range from input's Properties.");
            AssertTrue(Stop >= 0 && Stop < input.Properties, "Stop and Start is out of range from input's Properties.");
            var propsLen = Stop - Start;
            if (input.Count == 1) {
                var ret = new DoubleArray2DManaged(1, propsLen);
                for (int i = 0; i < propsLen; i++) {
                    ret[0, i] = input[0, i + Start];
                }

                return ret;
            } else {
                var ret = new DoubleArray2DManaged(input.Count, propsLen);
                var inputCount = input.Count;
                for (int cnt = 0; cnt < inputCount; cnt++) {
                    for (int prop = 0; prop < propsLen; prop++) {
                        ret[cnt, prop] = input[cnt, prop + Start];
                    }
                }

                return ret;
            }
        }
    }
}