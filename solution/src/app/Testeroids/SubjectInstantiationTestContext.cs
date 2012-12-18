// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubjectInstantiationTestContext.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using System.Diagnostics;

    using JetBrains.Annotations;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    ///   Implements the base class to define a Context/Specification style test fixture to test a class constructor.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest"> Type of the class which constructor is to be tested. </typeparam>
    public abstract class SubjectInstantiationTestContext<TSubjectUnderTest> : IContextSpecification
        where TSubjectUnderTest : class
    {
        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether there are prerequisite tests running. Here, no prerequisite tests are needed for constructor testing, but interface must be implemented.
        /// </summary>
        public bool ArePrerequisiteTestsRunning
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the subject under test.
        /// </summary>
        protected TSubjectUnderTest Sut { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The base set up.
        /// </summary>
        [SetUp]
        public virtual void BaseSetUp()
        {
            this.PreTestSetUp();
            this.EstablishContext();
        }

        /// <summary>
        ///   Disposes the <see cref="Sut"/> and the context.
        /// </summary>
        [TearDown]
        public virtual void BaseTearDown()
        {
            this.DisposeContext();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   This will be called by the ArrangeActAssertAspectAttribute aspect.
        /// </summary>
        protected void Because()
        {
            this.Sut = this.BecauseSutIsCreated();
        }

        /// <summary>
        ///   The method being tested. It instantiates the <see cref="Sut"/>.
        /// </summary>
        /// <returns> The instance of TSubjectUnderTest. </returns>
        protected abstract TSubjectUnderTest BecauseSutIsCreated();

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
        ///   Called to dispose all unmanaged resources used by the test.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void DisposeContext()
        {
        }

        /// <summary>
        ///   The arrange part.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void EstablishContext()
        {
        }

        /// <summary>
        /// This test is meant for internal library use only.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void PreTestSetUp()
        {
        }

        /// <summary>
        ///   This will be called by the ArrangeActAssertAspectAttribute aspect.
        /// </summary>
        protected void RunPrerequisiteTests()
        {
        }

        #endregion
    }
}