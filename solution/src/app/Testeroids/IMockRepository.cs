// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMockRepository.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using Moq;

    /// <summary>
    /// Provides consistent mock creation and tracking for later verification. Aspects subject to verification
    /// include whether mock setups were exercised by the production code, and whether all mocked methods were later verified
    /// through a call to <see cref="IMock.Verify"/>.
    /// </summary>
    public interface IMockRepository
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks that all methods mocked were verified at least once using <see cref="IMock.Verify"/>.
        /// </summary>
        /// <exception cref="MockNotVerifiedException">Thrown when a mock which was set up was not subsequently verified.</exception>
        void CheckAllSetupsVerified();

        /// <summary>
        /// Creates a mock of an interface or a class, which can be later verified by calling <see cref="VerifyAll"/>.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the Subject Under Test and verified afterwards.</returns>
        /// <remarks>The created mock is always "strict", meaning that every behavior has to be set up explicitly.</remarks>
        IMock<TMock> CreateMock<TMock>() where TMock : class;

        /// <summary>
        /// Creates a mock of an interface or a class.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the Subject Under Test and verified afterwards.</returns>
        /// <remarks>The created mock is always "strict", meaning that every behavior has to be set up explicitly.</remarks>
        IMock<TMock> CreateUnverifiedMock<TMock>() where TMock : class;

        /// <summary>
        /// Ensures that all the set up mocks were actually used.
        /// </summary>
        /// <exception cref="MockException">Thrown if not all mocks were actually used by the SUT.</exception>
        void VerifyAll();

        #endregion
    }
}