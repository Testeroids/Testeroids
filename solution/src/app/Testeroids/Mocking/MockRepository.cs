// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockRepository.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using Moq;

    internal class MockRepository : IMockRepository
    {
        #region Fields

        /// <summary>
        /// List of mocks which were delivered through <see cref="CreateMock{TMock}"/>. These mocks will be verified to check if a set up method was verified through <see cref="IMock.Verify"/>.
        /// </summary>
        private readonly List<IMock> mocksTrackedForMatchingVerifyCallCheck = new List<IMock>();

        /// <summary>
        /// List of mocks which were delivered through <see cref="CreateMock{TMock}"/>. These mocks will be verified on <see cref="VerifyMocks"/>.
        /// </summary>
        private readonly List<IMock> mocksTrackedForUsageVerification = new List<IMock>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks that all methods mocked were verified at least once using <see cref="IMock.Verify"/>.
        /// </summary>
        /// <exception cref="MockNotVerifiedException">Thrown when a mock which was set up was not subsequently verified.</exception>
        public void CheckAllSetupsVerified()
        {
            try
            {
                var unverifiedMembers =
                    this.mocksTrackedForMatchingVerifyCallCheck
                        .Cast<IMockInternals>()
                        .SelectMany(x => x.VerifiedSetups)
                        .GroupBy(x => x.Item1) // MemberInfo
                        .Where(x => x.All(setup => !setup.Item2)); // WasVerified
                var unverifiedMember =
                    unverifiedMembers
                        .Select(x => x.Key)
                        .FirstOrDefault();

                if (unverifiedMember != null)
                {
                    throw new MockNotVerifiedException(unverifiedMember);
                }
            }
            finally
            {
                this.mocksTrackedForMatchingVerifyCallCheck.Clear();
            }
        }

        /// <summary>
        /// Creates a mock of an interface or a class, which can be later verified by calling <see cref="IMockRepository.VerifyMocks"/>.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the Subject Under Test and verified afterwards.</returns>
        /// <remarks>The created mock is always "strict", meaning that every behavior has to be set up explicitly.</remarks>
        [NotNull]
        public IMock<TMock> CreateMock<TMock>() where TMock : class
        {
            var mock = new TesteroidsMock<TMock>().As<TMock>();

            this.mocksTrackedForMatchingVerifyCallCheck.Add(mock);
            this.mocksTrackedForUsageVerification.Add(mock);

            return mock;
        }

        /// <summary>
        /// Reset the counts of all the method calls done previously.
        /// </summary>
        /// <remarks>Only verified mocks will be affected.</remarks>
        public void ResetAllCalls()
        {
            var resetableMocks = this.mocksTrackedForUsageVerification.OfType<IMockInternals>();
            foreach (var mock in resetableMocks)
            {
                mock.ResetAllCallCounts();
            }
        }

        /// <summary>
        /// Ensures that all the verifiable mock setups were actually used, by invoking <see cref="IMock.Verify"/> on all mocks.
        /// </summary>
        /// <exception cref="MockException">Thrown if not all mocks were actually used by the SUT.</exception>
        public void VerifyMocks()
        {
            try
            {
                foreach (var mock in this.mocksTrackedForUsageVerification)
                {
                    mock.Verify();
                }
            }
            finally
            {
                this.mocksTrackedForUsageVerification.Clear();
            }
        }

        #endregion
    }
}