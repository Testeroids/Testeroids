namespace Testeroids
{
    using System;
    using System.Collections.Generic;
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
    [InstrumentTestsAspect]
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
            this.MockRepository = new MockRepository();
            this.CheckSetupsAreMatchedWithVerifyCalls = true;
            this.AutoVerifyMocks = true;

            this.SetupTasks = new List<Action<IContextSpecification>>();
            this.TeardownTasks = new List<Action<IContextSpecification>>();
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

        /// <summary>
        ///   Gets the first exception raised during the <see cref="Act()"/> method. It will be rethrown during each test that is not resilient to this exception type.
        /// </summary>
        public Exception ThrownException { get; private set; }

        /// <summary>
        ///   Gets the list of tasks to be executed during context setup.
        /// </summary>
        public IList<Action<IContextSpecification>> SetupTasks { get; private set; }

        /// <summary>
        ///   Gets the list of tasks to be executed during context teardown.
        /// </summary>
        public IList<Action<IContextSpecification>> TeardownTasks { get; private set; }

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
        }

        /// <summary>
        ///   Called when the test fixture is set up.
        /// </summary>
        [TestFixtureSetUp]
        public void BaseTestFixtureSetUp()
        {
            this.PreTestFixtureSetUp();

            this.RegisterTestFixtureWithAspects();

            foreach (var setupTask in this.SetupTasks)
            {
                setupTask(this);
            }

            this.InstantiateMocks();
            this.EstablishContext();
            this.InitializeSubjectUnderTest();

            this.Act();
        }

        /// <summary>
        ///   Called when the test fixture is tore down (invokes <see cref="IMockRepository.CheckAllSetupsVerified"/>).
        /// </summary>
        [TestFixtureTearDown]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void BaseTestFixtureTearDown()
        {
            this.DisposeContext();

            foreach (var teardownTask in this.TeardownTasks)
            {
                teardownTask(this);
            }

            if (this.AutoVerifyMocks || this.CheckSetupsAreMatchedWithVerifyCalls)
            {
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
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///   This will be called by the <see cref="InstrumentTestsAspect"/> aspect. Rethrows any exception logged during the test run.
        /// </summary>
        /// <param name="testMethodInfo">
        ///   The <see cref="MethodBase"/> instance that describes the executing test.
        /// </param>
        /// <param name="isExceptionResilient">
        ///   <c>true</c> if the test is set to ignore some exception. <c>false</c> otherwise.
        /// </param>
        void IContextSpecification.OnTestMethodCalled(MethodBase testMethodInfo,
                                                      bool isExceptionResilient)
        {
            if (this.ThrownException == null)
            {
                return;
            }

            if (isExceptionResilient)
            {
                // we don't care about exceptions right now
                var type = this.GetType();
                var testMethodsInContext = TypeInvestigationService.GetTestMethods(type, true)
                                                                   .Concat(TypeInvestigationService.GetAllContextSpecificationTypes(type)
                                                                                                   .SelectMany(nestedType => TypeInvestigationService.GetTestMethods(nestedType, true)));
                var expectedExceptions =
                    testMethodsInContext.SelectMany(testMethod => testMethod.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false))
                                        .Cast<ExpectedExceptionAttribute>()
                                        .Select(attr => attr.ExpectedException)
                                        .Distinct()
                                        .ToArray();

                if (expectedExceptions.Contains(null) || expectedExceptions.Any(exceptionTypeToIgnore => exceptionTypeToIgnore.IsInstanceOfType(this.ThrownException)))
                {
                    // Exception is ignored
                    return;
                }
            }

            throw this.ThrownException;
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
        protected virtual void PreTestFixtureSetUp()
        {
            TplTestPlatformHelper.SetDefaultScheduler(new TplTestPlatformHelper.InvalidTaskScheduler());
        }

        /// <summary>
        ///   This will be called before the actual assert method. Executes all methods in the context marked with the <see cref="PrerequisiteAttribute"/>.
        /// </summary>
        protected void RunPrerequisites()
        {
            var prerequisiteTestsToRun = TypeInvestigationService.GetPrerequisiteTestMethods(this.GetType());

            foreach (var prerequisiteTest in prerequisiteTestsToRun)
            {
                try
                {
                    prerequisiteTest.Invoke(this,
                                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                                            null,
                                            null,
                                            CultureInfo.InvariantCulture);
                }
                catch (TargetInvocationException ex)
                {
                    var exception = ex.InnerException;

                    if (exception is AssertionException)
                    {
                        if (!exception.Message.TrimStart().StartsWith("Expected"))
                        {
                            return;
                        }

                        var message = string.Format("{0}.{1}\r\n{2}", this.GetType().Name, prerequisiteTest.Name, exception.Message);

                        message = string.Format("Prerequisite failed: {0}", message);
                        exception = new PrerequisiteFailureException(message, exception);
                    }

                    throw exception;
                }
            }
        }

        /// <summary>
        ///   This will be called by the <see cref="InstrumentTestsAspect"/> aspect. Performs the "Act" part, or the logic which is to be tested.
        /// </summary>
        private void Act()
        {
            this.MockRepository.ResetAllCalls();

            try
            {
                this.Because();

                this.RunPrerequisites();
            }
            catch (AssertionException e)
            {
                if (!e.Message.TrimStart().StartsWith("Expected"))
                {
                    return;
                }

                this.ThrownException = e;
            }
            catch (Exception ex)
            {
                this.ThrownException = ex;
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

        /// <summary>
        /// Registers this context with each attribute derived from <see cref="TestFixtureSetupAttributeBase"/>. Those attributes will get invoked for setup and teardown notifications.
        /// </summary>
        private void RegisterTestFixtureWithAspects()
        {
            var aspects = this.GetType()
                              .GetCustomAttributes(typeof(TestFixtureSetupAttributeBase), true)
                              .Cast<TestFixtureSetupAttributeBase>();
            foreach (var aspect in aspects)
            {
                aspect.Register(this);
            }
        }

        #endregion
    }
}