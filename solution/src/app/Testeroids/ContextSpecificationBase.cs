// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="ContextSpecificationBase.cs">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reactive.PlatformServices;
    using System.Reflection;
    using System.Threading;

    using JetBrains.Annotations;

    using NUnit.Framework;

    using Testeroids.Aspects;
    using Testeroids.Mocking;
    using Testeroids.TriangulationEngine;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    [ProhibitGetOnNotInitializedPropertyAspect]
    public abstract class ContextSpecificationBase : IContextSpecification
    {
        #region Fields

        /// <summary>
        /// The mock repository which will allow the derived classes centralized mock creation and tracking.
        /// </summary>
        private readonly IMockRepository mockRepository = new MockRepository();

        /// <summary>
        /// Keeps a count of the number of tests executed in the current run of this test fixture.
        /// </summary>
        private int numberOfTestsExecuted;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ContextSpecificationBase"/> class.
        /// </summary>
        static ContextSpecificationBase()
        {
            var testPlatformEnlightenmentProvider = PlatformEnlightenmentProvider.Current as TestPlatformEnlightenmentProvider;
            if (testPlatformEnlightenmentProvider == null)
            {
                testPlatformEnlightenmentProvider = new TestPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = testPlatformEnlightenmentProvider;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextSpecificationBase"/> class.
        /// </summary>
        protected ContextSpecificationBase()
        {
            this.CheckSetupsAreMatchedWithVerifyCalls = true;
            this.AutoVerifyMocks = true;
            this.ArePrerequisiteTestsRunning = false;
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
            get
            {
                return this.mockRepository;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether mock setups will automatically be verified through <see cref="IMock.Verify"/> at test fixture teardown.
        /// </summary>
        [PublicAPI]
        protected bool AutoVerifyMocks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mocks will be checked to ensure that all setups were verified in a test.
        /// </summary>
        [PublicAPI]
        protected bool CheckSetupsAreMatchedWithVerifyCalls { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Sets up the test (calls <see cref="EstablishContext"/> followed by <see cref="InitializeSubjectUnderTest"/>).
        /// </summary>
        [SetUp]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseSetUp()
        {
            Interlocked.Increment(ref this.numberOfTestsExecuted);

            this.PreTestSetUp();
            this.InstantiateMocks();
            this.BeforeEstablishContext();
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
        }

        /// <summary>
        ///   Called when the test fixture is set up.
        /// </summary>
        [TestFixtureSetUp]
        public virtual void BaseTestFixtureSetUp()
        {
            TplTestPlatformHelper.SetDefaultScheduler(new TplTestPlatformHelper.InvalidTaskScheduler());
        }

        /// <summary>
        ///   Called when the test fixture is tore down (invokes <see cref="IMockRepository.CheckAllSetupsVerified"/>).
        /// </summary>
        [TestFixtureTearDown]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseTestFixtureTearDown()
        {
            if (!this.AutoVerifyMocks && !this.CheckSetupsAreMatchedWithVerifyCalls)
            {
                return;
            }

            var allTestsInFixtureWereExecuted = this.numberOfTestsExecuted == this.GetNumberOfTestsInTestFixture();

            if (allTestsInFixtureWereExecuted)
            {
                if (this.AutoVerifyMocks)
                {
                    this.MockRepository.VerifyMocks();
                }

                if (this.CheckSetupsAreMatchedWithVerifyCalls)
                {
                    this.MockRepository.CheckAllSetupsVerified();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The because method as overridable by the user of Testeroids. Will be called by <see cref="OnBecauseRequested"/>.
        /// </summary>
        /// <remarks>Internally, <see cref="OnBecauseRequested"/> does make sure any verified mock created by the <see cref="MockRepository"/> has its recorded calls reset.
        /// This means that any call to a mocked method will "forget" about the method calls done prior to calling <see cref="Because"/>.
        /// </remarks>
        protected internal abstract void Because();

        /// This test is meant for internal library use only.
        protected virtual void BeforeEstablishContext()
        {
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
        ///   Allows instantiation of mocks before the call to <see cref="EstablishContext"/>, so that all mock instances are available for use, even if they are not yet configured.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void InstantiateMocks()
        {
        }

        /// <summary>
        ///   This will be called by the <see cref="ArrangeActAssertAspectAttribute"/> aspect. Performs the "Act" part, or the logic which is to be tested.
        /// </summary>      
        [UsedImplicitly]
        protected void OnBecauseRequested()
        {
            this.MockRepository.ResetAllCalls();
            this.Because();
        }

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
        /// Calculates the number of tests (marked with <see cref="TestAttribute"/>) in this test fixture.
        /// </summary>
        /// <returns>
        /// The number of public non-static methods marked with the <see cref="TestAttribute"/>.
        /// </returns>
        private int GetNumberOfTestsInTestFixture()
        {
            return this.GetType()
                       .GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public)
                       .Count(x => x.IsDefined(typeof(TestAttribute), true));
        }

        #endregion
    }
}