namespace Testeroids
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using Moq;

    using NUnit.Framework;

    public abstract class TestFixtureBase
    {
        #region Fields

        private readonly List<Mock> deliveredMocksList = new List<Mock>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Called when the test fixture is teared down.
        /// </summary>
        [TearDown]
        public virtual void BaseTearDown()
        {
            this.VerifyAllMocks();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a mock of an interface or a class.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the <see cref="ContextSpecification{TSubjectUnderTest}.Sut"/> and verified afterwards.</returns>
        /// <remarks>The created mock is always "strict", meaning that every behavior has to be set up explicitly.</remarks>
        [NotNull]
        protected Moq.Mock<TMock> CreateMock<TMock>()
            where TMock : class
        {
            var mock = new Mock<TMock>(MockBehavior.Strict).As<TMock>();
            this.deliveredMocksList.Add(mock);
            return mock;
        }

        /// <summary>
        /// Ensures that all the set up mocks were actually used after the test fixture is complete. Will throw <see cref="MockException"/> if not.
        /// </summary>
        protected void VerifyAllMocks()
        {
            try
            {
                foreach (var mock in this.deliveredMocksList)
                {
                    mock.VerifyAll();
                }
            }
            finally
            {
                this.deliveredMocksList.Clear();
            }
        }

        #endregion
    }
}