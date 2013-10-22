namespace Testeroids
{
    using System;

    using NUnit.Framework;

    /// <summary>
    /// The exception type thrown when a method marked with the <see cref="PrerequisiteAttribute"/> fails.
    /// </summary>
    [Serializable]
    public class PrerequisiteFailureException : AssertionException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrerequisiteFailureException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="inner">
        /// The exception that caused the current exception.
        /// </param>
        public PrerequisiteFailureException(
            string message, 
            Exception inner)
            : base(message, inner)
        {
        }

        #endregion
    }
}