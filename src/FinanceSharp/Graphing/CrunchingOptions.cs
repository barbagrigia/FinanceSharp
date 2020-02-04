namespace FinanceSharp.Graphing {
    /// <summary>
    ///     <see cref="Cruncher"/>'s update trigger behavior.
    /// </summary>
    public enum CrunchingOptions {
        /// <summary>
        ///     All inputs must be updated atleast once.
        /// </summary>
        OnAllUpdatedOnce,

        /// <summary>
        ///     Every update a crunch is sent.
        /// </summary>
        OnEveryUpdate,

        /// <summary>
        ///     After a specific <see cref="IUpdatable"/> update a crunch will be called.
        /// </summary>
        OnSpecificUpdated
    }
}