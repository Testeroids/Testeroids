namespace Testeroids
{
    using System;

    /// <summary>
    /// Aspect which adds support for TPL testing.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TplContextAspect : TestFixtureSetupAttributeBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TplContextAspect"/> class.
        /// </summary>
        public TplContextAspect()
        {
            this.ExecuteTplTasks = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether queued tasks will be executes.
        /// Setting this property to <c>false</c> will prevent any queued task from starting.
        /// </summary>
        /// <remarks>
        /// This could be extended in the future to be a predicate which allows more fine-grained control over the tasks which should not be started.
        /// </remarks>
        public bool ExecuteTplTasks { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Called by the framework to register the attribute with the context for setup/teardown notifications.
        /// </summary>
        /// <param name="contextSpecification">
        /// The target context specification instance.
        /// </param>
        public override void Register(IContextSpecification contextSpecification)
        {
            contextSpecification.SetupTasks.Add(this.SetupTestTaskScheduler);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestFixtureSetUp"/> method to set the <see cref="TplTestPlatformHelper.TestTaskScheduler"/> as the default scheduler.
        /// </summary>
        /// <param name="contextSpecification">
        /// The target context specification instance.
        /// </param>
        private void SetupTestTaskScheduler(IContextSpecification contextSpecification)
        {
            var testTaskScheduler = new TplTestPlatformHelper.TestTaskScheduler(this.ExecuteTplTasks);

            TplTestPlatformHelper.SetDefaultScheduler(testTaskScheduler);
        }

        #endregion
    }
}