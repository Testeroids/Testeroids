// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="ContextSpecificationBase.cs">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using Moq;

    using NUnit.Framework;

    using Testeroids.Aspects;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    public abstract class ContextSpecificationBase : IContextSpecification
    {
        #region Fields

        private readonly List<Mock> deliveredMocksList = new List<Mock>();

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether there are prerequisite tests running.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ArePrerequisiteTestsRunning { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Sets up the test fixture (calls <see cref="EstablishContext"/> followed by <see cref="InitializeSubjectUnderTest"/>).
        /// </summary>
        [SetUp]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseSetUp()
        {
            this.PreTestSetUp();
            this.EstablishContext();
            this.InitializeSubjectUnderTest();
        }

        /// <summary>
        ///   Called when the test fixture is teared down (invokes <see cref="DisposeContext"/>).
        /// </summary>
        [TearDown]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseTearDown()
        {
            this.DisposeContext();

            this.VerifyAllMocks();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   This will be called by the <see cref="ArrangeActAssertAspectAttribute"/> aspect. Performs the "Act" part, or the logic which is to be tested.
        /// </summary>
        protected internal abstract void Because();

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
        ///   Performs additional initialization after the subject under test has been created.
        /// </summary>
        protected abstract void InitializeSubjectUnderTest();

        /// <summary>
        /// This test is meant for internal library use only.
        /// </summary>
        [DebuggerNonUserCode]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void PreTestSetUp()
        {
        }

        /// <summary>
        ///   This will be called by the ArrangeActAssertAspectAttribute aspect. Executes all tests in the context marked with the <see cref="PrerequisiteAttribute"/>.
        /// </summary>
        protected void RunPrerequisiteTests()
        {
            this.ArePrerequisiteTestsRunning = true;

            try
            {
                var prerequisiteTestsToRun =
                    this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(
                        m => m.IsDefined(typeof(PrerequisiteAttribute), true));

                foreach (var prerequisiteTest in prerequisiteTestsToRun)
                {
                    prerequisiteTest.Invoke(
                        this,
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                        null,
                        null,
                        CultureInfo.InvariantCulture);
                }
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            finally
            {
                this.ArePrerequisiteTestsRunning = false;
            }
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