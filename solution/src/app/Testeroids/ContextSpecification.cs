// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSpecification.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using JetBrains.Annotations;

    using Moq;

    /// <summary>
    ///   Implements the base class to define a Context/Specification style test fixture to test a class method.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest"> The type of the subject under test. </typeparam>
    public abstract class ContextSpecification<TSubjectUnderTest> : ContextSpecificationBase
        where TSubjectUnderTest : class
    {
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
            return new Mock<TMock>(MockBehavior.Strict);
        }

        /// <summary>
        ///   Called to allow the derived class to instantiate the subject under test (<see cref="Sut"/>).
        /// </summary>
        /// <returns> Returns the subject under test </returns>
        protected abstract TSubjectUnderTest CreateSubjectUnderTest();

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