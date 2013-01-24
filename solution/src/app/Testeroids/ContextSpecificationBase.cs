// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="ContextSpecificationBase.cs">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using NUnit.Framework;

    using Testeroids.Aspects;
    using Testeroids.Mocking;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    public abstract class ContextSpecificationBase : IContextSpecification
    {
        #region Fields

        /// <summary>
        /// The mock repository which will allow the derived classes centralized mock creation and tracking.
        /// </summary>
        private readonly IMockRepository mockRepository = new MockRepository();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextSpecificationBase"/> class.
        /// </summary>
        protected ContextSpecificationBase()
        {
            this.CheckAllSetupsVerified = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether there are prerequisite tests running.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ArePrerequisiteTestsRunning { get; private set; }

        /// <summary>
        /// Gets the mock repository which allows the derived classes centralized mock creation and tracking.
        /// </summary>
        [PublicAPI]
        public IMockRepository MockRepository
        {
            get { return this.mockRepository; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether mocks will be checked to ensure that all setups were verified in a test.
        /// </summary>
        protected bool CheckAllSetupsVerified { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Sets up the test (calls <see cref="EstablishContext"/> followed by <see cref="InitializeSubjectUnderTest"/>).
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
        ///   Called when the test is tore down (invokes <see cref="DisposeContext"/>).
        /// </summary>
        [TearDown]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseTearDown()
        {
            this.DisposeContext();

            this.MockRepository.VerifyMocks();
        }

        /// <summary>
        ///   Called when the test fixture is tore down (invokes <see cref="IMockRepository.CheckAllSetupsVerified"/>).
        /// </summary>
        [TestFixtureTearDown]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseTestFixtureTearDown()
        {
            if (this.CheckAllSetupsVerified)
            {
                this.MockRepository.CheckAllSetupsVerified();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   This will be called by the <see cref="ArrangeActAssertAspectAttribute"/> aspect. Performs the "Act" part, or the logic which is to be tested.
        /// </summary>
        protected internal abstract void Because();

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

        #endregion
    }
}