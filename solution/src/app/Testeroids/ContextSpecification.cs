// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSpecification.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using Moq;

    /// <summary>
    ///   Implements the base class to define a Context/Specification style test fixture to test a class method.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest"> The type of the subject under test. </typeparam>
    public abstract class ContextSpecification<TSubjectUnderTest> : ContextSpecificationBase
        where TSubjectUnderTest : class
    {
        #region Fields

        private readonly List<Mock> deliveredMocksList = new List<Mock>();

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the instance of the subject under test.
        /// </summary>
        protected TSubjectUnderTest Sut { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a mock of an interface or a class.
        /// </summary>
        /// <typeparam name="TMock">The type to be mocked.</typeparam>
        /// <returns>An instance of <typeparamref name="TMock"/> which can be passed to the <see cref="Sut"/> and verified afterwards.</returns>
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
        ///   Called to allow the derived class to instantiate the subject under test (<see cref="Sut"/>).
        /// </summary>
        /// <returns> Returns the subject under test </returns>
        protected abstract TSubjectUnderTest CreateSubjectUnderTest();

        /// <summary>
        /// Ensures that all the set up mocks were actually used after the test fixture is complete. Will throw <see cref="MockException"/> if not.
        /// </summary>
        protected override void DisposeContext()
        {
            base.DisposeContext();

            foreach (var mock in this.deliveredMocksList)
            {
                mock.VerifyAll();
            }

            this.deliveredMocksList.Clear();
        }

        /// <summary>
        ///   Takes care of creating (through <see cref="CreateSubjectUnderTest"/>) and initializing the subject under test.
        /// </summary>
        protected override void InitializeSubjectUnderTest()
        {
            this.Sut = this.CreateSubjectUnderTest();
        }

        #endregion
    }
}