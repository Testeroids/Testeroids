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
        #region Fields

        private readonly string message;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrerequisiteFailureException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public PrerequisiteFailureException(string message)
            : base(message)
        {
            this.message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        #endregion
    }
}