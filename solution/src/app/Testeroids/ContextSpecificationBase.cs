namespace Testeroids
{
    using System;
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

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    [InvokeTestsAspect]
    [CategorizeUnitTestFixturesAspect]
    [MakeEmptyTestsInconclusiveAspect]
    [EnforceInstanceLevelRulesAspect]
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
        ///   Gets the mock repository which allows the derived classes centralized mock creation and tracking.
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
        ///   Gets or sets a value indicating whether mock setups will automatically be verified through <see cref="ITesteroidsMock.Verify"/> at test fixture teardown.
        /// </summary>
        [PublicAPI]
        protected bool AutoVerifyMocks { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether mocks will be checked to ensure that all setups were verified in a test.
        /// </summary>
        [PublicAPI]
        protected bool CheckSetupsAreMatchedWithVerifyCalls { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether there are prerequisite tests running.
        /// </summary>
        private bool ArePrerequisiteTestsRunning { get; set; }

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

        #region Explicit Interface Methods

        /// <summary>
        ///   This will be called by the <see cref="InvokeTestsAspect"/> aspect. Performs the "Act" part, or the logic which is to be tested.
        /// </summary>
        /// <param name="testMethodInfo">
        ///   The <see cref="MethodBase"/> instance that describes the executing test.
        /// </param>
        /// <param name="isExceptionResilient">
        ///   <c>true</c> if the test is set to ignore some exception. <c>false</c> otherwise.
        /// </param>
        void IContextSpecification.Act(MethodBase testMethodInfo,
                                       bool isExceptionResilient)
        {
            this.MockRepository.ResetAllCalls();

            var isRunningInTheContextOfAnotherTest = this.ArePrerequisiteTestsRunning;

            if (isRunningInTheContextOfAnotherTest)
            {
                return;
            }

            var isRunningPrerequisite = testMethodInfo.IsDefined(typeof(PrerequisiteAttribute), true);

            try
            {
                if (!isRunningPrerequisite)
                {
                    this.RunPrerequisiteTests();
                }

                this.Because();
            }
            catch (AssertionException e)
            {
                if (!e.Message.TrimStart().StartsWith("Expected"))
                {
                    return;
                }

                var message = string.Format("{0}.{1}\r\n{2}", this.GetType().Name, testMethodInfo.Name, e.Message);

                if (isRunningPrerequisite)
                {
                    message = string.Format("Prerequisite failed: {0}", message);
                    throw new PrerequisiteFailureException(message, e);
                }

                throw;
            }
            catch (Exception e)
            {
                if (!isExceptionResilient)
                {
                    throw;
                }

                // we don't care about exceptions right now
                var testMethodsInContext = TypeInvestigationService.GetTestMethods(this.GetType(), true);
                var expectedExceptions =
                    testMethodsInContext.SelectMany(testMethod => testMethod.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false))
                                        .Cast<ExpectedExceptionAttribute>()
                                        .Select(attr => attr.ExpectedException)
                                        .Distinct()
                                        .ToArray();

                if (!expectedExceptions.Contains(null) && expectedExceptions.All(exceptionTypeToIgnore => !exceptionTypeToIgnore.IsInstanceOfType(e)))
                {
                    throw;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The because method as overridable by the user of Testeroids. Will be called by <see cref="Act"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Because"/> will be injected through AOP on entering a test method.
        /// Internally, <see cref="Act"/> does make sure any verified mock created by the <see cref="MockRepository"/> has its recorded calls reset.
        /// This means that any call to a mocked method will "forget" about the method calls done prior to calling <see cref="Because"/>.
        /// </remarks>
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
        ///   Allows instantiation of mocks before the call to <see cref="EstablishContext"/>, so that all mock instances are available for use, even if they are not yet configured.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void InstantiateMocks()
        {
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
        ///   This will be called by the InvokeTestsAspect aspect. Executes all tests in the context marked with the <see cref="PrerequisiteAttribute"/>.
        /// </summary>
        protected void RunPrerequisiteTests()
        {
            this.ArePrerequisiteTestsRunning = true;

            try
            {
                var prerequisiteTestsToRun =
                    from method in TypeInvestigationService.GetTestMethods(this.GetType(), true)
                    where method.IsDefined(typeof(PrerequisiteAttribute), true)
                    select method;

                foreach (var prerequisiteTest in prerequisiteTestsToRun)
                {
                    prerequisiteTest.Invoke(this,
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
            return TypeInvestigationService.GetTestMethods(this.GetType(), true).Count();
        }

        #endregion
    }
}