namespace Testeroids.Rules
{
    using System;

    /// <summary>
    /// Abstract rule which can verify test classes at compile- and runtime.
    /// </summary>
    [Serializable]
    public abstract class Rule : IRule
    {
        /// <summary>
        /// Initializes the rule once per instance.
        /// </summary>
        public virtual void Initialize()
        {
        }
    }
}