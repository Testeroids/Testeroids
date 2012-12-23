// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockRepository.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
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
        private readonly List<IMock> mocksTrackedForSetupsNotVerifiedCheck = new List<IMock>();

        /// <summary>
        /// List of mocks which were delivered through <see cref="CreateMock{TMock}"/>. These mocks will be verified on <see cref="VerifyAll"/>.
        /// </summary>
        private readonly List<IMock> mocksTrackedForVerifyAll = new List<IMock>();

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
                    this.mocksTrackedForSetupsNotVerifiedCheck
                        .Cast<IMockInternals>()
                        .SelectMany(x => x.VerifiedSetups)
                        .GroupBy(x => x.Item1) // MemberInfo
                        .Where(x => x.All(setup => !setup.Item2)); // WasVerified
                var unverifiedMember = unverifiedMembers.Select(x => x.Key).FirstOrDefault();

                if (unverifiedMember != null)
                {
                    throw new MockNotVerifiedException(unverifiedMember);
                }
            }
            finally
            {
                this.mocksTrackedForSetupsNotVerifiedCheck.Clear();
            }
        }

        /// <summary>
        /// Creates a mock of an interface or a class, which can be later verified by callling <see cref="IMockRepository.VerifyAll"/>.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the Subject Under Test and verified afterwards.</returns>
        /// <remarks>The created mock is always "strict", meaning that every behavior has to be set up explicitly.</remarks>
        [NotNull]
        public IMock<TMock> CreateMock<TMock>() where TMock : class
        {
            var mock = this.CreateUnverifiedMock<TMock>();
            this.mocksTrackedForVerifyAll.Add(mock);
            return mock;
        }

        /// <summary>
        /// Creates a mock of an interface or a class.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the Subject Under Test and verified afterwards.</returns>
        /// <remarks>The created mock is always "strict", meaning that every behavior has to be set up explicitly.</remarks>
        [NotNull]
        public IMock<TMock> CreateUnverifiedMock<TMock>() where TMock : class
        {
            var mock = new TesteroidsMock<TMock>().As<TMock>();
            this.mocksTrackedForSetupsNotVerifiedCheck.Add(mock);
            return mock;
        }

        /// <summary>
        /// Ensures that all the set up mocks were actually used.
        /// </summary>
        /// <exception cref="MockException">Thrown if not all mocks were actually used by the SUT.</exception>
        public void VerifyAll()
        {
            try
            {
                foreach (var mock in this.mocksTrackedForVerifyAll)
                {
                    mock.VerifyAll();
                }
            }
            finally
            {
                this.mocksTrackedForVerifyAll.Clear();
            }
        }

        #endregion
    }
}