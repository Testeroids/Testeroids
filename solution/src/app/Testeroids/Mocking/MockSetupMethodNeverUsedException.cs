namespace Testeroids.Mocking
{
    using System;

    using Moq;

    /// <summary>
    /// Provides a more explicit error message for <see cref="MockException"/>s thrown when Verify() or VerifyAll() is called.
    /// </summary>
    public class MockSetupMethodNeverUsedException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MockSetupMethodNeverUsedException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message describing the exception.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public MockSetupMethodNeverUsedException(
            string message, 
            Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockSetupMethodNeverUsedException"/> class. This override of the constructor provides a proper error message to describe the reason for the exception.
        /// </summary>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public MockSetupMethodNeverUsedException(MockException innerException)
            :
                this(string.Format("Some methods of the mock were setup, but never used.\r\n\r\n{0}", innerException.Message), innerException)
        {
        }

        #endregion
    }
}