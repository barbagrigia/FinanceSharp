namespace FinanceSharp.Helpers {
    /// <summary>
    /// 	 Specifies the style of an option
    /// </summary>
    public enum OptionStyle {
        /// <summary>
        /// 	 American style options are able to be exercised at any time on or before the expiration date
        /// </summary>
        American,

        /// <summary>
        /// 	 European style options are able to be exercised on the expiration date only.
        /// </summary>
        European
    }
}