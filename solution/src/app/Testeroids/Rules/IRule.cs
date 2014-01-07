namespace Testeroids.Rules
{
    /// <summary>
    ///   Defines the contract for a rule which can verify test classes at compile- and runtime.
    /// </summary>
    public interface IRule
    {
        #region Public Methods and Operators

        /// <summary>
        /// Initializes the rule once per instance.
        /// </summary>
        void Initialize();

        #endregion
    }
}